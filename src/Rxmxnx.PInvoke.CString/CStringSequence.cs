namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a sequence of null-terminated UTF-8 texts.
/// </summary>
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
    public CStringSequence(params CString?[] values)
    {
        this._lengths = values.Select(c => c?.Length ?? default).ToArray();
        this._value = CreateBuffer(this._lengths, values);
    }

    /// <summary>
    /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance and <paramref name="state"/>
    /// as parameters for <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
    /// <param name="state">The element to pass to <paramref name="action"/>.</param>
    /// <param name="action">A callback to invoke.</param>
    public void Transform<TState>(TState state, CStringSequenceAction<TState> action)
    {
        unsafe
        {
            fixed (Char* ptr = this._value)
            {
                _ = this.AsSpanUnsafe(out CString[] output);
                action(output, state);
            }
        }
    }

    /// <summary>
    /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance and <paramref name="state"/>
    /// as parameters for <paramref name="func"/> delegate.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to <paramref name="func"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="state">The element to pass to <paramref name="func"/>.</param>
    /// <param name="func">A callback to invoke.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    public TResult Transform<TState, TResult>(TState state, CStringSequenceFunc<TState, TResult> func)
    {
        unsafe
        {
            fixed (Char* ptr = this._value)
            {
                _ = this.AsSpanUnsafe(out CString[] output);
                return func(output, state);
            }
        }
    }

    /// <summary>
    /// Returns a reference to this instance of <see cref="CStringSequence"/>.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public Object Clone() => new CStringSequence(this);

    /// <summary>
    /// Returns a <see cref="CString"/> that represents the current object.
    /// </summary>
    /// <returns>A <see cref="CString"/> that represents the current object.</returns>
    public CString ToCString()
    {
        Int32 bytesLength = this._lengths.Where(x => x > 0).Sum(x => x + 1);
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
    public Boolean Equals(CStringSequence? other)
        => other != default && this._value.Equals(other._value) &&
        this._lengths.SequenceEqual(other._lengths);

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true"/> if the specified object is equal to the current object; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public override Boolean Equals(Object? obj) => obj is CStringSequence cstr && this.Equals(cstr);

    /// <summary>
    /// Returns a <see cref="String"/> that represents the current object.
    /// </summary>
    /// <returns>A <see cref="String"/> that represents the current object.</returns>
    public override String ToString() => this._value;

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override Int32 GetHashCode() => this._value.GetHashCode();
}
