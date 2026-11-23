// ReSharper disable ConvertToExtensionBlock

namespace Rxmxnx.PInvoke.VisualBasic;

/// <summary>
/// Visual Basic .NET span extensions.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class ByRefExtensions
{
	/// <summary>
	/// Returns the value pointed by the read-only value pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="valPtr">The current read-only value pointer.</param>
	/// <returns>The value pointed by the read-only value pointer.</returns>
	/// <exception cref="NullReferenceException">Thrown when the pointer is <see langword="null"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T GetUnsafe<T>(this ReadOnlyValPtr<T> valPtr)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> valPtr.Reference;
	/// <summary>
	/// Returns the value pointed by the value pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="valPtr">The current value pointer.</param>
	/// <returns>The value pointed by the value pointer.</returns>
	/// <exception cref="NullReferenceException">Thrown when the pointer is <see langword="null"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T GetUnsafe<T>(this ValPtr<T> valPtr)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> valPtr.Reference;
	/// <summary>
	/// Returns the first <typeparamref name="T"/> value in <paramref name="item"/>.
	/// </summary>
	/// <typeparam name="T">Type of the value to read.</typeparam>
	/// <param name="item">The fixed memory block.</param>
	/// <returns>The first <typeparamref name="T"/> value in the block.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when <paramref name="item"/> cannot be read as <typeparamref name="T"/>.
	/// </exception>
	/// <exception cref="NullReferenceException">
	/// Thrown when <paramref name="item"/> cannot be read as <typeparamref name="T"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T GetUnsafe<T>(this FixedPointerValueList.ItemValue item)
	{
		ref readonly T result = ref Unsafe.NullRef<T>();
		if (item.Value.IsValidValueTransformation<T>())
			result = ref ((ReadOnlyFixedReferenceValue<T>)item.Value).Reference;
		return result;
	}
	/// <summary>
	/// Returns the specified element of the read-only span.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="ReadOnlySpan{T}"/>.</typeparam>
	/// <param name="span">The current read-only span.</param>
	/// <param name="index">The zero-based index.</param>
	/// <returns>The element for <paramref name="index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when index less than 0 or index greater than or equal to Length
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T GetUnsafe<T>(this ReadOnlySpan<T> span, Int32 index) => span[index];
	/// <summary>
	/// Returns the specified element of the span.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="Span{T}"/>.</typeparam>
	/// <param name="span">The current span.</param>
	/// <param name="index">The zero-based index.</param>
	/// <returns>The element for <paramref name="index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when index less than 0 or index greater than or equal to Length
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T GetUnsafe<T>(this Span<T> span, Int32 index) => span[index];
	/// <summary>
	/// Sets <paramref name="value"/> as the value pointed by the value pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="valPtr">The current value pointer.</param>
	/// <param name="value">The value to set to.</param>
	/// <exception cref="NullReferenceException">Thrown when the pointer is <see langword="null"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T SetUnsafe<T>(this ValPtr<T> valPtr, T value)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> valPtr.Reference = value;
	/// <summary>
	/// Sets <paramref name="value"/> as the first <typeparamref name="T"/> value in <paramref name="item"/>.
	/// </summary>
	/// <typeparam name="T">Type of the value to write.</typeparam>
	/// <param name="item">The fixed memory block.</param>
	/// <param name="value">The value to set to.</param>
	/// <returns>The value written.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the block is read-only.</exception>
	/// <exception cref="NullReferenceException">
	/// Thrown when <paramref name="item"/> cannot be written as <typeparamref name="T"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T SetUnsafe<T>(this FixedPointerValueList.ItemValue item, T value)
	{
		ref T result = ref Unsafe.NullRef<T>();
		if (item.Value.IsValidValueTransformation<T>(false))
			result = ref ((FixedReferenceValue<T>)item.Value).Reference;
		return result = value;
	}
	/// <summary>
	/// Sets <paramref name="value"/> as the value of the specified element of the span.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="Span{T}"/>.</typeparam>
	/// <param name="span">The current span.</param>
	/// <param name="index">The zero-based index.</param>
	/// <param name="value">The value to set to.</param>
	/// <returns>The element for <paramref name="index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when index less than 0 or index greater than or equal to Length
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T SetUnsafe<T>(this Span<T> span, Int32 index, T value) => span[index] = value;
	/// <summary>
	/// Copies <paramref name="source"/> into the memory pointed by <paramref name="destination"/>.
	/// </summary>
	/// <typeparam name="T">The type of the elements.</typeparam>
	/// <param name="source">The source read-only span.</param>
	/// <param name="destination">The destination pointer.</param>
	/// <remarks>
	/// Unsafe because <paramref name="destination"/> is not validated for size or availability.
	/// </remarks>
	/// <exception cref="NullReferenceException">
	/// Thrown when <paramref name="destination"/> is <see langword="null"/> and <paramref name="source"/> is not empty.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void UnsafeCopyTo<T>(this ReadOnlySpan<T> source, ValPtr<T> destination)
	{
		if (source.IsEmpty) return;
#pragma warning disable CS8500
		fixed (void* ptr = &destination.Reference)
			source.CopyTo(MemoryMarshalCompat.CreateUnsafeSpan<T>(ptr, source.Length));
#pragma warning restore CS8500
	}
	/// <summary>
	/// Copies <paramref name="source"/> into the memory pointed by <paramref name="destination"/>.
	/// </summary>
	/// <typeparam name="T">The type of the elements.</typeparam>
	/// <param name="source">The source span.</param>
	/// <param name="destination">The destination pointer.</param>
	/// <remarks>
	/// Unsafe because <paramref name="destination"/> is not validated for size or availability.
	/// </remarks>
	/// <exception cref="NullReferenceException">
	/// Thrown when <paramref name="destination"/> is <see langword="null"/> and <paramref name="source"/> is not empty.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void UnsafeCopyTo<T>(this Span<T> source, ValPtr<T> destination)
		=> ((ReadOnlySpan<T>)source).UnsafeCopyTo(destination);
	/// <summary>
	/// Copies the memory pointed by <paramref name="source"/> into <paramref name="destination"/>.
	/// </summary>
	/// <typeparam name="T">The type of the elements.</typeparam>
	/// <param name="destination">The destination span.</param>
	/// <param name="source">The source pointer.</param>
	/// <remarks>
	/// Unsafe because <paramref name="source"/> is not validated for size or availability.
	/// </remarks>
	/// <exception cref="NullReferenceException">
	/// Thrown when <paramref name="source"/> is <see langword="null"/> and <paramref name="destination"/> is not empty.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void UnsafeCopyFrom<T>(this Span<T> destination, ReadOnlyValPtr<T> source)
	{
		if (destination.IsEmpty) return;
#pragma warning disable CS8500
		fixed (void* ptr = &source.Reference)
			MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(ptr, destination.Length).CopyTo(destination);
#pragma warning restore CS8500
	}
	/// <summary>
	/// Returns a read-only span over the memory described by <paramref name="item"/>.
	/// </summary>
	/// <typeparam name="T">Type of the span elements.</typeparam>
	/// <param name="item">The fixed memory block.</param>
	/// <returns>
	/// A read-only span over the memory described by <paramref name="item"/>, or empty if the operation is not allowed.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpan<T> GetReadOnlySpan<T>(this FixedPointerValueList.ItemValue item)
		=> item.Value.IsValidTransformation<T>() ? ((ReadOnlyFixedContextValue<T>)item.Value).Values : default;
	/// <summary>
	/// Returns a span over the memory described by <paramref name="item"/>.
	/// </summary>
	/// <typeparam name="T">Type of the span elements.</typeparam>
	/// <param name="item">The fixed memory block.</param>
	/// <returns>
	/// A span over the memory described by <paramref name="item"/>, or empty if the operation is not allowed.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> GetSpan<T>(this FixedPointerValueList.ItemValue item)
		=> item.Value.IsValidTransformation<T>(false) ? ((FixedContextValue<T>)item.Value).Values : default;
}