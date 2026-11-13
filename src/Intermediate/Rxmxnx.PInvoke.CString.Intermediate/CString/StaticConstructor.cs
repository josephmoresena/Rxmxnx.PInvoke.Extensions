namespace Rxmxnx.PInvoke;

public sealed partial class CString
{
	/// <summary>
	/// Static constructor.
	/// </summary>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#if !NET5_0_OR_GREATER
	static CString()
	{
		CString.Empty = new(ValueRegion<Byte>.Create(TrimInfo.EmptyUt8Text), true, true, 0);
		CString.NewLine = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
			new(ValueRegion<Byte>.Create(TrimInfo.WindowsNewLine), true, true, 2) :
			new(ValueRegion<Byte>.Create(TrimInfo.NonWindowsNewLine), true, true, 1);
#else
	static unsafe CString()
	{
		CString.Empty = new(ValueRegion<Byte>.Create(&TrimInfo.EmptyUt8Text), true, true, 0);
		CString.NewLine = OperatingSystem.IsWindows() ?
			new(ValueRegion<Byte>.Create(&TrimInfo.WindowsNewLine), true, true, 2) :
			new(ValueRegion<Byte>.Create(&TrimInfo.NonWindowsNewLine), true, true, 1);
#endif
		CString.Zero = new(IntPtr.Zero, 0, true);
	}
}