namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Delegate"/> instances.
/// </summary>
public static partial class DelegateExtensions
{
    /// <summary>
    /// Creates a <see cref="IntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> delegate.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
    /// <param name="delegateInstance"><typeparamref name="T"/> delegate.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr AsIntPtr<T>(this T delegateInstance) where T : Delegate
        => delegateInstance is null ? IntPtr.Zero : Marshal.GetFunctionPointerForDelegate(delegateInstance);

    /// <summary>
    /// Creates a <see cref="UIntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> delegate.
    /// </summary>
    /// <typeparam name="T">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
    /// <param name="delegateInstance"><typeparamref name="T"/> delegate.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UIntPtr AsUIntPtr<T>(this T delegateInstance) where T : Delegate
    {
        IntPtr ptr = delegateInstance.AsIntPtr();
        return Unsafe.As<IntPtr, UIntPtr>(ref ptr);
    }
}

