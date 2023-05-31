namespace Rxmxnx.PInvoke;

public static partial class DelegateExtensions
{
    /// <summary>
    /// Prevents the garbage collector from relocating the current delegate and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="TDelegate"><see cref="Delegate"/> type method.</typeparam>
    /// <param name="del">Current <typeparamref name="TDelegate"/> delegate.</param>
    /// <param name="action">A <see cref="FixedMethodAction{T}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WithSafeFixed<TDelegate>(this TDelegate del, FixedMethodAction<TDelegate> action)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, action);

    /// <summary>
    /// Prevents the garbage collector from relocating the current delegate and fixes its memory 
    /// address until <paramref name="action"/> finish.
    /// </summary>
    /// <typeparam name="TDelegate"><see cref="Delegate"/> type method.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="del">Current <typeparamref name="TDelegate"/> delegate.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedMethodAction{T, TArg}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WithSafeFixed<TDelegate, TArg>(this TDelegate del, TArg arg, FixedMethodAction<TDelegate, TArg> action)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, arg, action);

    /// <summary>
    /// Prevents the garbage collector from relocating the current delegate and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TDelegate"><see cref="Delegate"/> type method.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="del">Current <typeparamref name="TDelegate"/> delegate.</param>
    /// <param name="func">A <see cref="FixedMethodFunc{TDelegate, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSafeFixed<TDelegate, TResult>(this TDelegate del, FixedMethodFunc<TDelegate, TResult> func)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, func);

    /// <summary>
    /// Prevents the garbage collector from relocating the current delegate and fixes its memory 
    /// address until <paramref name="func"/> finish.
    /// </summary>
    /// <typeparam name="TDelegate"><see cref="Delegate"/> type method.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
    /// <param name="del">Current <typeparamref name="TDelegate"/> delegate.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="func">A <see cref="FixedMethodFunc{TDelegate, TArg, TResult}"/> delegate.</param>
    /// <returns>The result of <paramref name="func"/> execution.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSafeFixed<TDelegate, TArg, TResult>(this TDelegate del, TArg arg, FixedMethodFunc<TDelegate, TArg, TResult> func)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, arg, func);
}
