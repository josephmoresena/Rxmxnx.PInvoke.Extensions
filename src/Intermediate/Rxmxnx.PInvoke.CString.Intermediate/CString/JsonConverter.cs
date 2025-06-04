namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Json converter for <see cref="CString"/> class.
	/// </summary>
	public sealed class JsonConverter : JsonConverter<CString>
	{
		/// <inheritdoc/>
		public override CString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			=> JsonConverter.Read(reader);
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, CString? value, JsonSerializerOptions options)
			=> JsonConverter.Write(writer, value, value is null || value.IsZero, options.DefaultIgnoreCondition);

		/// <summary>
		/// Reads and converts the JSON to type <see cref="CString"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>The converted value.</returns>
		public static CString? Read(Utf8JsonReader reader)
		{
			ValidationUtilities.ThrowIfNotString(reader.TokenType);
			if (reader.TokenType is JsonTokenType.Null) return default;

			Boolean isEmpty = (reader.HasValueSequence ? reader.ValueSequence.Length : reader.ValueSpan.Length) <= 0;
			return isEmpty ? CString.Empty : new(reader);
		}
		/// <summary>
		/// Writes a UTF-8 text bytes as JSON string.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="value">UTF-8 text bytes.</param>
		/// <param name="isNull">Indicates whether UTF-8 text is null.</param>
		/// <param name="ignoreCondition">Ignore condition.</param>
		/// <remarks>
		/// <paramref name="isNull"/> flag is ignored if <paramref name="value"/> is not an empty span.
		/// </remarks>
		public static void Write(Utf8JsonWriter writer, ReadOnlySpan<Byte> value, Boolean isNull,
			JsonIgnoreCondition ignoreCondition)
		{
			if (isNull && value.IsEmpty)
				switch (ignoreCondition)
				{
					case JsonIgnoreCondition.WhenWritingNull:
					case JsonIgnoreCondition.WhenWritingDefault:
					case JsonIgnoreCondition.Always:
						break;
					default:
						writer.WriteNullValue();
						return;
				}
			writer.WriteStringValue(value);
		}

		/// <summary>
		/// Retrieves the length of the UTF-8 text bytes from the reader.
		/// </summary>
		/// <param name="reader">A <see cref="Utf8JsonReader"/> instance.</param>
		/// <returns>The length of the UTF-8 text bytes from the reader.</returns>
		internal static Int32 GetLength(Utf8JsonReader reader)
		{
			Boolean isSequence = reader.HasValueSequence;
			checked
			{
				return (Int32)(isSequence ? reader.ValueSequence.Length : reader.ValueSpan.Length);
			}
		}
		/// <summary>
		/// Reads UTF-8 text bytes from the reader into a buffer and returns the adjustment value for text length.
		/// </summary>
		/// <param name="reader">A <see cref="Utf8JsonReader"/> instance.</param>
		/// <param name="buffer">Buffer to write to.</param>
		/// <returns>Adjustment value for text length.</returns>
		internal static Int32 ReadBytes(Utf8JsonReader reader, Span<Byte> buffer)
		{
			Int32 adjustment = 0;
#if NET7_0_OR_GREATER
			Int32 nBytes = reader.CopyString(buffer);
			adjustment -= buffer.Length - nBytes;
			if (nBytes > 0)
				adjustment -= buffer[^nBytes] == 0 ? 1 : 0;
#else
			if (reader.HasValueSequence)
				reader.ValueSequence.CopyTo(buffer);
			else
				reader.ValueSpan.CopyTo(buffer);
			adjustment -= buffer[^1] == 0 ? 1 : 0;
			adjustment -= JsonConverter.EscapeString(buffer);
#endif
			return adjustment;
		}

