namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="ValueType"/> <see langword="unmanaged"/> values.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class UnmanagedValueExtensions
{
	/// <summary>
	/// Converts a given <see langword="unmanaged"/> value of type <typeparamref name="T"/> into an array of <see cref="Byte"/>
	/// .
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
		if (!array.Any())
			return Array.Empty<Byte>();

		return array.AsSpan().AsBytes().ToArray();
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
		if (!array.Any())
			return Array.Empty<TDestination>();

		return array.AsSpan().AsValues<TSource, TDestination>().ToArray();
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
		if (!array.Any())
		{
			residual = Array.Empty<Byte>();
			return Array.Empty<TDestination>();
		}

		TDestination[] result = array.AsSpan().AsValues<TSource, TDestination>(out Span<Byte> rSpan).ToArray();
		residual = rSpan.ToArray();
		return result;
	}
}