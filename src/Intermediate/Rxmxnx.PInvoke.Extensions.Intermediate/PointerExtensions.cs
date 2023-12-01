namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="IntPtr"/> and <see cref="UIntPtr"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[SuppressMessage("csharpsquid", "S6640")]
public static unsafe class PointerExtensions
{
	/// <summary>
	/// Indicates whether the <see cref="IntPtr"/> instance is zero.
	/// </summary>
	/// <param name="ptr">The <see cref="IntPtr"/> instance to check.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="IntPtr"/> instance equals <see cref="IntPtr.Zero"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsZero(this IntPtr ptr) => ptr == IntPtr.Zero;
	/// <summary>
	/// Indicates whether the <see cref="UIntPtr"/> instance is zero.
	/// </summary>
	/// <param name="uptr">The <see cref="UIntPtr"/> instance to check.</param>
	/// <returns>
	/// <see langword="true"/> if the <see cref="UIntPtr"/> instance equals <see cref="UIntPtr.Zero"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsZero(this UIntPtr uptr) => uptr == UIntPtr.Zero;
	/// <summary>
	/// Converts a given <see cref="IntPtr"/> instance to a <see cref="UIntPtr"/> instance.
	/// </summary>
	/// <param name="ptr">The <see cref="IntPtr"/> instance to convert.</param>
	/// <returns>The <see cref="UIntPtr"/> instance that represents the same pointer as this instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIntPtr ToUIntPtr(this IntPtr ptr) => (UIntPtr)ptr.ToPointer();
	/// <summary>
	/// Converts a given <see cref="UIntPtr"/> instance to a <see cref="IntPtr"/> instance.
	/// </summary>
	/// <param name="uptr">The <see cref="UIntPtr"/> instance to convert.</param>
	/// <returns>The <see cref="IntPtr"/> instance that represents the same pointer as this instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr ToIntPtr(this UIntPtr uptr) => (IntPtr)uptr.ToPointer();

