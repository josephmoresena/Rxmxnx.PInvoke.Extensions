namespace Rxmxnx.PInvoke;

public static partial class ReferenceExtensions
{
    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes
    /// a provided action. 
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="action">An action to be performed on the fixed reference.</param>
    /// <remarks>The location is fixed until the action completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T>(this ref T refValue, FixedReferenceAction<T> action)
        where T : unmanaged
        => NativeUtilities.WithSafeFixed(ref refValue, action);
    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes
    /// a provided read-only action.
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="action">A read-only action to be performed on the fixed reference.</param>
    /// <remarks>The location is fixed until the action completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T>(this ref T refValue, ReadOnlyFixedReferenceAction<T> action)
        where T : unmanaged
        => NativeUtilities.WithSafeReadOnlyFixed(ref refValue, action);

    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a
    /// provided action along with an argument.
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <typeparam name="TArg">The type of the argument that represents the state.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="arg">An object representing the state.</param>
    /// <param name="action">An action to be performed on the fixed reference along with an argument.</param>
    /// <remarks>The location is fixed until the action completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T, TArg>(this ref T refValue, TArg arg, FixedReferenceAction<T, TArg> action)
        where T : unmanaged
        => NativeUtilities.WithSafeFixed(ref refValue, arg, action);
    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a
    /// provided read-only action along with an argument. 
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <typeparam name="TArg">The type of the argument that represents the state.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="arg">An object representing the state.</param>
    /// <param name="action">A read-only action to be performed on the fixed reference along with an argument.</param>
    /// <remarks>The location is fixed until the action completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void WithSafeFixed<T, TArg>(this ref T refValue, TArg arg, ReadOnlyFixedReferenceAction<T, TArg> action)
        where T : unmanaged
        => NativeUtilities.WithSafeReadOnlyFixed(ref refValue, arg, action);

    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a
    /// provided function. 
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="func">A function to be executed on the fixed reference.</param>
    /// <returns>The result of the function execution.</returns>
    /// <remarks>The location is fixed until the function completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TResult>(this ref T refValue, FixedReferenceFunc<T, TResult> func)
        where T : unmanaged
        => NativeUtilities.WithSafeFixed(ref refValue, func);
    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a
    /// provided read-only function. 
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="func">A read-only function to be executed on the fixed reference.</param>
    /// <returns>The result of the function execution.</returns>
    /// <remarks>The location is fixed until the function completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TResult>(this ref T refValue, ReadOnlyFixedReferenceFunc<T, TResult> func)
        where T : unmanaged
        => NativeUtilities.WithSafeReadOnlyFixed(ref refValue, func);

    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a
    /// provided function along with an argument. 
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <typeparam name="TArg">The type of the argument that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="arg">An object representing the state.</param>
    /// <param name="func">A function to be executed on the fixed reference along with an argument.</param>
    /// <returns>The result of the function execution.</returns>
    /// <remarks>The location is fixed until the function completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TArg, TResult>(this ref T refValue, TArg arg, FixedReferenceFunc<T, TArg, TResult> func)
        where T : unmanaged
        => NativeUtilities.WithSafeFixed(ref refValue, arg, func);
    /// <summary>
    /// Temporarily fixes the location of a reference by preventing the garbage collector from moving it and executes a
    /// provided read-only function along with an argument. 
    /// </summary>
    /// <typeparam name="T">The type of the reference, which must be a value type and <see langword="unmanaged"/>.</typeparam>
    /// <typeparam name="TArg">The type of the argument that represents the state.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the function.</typeparam>
    /// <param name="refValue">The reference to be fixed.</param>
    /// <param name="arg">An object representing the state.</param>
    /// <param name="func">A read-only function to be executed on the fixed reference along with an argument.</param>
    /// <returns>The result of the function execution.</returns>
    /// <remarks>The location is fixed until the function completes.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe TResult WithSafeFixed<T, TArg, TResult>(this ref T refValue, TArg arg, ReadOnlyFixedReferenceFunc<T, TArg, TResult> func)
        where T : unmanaged
        => NativeUtilities.WithSafeReadOnlyFixed(ref refValue, arg, func);
}