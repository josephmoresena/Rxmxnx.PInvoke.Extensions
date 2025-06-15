namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class allows to retrieve information about memory directions.
/// </summary>
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
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static MemoryInspector()
	{
#if NET5_0_OR_GREATER
		if (OperatingSystem.IsWindows())
#else
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
#endif
			MemoryInspector.instance = new Windows();
#if NET5_0_OR_GREATER
		else if (OperatingSystem.IsLinux())
#else
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
#endif
			MemoryInspector.instance = new Linux();
#if NET5_0_OR_GREATER
		else if (!OperatingSystem.IsBrowser() && !OperatingSystem.IsFreeBSD() && !OperatingSystem.IsWatchOS())
#else
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
#endif
			MemoryInspector.instance = new Mac(); // OSX
	}

	/// <summary>
	/// Indicates whether current span represents a literal or hardcoded memory region.
	/// </summary>
	/// <typeparam name="T">Type of items in <paramref name="span"/>.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="T"/> items.</param>
	/// <returns>
	/// <see langword="true"/> if current span represents a constant, literal o hardcoded memory region;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public abstract Boolean IsLiteral<T>(ReadOnlySpan<T> span);
}