	/// <summary>
	/// Creates a <see cref="String"/> instance from the memory at the given <see cref="IntPtr"/>,
	/// interpreting the contents as UTF-16 text.
	/// </summary>
	/// <param name="ptr">The <see cref="IntPtr"/> pointing to the start of UTF-16 text in memory.</param>
	/// <returns>A <see cref="String"/> representation of the UTF-16 text in memory.</returns>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String? GetUnsafeString(this IntPtr ptr) => ptr.GetUnsafeString(0);
	/// <summary>
	/// Creates a <see cref="String"/> instance from the memory at the given <see cref="UIntPtr"/>,
	/// interpreting the contents as UTF-16 text.
	/// </summary>
	/// <param name="uptr">The <see cref="UIntPtr"/> pointing to the start of UTF-16 text in memory.</param>
	/// <returns>A <see cref="String"/> representation of the UTF-16 text in memory.</returns>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String? GetUnsafeString(this UIntPtr uptr) => uptr.GetUnsafeString(0);
	/// <summary>
	/// Creates a <see cref="String"/> instance from the memory at the given <see cref="IntPtr"/>,
	/// interpreting the contents as UTF-16 text.
	/// </summary>
	/// <param name="ptr">The <see cref="IntPtr"/> pointing to the start of UTF-16 text in memory.</param>
	/// <param name="length">
	/// The number of <see cref="Char"/> elements to include in the resulting string,
	/// starting from the pointer. If this value is zero, the function reads until the first null character.
	/// </param>
	/// <returns>A <see cref="String"/> representation of the UTF-16 text in memory.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String? GetUnsafeString(this IntPtr ptr, Int32 length)
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		return ptr.IsZero() ? default : PointerExtensions.GetStringFromCharPointer((Char*)ptr.ToPointer(), length);
	}
	/// <summary>
	/// Creates a <see cref="String"/> instance from a <see cref="Char"/> pointer, interpreting the contents as UTF-16 text.
	/// </summary>
	/// <param name="uptr">A pointer to the first character of the UTF-16 text in memory.</param>
	/// <param name="length">
	/// The number of <see cref="Char"/> elements to include in the resulting string,
	/// starting from the pointer. If this value is zero, the function reads until the first null character.
	/// </param>
	/// <returns>A <see cref="String"/> representation of the UTF-16 text in memory.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String? GetUnsafeString(this UIntPtr uptr, Int32 length)
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		return uptr.IsZero() ? default : PointerExtensions.GetStringFromCharPointer((Char*)uptr.ToPointer(), length);
	}

	/// <summary>
	/// Creates a <typeparamref name="T"/> array by copying values from memory starting at the location referenced by an
	/// <see cref="IntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The type of <see langword="unmanaged"/> values in memory.</typeparam>
	/// <param name="ptr">
	/// The <see cref="IntPtr"/> pointing to the start of a series of <typeparamref name="T"/> values in memory.
	/// </param>
	/// <param name="length">The number of <typeparamref name="T"/> values to include in the array.</param>
	/// <returns>A new array of <typeparamref name="T"/>, or  <see langword="null"/> if the pointer is zero.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	public static T[]? GetUnsafeArray<T>(this IntPtr ptr, Int32 length) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		return ptr.IsZero() ? default : ptr.GetUnsafeReadOnlySpan<T>(length).ToArray();
	}
	/// <summary>
	/// Creates a <typeparamref name="T"/> array by copying values from memory starting at the location referenced by a
	/// <see cref="UIntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The type of <see langword="unmanaged"/> values in memory.</typeparam>
	/// <param name="uptr">
	/// The <see cref="UIntPtr"/> pointing to the start of a series of <typeparamref name="T"/> values in memory.
	/// </param>
	/// <param name="length">The number of <typeparamref name="T"/> values to include in the array.</param>
	/// <returns>A new array of <typeparamref name="T"/>, or <see langword="null"/> if the pointer is zero.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	public static T[]? GetUnsafeArray<T>(this UIntPtr uptr, Int32 length) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		return uptr.IsZero() ? default : uptr.GetUnsafeReadOnlySpan<T>(length).ToArray();
	}

	/// <summary>
	/// Creates a <see cref="Span{T}"/> instance from a <see cref="IntPtr"/>, treating the memory at the location as a
	/// series of <see langword="unmanaged"/> values.
	/// </summary>
	/// <typeparam name="T">The type of <see langword="unmanaged"/> values in memory.</typeparam>
	/// <param name="ptr">
	/// The <see cref="IntPtr"/> pointing to the start of a series of <typeparamref name="T"/> values in memory.
	/// </param>
	/// <param name="length">The number of <typeparamref name="T"/> values to include in the span.</param>
	/// <returns>A <see cref="Span{T}"/> representing the series of <see langword="unmanaged"/> values in memory.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained span depends on the lifetime and validity of the pointer during the usage of
	/// the span.
	/// The span does not own the memory it points to, it's merely a projection over the existing memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> GetUnsafeSpan<T>(this IntPtr ptr, Int32 length) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		if (ptr.IsZero())
			return default;
		return new(ptr.ToPointer(), length);
	}
	/// <summary>
	/// Creates a <see cref="Span{T}"/> instance from a <see cref="UIntPtr"/>, treating the memory at the location as a
	/// series of <see langword="unmanaged"/> values.
	/// </summary>
	/// <typeparam name="T">The type of <see langword="unmanaged"/> values in memory.</typeparam>
	/// <param name="uptr">
	/// The <see cref="UIntPtr"/> pointing to the start of a series of <typeparamref name="T"/> values in memory.
	/// </param>
	/// <param name="length">The number of <typeparamref name="T"/> values to include in the span.</param>
	/// <returns>A <see cref="Span{T}"/> representing the series of <see langword="unmanaged"/> values in memory.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained span depends on the lifetime and validity of the pointer during the usage of
	/// the span.
	/// The span does not own the memory it points to, it's merely a projection over the existing memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> GetUnsafeSpan<T>(this UIntPtr uptr, Int32 length) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		if (uptr.IsZero())
			return default;
		return new(uptr.ToPointer(), length);
	}
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{T}"/> instance from a <see cref="IntPtr"/>, treating the memory at the location as a
	/// series of <see langword="unmanaged"/> values.
	/// </summary>
	/// <typeparam name="T">The type of <see langword="unmanaged"/> values in memory.</typeparam>
	/// <param name="ptr">
	/// The <see cref="IntPtr"/> pointing to the start of a series of <typeparamref name="T"/> values in memory
	/// .
	/// </param>
	/// <param name="length">The number of <typeparamref name="T"/> values to include in the span.</param>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> representing the series of <see langword="unmanaged"/> values in memory.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained span depends on the lifetime and validity of the pointer during the usage of
	/// the span.
	/// The span does not own the memory it points to, it's merely a projection over the existing memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[SuppressMessage("csharpsquid", "S4144")]
	public static ReadOnlySpan<T> GetUnsafeReadOnlySpan<T>(this IntPtr ptr, Int32 length) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		if (ptr.IsZero())
			return default;
		return new(ptr.ToPointer(), length);
	}
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{T}"/> instance from a <see cref="UIntPtr"/>, treating the memory at the location as a
	/// series of <see langword="unmanaged"/> values.
	/// </summary>
	/// <typeparam name="T">The type of <see langword="unmanaged"/> values in memory.</typeparam>
	/// <param name="uptr">
	/// The <see cref="UIntPtr"/> pointing to the start of a series of <typeparamref name="T"/> values in memory.
	/// </param>
	/// <param name="length">The number of <typeparamref name="T"/> values to include in the span.</param>
	/// <returns>A <see cref="ReadOnlySpan{T}"/> representing the series of <see langword="unmanaged"/> values in memory.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained span depends on the lifetime and validity of the pointer during the usage of
	/// the span.
	/// The span does not own the memory it points to, it's merely a projection over the existing memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[SuppressMessage("csharpsquid", "S4144")]
	public static ReadOnlySpan<T> GetUnsafeReadOnlySpan<T>(this UIntPtr uptr, Int32 length) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidMemoryLength(length);
		if (uptr.IsZero())
			return default;
		return new(uptr.ToPointer(), length);
	}

	/// <summary>
	/// Creates a delegate of type <typeparamref name="T"/> from an <see cref="IntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The type of the delegate.</typeparam>
	/// <param name="ptr">The <see cref="IntPtr"/> referencing the delegate in memory.</param>
	/// <returns>A delegate of type <typeparamref name="T"/>, or <see langword="null"/> if the pointer is zero.</returns>
	/// <remarks>
	/// The validity and safety of the obtained delegate depends on the lifetime and validity of the pointer.
	/// If the function the delegate represents is moved or deallocated, invoking the delegate can cause unexpected behavior or
	/// application crashes.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? GetUnsafeDelegate<T>(this IntPtr ptr) where T : Delegate
		=> !ptr.IsZero() ? Marshal.GetDelegateForFunctionPointer<T>(ptr) : default;
	/// <summary>
	/// Creates a delegate of type <typeparamref name="T"/> from an <see cref="UIntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The type of the delegate.</typeparam>
	/// <param name="uptr">The <see cref="UIntPtr"/> referencing the delegate in memory.</param>
	/// <returns>A delegate of type <typeparamref name="T"/>, or <see langword="null"/> if the pointer is zero.</returns>
	/// <remarks>
	/// The validity and safety of the obtained delegate depends on the lifetime and validity of the pointer.
	/// If the function the delegate represents is moved or deallocated, invoking the delegate can cause unexpected behavior or
	/// application crashes.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? GetUnsafeDelegate<T>(this UIntPtr uptr) where T : Delegate
		=> uptr.ToIntPtr().GetUnsafeDelegate<T>();

	/// <summary>
	/// Creates a memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/> from
	/// an <see cref="IntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The <see langword="unmanaged"/> value type.</typeparam>
	/// <param name="ptr">The <see cref="IntPtr"/> pointer.</param>
	/// <returns>A memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// The validity and safety of the returned reference depends on the lifetime and validity of the pointer.
	/// If the data the reference represents is moved or deallocated, accessing the reference can cause unexpected behavior
	/// or application crashes.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetUnsafeReference<T>(this IntPtr ptr) where T : unmanaged
		=> ref Unsafe.AsRef<T>(ptr.ToPointer());
	/// <summary>
	/// Creates a memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/> from
	/// an <see cref="UIntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The <see langword="unmanaged"/> value type.</typeparam>
	/// <param name="uptr">The <see cref="UIntPtr"/> pointer.</param>
	/// <returns>A memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// The validity and safety of the returned reference depends on the lifetime and validity of the pointer.
	/// If the data the reference represents is moved or deallocated, accessing the reference can cause unexpected behavior
	/// or application crashes.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetUnsafeReference<T>(this UIntPtr uptr) where T : unmanaged
		=> ref Unsafe.AsRef<T>(uptr.ToPointer());
	/// <summary>
	/// Creates a read-only memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/>
	/// from an <see cref="IntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The <see langword="unmanaged"/> value type.</typeparam>
	/// <param name="ptr">The <see cref="IntPtr"/> pointer.</param>
	/// <returns>A read-only memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// The validity and safety of the returned reference depends on the lifetime and validity of the pointer.
	/// If the data the reference represents is moved or deallocated, accessing the reference can cause unexpected behavior
	/// or application crashes.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref readonly T GetUnsafeReadOnlyReference<T>(this IntPtr ptr) where T : unmanaged
		=> ref Unsafe.AsRef<T>(ptr.ToPointer());
	/// <summary>
	/// Creates a read-only memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/>
	/// from an <see cref="UIntPtr"/>.
	/// </summary>
	/// <typeparam name="T">The <see langword="unmanaged"/> value type.</typeparam>
	/// <param name="uptr">The <see cref="UIntPtr"/> pointer.</param>
	/// <returns>A read-only memory reference to an <see langword="unmanaged"/> value of type <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// The validity and safety of the returned reference depends on the lifetime and validity of the pointer.
	/// If the data the reference represents is moved or deallocated, accessing the reference can cause unexpected behavior
	/// or application crashes.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref readonly T GetUnsafeReadOnlyReference<T>(this UIntPtr uptr) where T : unmanaged
		=> ref Unsafe.AsRef<T>(uptr.ToPointer());

	/// <summary>
	/// Creates an <see langword="unmanaged"/> value of type <typeparamref name="T"/> from an <see cref="IntPtr"/> pointer.
	/// </summary>
	/// <typeparam name="T">The <see langword="unmanaged"/> value type.</typeparam>
	/// <param name="ptr">The <see cref="IntPtr"/> pointer.</param>
	/// <returns>An <see langword="unmanaged"/> value of type <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? GetUnsafeValue<T>(this IntPtr ptr) where T : unmanaged 
		=> ptr.IsZero() ? default(T?) : ptr.GetUnsafeReadOnlyReference<T>();
	/// <summary>
	/// Creates an <see langword="unmanaged"/> value of type <typeparamref name="T"/> from an <see cref="UIntPtr"/> pointer.
	/// </summary>
	/// <typeparam name="T">The <see langword="unmanaged"/> value type.</typeparam>
	/// <param name="uptr">The <see cref="UIntPtr"/> pointer.</param>
	/// <returns>An <see langword="unmanaged"/> value of type <typeparamref name="T"/>.</returns>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T? GetUnsafeValue<T>(this UIntPtr uptr) where T : unmanaged 
		=> uptr.IsZero() ? default(T?) : uptr.GetUnsafeReadOnlyReference<T>();

	/// <summary>
	/// Creates a <see cref="String"/> instance from a <see cref="Char"/> pointer, interpreting the contents as UTF-16 text.
	/// </summary>
	/// <param name="chrPtr">A pointer to the first character of the UTF-16 text in memory.</param>
	/// <param name="length">
	/// The number of <see cref="Char"/> elements to include in the resulting string,
	/// starting from the pointer. If this value is zero, the function reads until the first null character.
	/// </param>
	/// <returns>A <see cref="String"/> representation of the UTF-16 text in memory.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if length is less than zero.</exception>
	/// <remarks>
	/// The validity and safety of the obtained information depends on the lifetime and validity of the pointer at the time
	/// of method invocation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static String GetStringFromCharPointer(Char* chrPtr, Int32 length)
		=> length == default ? new String(chrPtr) : new(new ReadOnlySpan<Char>(chrPtr, length));
}