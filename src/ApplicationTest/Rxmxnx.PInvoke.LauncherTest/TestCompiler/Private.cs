namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	private static async Task CompileNetApp(Boolean onlyNativeAot, RestoreNetArgs restoreArgs, Architecture arch,
		String outputPath)
	{
		CompileNetArgs compileArgs = new(restoreArgs, outputPath)
		{
			BuildDependencies = true, Publish = Publish.SelfContained,
		};

		await TestCompiler.RestoreNet(restoreArgs);

		if (!onlyNativeAot)
		{
			await TestCompiler.CompileNet(compileArgs);
			compileArgs.BuildDependencies = false;

			compileArgs.Publish = Publish.ReadyToRun;
			await TestCompiler.CompileNet(compileArgs);
		}

		if (!Utilities.IsNativeAotSupported(arch, restoreArgs.Version)) return;

		compileArgs.Publish = Publish.NativeAot;
		await TestCompiler.CompileNet(compileArgs);
		await TestCompiler.CompileNetWithSwitches(compileArgs);

		if (!restoreArgs.ProjectFile.EndsWith(".csproj") || restoreArgs.Version > NetVersion.Net90)
		{
			compileArgs.BuildDependencies = false;
			compileArgs.Publish |= Publish.NoReflection;
			await TestCompiler.CompileNet(compileArgs);
			await TestCompiler.CompileNetWithSwitches(compileArgs);
		}
	}
	private static async Task CompileNetWithSwitches(CompileNetArgs compileArgs)
	{
		await TestCompiler.CompileNet(compileArgs with
		{
			Publish = compileArgs.Publish | Publish.DisableBufferAutoComposition,
		});
		await TestCompiler.CompileNet(compileArgs with
		{
			Publish = compileArgs.Publish | Publish.InvariantGlobalization,
		});
		await TestCompiler.CompileNet(compileArgs with
		{
			Publish = compileArgs.Publish | Publish.DisableBufferAutoComposition |
			Publish.InvariantGlobalization,
		});
	}
	private static async Task CompileNet(CompileNetArgs args)
	{
		ExecuteState<CompileNetArgs> state = new()
		{
			ExecutablePath = "dotnet",
			ArgState = args,
			AppendArgs = CompileNetArgs.Append,
			Notifier = ConsoleNotifier.Notifier,
		};
		await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
	}
	private static async Task RestoreNet(RestoreNetArgs args)
	{
		ExecuteState<RestoreNetArgs> state = new()
		{
			ExecutablePath = "dotnet",
			ArgState = args,
			AppendArgs = RestoreNetArgs.Append,
			Notifier = ConsoleNotifier.Notifier,
		};
		await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
		await Utilities.Execute(state, ConsoleNotifier.CancellationToken);
	}
	private static Boolean ArchSupported(Architecture arch)
	{
		Architecture currentArch = RuntimeInformation.OSArchitecture;
		return arch == currentArch || currentArch switch
		{
			Architecture.X86 => false,
			Architecture.Arm => false,
			Architecture.Armv6 => false,
			_ => true,
		};
	}
}