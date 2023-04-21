namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a sequence of null-terminated UTF-8 texts.
/// </summary>
[DebuggerDisplay("Count = {Count}")]
public sealed partial class CStringSequence : ICloneable, IEquatable<CStringSequence>
{
    /// <summary>
    /// Size of <see cref="Char"/> value.
    /// </summary>
    internal const Int32 SizeOfChar = sizeof(Char);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="values">Text collection.</param>
    public CStringSequence(params String?[] values)
    {
        List<CString?> cvalues = new(values.Length);
        this._lengths = new Int32?[values.Length];
        for (Int32 i = 0; i < values.Length; i++)
        {
            CString? cstr = GetCString(values[i]);
            cvalues.Add(cstr);
            this._lengths[i] = cstr?.Length;
        }
        this._value = CreateBuffer(cvalues);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="values">Text collection.</param>
    public CStringSequence(params CString?[] values)
    {
        this._lengths = values.Select(GetLength).ToArray();
        this._value = CreateBuffer(values);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="values">Text collection.</param>
    public CStringSequence(IEnumerable<String?> values)
    {
        List<CString?> cvalues = values.Select(GetCString).ToList();
        this._lengths = cvalues.Select(GetLength).ToArray();
        this._value = CreateBuffer(cvalues);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="values">Text collection.</param>
    public CStringSequence(IEnumerable<CString?> values) : this(values.ToArray())
    {
        List<CString?> cvalues = values.ToList();
        this._lengths = cvalues.Select(GetLength).ToArray();
        this._value = CreateBuffer(cvalues);
    }

    /// <summary>
    /// Returns a reference to this instance of <see cref="CStringSequence"/>.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    /// <exception cref="NotImplementedException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Object Clone() => new CStringSequence(this);

    /// <summary>
    /// Returns a <see cref="CString"/> that represents the current object.
    /// </summary>
    /// <returns>A <see cref="CString"/> that represents the current object.</returns>
    public CString ToCString()
    {
        Int32 bytesLength = this._lengths.Sum(GetSpanLength);
        Byte[] result = new Byte[bytesLength];
        this.Transform(result, BinaryCopyTo);
        return result;
    }

    /// <summary>
    /// Indicates whether the current <see cref="CStringSequence"/> is equal to another <see cref="CStringSequence"/> 
    /// instance.
    /// </summary>
    /// <param name="other">A <see cref="CStringSequence"/> to compare with this object.</param>
    /// <returns>
    /// <see langword="true"/> if the current <see cref="CStringSequence"/> is equal to the <paramref name="other"/> 
    /// parameter; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean Equals(CStringSequence? other)
        => other is not null && this._value.Equals(other._value) && this._lengths.SequenceEqual(other._lengths);

    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => obj is CStringSequence cstr && this.Equals(cstr);
    /// <inheritdoc/>
    public override String ToString() => this._value;
    /// <inheritdoc/>
    public override Int32 GetHashCode() => this._value.GetHashCode();
}
