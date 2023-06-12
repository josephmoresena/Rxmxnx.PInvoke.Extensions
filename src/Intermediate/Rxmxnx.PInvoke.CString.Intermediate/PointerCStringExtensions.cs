namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for <see cref="CString"/> operations with <see cref="IntPtr"/> and <see cref="UIntPtr"/> instances.
/// </summary>
public static class PointerCStringExtensions
{
    /// <summary>
    /// Creates a <see cref="CString"/> instance using the memory reference pointed to by the 
    /// given <see cref="IntPtr"/>, considering it as the start of a UTF-8 encoded string.
    /// </summary>
    /// <param name="ptr">An <see cref="IntPtr"/> pointing to the start of the UTF-8 text.</param>
    /// <param name="length">The number of <see cref="Byte"/> elements in the UTF-8 text.</param>
    /// <returns>A <see cref="CString"/> representation of the UTF-8 text.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided length is negative.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CString GetUnsafeCString(this IntPtr ptr, Int32 length)
    {
        ValidationUtilities.ThrowIfInvalidMemoryLength(length);
        if (ptr == IntPtr.Zero)
            return CString.Zero;
        return (CString)CString.CreateUnsafe(ptr, length).Clone();
    }
    /// <summary>
    /// Creates a <see cref="CString"/> instance using the memory reference pointed to by the 
    /// given <see cref="UIntPtr"/>, considering it as the start of a UTF-8 encoded string.
    /// </summary>
    /// <param name="uptr">An <see cref="UIntPtr"/> pointing to the start of the UTF-8 text.</param>
    /// <param name="length">The number of <see cref="Byte"/> elements in the UTF-8 text.</param>
    /// <returns>A <see cref="CString"/> representation of the UTF-8 text.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the provided length is negative.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe CString GetUnsafeCString(this UIntPtr uptr, Int32 length)
    {
        ValidationUtilities.ThrowIfInvalidMemoryLength(length);
        if (uptr == UIntPtr.Zero)
            return CString.Zero;
        return (CString)CString.CreateUnsafe((IntPtr)uptr.ToPointer(), length).Clone();
    }
}

