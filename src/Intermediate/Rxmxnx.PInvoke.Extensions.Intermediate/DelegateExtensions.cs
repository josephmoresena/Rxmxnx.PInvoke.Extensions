namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Delegate"/> instances.
/// </summary>
public static partial class DelegateExtensions
{
    /// <summary>
    /// Creates a <see cref="IntPtr"/> pointer from a memory reference to a <typeparamref name="TDelegate"/> delegate.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
    /// <param name="delegateInstance"><typeparamref name="TDelegate"/> delegate.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr GetUnsafeIntPtr<TDelegate>(this TDelegate? delegateInstance) where TDelegate : Delegate
        => delegateInstance is null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(delegateInstance);

    /// <summary>
    /// Creates a <see cref="UIntPtr"/> pointer from a memory reference to a <typeparamref name="TDelegate"/> delegate.
    /// </summary>
    /// <typeparam name="TDelegate">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
    /// <param name="delegateInstance"><typeparamref name="TDelegate"/> delegate.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe UIntPtr GetUnsafeUIntPtr<TDelegate>(this TDelegate? delegateInstance) where TDelegate : Delegate
    {
        IntPtr ptr = delegateInstance.GetUnsafeIntPtr();
        return (UIntPtr)ptr.ToPointer();
    }
}