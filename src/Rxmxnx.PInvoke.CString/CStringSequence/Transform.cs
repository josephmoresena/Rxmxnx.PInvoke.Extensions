namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance and as
    /// parameter for <paramref name="action"/> delegate.
    /// </summary>
    /// <param name="action">A callback to invoke.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Transform(CStringSequenceAction action)
    {
        ArgumentNullException.ThrowIfNull(action);
        unsafe
        {
            fixed (Char* ptr = this._value)
            {
                _ = this.AsSpanUnsafe(out CString[] output);
                action(new(ptr, this._value.Length * SizeOfChar, output));
            }
        }
    }

    /// <summary>
    /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance and <paramref name="state"/>
    /// as parameters for <paramref name="action"/> delegate.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
    /// <param name="state">The element to pass to <paramref name="action"/>.</param>
    /// <param name="action">A callback to invoke.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Transform<TState>(TState state, CStringSequenceAction<TState> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        unsafe
        {
            fixed (Char* ptr = this._value)
            {
                _ = this.AsSpanUnsafe(out CString[] output);
                action(new(ptr, this._value.Length * SizeOfChar, output), state);
            }
        }
    }

    /// <summary>
    /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance as
    /// parameter for <paramref name="func"/> delegate.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="func">A callback to invoke.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Transform<TResult>(CStringSequenceFunc<TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        unsafe
        {
            fixed (Char* ptr = this._value)
            {
                _ = this.AsSpanUnsafe(out CString[] output);
                return func(new(ptr, this._value.Length * SizeOfChar, output));
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Transform<TState, TResult>(TState state, CStringSequenceFunc<TState, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        unsafe
        {
            fixed (Char* ptr = this._value)
            {
                _ = this.AsSpanUnsafe(out CString[] output);
                return func(new(ptr, this._value.Length * SizeOfChar, output), state);
            }
        }
    }
}