#if !NET7_0_OR_GREATER
		/// <summary>
		/// Escapes the string in the buffer.
		/// </summary>
		/// <param name="buffer">A UTF-8 unescaped buffer.</param>
		/// <returns>Number of bytes of escape adjustment.</returns>
		private static Int32 EscapeString(Span<Byte> buffer)
		{
			Int32 adjustment = 0;
			Int32 slashIdx;
			Span<Byte> ueBuffer = buffer;
			while ((slashIdx = ueBuffer.LastIndexOf((Byte)'\\')) != -1)
			{
				if (slashIdx > 0 && ueBuffer[slashIdx - 1] == (Byte)'\\')
					// The backslash is escaped, we escape it.
					slashIdx--;
				else if (slashIdx + 1 == ueBuffer.Length)
					// The last character is a backslash, we cannot escape it.
					break;

				switch (ueBuffer[slashIdx + 1])
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
						JsonConverter.EscapeUnit(buffer[slashIdx..]);
						adjustment += 1; // Only a UTF-8 unit is removed.
						break;
					case (Byte)'u':
					case (Byte)'U':
						adjustment += JsonConverter.EscapeUnicode(buffer, ref slashIdx);
						break;
				}
				ueBuffer = ueBuffer[..slashIdx];
			}
			return adjustment;
		}
		/// <summary>
		/// Escapes a UTF-8 unit in the buffer.
		/// </summary>
		/// <param name="buffer">A UTF-8 unescaped buffer.</param>
		private static void EscapeUnit(Span<Byte> buffer)
		{
			Int32 nEscaped = buffer.Length - 2;
			Span<Byte> escaped = JsonConverter.BackupEscaped(stackalloc Byte[nEscaped], buffer[2..]);
			buffer[0] = buffer[1] switch
			{
				(Byte)'n' => (Byte)'\n',
				(Byte)'r' => (Byte)'\r',
				(Byte)'t' => (Byte)'\t',
				(Byte)'b' => (Byte)'\b',
				(Byte)'f' => (Byte)'\f',
				(Byte)'0' => (Byte)'\0',
				_ => buffer[0],
			};
			buffer[^nEscaped..].CopyTo(escaped); // Backup the rest of the buffer.
			escaped.CopyTo(buffer[1..]); // Copy the rest of the buffer after the replacement.
			buffer[nEscaped + 1] = default; // Remove the last character.
		}
		/// <summary>
		/// Escapes a Unicode character in the buffer.
		/// </summary>
		/// <param name="buffer">A UTF-8 unescaped buffer.</param>
		/// <param name="escapeIndex">Index of escape begin.</param>
		/// <returns>The number of bytes that were not replaced.</returns>
		private static Int32 EscapeUnicode(Span<Byte> buffer, ref Int32 escapeIndex)
		{
			Char low = JsonConverter.GetUnicodeChar(buffer.Slice(escapeIndex + 2, 4));
			Int32 baseLength = 6; // Length of "\uXXXX" sequence.
			Int32 nEscaped = buffer.Length - escapeIndex - baseLength;
			Span<Byte> escaped =
				JsonConverter.BackupEscaped(stackalloc Byte[nEscaped], buffer[(escapeIndex + baseLength)..]);
			Rune rune = JsonConverter.GetEscapeRune(buffer, ref escapeIndex, low, ref baseLength);
			Int32 nBytes = rune.EncodeToUtf8(buffer[escapeIndex..]);
			Int32 offset = escapeIndex + nBytes;
			Int32 result = baseLength - nBytes;

			escaped.CopyTo(buffer[offset..]);
			buffer.Slice(offset + nEscaped, result).Clear(); // Clear the rest of the buffer.
			return result;
		}
		/// <summary>
		/// Retrieves the escape rune from the buffer.
		/// </summary>
		/// <param name="buffer">A UTF-8 unescaped buffer.</param>
		/// <param name="escapeIndex">Index of escape begin.</param>
		/// <param name="lowChar">Low surrogate char.</param>
		/// <param name="escapeSize">Escape size in bytes.</param>
		/// <returns>The escape rune from the buffer.</returns>
		private static Rune GetEscapeRune(Span<Byte> buffer, ref Int32 escapeIndex, Char lowChar, ref Int32 escapeSize)
		{
			Rune rune;
			if (!Char.IsLowSurrogate(lowChar) || !JsonConverter.HasHighSurrogate(buffer, escapeIndex, out Char high))
			{
				rune = new(lowChar);
			}
			else
			{
				rune = new(high, lowChar);
				escapeIndex -= 6; // Adjust for "\uXXXX" prefix.
				escapeSize *= 2; // Double the size for surrogate pairs.
			}
			return rune;
		}
		/// <summary>
		/// Indicates whether the buffer has a high surrogate character at the specified escape index.
		/// </summary>
		/// <param name="buffer">A UTF-8 unescaped buffer.</param>
		/// <param name="escapeIndex">Index of escape begin.</param>
		/// <param name="high">Output. High surrogate character.</param>
		/// <returns>
		/// <see langword="true"/> if the buffer has a high surrogate character at the specified escape index;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		private static Boolean HasHighSurrogate(Span<Byte> buffer, Int32 escapeIndex, out Char high)
		{
			Boolean hasHighSurrogate = buffer.Length - escapeIndex >= 6 &&
				buffer.Slice(escapeIndex - 6, 2).SequenceEqual("\\u"u8); // Check for "\u" prefix.
			high = JsonConverter.GetUnicodeChar(buffer.Slice(escapeIndex - 4, 4));
			return hasHighSurrogate;
		}
		/// <summary>
		/// Retrieves a Unicode character from the specified span of bytes.
		/// </summary>
		/// <param name="charCodeSpan">A UTF-8 span containing the hexadecimal value of a UTF-16 char.</param>
		/// <returns>An unicode char.</returns>
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
		/// <summary>
		/// Backs up the escaped string in the destination buffer.
		/// </summary>
		/// <param name="destination">Destination buffer.</param>
		/// <param name="source">Source buffer.</param>
		/// <returns>Destination buffer.</returns>
		private static Span<Byte> BackupEscaped(Span<Byte> destination, ReadOnlySpan<Byte> source)
		{
			source.CopyTo(destination);
			return destination;
		}
#endif
	}
}