namespace Rxmxnx.PInvoke;

public static partial class ReferenceExtensions
{
    /// <summary>
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="action">A <see cref="FixedReferenceAction{T}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed<T>(this ref T refValue, FixedReferenceAction<T> action)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedReferenceAction{T}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed<T>(this ref T refValue, ReadOnlyFixedReferenceAction<T> action)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedReferenceAction{T, TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed<T, TArg>(this ref T refValue, TArg arg, FixedReferenceAction<T, TArg> action)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedReferenceAction{T, TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static void WithSafeFixed<T, TArg>(this ref T refValue, TArg arg, ReadOnlyFixedReferenceAction<T, TArg> action)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="func">A <see cref="FixedReferenceFunc{T, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<T, TResult>(this ref T refValue, FixedReferenceFunc<T, TResult> func)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedReferenceFunc{T, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<T, TResult>(this ref T refValue, ReadOnlyFixedReferenceFunc<T, TResult> func)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="FixedReferenceFunc{T, TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<T, TArg, TResult>(this ref T refValue, TArg arg, FixedReferenceFunc<T, TArg, TResult> func)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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
    /// Prevents the garbage collector from relocating the current reference and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="refValue">The current <typeparamref name="T"/> reference.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="ReadOnlyFixedReferenceFunc{T, TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe static TResult WithSafeFixed<T, TArg, TResult>(this ref T refValue, TArg arg, ReadOnlyFixedReferenceFunc<T, TArg, TResult> func)
        where T : unmanaged
    {
        fixed (void* ptr = &refValue)
        {
            FixedReference<T> ctx = new(ptr);
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