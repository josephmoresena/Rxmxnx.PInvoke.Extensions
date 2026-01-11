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

if (compile)
{
	await TestCompiler.CompileNet(projectDirectory, launcher.RuntimeIdentifierPrefix, outputDirectory.FullName,
	                              launcher.NetVersions, onlyNativeAot);
	if (!launcher.MonoLaunchers.IsEmpty && !OperatingSystem.IsWindows() && !OperatingSystem.IsFreeBSD())
		await TestCompiler.CompileMono(projectDirectory, launcher.MonoLaunchers[0],
		                               launcher.MonoOutputDirectory!.FullName);
}

if (run)
{
	await launcher.Execute();
	await launcher.CompileMonoAot();
	await launcher.ExecuteMonoAot();
}