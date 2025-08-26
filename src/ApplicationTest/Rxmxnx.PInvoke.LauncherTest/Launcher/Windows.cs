namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed class Windows : Launcher, ILauncher<Windows>
	{
		public static OSPlatform Platform => OSPlatform.Windows;
		private readonly List<MonoLauncher>? _monoLaunchers;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "win";
		public override ReadOnlySpan<MonoLauncher> MonoLaunchers => CollectionsMarshal.AsSpan(this._monoLaunchers);

		private Windows(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X86 ||
				                                (a is Architecture.X64 && this.CurrentArch is not Architecture.X86))
			                         .ToArray();
			initialize = Task.CompletedTask;
			Windows.AppendMonoLauncher(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono", "bin"),
				Architecture.X64, ref this._monoLaunchers);
			if (this.CurrentArch is not Architecture.X86)
				Windows.AppendMonoLauncher(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Mono", "bin"),
					Architecture.X86, ref this._monoLaunchers);
		}

		private static void AppendMonoLauncher(String binPath, Architecture arch, ref List<MonoLauncher>? monoLaunchers)
		{
			String executablePath = Path.Combine(binPath, "mono.exe");
			if (!File.Exists(executablePath))
				return;
			monoLaunchers ??= new(2);
			monoLaunchers.Add(new()
			{
				Architecture = arch,
				MsbuildPath = Path.Combine(binPath, "msbuild.bat"),
				LinkerPath = Path.Combine(binPath, "monolinker.bat"),
				MakerPath = Path.Combine(binPath, "mkbundle.bat"),
				ExecutablePath = executablePath,
			});
		}

		public static Windows Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);
	}
}