namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for <see cref="CString"/> operations with <see cref="IntPtr"/> and <see cref="UIntPtr"/>
/// instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class PointerCStringExtensions
{
	/// <summary>
	/// Generates a <see cref="CString"/> instance using the memory reference pointed to by the
	/// given <see cref="IntPtr"/>, considering it as the start of a UTF-8 encoded string.
	/// </summary>
	/// <param name="ptr">An <see cref="IntPtr"/> pointing to the start of the UTF-8 text.</param>
	/// <param name="length">The number of <see cref="Byte"/> elements in the UTF-8 text.</param>
	/// <returns>A <see cref="CString"/> representation of the UTF-8 text.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the provided length is negative.</exception>
	/// <remarks>
	/// The reliability of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CString GetUnsafeCString(this IntPtr ptr, Int32 length)
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		if (ptr == IntPtr.Zero)
			return CString.Empty;
		return (CString)CString.CreateUnsafe(ptr, length).Clone();
	}
	/// <summary>
	/// Generates a <see cref="CString"/> instance using the memory reference pointed to by the
	/// given <see cref="UIntPtr"/>, considering it as the start of a UTF-8 encoded string.
	/// </summary>
	/// <param name="uptr">An <see cref="UIntPtr"/> pointing to the start of the UTF-8 text.</param>
	/// <param name="length">The number of <see cref="Byte"/> elements in the UTF-8 text.</param>
	/// <returns>A <see cref="CString"/> representation of the UTF-8 text.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the provided length is negative.</exception>
	/// <remarks>
	/// The reliability of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CString GetUnsafeCString(this UIntPtr uptr, Int32 length)
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		if (uptr == UIntPtr.Zero)
			return CString.Empty;
		return (CString)CString.CreateUnsafe((IntPtr)uptr.ToPointer(), length).Clone();
	}
	/// <summary>
	/// Generates a <see cref="CString"/> instance using the memory reference pointed to by the
	/// given <see cref="MemoryHandle"/>, considering it as the start of a UTF-8 encoded string.
	/// </summary>
	/// <param name="handle">The <see cref="MemoryHandle"/> pointing to the start of the UTF-8 text.</param>
	/// <param name="length">The number of <see cref="Byte"/> elements in the UTF-8 text.</param>
	/// <returns>A <see cref="CString"/> representation of the UTF-8 text.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the provided length is negative.</exception>
	/// <remarks>
	/// The reliability of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static CString GetUnsafeCString(this MemoryHandle handle, Int32 length)
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		if (handle.Pointer == default)
			return CString.Empty;
		return (CString)CString.CreateUnsafe((IntPtr)handle.Pointer, length).Clone();
	}
}