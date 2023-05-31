namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Delegate. Indicates whether <paramref name="current"/>
    /// <see cref="ReadOnlySpan{Byte}"/> is equal to <paramref name="other"/>
    /// <see cref="ReadOnlySpan{Byte}"/>. 
    /// </summary>
    /// <param name="current">
    /// A <see cref="ReadOnlySpan{Byte}"/> to compare with <paramref name="other"/>.
    /// </param>
    /// <param name="other">
    /// A <see cref="ReadOnlySpan{Byte}"/> to compare with this <paramref name="current"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="current"/> <see cref="ReadOnlySpan{Byte}"/>
    /// is equal to <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    private delegate Boolean EqualsDelegate(ReadOnlySpan<Byte> current, ReadOnlySpan<Byte> other);

    /// <summary>
    /// Represents the empty UTF-8 byte array. This field is read-only.
    /// </summary>
    private static readonly Byte[] empty = new Byte[] { default };
    /// <summary>
    /// <see cref="EqualsDelegate"/> delegate for native comparision.
    /// </summary>
    private static readonly EqualsDelegate equals = GetEquals();
}