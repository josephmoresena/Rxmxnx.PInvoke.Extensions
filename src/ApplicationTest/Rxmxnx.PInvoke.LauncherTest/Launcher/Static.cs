namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	public static async Task<Launcher> Create(DirectoryInfo outputDirectory, Boolean useMono)
	{
		ConsoleNotifier.PlatformNotifier.BeginDetection();

		OSPlatform platform = OperatingSystem.IsWindows() ? OSPlatform.Windows :
			OperatingSystem.IsMacOS() ? OSPlatform.OSX :
			OperatingSystem.IsLinux() ? OSPlatform.Linux :
			OperatingSystem.IsFreeBSD() ? OSPlatform.FreeBSD : default;

		if (platform == OSPlatform.OSX)
			return await Launcher.Create<Mac>(outputDirectory, useMono);
		if (platform == OSPlatform.Windows)
			return await Launcher.Create<Windows>(outputDirectory, useMono);
		if (platform == OSPlatform.Linux)
			return await Launcher.Create<Linux>(outputDirectory, useMono);
		if (platform == OSPlatform.FreeBSD)
			return await Launcher.Create<FreeBsd>(outputDirectory, useMono);

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