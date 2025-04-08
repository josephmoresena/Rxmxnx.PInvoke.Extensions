namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class allows to retrieve information about memory directions.
/// </summary>
internal abstract partial class MemoryInspector
{
	/// <summary>
	/// A <see cref="MemoryInspector"/> instance.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3358)]
#endif
	public static readonly MemoryInspector Instance = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
		new Windows() :
		RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? new Mac() : new Linux();

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