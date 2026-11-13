namespace Rxmxnx.PInvoke.ApplicationTest;

// ReSharper disable once ClassCannotBeInstantiated
public partial class Launcher
{
	protected interface ILauncher<out TLauncher> where TLauncher : Launcher, ILauncher<TLauncher>
	{
		static abstract OSPlatform Platform { get; }
		static abstract TLauncher Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask);
	}
}