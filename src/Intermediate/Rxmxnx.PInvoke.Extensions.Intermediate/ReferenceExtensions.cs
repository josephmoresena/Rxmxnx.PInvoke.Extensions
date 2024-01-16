namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with references to <see cref="ValueType"/>
/// <see langword="unmanaged"/> values.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
[SuppressMessage("csharpsquid", "S6640")]
public static unsafe partial class ReferenceExtensions
{
	/// <summary>
	/// Retrieves an unsafe pointer of type <see cref="ValPtr{T}"/> from a reference to an
	/// <see langword="unmanaged"/> value of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The type of value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <param name="refValue">The reference to the value from which to retrieve the pointer.</param>
	/// <returns>An unsafe pointer of type <see cref="ValPtr{T}"/> pointing to the referenced value.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValPtr<T> GetUnsafeValPtr<T>(ref this T refValue) where T : unmanaged
	{
		void* ptr = Unsafe.AsPointer(ref refValue);
		return new(ptr);
	}
	/// <summary>
	/// Retrieves an unsafe pointer of type <see cref="IntPtr"/> from a reference to an
	/// <see langword="unmanaged"/> value of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The type of value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <param name="refValue">The reference to the value from which to retrieve the pointer.</param>
	/// <returns>An unsafe pointer of type <see cref="IntPtr"/> pointing to the referenced value.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr GetUnsafeIntPtr<T>(ref this T refValue) where T : unmanaged
	{
		void* ptr = Unsafe.AsPointer(ref refValue);
		return (IntPtr)ptr;
	}
	/// <summary>
	/// Retrieves an unsafe pointer of type <see cref="UIntPtr"/> from a reference to an
	/// <see langword="unmanaged"/> value of type <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The type of value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <param name="refValue">The reference to the value from which to retrieve the pointer.</param>
	/// <returns>An unsafe pointer of type <see cref="UIntPtr"/> pointing to the referenced value.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIntPtr GetUnsafeUIntPtr<T>(ref this T refValue) where T : unmanaged
	{
		void* ptr = Unsafe.AsPointer(ref refValue);
		return (UIntPtr)ptr;
	}
	/// <summary>
	/// Creates a reference to an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>
	/// from an existing reference to an <see langword="unmanaged"/> value of type <typeparamref name="TSource"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the source value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <typeparam name="TDestination">
	/// The type of the destination value to which to create a reference. This must be an <see langword="unmanaged"/> value
	/// type.
	/// </typeparam>
	/// <param name="refValue">
	/// The reference to the source value from which to create the destination reference.
	/// </param>
	/// <returns>A reference to an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when <typeparamref name="TSource"/> and <typeparamref name="TDestination"/> do not have the same memory size.
	/// </exception>
	/// <remarks>
	/// This transformation can be performed between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>
	/// types
	/// that have the same size in memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref TDestination Transform<TSource, TDestination>(this ref TSource refValue)
		where TSource : unmanaged where TDestination : unmanaged
		=> ref NativeUtilities.TransformReference<TSource, TDestination>(ref refValue);
	/// <summary>
	/// Creates a <see cref="Span{Byte}"/> from a reference to an <see langword="unmanaged"/> value of
	/// type <typeparamref name="TSource"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <param name="refValue">The reference to the value from which to create the <see cref="Span{Byte}"/>.</param>
	/// <returns>A <see cref="Span{Byte}"/> that represents the referenced <see langword="unmanaged"/> value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<Byte> AsBytes<TSource>(this ref TSource refValue) where TSource : unmanaged
		=> NativeUtilities.AsBinarySpan(ref refValue);
}