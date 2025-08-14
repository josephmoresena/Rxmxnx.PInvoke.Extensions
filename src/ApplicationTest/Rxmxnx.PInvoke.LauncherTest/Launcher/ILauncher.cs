namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	protected interface ILauncher<out TLauncher> where TLauncher : Launcher, ILauncher<TLauncher>
	{
		static abstract OSPlatform Platform { get; }
		static abstract TLauncher Create(DirectoryInfo outputDirectory, DirectoryInfo monoOutputDirectory,
			out Task initTask);
	}
}