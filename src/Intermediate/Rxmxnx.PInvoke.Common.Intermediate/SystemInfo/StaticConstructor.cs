#if !NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public static partial class SystemInfo
{
	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static SystemInfo()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			SystemInfo.isWindows = true;
			return;
		}
#if NETCOREAPP
		if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
#else
		if (SystemInfo.IsOsPlatform(SystemInfo.freePlatform))
#endif
		{
			SystemInfo.isFreeBsd = true;
			return;
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || SystemInfo.IsOsPlatform(SystemInfo.androidPlatform))
		{
			SystemInfo.isLinux = true;
			return;
		}
		if (RuntimeInformation.ProcessArchitecture == TrimInfo.WasmArch ||
		    SystemInfo.IsOsPlatform(SystemInfo.browserPlatform, SystemInfo.wPlatform))
		{
			SystemInfo.isWebRuntime = true;
			return;
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
		    SystemInfo.IsOsPlatform(SystemInfo.iPlatform, SystemInfo.tPlatform, SystemInfo.macCatalystPlatform))
		{
			SystemInfo.isMac = true;
			return;
		}

		switch (Environment.OSVersion.Platform)
		{
			case PlatformID.Xbox:
				return;
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.Win32NT:
			case PlatformID.WinCE:
				SystemInfo.isWindows = true;
				break;
			case PlatformID.MacOSX:
				SystemInfo.isMac = true;
				break;
			case (PlatformID)7:
				SystemInfo.isWebRuntime = true;
				break;
			case PlatformID.Unix:
				switch (Unix.GetUnixName())
				{
					case "linux":
						SystemInfo.isLinux = true;
						break;
					case "freebsd":
						SystemInfo.isFreeBsd = true;
						break;
					case "netbsd":
						SystemInfo.isNetBsd = true;
						break;
					case "sunos":
						SystemInfo.isSolaris = true;
						break;
					case "darwin":
						SystemInfo.isMac = true;
						break;
				}
				break;
			default:
				SystemInfo.isWebRuntime = (Int32)Environment.OSVersion.Platform == 7;
				break;
		}
	}

	/// <summary>
	/// Helper class to retrieve the current platform name.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private static unsafe class Unix
	{
		[DllImport("libc", EntryPoint = "uname", CallingConvention = CallingConvention.Cdecl)]
		private static extern Int32 GetKernelName(IntPtr ptr);

		/// <summary>
		/// Retrieves the name of the current Unix platform.
		/// </summary>
		/// <returns>The name of the current Unix platform.</returns>
		public static String? GetUnixName()
		{
			IntPtr bufferPtr = IntPtr.Zero;
			try
			{
				bufferPtr = Marshal.AllocHGlobal(256 * 6);
				Unix.GetKernelName(bufferPtr);

				ref Byte byte0 = ref Unsafe.AsRef<Byte>(bufferPtr.ToPointer());
				Int32 nameLength = MemoryMarshalCompat.IndexOfNull(ref byte0);

				if (nameLength <= 0)
					return default;

				ReadOnlySpan<Byte> ascii = MemoryMarshal.CreateReadOnlySpan(ref byte0, nameLength);
				return Encoding.ASCII.GetString(ascii).ToLowerInvariant();
			}
			catch (Exception)
			{
				return default;
			}
			finally
			{
				if (bufferPtr != IntPtr.Zero)
					Marshal.AllocHGlobal(bufferPtr);
			}
		}
	}
}
#endif