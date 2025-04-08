namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class allows to retrieve information about memory directions.
/// </summary>
internal abstract partial class MemoryInspector
{
	/// <summary>
	/// Current platform.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3358)]
#endif
	private static readonly OperatingSystem operatingSystem = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
		OperatingSystem.Windows :
		RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? OperatingSystem.Mac : OperatingSystem.Linux;

	/// <summary>
	/// Singleton instance.
	/// </summary>
	private static MemoryInspector? instance;

	/// <summary>
	/// A <see cref="MemoryInspector"/> instance.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3358)]
#endif
	public static MemoryInspector Instance
		=> MemoryInspector.instance ?? (MemoryInspector.instance = MemoryInspector.operatingSystem switch
		{
			OperatingSystem.Windows => new Windows(),
			OperatingSystem.Mac => new Mac(),
			_ => new Linux(),
		});

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