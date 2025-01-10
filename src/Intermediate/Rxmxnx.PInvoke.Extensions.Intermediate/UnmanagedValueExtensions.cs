namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see langword="unmanaged"/> values.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public static partial class UnmanagedValueExtensions
{
	/// <summary>
	/// Rents and pins an array of minimum <paramref name="count"/> elements from <paramref name="arrayPool"/>,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">
	/// The unmanaged type from which the contiguous region of memory will be fixed.
	/// </typeparam>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="count">Minimum size of rented array.</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
	[ExcludeFromCodeCoverage]
	public static IFixedContext<T>.IDisposable RentFixed<T>(this ArrayPool<T> arrayPool, Int32 count,
		Boolean clearArray = false) where T : unmanaged
		=> arrayPool.RentFixed(count, clearArray, out _);
	/// <summary>
	/// Rents and pins an array of minimum <paramref name="count"/> elements from <paramref name="arrayPool"/>,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">
	/// The unmanaged type from which the contiguous region of memory will be fixed.
	/// </typeparam>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="count">Minimum size of rented array.</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <param name="arrayLength">Output. Rented array length.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
	public static IFixedContext<T>.IDisposable RentFixed<T>(this ArrayPool<T> arrayPool, Int32 count,
		Boolean clearArray, out Int32 arrayLength) where T : unmanaged
	{
		FixedRentedContext<T> result = new(arrayPool, count, clearArray);
		arrayLength = result.Array.Length;
		return result;
	}

	/// <summary>
	/// Converts a given <see langword="unmanaged"/> value of type <typeparamref name="T"/> into an array of
	/// <see cref="Byte"/>.
	/// </summary>
	/// <typeparam name="T">The type of value. This must be an <see langword="unmanaged"/> value type.</typeparam>
	/// <param name="value">The value to convert.</param>
	/// <returns>An array of bytes that represent the input value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Byte[] ToBytes<T>(this T value) where T : unmanaged => NativeUtilities.ToBytes(value);
	/// <summary>
	/// Converts an array of <see langword="unmanaged"/> values of type <typeparamref name="TSource"/> into an array
	/// of <see cref="Byte"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of values in the input array. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <param name="array">The array of values to convert.</param>
	/// <returns>An array of bytes that represent the input array of values.</returns>
	/// <remarks>
	/// If the input array is <see langword="null"/>, the method returns <see langword="null"/>.
	/// If the input array is empty, the method returns an empty array.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNullIfNotNull(nameof(array))]
	public static Byte[]? ToBytes<TSource>(this TSource[]? array) where TSource : unmanaged
	{
		if (array is null)
			return default;
		return array.Length > 0 ? array.AsSpan().AsBytes().ToArray() : [];
	}

	/// <summary>
	/// Converts an array of <see langword="unmanaged"/> values of type <typeparamref name="TSource"/> into an array
	/// of another <see langword="unmanaged"/> value type <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TDestination">
	/// The destination type for the conversion. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <typeparam name="TSource">
	/// The type of values in the input array. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <param name="array">The array of values to convert.</param>
	/// <returns>
	/// An array of values of type <typeparamref name="TDestination"/> that represent the input array of values.
	/// </returns>
	/// <remarks>
	/// If the input array is <see langword="null"/>, the method returns <see langword="null"/>.
	/// If the input array is empty, the method returns an empty array.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNullIfNotNull(nameof(array))]
	public static TDestination[]? ToValues<TSource, TDestination>(this TSource[]? array)
		where TSource : unmanaged where TDestination : unmanaged
	{
		if (array is null)
			return default;
		return array.Length > 0 ? array.AsSpan().AsValues<TSource, TDestination>().ToArray() : [];
	}
	/// <summary>
	/// Converts an array of <see langword="unmanaged"/> values of type <typeparamref name="TSource"/> into an array
	/// of another <see langword="unmanaged"/> value type <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TDestination">
	/// The destination type for the conversion. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <typeparam name="TSource">
	/// The type of values in the input array. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <param name="array">The array of values to convert.</param>
	/// <param name="residual">The residual binary array of the reinterpretation.</param>
	/// <returns>
	/// An array of values of type <typeparamref name="TDestination"/> that represent the input array of values.
	/// </returns>
	/// <remarks>
	/// If the input array is <see langword="null"/>, the method returns <see langword="null"/> and sets
	/// <paramref name="residual"/> to <see langword="null"/>.
	/// If the input array is empty, the method returns an empty array and sets
	/// <paramref name="residual"/> to an empty array.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[return: NotNullIfNotNull(nameof(array))]
	public static TDestination[]? ToValues<TSource, TDestination>(this TSource[]? array,
		[NotNullIfNotNull(nameof(array))] out Byte[]? residual) where TSource : unmanaged where TDestination : unmanaged
	{
		if (array is null)
		{
			residual = default;
			return default;
		}

		if (array.Length <= 0)
		{
			residual = [];
			return [];
		}

		TDestination[] result = array.AsSpan().AsValues<TSource, TDestination>(out Span<Byte> rSpan).ToArray();
		residual = rSpan.ToArray();
		return result;
	}
}