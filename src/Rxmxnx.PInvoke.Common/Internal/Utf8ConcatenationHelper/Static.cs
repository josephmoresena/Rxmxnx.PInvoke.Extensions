namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8ConcatenationHelper<T>
{
    /// <summary>
    /// Prepares a UTF-8 text for concatenation process.
    /// </summary>
    /// <param name="span"><see cref="ReadOnlySpan{Byte}"/> to UTF-8 text.</param>
    /// <returns><see cref="ReadOnlySpan{Byte}"/> to UTF-8 binary data.</returns>
    private static ReadOnlySpan<Byte> PrepareUtf8Text(ReadOnlySpan<Byte> span)
    {
        if (!span.IsEmpty)
        {
            Int32 iPosition = GetInitialPosition(span);
            Int32 fLength = GetFinalLength(span, iPosition);
            return span[iPosition..fLength];
        }
        return span;
    }

    /// <summary>
    /// Gets the initial position of the UTF-8 text.
    /// </summary>
    /// <param name="span"><see cref="ReadOnlySpan{Byte}"/> to UTF-8 text.</param>
    /// <returns>Initial position of the UTF-8 text.</returns>
    private static Int32 GetInitialPosition(ReadOnlySpan<Byte> span)
    {
        Int32 iPosition = 0;
        while (iPosition < span.Length && IsNullUtf8Char(span[iPosition]))
            iPosition++;
        while (iPosition + 2 < span.Length && IsBOMChar(span[iPosition], span[iPosition + 1], span[iPosition + 2]))
            iPosition += 3;
        return iPosition;
    }

    /// <summary>
    /// Indicates whether the given <see cref="Byte"/> is UTF-8 null character. 
    /// </summary>
    /// <param name="utf8Char">UTF-8 character.</param>
    /// <returns>
    /// <see langword="true"/> if <see cref="Byte"/> instance is a null character; otherwise, 
    /// <see langword="false"/>.
    /// </returns>
    private static Boolean IsNullUtf8Char(in Byte utf8Char)
        => utf8Char == default;

    /// <summary>
    /// Indicates whether the given <see cref="Byte"/> sequence is UTF-8 BOM character. 
    /// </summary>
    /// <param name="utf8Char1">First UTF-8 character.</param>
    /// <param name="utf8Char2">Second UTF-8 character.</param>
    /// <param name="utf8Char3">Third UTF-8 character.</param>
    /// <returns>
    /// <see langword="true"/> if <see cref="Byte"/> sequence is a UTF-8 BOM character; otherwise, 
    /// <see langword="false"/>.
    /// </returns>
    private static Boolean IsBOMChar(in Byte utf8Char1, in Byte utf8Char2, in Byte utf8Char3)
        => utf8Char1 == 239 && utf8Char2 == 187 && utf8Char3 == 191;

    /// <summary>
    /// Gets the final length of the UTF-8 text.
    /// </summary>
    /// <param name="span"><see cref="ReadOnlySpan{Byte}"/> to UTF-8 text.</param>
    /// <param name="iPosition">Initial position of the UTF-8 text.</param>
    /// <returns>Final length of the UTF-8 text.</returns>
    private static Int32 GetFinalLength(ReadOnlySpan<Byte> span, Int32 iPosition)
    {
        Int32 fPosition = span.Length - 1;
        while (fPosition >= iPosition && IsNullUtf8Char(span[fPosition]))
            fPosition--;
        return fPosition - iPosition + 1;
    }
}
