namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed class Mac : Launcher, ILauncher<Mac>
	{
		public static OSPlatform Platform => OSPlatform.OSX;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "osx";
		public override String MonoExecutablePath => "/Library/Frameworks/Mono.framework/Versions/Current/bin/mono";
		public override String MonoMsbuildPath => "/Library/Frameworks/Mono.framework/Versions/Current/bin/msbuild";
		public override String MonoFrameworkPath => "/Library/Frameworks/Mono.framework/Versions/Current/lib/mono";

		private Mac(DirectoryInfo outputDirectory, DirectoryInfo monoOutputDirectory, out Task initialize) : base(
			outputDirectory, monoOutputDirectory)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X64).ToArray();
			initialize = Task.CompletedTask;
		}

		public static Mac Create(DirectoryInfo outputDirectory, DirectoryInfo monoOutputDirectory, out Task initTask)
			=> new(outputDirectory, monoOutputDirectory, out initTask);
	}
}