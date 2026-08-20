using System.Runtime.CompilerServices;

using Rxmxnx.PInvoke.ApplicationTest;

if (args.Length == 0)
	throw new ArgumentException("Please set project directory.");

DirectoryInfo projectDirectory = new(args[0]);
DirectoryInfo outputDirectory = args.Length >= 2 ?
	new(args[1]) :
	new DirectoryInfo(Environment.CurrentDirectory).CreateSubdirectory("Output");
Boolean compile = args.Length < 3 || "compile".AsSpan().SequenceEqual(args[2].ToLowerInvariant());
Boolean run = args.Length < 3 || "run".AsSpan().SequenceEqual(args[2].ToLowerInvariant());

Launcher launcher = await Launcher.Create(outputDirectory, true);
_ = Boolean.TryParse(Environment.GetEnvironmentVariable("PINVOKE_ONLY_NATIVE_TEST"), out Boolean onlyNativeAot);
_ = Boolean.TryParse(Environment.GetEnvironmentVariable("NO_MONO_COMPILATION"), out Boolean noMonoCompilation);

if (compile)
{
	await TestCompiler.CompileNet(projectDirectory, launcher.RuntimeIdentifierPrefix, outputDirectory.FullName,
	                              launcher.NetVersions, onlyNativeAot);
	noMonoCompilation |= OperatingSystem.IsWindows() || OperatingSystem.IsFreeBSD();
	if (!launcher.MonoLaunchers.IsEmpty && !noMonoCompilation)
		await TestCompiler.CompileMono(projectDirectory, launcher.MonoLaunchers[0],
		                               launcher.MonoOutputDirectory!.FullName);
	if (OperatingSystem.IsWindows() && launcher is IStrongBox { Value: String msbuildPath, })
		await TestCompiler.CompileAppx(msbuildPath, projectDirectory, outputDirectory.FullName);
}

if (run)
{
	await launcher.Execute();
	await launcher.CompileMonoBundle(onlyNativeAot);
	await launcher.ExecuteMonoBundle();
	if (!onlyNativeAot && OperatingSystem.IsWindows())
	{
		String[] executablePaths = await TestCompiler.CompileFramework(projectDirectory);
		foreach (String executable in executablePaths)
			await Utilities.Execute(new() { ExecutablePath = executable, }, ConsoleNotifier.CancellationToken);
	}
}