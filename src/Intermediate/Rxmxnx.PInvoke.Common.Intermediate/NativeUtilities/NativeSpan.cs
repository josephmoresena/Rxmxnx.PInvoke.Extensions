#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
namespace Rxmxnx.PInvoke;

public partial class NativeUtilities
{
	/// <summary>
	/// Creates a <see cref="ReadOnlySpan{Byte}"/> from an exising read-only reference to a
	/// <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
	/// </summary>
	/// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
	/// <param name="value">A read-only reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
	/// <returns>
	/// A <see cref="ReadOnlySpan{Byte}"/> from an exising memory reference to a <typeparamref name="TSource"/>
	/// <see langword="unmanaged"/> value.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<Byte> AsBytes<TSource>(in TSource value) where TSource : unmanaged
	{
		ref TSource refValue = ref Unsafe.AsRef(in value);
		ReadOnlySpan<TSource> span = MemoryMarshal.CreateReadOnlySpan(ref refValue, 1);
		return MemoryMarshal.AsBytes(span);
	}
	/// <summary>
	/// Creates a <see cref="Span{Byte}"/> from an exising reference to a
	/// <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
	/// </summary>
	/// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
	/// <param name="refValue">A read-only reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
	/// <returns>
	/// A <see cref="ReadOnlySpan{Byte}"/> from an exising memory reference to a <typeparamref name="TSource"/>
	/// <see langword="unmanaged"/> value.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<Byte> AsBinarySpan<TSource>(ref TSource refValue) where TSource : unmanaged
	{
		Span<TSource> span = MemoryMarshal.CreateSpan(ref refValue, 1);
		return MemoryMarshal.AsBytes(span);
	}
	/// <summary>
	/// Creates a new <typeparamref name="T"/> array with a specific length and initializes it after
	/// creation by using the specified callback.
	/// </summary>
	/// <typeparam name="T">A type of elements in the array.</typeparam>
	/// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
	/// <param name="length">The length of the array to create.</param>
	/// <param name="state">The element to pass to <paramref name="action"/>.</param>
	/// <param name="action">A callback to initialize the array.</param>
	/// <returns>The created array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T[] CreateArray<T, TState>(Int32 length, TState state, SpanAction<T, TState> action)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		T[] result = new T[length];
		Span<T> span = result;
		NativeUtilities.WriteSpan(span, state, action);
		return result;
	}
}
#endif