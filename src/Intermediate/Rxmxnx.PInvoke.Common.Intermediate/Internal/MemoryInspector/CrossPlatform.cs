#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class MemoryInspector
{
	/// <summary>
	/// Current platform <see cref="MemoryInspector"/> instance.
	/// </summary>
	private static readonly MemoryInspector? instance;

	/// <summary>
	/// A <see cref="MemoryInspector"/> instance.
	/// </summary>
	public static MemoryInspector Instance => ValidationUtilities.ThrowIfNotSupportedPlatform(MemoryInspector.instance);
	/// <summary>
	/// Indicates whether the memory inspection is supported.
	/// </summary>
	public static Boolean IsSupported => MemoryInspector.instance is not null;
	/// <summary>
	/// Indicates whether the current process is running over an emulated platform.
	/// </summary>
	public virtual Boolean IsEmulated => false;

	/// <summary>
	/// Static constructor.
	/// </summary>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static MemoryInspector()
	{
		if (SystemInfo.IsWindows)
			MemoryInspector.instance = new Windows();
		else if (SystemInfo.IsLinux)
			MemoryInspector.instance = new Linux();
		else if (SystemInfo.IsFreeBsd)
			MemoryInspector.instance = new FreeBsd();
		else if (SystemInfo.IsMac)
			MemoryInspector.instance = new Mac();

		if (TrimInfo.IsPlatformTrimmed()) return;

		if (SystemInfo.IsSolaris)
			MemoryInspector.instance = new Solaris();
		else if (SystemInfo.IsNetBsd)
			MemoryInspector.instance = new NetBsd();
	}
}
#endif