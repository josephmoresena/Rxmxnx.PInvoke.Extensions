namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	public static async Task<Launcher> Create(DirectoryInfo outputDirectory, Boolean useMono)
	{
		ConsoleNotifier.PlatformNotifier.BeginDetection();

		if (OperatingSystem.IsWindows())
			return await Launcher.Create<Windows>(outputDirectory, useMono);
		if (OperatingSystem.IsMacOS())
			return await Launcher.Create<Mac>(outputDirectory, useMono);
		if (OperatingSystem.IsLinux())
			return await Launcher.Create<Linux>(outputDirectory, useMono);
		if (OperatingSystem.IsFreeBSD())
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