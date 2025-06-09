#if !PACKAGE || !NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="Convert"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static class ConvertCompat
{
	/// <summary>
	/// Converts a span of 8-bit unsigned integers to its equivalent string representation that is
	/// encoded with uppercase hex characters.
	/// </summary>
	/// <param name="bytes">A span of 8-bit unsigned integers.</param>
	/// <returns>The string representation in hex of the elements in <paramref name="bytes"/>.</returns>
	public static unsafe String ToHexString(ReadOnlySpan<Byte> bytes)
	{
		fixed (Byte* bytesPtr = &MemoryMarshal.GetReference(bytes))
		{
			return String.Create(bytes.Length * 2, (Ptr: (IntPtr)bytesPtr, bytes.Length), static (chars, args) =>
			{
				ReadOnlySpan<Byte> ros = new((Byte*)args.Ptr, args.Length);
				for (Int32 pos = 0; pos < args.Length; ++pos)
					ConvertCompat.ToCharsBuffer(ros[pos], chars, pos * 2);
			});
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ToCharsBuffer(Byte value, Span<Char> buffer, Int32 startingIndex = 0)
	{
		UInt32 difference = ((value & 0xF0U) << 4) + (value & 0x0FU) - 0x8989U;
		UInt32 packedResult = (((UInt32)(-(Int32)difference) & 0x7070U) >> 4) + difference + 0xB9B9U;

		buffer[startingIndex + 1] = (Char)(packedResult & 0xFF);
		buffer[startingIndex] = (Char)(packedResult >> 8);
	}
}
#endif