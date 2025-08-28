namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private partial class Linux
	{
		private readonly Boolean _isArmHf;
		private readonly MonoLauncher[]? _monoLaunchers;

		private Linux(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this._isArmHf = Linux.IsArmHf(this.CurrentArch);
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => this.IsCurrentArch(a) || Linux.IsArmHf(a) ||
				                                (a is Architecture.X64 or Architecture.Arm64 &&
					                                !Linux.IsArmHf(this.CurrentArch))).ToArray();
			initialize = Task.CompletedTask;
			if (!File.Exists("/usr/bin/mono"))
			{
				this._monoLaunchers = default;
				return;
			}
			this._monoLaunchers = new MonoLauncher[1];
			this._monoLaunchers[0] = new()
			{
				Architecture = this.CurrentArch,
				MsbuildPath = "/usr/bin/msbuild",
				LinkerPath = "/usr/bin/monolinker",
				MakerPath = "/usr/bin/mkbundle",
				MonoCilStripAssemblyPath = "/usr/lib/mono/4.5/mono-cil-strip.exe",
				ExecutablePath = "/usr/bin/mono",
			};
		}
		private async Task<Int32> RunAppQemu(FileInfo appFile, Architecture arch, String executionName,
			CancellationToken cancellationToken)
		{
			(String qemuExe, String qemuRoot) = Linux.qemu[arch];
			QemuExecuteState state = new()
			{
				QemuRoot = qemuRoot,
				QemuExecutable = qemuExe,
				ExecutablePath = appFile.FullName,
				WorkingDirectory = this.OutputDirectory.FullName,
				Notifier = ConsoleNotifier.Notifier,
			};
			Int32 result = await Utilities.QemuExecute(state, cancellationToken);
			ConsoleNotifier.Notifier.Result(result, executionName);
			return result;
		}

		private Boolean IsCurrentArch(Architecture arch)
			=> arch == this.CurrentArch || (this._isArmHf && Linux.IsArmHf(arch));

		private static Boolean IsArmHf(Architecture arch) => arch is Architecture.Arm or Architecture.Armv6;
	}
}