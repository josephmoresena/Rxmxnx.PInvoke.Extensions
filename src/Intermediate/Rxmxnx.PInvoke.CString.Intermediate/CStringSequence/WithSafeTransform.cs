namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Use current instance as <see cref="ReadOnlySpan{CString}"/> instance and as
    /// parameter for <paramref name="action"/> delegate.
    /// </summary>
    /// <param name="action">A callback to invoke.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void WithSafeTransform(CStringSequenceAction action)
    {
        ArgumentNullException.ThrowIfNull(action);
        fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
        {
            _ = this.AsSpanUnsafe(out CString[] output);
            FixedCStringSequence fseq = new(output, CString.CreateUnsafe(new IntPtr(ptr), this._value.Length * SizeOfChar, true));
            try
            {
                action(fseq);
            }
            finally
            {
                fseq.Unload();
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
    public unsafe void WithSafeTransform<TState>(TState state, CStringSequenceAction<TState> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
        {
            _ = this.AsSpanUnsafe(out CString[] output);
            FixedCStringSequence fseq = new(output, CString.CreateUnsafe(new IntPtr(ptr), this._value.Length * SizeOfChar, true));
            try
            {
                action(fseq, state);
            }
            finally
            {
                fseq.Unload();
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
    public unsafe TResult WithSafeTransform<TResult>(CStringSequenceFunc<TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
        {
            _ = this.AsSpanUnsafe(out CString[] output);
            FixedCStringSequence fseq = new(output, CString.CreateUnsafe(new IntPtr(ptr), this._value.Length * SizeOfChar, true));
            try
            {
                return func(fseq);
            }
            finally
            {
                fseq.Unload();
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
    public unsafe TResult WithSafeTransform<TState, TResult>(TState state, CStringSequenceFunc<TState, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
        {
            _ = this.AsSpanUnsafe(out CString[] output);
            FixedCStringSequence fseq = new(output, CString.CreateUnsafe(new IntPtr(ptr), this._value.Length * SizeOfChar, true));
            try
            {
                return func(fseq, state);
            }
            finally
            {
                fseq.Unload();
            }
        }
    }
}

