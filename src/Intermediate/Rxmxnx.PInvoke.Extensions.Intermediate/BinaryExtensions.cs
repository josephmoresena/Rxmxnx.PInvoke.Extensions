// ReSharper disable ConvertToExtensionBlock

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Byte"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe partial class BinaryExtensions
{
	/// <summary>
	/// Retrieves a <typeparamref name="T"/> value from the given byte array.
	/// </summary>
	/// <typeparam name="T">The type of the value to be retrieved.</typeparam>
	/// <param name="array">The source byte array.</param>
	/// <returns>The <typeparamref name="T"/> value read from the array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ToValue<T>(this Byte[] array) where T : unmanaged => array.AsSpan().ToValue<T>();
	/// <summary>
	/// Retrieves a <typeparamref name="T"/> value from the given byte span.
	/// </summary>
	/// <typeparam name="T">The type of the value to be retrieved.</typeparam>
	/// <param name="span">The source byte span.</param>
	/// <returns>The <typeparamref name="T"/> value read from the span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ToValue<T>(this Span<Byte> span) where T : unmanaged => ((ReadOnlySpan<Byte>)span).ToValue<T>();
	/// <summary>
	/// Retrieves a <typeparamref name="T"/> value from the given read-only byte span.
	/// </summary>
	/// <typeparam name="T">The type of the value to be retrieved.</typeparam>
	/// <param name="span">The source read-only byte span.</param>
	/// <returns>The <typeparamref name="T"/> value read from the span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T ToValue<T>(this ReadOnlySpan<Byte> span) where T : unmanaged
	{
		Span<Byte> result = stackalloc Byte[sizeof(T)];
		Int32 bytesToCopy = Math.Min(result.Length, span.Length);

		span[..bytesToCopy].CopyTo(result);
		return MemoryMarshal.Read<T>(result);
	}
	/// <summary>
	/// Retrieves a read-only reference to a <typeparamref name="T"/> value from the given read-only byte span.
	/// </summary>
	/// <typeparam name="T">The type of the value to be referenced.</typeparam>
	/// <param name="span">The source read-only byte span.</param>
	/// <returns>A read-only reference to the <typeparamref name="T"/> value.</returns>
	/// <exception cref="InsufficientMemoryException">
	/// Thrown if the size of the binary span is less than the size of the type <typeparamref name="T"/>.
	/// </exception>
	/// <exception cref="InvalidCastException">
	/// Thrown if the size of the binary span is greater than the size of the type <typeparamref name="T"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref readonly T AsValue<T>(this ReadOnlySpan<Byte> span) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidBinarySpanSize(span, sizeof(T));
		return ref MemoryMarshal.Cast<Byte, T>(span)[0];
	}
	/// <summary>
	/// Retrieves a reference to a <typeparamref name="T"/> value from the given byte span.
	/// </summary>
	/// <typeparam name="T">The type of the value to be referenced.</typeparam>
	/// <param name="span">The source byte span.</param>
	/// <returns>A reference to the <typeparamref name="T"/> value.</returns>
	/// <exception cref="InsufficientMemoryException">
	/// Thrown if the size of the binary span is less than the size of the type <typeparamref name="T"/>.
	/// </exception>
	/// <exception cref="InvalidCastException">
	/// Thrown if the size of the binary span is greater than the size of the type <typeparamref name="T"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T AsValue<T>(this Span<Byte> span) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidBinarySpanSize(span, sizeof(T));
		return ref MemoryMarshal.Cast<Byte, T>(span)[0];
	}

	/// <summary>
	/// Gets the hexadecimal string representation of a byte array.
	/// </summary>
	/// <param name="bytes">The source byte array.</param>
	/// <returns>The hexadecimal string representation of the byte array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String AsHexString(this Byte[] bytes) => String.Concat(bytes.Select(b => b.AsHexString()));
	/// <summary>
	/// Gets the hexadecimal string representation of a byte.
	/// </summary>
	/// <param name="value">The source byte.</param>
	/// <returns>The hexadecimal string representation of the byte.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String AsHexString(this Byte value) => value.ToString("X2").ToLowerInvariant();
}