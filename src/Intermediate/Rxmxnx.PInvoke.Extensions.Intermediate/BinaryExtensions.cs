// ReSharper disable ConvertToExtensionBlock

#if !NET6_0_OR_GREATER
using ArgumentNullExceptionCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="Byte"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#if NETSTANDARD2_1 || NETCOREAPP
public static unsafe partial class BinaryExtensions
#else
public static unsafe class BinaryExtensions
#endif
{
	/// <summary>
	/// Map of hexadecimal values.
	/// </summary>
	private const String hexValues = "0123456789abcdef";

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
		if (span.IsEmpty) return default;
		ref Byte refByte = ref MemoryMarshal.GetReference(span);
		if (span.Length >= sizeof(T))
			return Unsafe.ReadUnaligned<T>(ref refByte);

		T result = default;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		Span<Byte> resultBytes = MemoryMarshal.CreateSpan(ref Unsafe.As<T, Byte>(ref result), span.Length);
#else
		Span<Byte> resultBytes = new(Unsafe.AsPointer(ref result), span.Length);
#endif
		span.CopyTo(resultBytes);
		return result;
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
	public static String AsHexString(this Byte[] bytes)
	{
#if !NET6_0_OR_GREATER
		ArgumentNullExceptionCompat.ThrowIfNull(bytes);
#else
		ArgumentNullException.ThrowIfNull(bytes);
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		return bytes.Length > 0 ? String.Create(bytes.Length * 2, bytes, BinaryExtensions.CopyHexChars) : String.Empty;
#else
		if (bytes.Length <= 0) return String.Empty;
		Int32 stringLength = bytes.Length * 2;
		Span<Char> chars = stringLength <= StackAllocationHelper.StackallocByteThreshold ?
			stackalloc Char[stringLength] :
			new Char[stringLength];
		BinaryExtensions.CopyHexChars(chars, bytes);
		return chars.ToString();
#endif
	}
	/// <summary>
	/// Gets the hexadecimal string representation of a byte.
	/// </summary>
	/// <param name="value">The source byte.</param>
	/// <returns>The hexadecimal string representation of the byte.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String AsHexString(this Byte value)
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> String.Create(2, value, BinaryExtensions.CopyHexChars);
#else
	{
		Span<Char> chars = stackalloc Char[2];
		BinaryExtensions.CopyHexChars(chars, value);
		return chars.ToString();
	}
#endif

	/// <summary>
	/// Copies to <paramref name="chars"/> the Hexadecimal representation of <paramref name="value"/>.
	/// </summary>
	/// <param name="chars">Destination character span.</param>
	/// <param name="value">The source byte.</param>
	private static void CopyHexChars(Span<Char> chars, Byte value)
	{
		chars[0] = BinaryExtensions.hexValues[value >> 4];
		chars[1] = BinaryExtensions.hexValues[value & 0x0F];
	}
	/// <summary>
	/// Copies to <paramref name="chars"/> the Hexadecimal representation of <paramref name="bytes"/>.
	/// </summary>
	/// <param name="chars">Destination character span.</param>
	/// <param name="bytes">The source byte array.</param>
	private static void CopyHexChars(Span<Char> chars, Byte[] bytes)
	{
		for (Int32 i = 0; i < bytes.Length; i++)
		{
			Byte value = bytes[i];
			chars[i * 2] = BinaryExtensions.hexValues[value >> 4];
			chars[i * 2 + 1] = BinaryExtensions.hexValues[value & 0x0F];
		}
	}
}