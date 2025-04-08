namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class allows to retrieve information about memory directions.
/// </summary>
internal abstract partial class MemoryInspector
{
	/// <summary>
	/// A <see cref="MemoryInspector"/> instance.
	/// </summary>
	public static readonly MemoryInspector Instance;

	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3358)]
#endif
	static MemoryInspector()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			MemoryInspector.Instance = new Windows();
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			MemoryInspector.Instance = new Mac();
		else
			MemoryInspector.Instance = new Linux();
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