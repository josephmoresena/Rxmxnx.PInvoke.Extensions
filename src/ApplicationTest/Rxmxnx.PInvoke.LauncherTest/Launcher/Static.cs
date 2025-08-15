namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	public static async Task<Launcher> Create(DirectoryInfo outputDirectory, Boolean useMono)
	{
		ConsoleNotifier.PlatformNotifier.BeginDetection();

		OSPlatform platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OSPlatform.Windows :
			RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? OSPlatform.OSX :
			RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? OSPlatform.Linux : default;

		if (platform == OSPlatform.OSX)
			return await Launcher.Create<Mac>(outputDirectory, useMono);
		if (platform == OSPlatform.Windows)
			return await Launcher.Create<Windows>(outputDirectory, useMono);
		if (platform == OSPlatform.Linux)
			return await Launcher.Create<Linux>(outputDirectory, useMono);

		throw new InvalidOperationException("Unsupported platform");
	}
	private static async Task<TLauncher> Create<TLauncher>(DirectoryInfo outputDirectory, Boolean useMono)
		where TLauncher : Launcher, ILauncher<TLauncher>
	{
		TLauncher result = TLauncher.Create(outputDirectory, useMono, out Task initialize);
		ConsoleNotifier.PlatformNotifier.EndDetection(TLauncher.Platform, result.CurrentArch);

		await initialize;
		ConsoleNotifier.PlatformNotifier.Initialization(TLauncher.Platform, result.CurrentArch);
		return result;
	}
}