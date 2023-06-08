namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for <see cref="CString"/> operations with <see cref="IntPtr"/> and <see cref="UIntPtr"/> instances.
/// </summary>
public static class PointerCStringExtensions
{
    /// <summary>
    /// Creates a <see cref="String"/> instance taking the memory reference of <see cref="IntPtr"/> 
    /// value as the UTF-8 text starting point.
    /// </summary>
    /// <param name="ptr"><see cref="IntPtr"/> pointer to starting point of UTF-8 text.</param>
    /// <param name="length">Number of <see cref="Byte"/> values contained into the UTF-8 text.</param>
    /// <returns><see cref="CString"/> representation of UTF-8 text.</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CString GetUnsafeCString(this IntPtr ptr, Int32 length)
    {
        ValidationUtilities.ThrowIfInvalidMemoryLength(length);
        if (ptr == IntPtr.Zero)
            return CString.Zero;
        return (CString)CString.CreateUnsafe(ptr, length).Clone();
    }

    /// <summary>
    /// Creates a <see cref="String"/> instance taking the memory reference of <see cref="UIntPtr"/> 
    /// value as the UTF-8 text starting point.
    /// </summary>
    /// <param name="uptr"><see cref="UIntPtr"/> pointer to starting point of UTF-8 text.</param>
    /// <param name="length">Number of <see cref="Byte"/> values contained into the UTF-8 text.</param>
    /// <returns><see cref="CString"/> representation of UTF-8 text.</returns>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe CString GetUnsafeCString(this UIntPtr uptr, Int32 length)
    {
        ValidationUtilities.ThrowIfInvalidMemoryLength(length);
        if (uptr == UIntPtr.Zero)
            return CString.Zero;
        return (CString)CString.CreateUnsafe((IntPtr)uptr.ToPointer(), length).Clone();
    }
}

