#if !NETCOREAPP
using RuneCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuneCompat;
using Rune = System.UInt32;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Unescape text utilities.
/// </summary>
internal static class TextUnescape
{
	/// <summary>
	/// Unescapes the UTF-8 string in the buffer.
	/// </summary>
	/// <param name="buffer">A UTF-8 unescaped buffer.</param>
	/// <returns>Number of bytes of escape adjustment.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 Unescape(Span<Byte> buffer)
	{
		Int32 adjustment = 0;
		Int32 slashIdx;
		Span<Byte> eBuffer = buffer;
		while ((slashIdx = eBuffer.LastIndexOf((Byte)'\\')) != -1)
		{
			if (slashIdx > 0 && eBuffer[slashIdx - 1] == (Byte)'\\')
				// The backslash is escaped, we skip it.
				slashIdx--;
			else if (slashIdx + 1 == eBuffer.Length)
				// The last character is a backslash, we cannot unescape it.
				break;

			switch (eBuffer[slashIdx + 1])
			{
				case (Byte)'n':
				case (Byte)'r':
				case (Byte)'t':
				case (Byte)'b':
				case (Byte)'f':
				case (Byte)'0':
				case (Byte)'\\':
				case (Byte)'/':
				case (Byte)'"':
					TextUnescape.UnescapeUnit(buffer[slashIdx..]);
					// Only a UTF-8 unit is removed.
					adjustment += 1;
					buffer = buffer[..^1];
					break;
				case (Byte)'u':
				case (Byte)'U':
					adjustment += TextUnescape.UnescapeUnicode(ref buffer, ref slashIdx);
					break;
			}
			eBuffer = eBuffer[..slashIdx];
		}
		return adjustment;
	}

	/// <summary>
	/// Unescapes a UTF-8 unit in the buffer.
	/// </summary>
	/// <param name="buffer">A UTF-8 unescaped buffer.</param>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
	private static void UnescapeUnit(Span<Byte> buffer)
	{
		ReadOnlySpan<Byte> unescaped = buffer[2..];
		buffer[0] = buffer[1] switch
		{
			(Byte)'n' => (Byte)'\n',
			(Byte)'r' => (Byte)'\r',
			(Byte)'t' => (Byte)'\t',
			(Byte)'b' => (Byte)'\b',
			(Byte)'f' => (Byte)'\f',
			(Byte)'0' => (Byte)'\0',
			_ => buffer[1],
		};
		unescaped.CopyTo(buffer[1..]); // Copy the rest of the buffer after the replacement.
	}
	/// <summary>
	/// Unescapes a Unicode character in the buffer.
	/// </summary>
	/// <param name="escapedBuffer">Reference. A UTF-8 unescaped buffer.</param>
	/// <param name="escapeIndex">Reference. Index of escape begin.</param>
	/// <returns>The number of bytes that were not replaced.</returns>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
	private static Int32 UnescapeUnicode(ref Span<Byte> escapedBuffer, ref Int32 escapeIndex)
	{
		Int32 stackConsumed = 0;
		Byte[]? byteArray = default;
		try
		{
			Char low = TextUnescape.GetUnicodeChar(escapedBuffer.Slice(escapeIndex + 2, 4));
			Int32 baseLength = 6; // Length of "\uXXXX" sequence.
			ReadOnlySpan<Byte> unescaped = escapedBuffer[(escapeIndex + baseLength)..];
			Rune rune = TextUnescape.GetUnescapeRune(escapedBuffer, ref escapeIndex, low, ref baseLength);
#if NETCOREAPP
			Int32 nBytes = rune.EncodeToUtf8(escapedBuffer[escapeIndex..]);
#else
			Int32 nBytes = RuneCompat.EncodeToUtf8(rune, escapedBuffer[escapeIndex..]);
#endif
			Int32 offset = escapeIndex + nBytes;
			Int32 result = baseLength - nBytes;

			unescaped.CopyTo(escapedBuffer[offset..]);
			escapedBuffer = escapedBuffer[..^result];
			return result;
		}
		finally
		{
			StackAllocationHelper.ReturnArray(byteArray);
			StackAllocationHelper.ReleaseStackBytes(stackConsumed);
		}
	}
	/// <summary>
	/// Retrieves the unescape rune from the buffer.
	/// </summary>
	/// <param name="escapedBuffer">A UTF-8 escaped buffer.</param>
	/// <param name="escapeIndex">Index of escape begin.</param>
	/// <param name="lowChar">Low surrogate char.</param>
	/// <param name="escapeSize">Escape size in bytes.</param>
	/// <returns>The escape rune from the buffer.</returns>
	private static Rune GetUnescapeRune(Span<Byte> escapedBuffer, ref Int32 escapeIndex, Char lowChar,
		ref Int32 escapeSize)
	{
		Rune rune;
		if (!Char.IsLowSurrogate(lowChar) || !TextUnescape.HasHighSurrogate(escapedBuffer, escapeIndex, out Char high))
		{
#if NETCOREAPP
			rune = new(lowChar);
		}
		else
		{
			rune = new(high, lowChar);
#else
			rune = lowChar;
		}
		else
		{
			rune = (Rune)Char.ConvertToUtf32(high, lowChar);
#endif
			escapeIndex -= 6; // Adjust for "\uXXXX" prefix.
			escapeSize *= 2; // Double the size for surrogate pairs.
		}
		return rune;
	}
	/// <summary>
	/// Indicates whether the buffer has a high surrogate character at the specified escape index.
	/// </summary>
	/// <param name="escapedBuffer">A UTF-8 escaped buffer.</param>
	/// <param name="escapeIndex">Index of escape begin.</param>
	/// <param name="high">Output. High surrogate character.</param>
	/// <returns>
	/// <see langword="true"/> if the buffer has a high surrogate character at the specified escape index;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	private static Boolean HasHighSurrogate(Span<Byte> escapedBuffer, Int32 escapeIndex, out Char high)
	{
		Boolean hasHighSurrogate = escapedBuffer.Length - escapeIndex >= 6 &&
			escapedBuffer.Slice(escapeIndex - 6, 2).SequenceEqual(CString.UnicodePrefix()); // Check for "\u" prefix.
		high = TextUnescape.GetUnicodeChar(escapedBuffer.Slice(escapeIndex - 4, 4));
		return hasHighSurrogate;
	}
	/// <summary>
	/// Retrieves a Unicode character from the specified span of bytes.
	/// </summary>
	/// <param name="charCodeSpan">A UTF-8 span containing the hexadecimal value of a UTF-16 char.</param>
	/// <returns>A Unicode char.</returns>
	private static Char GetUnicodeChar(ReadOnlySpan<Byte> charCodeSpan)
	{
		UInt16 result = 0;
		unchecked
		{
			foreach (Byte t in charCodeSpan)
			{
				result <<= 4;
				result += NativeUtilities.GetDecimalValue(t);
			}
			return (Char)result;
		}
	}
}