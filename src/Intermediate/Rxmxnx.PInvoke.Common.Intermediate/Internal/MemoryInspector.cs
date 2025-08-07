namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class allows to retrieve information about memory directions.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
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
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static MemoryInspector()
	{
		if (SystemInfo.IsWindows)
			MemoryInspector.instance = new Windows();
		else if (SystemInfo.IsLinux)
			MemoryInspector.instance = new Linux();
		else if (SystemInfo.IsMac)
			MemoryInspector.instance = new Mac();
		else if (SystemInfo.IsFreeBsd)
			MemoryInspector.instance = new FreeBsd();
		else if (SystemInfo.IsSolaris)
			MemoryInspector.instance = new Solaris();
		else if (SystemInfo.IsNetBsd)
			MemoryInspector.instance = new NetBsd();
	}

	/// <summary>
	/// Indicates whether given span represents a literal or hardcoded memory region.
	/// </summary>
	/// <typeparam name="T">Type of items in <paramref name="span"/>.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="T"/> items.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents a literal o hardcoded memory region;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean IsLiteral<T>(ReadOnlySpan<T> span)
	{
		ref T refT = ref MemoryMarshal.GetReference(span);
		ReadOnlySpan<Byte> byteSpan = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, Byte>(ref refT), 1);
		return this.IsLiteral(byteSpan);
	}
	/// <summary>
	/// Indicates whether given span represents a literal or hardcoded memory region.
	/// </summary>
	/// <param name="span">A read-only span of bytes.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents a literal o hardcoded memory region;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public abstract Boolean IsLiteral(ReadOnlySpan<Byte> span);

	/// <summary>
	/// Indicates whether the given span represents memory that is not part of a hardcoded literal.
	/// </summary>
	/// <param name="span">A read-only span of bytes.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents memory that is not part of a hardcoded literal;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean MayBeNonLiteral(ReadOnlySpan<Byte> span)
	{
		try
		{
			if (MemoryInspector.instance is not null)
				return !MemoryInspector.instance.IsLiteral(span);
		}
		catch (Exception)
		{
			// Ignore
		}
		return true;
	}
}