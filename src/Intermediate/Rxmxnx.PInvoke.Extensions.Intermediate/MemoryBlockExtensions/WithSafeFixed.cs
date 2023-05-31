namespace Rxmxnx.PInvoke;

public static partial class MemoryBlockExtensions
{
    /// <summary>
    /// Prevents the garbage collector from relocating the current span and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="action">A <see cref="FixedContextAction{T}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T>(this Span<T> span, FixedContextAction<T> action)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedContextAction{T}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T>(this Span<T> span, ReadOnlyFixedContextAction<T> action)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <param name="span">Current read-only <typeparamref name="T"/> span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedContextAction{T}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T>(this ReadOnlySpan<T> span, ReadOnlyFixedContextAction<T> action)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length, true);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedContextAction{T, TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T, TArg>(this Span<T> span, TArg arg, FixedContextAction<T, TArg> action)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedContextAction{T, TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T, TArg>(this Span<T> span, TArg arg, ReadOnlyFixedContextAction<T, TArg> action)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span">Current read-only <typeparamref name="T"/> span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedContextAction{T, TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T, TArg>(this ReadOnlySpan<T> span, TArg arg, ReadOnlyFixedContextAction<T, TArg> action)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length, true);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="func">A <see cref="FixedContextFunc{T, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TResult>(this Span<T> span, FixedContextFunc<T, TResult> func)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedContextFunc{T, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TResult>(this Span<T> span, ReadOnlyFixedContextFunc<T, TResult> func)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current read-only <typeparamref name="T"/> span.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedContextFunc{T, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TResult>(this ReadOnlySpan<T> span, ReadOnlyFixedContextFunc<T, TResult> func)
        where T : unmanaged
    {
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length, true);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="FixedContextFunc{T, TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TArg, TResult>(this Span<T> span, TArg arg, FixedContextFunc<T, TArg, TResult> func)
        where T : unmanaged
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current <typeparamref name="T"/> span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedContextFunc{T, TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TArg, TResult>(this Span<T> span, TArg arg, ReadOnlyFixedContextFunc<T, TArg, TResult> func)
        where T : unmanaged
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length);
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
    /// <typeparam name="T">
    /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
    /// </typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="span">Current read-only <typeparamref name="T"/> span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedContextFunc{T, TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TArg, TResult>(this ReadOnlySpan<T> span, TArg arg, ReadOnlyFixedContextFunc<T, TArg, TResult> func)
        where T : unmanaged
    {
        ArgumentNullException.ThrowIfNull(func);
        fixed (void* ptr = &MemoryMarshal.GetReference(span))
        {
            FixedContext<T> ctx = new(ptr, span.Length, true);
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
