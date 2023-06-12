namespace Rxmxnx.PInvoke;

public static partial class DelegateExtensions
{
    /// <summary>
    /// Prevents the garbage collector from relocating a delegate in memory and fixes its address while
    /// an action is being performed.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the delegate.</typeparam>
    /// <param name="del">The delegate to be fixed.</param>
    /// <param name="action">The action to be performed.</param>
    /// <remarks>The location is fixed until the action completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WithSafeFixed<TDelegate>(this TDelegate del, FixedMethodAction<TDelegate> action)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, action);
    /// <summary>
    /// Prevents the garbage collector from relocating a delegate in memory and fixes its address while
    /// an action is being performed, passing an additional argument to the action.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the delegate.</typeparam>
    /// <typeparam name="TArg">The type of the additional argument to be passed to the action.</typeparam>
    /// <param name="del">The delegate to be fixed.</param>
    /// <param name="arg">The additional argument.</param>
    /// <param name="action">The action to be performed.</param>
    /// <remarks>The location is fixed until the action completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WithSafeFixed<TDelegate, TArg>(this TDelegate del, TArg arg, FixedMethodAction<TDelegate, TArg> action)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, arg, action);

    /// <summary>
    /// Prevents the garbage collector from relocating a delegate in memory, fixes its address, and performs
    /// a function that returns a value.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the delegate.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function.</typeparam>
    /// <param name="del">The delegate to be fixed.</param>
    /// <param name="func">The function to be executed.</param>
    /// <returns>The result of the function execution.</returns>
    /// <remarks>The location is fixed until the function completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSafeFixed<TDelegate, TResult>(this TDelegate del, FixedMethodFunc<TDelegate, TResult> func)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, func);
    /// <summary>
    /// Prevents the garbage collector from relocating a delegate in memory, fixes its address, performs
    /// a function that returns a value, and accepts an additional argument.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the delegate.</typeparam>
    /// <typeparam name="TArg">The type of the additional argument to be passed to the function.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function.</typeparam>
    /// <param name="del">The delegate to be fixed.</param>
    /// <param name="arg">The additional argument.</param>
    /// <param name="func">The function to be executed.</param>
    /// <returns>The result of the function execution.</returns>
    /// <remarks>The location is fixed until the function completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSafeFixed<TDelegate, TArg, TResult>(this TDelegate del, TArg arg, FixedMethodFunc<TDelegate, TArg, TResult> func)
        where TDelegate : Delegate
        => NativeUtilities.WithSafeFixed(del, arg, func);
}
