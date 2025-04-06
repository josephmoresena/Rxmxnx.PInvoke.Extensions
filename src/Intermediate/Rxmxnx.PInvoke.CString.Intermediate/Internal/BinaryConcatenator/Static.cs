namespace Rxmxnx.PInvoke.Internal;

internal partial class BinaryConcatenator<T>
{
	/// <summary>
	/// Prepares a UTF-8 encoded text for the concatenation process.
	/// </summary>
	/// <param name="span">
	/// The <see cref="ReadOnlySpan{Byte}"/> representing the UTF-8 encoded text.
	/// </param>
	/// <returns>
	/// A <see cref="ReadOnlySpan{Byte}"/> that represents the UTF-8 binary data derived from the
	/// original text, excluding any leading or trailing null or BOM (Byte Order Mark) characters.
	/// </returns>
	private static ReadOnlySpan<Byte> PrepareUtf8Text(ReadOnlySpan<Byte> span)
	{
		Int32 iPosition = BinaryConcatenator<T>.GetInitialPosition(span);
		Int32 fLength = BinaryConcatenator<T>.GetFinalLength(span, iPosition);
		Int32 fPosition = iPosition + fLength;
		return span[iPosition..fPosition];
	}
	/// <summary>
	/// Gets the initial position of the UTF-8 encoded text by skipping leading
	/// null or BOM (Byte Order Mark) characters.
	/// </summary>
	/// <param name="span">
	/// The <see cref="ReadOnlySpan{Byte}"/> representing the UTF-8 encoded text.
	/// </param>
	/// <returns>
	/// The initial position in the given <see cref="ReadOnlySpan{Byte}"/> after any
	/// leading null or BOM characters.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 GetInitialPosition(ReadOnlySpan<Byte> span)
	{
		Int32 iPosition = 0;
		while (iPosition < span.Length && BinaryConcatenator<T>.IsNullUtf8Char(span[iPosition]))
			iPosition++;
		while (iPosition + 2 < span.Length &&
		       BinaryConcatenator<T>.IsBomChar(span[iPosition], span[iPosition + 1], span[iPosition + 2]))
			iPosition += 3;
		return iPosition;
	}
	/// <summary>
	/// Determines if the given <see cref="Byte"/> represents a UTF-8
	/// null character.
	/// </summary>
	/// <param name="utf8Char">The byte to be checked.</param>
	/// <returns>
	/// <see langword="true"/> if the given <see cref="Byte"/> represents a null
	/// character in UTF-8; otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean IsNullUtf8Char(in Byte utf8Char) => utf8Char == default;
	/// <summary>
	/// Determines if the given sequence of bytes represents a UTF-8 BOM
	/// (Byte Order Mark).
	/// </summary>
	/// <param name="utf8Char1">The first byte in the sequence.</param>
	/// <param name="utf8Char2">The second byte in the sequence.</param>
	/// <param name="utf8Char3">The third byte in the sequence.</param>
	/// <returns>
	/// <see langword="true"/> if the given sequence of bytes represents a UTF-8 BOM;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean IsBomChar(in Byte utf8Char1, in Byte utf8Char2, in Byte utf8Char3)
		=> utf8Char1 == 239 && utf8Char2 == 187 && utf8Char3 == 191;
	/// <summary>
	/// Gets the final length of the UTF-8 text by skipping any trailing null
	/// characters.
	/// </summary>
	/// <param name="span">
	/// The <see cref="ReadOnlySpan{Byte}"/> representing the UTF-8 text.
	/// </param>
	/// <param name="iPosition">
	/// The initial position in the <see cref="ReadOnlySpan{Byte}"/> from which
	/// to start checking for trailing null characters.
	/// </param>
	/// <returns>
	/// The length of the UTF-8 text after skipping any trailing null characters.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 GetFinalLength(ReadOnlySpan<Byte> span, Int32 iPosition)
	{
		Int32 fPosition = span.Length - 1;
		while (fPosition >= iPosition && BinaryConcatenator<T>.IsNullUtf8Char(span[fPosition]))
			fPosition--;
		return fPosition - iPosition + 1;
	}
}