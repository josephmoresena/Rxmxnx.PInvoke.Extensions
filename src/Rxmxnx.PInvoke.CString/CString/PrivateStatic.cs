namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Retrives a <see cref="EqualsDelegate"/> delegate for native comparision.
    /// </summary>
    /// <returns><see cref="EqualsDelegate"/> delegate.</returns>
    [ExcludeFromCodeCoverage]
    private static EqualsDelegate GetEquals() => Environment.Is64BitProcess ? Equals<Int64> : Equals<Int32>;

    /// <summary>
    /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with 
    /// UTF-8 text.
    /// </summary>
    /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
    /// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
    private static Byte[] GetUtf8Bytes(String str) => !String.IsNullOrEmpty(str) ? Encoding.UTF8.GetBytes(str) : Array.Empty<Byte>();

    /// <summary>
    /// Indicates whether <paramref name="current"/> <see cref="CString"/> is equal to 
    /// <paramref name="other"/> <see cref="CString"/>.
    /// instance.
    /// </summary>
    /// <typeparam name="TInteger"><see cref="ValueType"/> for reduce comparation.</typeparam>
    /// <param name="current">A <see cref="CString"/> to compare with <paramref name="other"/>.</param>
    /// <param name="other">A <see cref="CString"/> to compare with this <paramref name="current"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="current"/> <see cref="CString"/> is equal to 
    /// <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    private static Boolean Equals<TInteger>(CString current, CString other)
        where TInteger : unmanaged
    {
        ReadOnlySpan<Byte> currSpan = current;
        ReadOnlySpan<Byte> otherSpan = other;

        if (currSpan.Length == otherSpan.Length)
            unsafe
            {
                fixed (void* currPtr = &MemoryMarshal.GetReference(currSpan))
                fixed (void* otherPtr = &MemoryMarshal.GetReference(otherSpan))
                {
                    IReadOnlyFixedContext<Byte> currCtx = new FixedContext<Byte>(currPtr, current.Length, true);
                    IReadOnlyFixedContext<Byte> otherCtx = new FixedContext<Byte>(otherPtr, other.Length, true);
                    return Equals<TInteger>(currCtx, otherCtx);
                }
            }
        return false;
    }

    /// <summary>
    /// Indicates whether <paramref name="currCtx"/> <see cref="IReadOnlyFixedContext{Byte}"/> is equal to 
    /// <paramref name="otherCtx"/> <see cref="IReadOnlyFixedContext{Byte}"/>.
    /// </summary>
    /// <typeparam name="TInteger"><see cref="ValueType"/> for reduce comparation.</typeparam>
    /// <param name="currCtx">A <see cref="IReadOnlyFixedContext{Byte}"/> to compare with <paramref name="otherCtx"/>.</param>
    /// <param name="otherCtx">A <see cref="IReadOnlyFixedContext{Byte}"/> to compare with this <paramref name="currCtx"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="currCtx"/> <see cref="IReadOnlyFixedContext{Byte}"/> is equal to 
    /// <paramref name="otherCtx"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    private static unsafe Boolean Equals<TInteger>(IReadOnlyFixedContext<Byte> currCtx, IReadOnlyFixedContext<Byte> otherCtx) where TInteger : unmanaged
    {
        IReadOnlyTransformationContext<Byte, TInteger> thisTx = currCtx.Transformation<TInteger>();
        IReadOnlyTransformationContext<Byte, TInteger> otherTx = otherCtx.Transformation<TInteger>();

        Int32 iCount = thisTx.Values.Length;
        Int32 bCount = thisTx.ResidualBytes.Length;

        for (Int32 i = 0; i < iCount; i++)
            if (!thisTx.Values[i].Equals(otherTx.Values[i]))
                return false;
        for (Int32 i = 0; i < bCount; i++)
            if (!thisTx.ResidualBytes[i].Equals(otherTx.ResidualBytes[i]))
                return false;

        return true;
    }

    /// <summary>
    /// Indicates whether <paramref name="data"/> contains a null-terminated UTF-8 text.
    /// </summary>
    /// <param name="data">A read-only byte span containing UTF-8 text.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="data"/> contains a null-terminated UTF-8 text; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static Boolean IsNullTerminatedSpan(ReadOnlySpan<Byte> data)
        => !data.IsEmpty && data[^1] == default;
}