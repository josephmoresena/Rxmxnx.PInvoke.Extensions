namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed class Windows : Launcher, ILauncher<Windows>
	{
		public static OSPlatform Platform => OSPlatform.Windows;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "win";
		public override String MonoExecutablePath { get; } =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono", "bin", "mono.exe");
		public override String MonoMsbuildPath { get; } = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono", "bin", "msbuild.bat");
		public override String MonoFrameworkPath { get; } = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono", "lib", "mono");

		private Windows(DirectoryInfo outputDirectory, out Task initialize) : base(outputDirectory)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X86 ||
				                                (a is Architecture.X64 && this.CurrentArch is not Architecture.X86))
			                         .ToArray();
			initialize = Task.CompletedTask;
		}

		public static Windows Create(DirectoryInfo outputDirectory, out Task initTask)
			=> new(outputDirectory, out initTask);
	}
}