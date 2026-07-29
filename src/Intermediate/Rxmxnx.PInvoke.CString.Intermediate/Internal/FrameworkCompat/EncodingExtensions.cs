#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Extensions for <see cref="Encoding"/> class.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe class EncodingExtensions
{
	/// <summary>
	/// When overridden in a derived class, calculates the number of bytes produced by encoding the characters in the
	/// specified character span.
	/// </summary>
	/// <param name="enc">A <see cref="Encoding"/> instance.</param>
	/// <param name="chars">The span of characters to encode.</param>
	/// <returns>The number of bytes produced by encoding the specified character span.</returns>
	public static Int32 GetByteCount(this Encoding enc, ReadOnlySpan<Char> chars)
	{
		fixed (Char* ptr = &MemoryMarshal.GetReference(chars))
			return enc.GetByteCount(ptr, chars.Length);
	}
	/// <summary>
	/// When overridden in a derived class, calculates the number of characters produced by decoding the provided
	/// read-only byte span.
	/// </summary>
	/// <param name="enc">A <see cref="Encoding"/> instance.</param>
	/// <param name="bytes">A read-only byte span to decode.</param>
	/// <returns>The number of characters produced by decoding the byte span.</returns>
	// ReSharper disable once ConvertToExtensionBlock
	public static Int32 GetCharCount(this Encoding enc, ReadOnlySpan<Byte> bytes)
	{
		fixed (Byte* ptr = &MemoryMarshal.GetReference(bytes))
			return enc.GetCharCount(ptr, bytes.Length);
	}
	/// <summary>
	/// When overridden in a derived class, decodes all the bytes in the specified byte span into a string.
	/// </summary>
	/// <param name="enc">A <see cref="Encoding"/> instance.</param>
	/// <param name="bytes">A read-only byte span to decode to a Unicode string.</param>
	/// <returns>A string that contains the decoded bytes from the provided read-only span.</returns>
	public static String GetString(this Encoding enc, ReadOnlySpan<Byte> bytes)
	{
		fixed (Byte* ptr = &MemoryMarshal.GetReference(bytes))
			return enc.GetString(ptr, bytes.Length);
	}
}
#endif