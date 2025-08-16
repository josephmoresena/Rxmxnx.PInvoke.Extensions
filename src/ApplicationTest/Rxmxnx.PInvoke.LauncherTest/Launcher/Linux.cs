namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed partial class Linux : Launcher, ILauncher<Linux>
	{
		public static OSPlatform Platform => OSPlatform.Linux;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "linux";
		public override String MonoExecutablePath => "/usr/bin/mono";
		public override String MonoMsbuildPath => "/usr/bin/msbuild";
		protected override Task<Int32> RunAppFile(FileInfo appFile, Architecture arch, String executionName,
			CancellationToken cancellationToken)
		{
			if (this.IsCurrentArch(arch) || (this.CurrentArch is Architecture.Arm64 && Linux.IsArmHf(arch)))
				return base.RunAppFile(appFile, arch, executionName, cancellationToken);
			return this.RunAppQemu(appFile, arch, executionName, cancellationToken);
		}
		public static Linux Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);
	}
}