namespace Rxmxnx.PInvoke.VisualBasic;

/// <summary>
/// Visual Basic .NET span extensions.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
public static class ByRefExtensions
{
	/// <summary>
	/// Returns the value pointed by the read-only value pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="valPtr">The current read-only value pointer.</param>
	/// <returns>The value pointed by the read-only value pointer.</returns>
	/// <exception cref="NullReferenceException">Thrown when the pointer is <see langword="null"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Get<T>(this ReadOnlyValPtr<T> valPtr)
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
	public static T Get<T>(this ValPtr<T> valPtr)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> valPtr.Reference;
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
	public static T Get<T>(this ReadOnlySpan<T> span, Int32 index) => span[index];
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
	public static T Get<T>(this Span<T> span, Int32 index) => span[index];
	/// <summary>
	/// Sets <paramref name="value"/> as the value pointed by the value pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="valPtr">The current value pointer.</param>
	/// <param name="value">The value to set to.</param>
	/// <exception cref="NullReferenceException">Thrown when the pointer is <see langword="null"/>.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T Set<T>(this ValPtr<T> valPtr, T value)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> valPtr.Reference = value;
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
	public static T Set<T>(this Span<T> span, Int32 index, T value) => span[index] = value;

	/// <summary>
	/// Obtains an unsafe pointer of type <see cref="ReadOnlySpanPtr{T}"/> from a reference to an
	/// <see cref="ReadOnlySpan{T}"/> value.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="ReadOnlySpan{T}"/>.</typeparam>
	/// <param name="refSpan">The reference to the value from which to retrieve the pointer.</param>
	/// <returns>An unsafe pointer of type <see cref="ReadOnlySpanPtr{T}"/> pointing to the referenced value.</returns>
	/// <remarks>
	/// The obtained pointer is "unsafe" because it does not guarantee that the referenced value will remain in memory or
	/// unaffected by garbage collection.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlySpanPtr<T> GetUnsafeSpanPtr<T>(this ref ReadOnlySpan<T> refSpan) => new(ref refSpan);
	/// <summary>
	/// Obtains an unsafe pointer of type <see cref="SpanPtr{T}"/> from a reference to an <see cref="Span{T}"/> value.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="Span{T}"/>.</typeparam>
	/// <param name="refSpan">The reference to the value from which to retrieve the pointer.</param>
	/// <returns>An unsafe pointer of type <see cref="SpanPtr{T}"/> pointing to the referenced value.</returns>
	/// <remarks>
	/// The obtained pointer is "unsafe" because it does not guarantee that the referenced value will remain in memory or
	/// unaffected by garbage collection.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static SpanPtr<T> GetUnsafeSpanPtr<T>(this ref Span<T> refSpan) => new(ref refSpan);
}