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
		public override String Mono2ExecutablePath => "/opt/homebrew/bin/mono";

		private Mac(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X64).ToArray();
			initialize = Task.CompletedTask;
		}

		public static Mac Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);
	}
}