namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="String"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class StringExtensions
{
    /// <summary>
    /// Prevents the garbage collector from relocating the current <see cref="String"/> by pinning its memory 
    /// address until the action specified in <paramref name="action"/> has completed.
    /// </summary>
    /// <param name="str">The current <see cref="String"/> instance.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedContextAction{Char}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed(this String? str, ReadOnlyFixedContextAction<Char> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (str is not null)        
            fixed (void* ptr = str)
            {
                ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
                try
                {
                    action(ctx);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        else
            action(ReadOnlyFixedContext<Char>.Empty);
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current <see cref="String"/> by pinning its memory 
    /// address until the action specified in <paramref name="action"/> has completed.
    /// </summary>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="str">The current <see cref="String"/> instance.</param>
    /// <param name="arg">An object of type <typeparamref name="TArg"/> that represents the state.</param>
    /// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{Char, TArg}"/>.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<TArg>(this String? str, TArg arg, ReadOnlyFixedContextAction<Char, TArg> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (str is not null)
            fixed (void* ptr = str)
            {
                ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
                try
                {
                    action(ctx, arg);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        else
            action(ReadOnlyFixedContext<Char>.Empty, arg);
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current <see cref="String"/> by pinning its memory 
    /// address until the function specified in <paramref name="func"/> has completed.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
    /// <param name="str">The current <see cref="String"/> instance.</param>
    /// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{Char, TResult}"/>.</param>
    /// <returns>The result of executing <paramref name="func"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<TResult>(this String? str, ReadOnlyFixedContextFunc<Char, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        if (str is not null)
            fixed (void* ptr = str)
            {
                ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
                try
                {
                    return func(ctx);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        else
           return func(ReadOnlyFixedContext<Char>.Empty);
    }

    /// <summary>
    /// Prevents the garbage collector from relocating the current <see cref="String"/> by pinning its memory 
    /// address until the function specified in <paramref name="func"/> has completed.
    /// </summary>
    /// <typeparam name="TArg">The type of the object representing the state.</typeparam>
    /// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
    /// <param name="str">The current <see cref="String"/> instance.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{Char, TArg, TResult}"/>.</param>
    /// <returns>The result of executing <paramref name="func"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<TArg, TResult>(this String? str, TArg arg, ReadOnlyFixedContextFunc<Char, TArg, TResult> func)
    {
        ArgumentNullException.ThrowIfNull(func);
        if (str is not null)
            fixed (void* ptr = str)
            {
                ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
                try
                {
                    return func(ctx, arg);
                }
                finally
                {
                    ctx.Unload();
                }
            }
        else
            return func(ReadOnlyFixedContext<Char>.Empty, arg);
    }
}

