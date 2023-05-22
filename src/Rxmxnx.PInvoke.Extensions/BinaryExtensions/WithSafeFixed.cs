namespace Rxmxnx.PInvoke;

public static partial class MemoryBlockExtensions
{
    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <param name="span">Current binary span.</param>
    /// <param name="action">A <see cref="FixedAction"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed(this Span<Byte> span, FixedAction action)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length);
            try
            {
                action(ctx);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <param name="span">Current binary span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedAction"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed(this Span<Byte> span, ReadOnlyFixedAction action)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length);
            try
            {
                action(ctx);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current read-only span and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <param name="span">Current read-only binary span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedAction"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed(this ReadOnlySpan<Byte> span, ReadOnlyFixedAction action)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length, true);
            try
            {
                action(ctx);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">Current binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedAction{TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed<TArg>(this Span<Byte> span, TArg arg, FixedAction<TArg> action)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length);
            try
            {
                action(ctx, arg);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">Current binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedAction{TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed<TArg>(this Span<Byte> span, TArg arg, ReadOnlyFixedAction<TArg> action)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length);
            try
            {
                action(ctx, arg);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current read-only span and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">Current read-only binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedAction{TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed<TArg>(this ReadOnlySpan<Byte> span, TArg arg, ReadOnlyFixedAction<TArg> action)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length, true);
            try
            {
                action(ctx, arg);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current binary span.</param>
    /// <param name="func">A <see cref="FixedFunc{TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<TResult>(this Span<Byte> span, FixedFunc<TResult> func)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length);
            try
            {
                return func(ctx);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current binary span.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedFunc{TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<TResult>(this Span<Byte> span, ReadOnlyFixedFunc<TResult> func)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length);
            try
            {
                return func(ctx);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current read-only span and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current read-only binary span.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedFunc{TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<TResult>(this ReadOnlySpan<Byte> span, ReadOnlyFixedFunc<TResult> func)
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length);
            try
            {
                return func(ctx);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="FixedFunc{TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<TArg, TResult>(this Span<Byte> span, TArg arg, FixedFunc<TArg, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length, true);
            try
            {
                return func(ctx, arg);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedFunc{TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<TArg, TResult>(this Span<Byte> span, TArg arg, ReadOnlyFixedFunc<TArg, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length, true);
            try
            {
                return func(ctx, arg);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current read-only span and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current read-only binary span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedFunc{TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<TArg, TResult>(this ReadOnlySpan<Byte> span, TArg arg, ReadOnlyFixedFunc<TArg, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<Byte> ctx = new(ptr, span.Length, true);
            try
            {
                return func(ctx, arg);
            }
            finally
            {
                ctx.Unload();
            }
        }
    }
}