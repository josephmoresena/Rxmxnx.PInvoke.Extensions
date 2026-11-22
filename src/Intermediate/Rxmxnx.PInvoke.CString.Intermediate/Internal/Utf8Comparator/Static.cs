#if NET462_OR_GREATER || NETSTANDARD2_0
using Utf8 = Rxmxnx.PInvoke.Internal.FrameworkCompat.Utf8Compat;
#endif

namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal abstract unsafe partial class Utf8Comparator
{
	/// <summary>
	/// Retrieves the string representation of <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>A string that represents the <paramref name="source"/> text.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE && (!NETCOREAPP || NET7_0_OR_GREATER)
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String GetStringFromUtf8(ReadOnlySpan<Byte> source) => source.ToUtf16();
	/// <summary>
	/// Decodes all the bytes in <paramref name="source"/> into a <paramref name="destination"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <param name="destination">The character span receiving the decoded bytes.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
#if !PACKAGE && NETCOREAPP && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void CopyCharsFromUtf8(ReadOnlySpan<Byte> source, Span<Char> destination)
		=> Utf8.ToUtf16(source, destination, out _, out _);
	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-8 encoded source buffer.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>The decoded <see cref="Rune"/>, if any; otherwise, <see langword="null"/>.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static Rune? DecodeRuneFromUtf8(ref ReadOnlySpan<Byte> source)
	{
		if (Rune.DecodeFromUtf8(source, out Rune result, out Int32 bytesConsumed) != OperationStatus.Done)
			return default;
		source = source[bytesConsumed..];
		return result;
	}
}