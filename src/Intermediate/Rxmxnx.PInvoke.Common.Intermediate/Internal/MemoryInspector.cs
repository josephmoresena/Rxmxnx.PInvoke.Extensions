// ReSharper disable MemberCanBeProtected.Global

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class retrieves information about memory addresses.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal abstract unsafe partial class MemoryInspector
{
	/// <summary>
	/// Indicates whether a given pointer references a read-only memory section.
	/// </summary>
	/// <param name="ptr">A native pointer.</param>
	/// <returns>
	/// <see langword="true"/> if the given pointer references a read-only memory section;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	public abstract Boolean IsReadOnlyAddress(void* ptr);
	/// <summary>
	/// Indicates whether the function pointer of <paramref name="methodHandle"/> references a native linked image.
	/// </summary>
	/// <param name="methodHandle">A <see cref="RuntimeMethodHandle"/> value.</param>
	/// <returns>
	/// <see langword="true"/> if the function pointer references a native linked image; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	public abstract Boolean IsImageMethod(RuntimeMethodHandle methodHandle);

	/// <summary>
	/// Indicates whether the given span represents memory that is not part of a hardcoded literal.
	/// </summary>
	/// <param name="span">A read-only span of bytes.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents memory that is not part of a hardcoded literal;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean MayBeNonLiteral(ReadOnlySpan<Byte> span)
	{
		try
		{
#if !UAP10_0
			if (MemoryInspector.instance is not null)
				return !MemoryInspector.instance.IsLiteral(span);
#else
			return !MemoryInspector.Instance.IsLiteral(span);
#endif
		}
		catch (Exception)
		{
			// Ignore
		}
		return true;
	}
	/// <summary>
	/// Indicates whether the given span represents memory that is not part of a hardcoded literal.
	/// </summary>
	/// <param name="refByte">A read-only <see cref="Byte"/> reference.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents memory that is not part of a hardcoded literal;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean MayBeNonLiteral(in Byte refByte)
	{
		try
		{
#if !UAP10_0
			if (MemoryInspector.instance is not null)
				fixed (void* ptr = &refByte)
					return !MemoryInspector.instance.IsReadOnlyAddress(ptr);
#else
			fixed (void* ptr = &refByte)
				return !MemoryInspector.Instance.IsReadOnlyAddress(ptr);
#endif
		}
		catch (Exception)
		{
			// Ignore
		}
		return true;
	}
	/// <summary>
	/// Indicates whether the given span represents memory that is not part of a hardcoded literal.
	/// </summary>
	/// <param name="ptr">A native pointer.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents memory that is not part of a hardcoded literal;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean MayBeNonLiteral(void* ptr)
	{
		try
		{
#if !UAP10_0
			if (MemoryInspector.instance is not null)
				return !MemoryInspector.instance.IsReadOnlyAddress(ptr);
#else
			return !MemoryInspector.Instance.IsReadOnlyAddress(ptr);
#endif
		}
		catch (Exception)
		{
			// Ignore
		}
		return true;
	}
}