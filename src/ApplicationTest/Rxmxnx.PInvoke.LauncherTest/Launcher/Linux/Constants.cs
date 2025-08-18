namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private partial class Linux
	{
		private static readonly Dictionary<Architecture, (String qemuExe, String qemuRoot)> qemu = new()
		{
			{ Architecture.Arm, ("qemu-arm", "/usr/arm-linux-gnueabihf") },
			{ Architecture.Armv6, ("qemu-arm", "/usr/arm-linux-gnueabihf") },
			{ Architecture.Arm64, ("qemu-aarch64", "/usr/aarch64-linux-gnu") },
			{ Architecture.X64, ("qemu-x86_64", "/usr/x86_64-linux-gnu") },
		};
	}
}