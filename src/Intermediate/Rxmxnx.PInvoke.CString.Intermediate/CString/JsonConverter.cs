#if !PACKAGE || NETCOREAPP

#if !NETCOREAPP
using RuneCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuneCompat;
using Rune = System.UInt32;
#endif

namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// JSON converter for <see cref="CString"/> class.
	/// </summary>
	public sealed class JsonConverter : JsonConverter<CString>
	{
		/// <summary>
		/// Specifies whether the rented span is cleared by default before use.
		/// </summary>
#if NET7_0_OR_GREATER
		private const Boolean clearArray = false;
#else
		private const Boolean clearArray = true;
#endif

		/// <summary>
		/// Threshold for stackalloc usage in bytes.
		/// </summary>
		[ThreadStatic]
		private static Int32 stackallocByteConsumed;

		/// <inheritdoc/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
#pragma warning disable CS8764
		public override CString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
#pragma warning restore CS8764
			=> JsonConverter.Read(reader);
		/// <inheritdoc/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public override void Write(Utf8JsonWriter writer, CString? value, JsonSerializerOptions options)
			=> JsonConverter.Write(writer, value, value is null || value.IsZero,
#if NETCOREAPP3_1_OR_GREATER
			                       options.DefaultIgnoreCondition is JsonIgnoreCondition.WhenWritingNull or
				                       JsonIgnoreCondition.WhenWritingDefault or JsonIgnoreCondition.Always);
#else
			                       options.IgnoreNullValues);
#endif

		/// <summary>
		/// Writes a UTF-8 text bytes as JSON string.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="value">UTF-8 text bytes.</param>
		/// <param name="isNull">Indicates whether UTF-8 text is null.</param>
		/// <param name="nullAsEmpty">Indicates whether null-value should serialize as empty value.</param>
		/// <remarks>
		/// <paramref name="isNull"/> flag is ignored if <paramref name="value"/> is not an empty span.
		/// </remarks>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static void Write(Utf8JsonWriter writer, ReadOnlySpan<Byte> value, Boolean isNull, Boolean nullAsEmpty)
		{
			if (isNull && value.IsEmpty && !nullAsEmpty)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStringValue(value);
		}

		/// <summary>
		/// Consumes the stackalloc bytes if the required size is within the threshold.
		/// </summary>
		/// <param name="stackRequired">Required stack bytes to consume.</param>
		/// <param name="stackConsumed">
		/// Ref. Stack bytes consumed so far. This value is updated with the newly consumed bytes.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the stackalloc bytes were successfully consumed; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Boolean ConsumeStackBytes(Int32 stackRequired, ref Int32 stackConsumed)
		{
			if (stackRequired <= 0) return true; // No bytes to consume, return true.
			if (JsonConverter.stackallocByteConsumed + stackRequired > CString.stackallocByteThreshold)
				return false; // Stackalloc threshold exceeded.
			JsonConverter.stackallocByteConsumed += stackRequired;
			stackConsumed += stackRequired; // Update the consumed bytes.
			return true;
		}
		/// <summary>
		/// Returns a rented array of the specified length and clears it.
		/// </summary>
		/// <typeparam name="T">Type of the array elements.</typeparam>
		/// <param name="length">Required length of the array to rent.</param>
		/// <param name="arr">Output. Rented array.</param>
		/// <param name="clear">Indicates whether the array is required to be cleared.</param>
		/// <returns>A span of the rented array with the specified length, cleared.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Span<T> RentArray<T>(Int32 length, out T[]? arr, Boolean clear = JsonConverter.clearArray)
			where T : unmanaged
		{
			arr = ArrayPool<T>.Shared.Rent(length); // Rent an array of the specified length.

			Span<T> result = arr.AsSpan()[..length];
			if (clear)
				result.Clear(); // Clears the usable span.
			return result;
		}
		/// <summary>
		/// Returns a rented array of the specified length and clears it.
		/// </summary>
		/// <typeparam name="T">Type of the array elements.</typeparam>
		/// <param name="tArray">Rented array to return to the pool.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ReturnArray<T>(T[]? tArray) where T : unmanaged
		{
			if (tArray is not null)
				ArrayPool<T>.Shared.Return(tArray);
		}
		/// <summary>
		/// Releases the stack bytes consumed by the converter.
		/// </summary>
		/// <param name="stackConsumed">Number of stack bytes consumed to release.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ReleaseStackBytes(Int32 stackConsumed)
		{
			if (stackConsumed <= 0) return; // No bytes to release, return.
			JsonConverter.stackallocByteConsumed -= stackConsumed;
			if (JsonConverter.stackallocByteConsumed < 0)
				JsonConverter.stackallocByteConsumed = 0; // Prevent negative consumption.
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Int32 ReadBytes(Utf8JsonReader reader, Span<Byte> buffer)
		{
			Int32 adjustment = 0;
#if NET7_0_OR_GREATER
			Int32 nBytes = reader.CopyString(buffer);
			Span<Byte> invalidBytes = buffer[nBytes..];
			if (!invalidBytes.IsEmpty)
				invalidBytes[0] = default; // Clears the first invalid byte.
			adjustment -= invalidBytes.Length;
#else
			if (reader.HasValueSequence)
				reader.ValueSequence.CopyTo(buffer);
			else
				reader.ValueSpan.CopyTo(buffer);
			adjustment -= JsonConverter.EscapeString(buffer);
#endif
			return adjustment;
		}

		/// <summary>
		/// Reads and converts the JSON to type <see cref="CString"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>The converted value.</returns>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		private static CString? Read(Utf8JsonReader reader)
		{
			ValidationUtilities.ThrowIfNotString(reader.TokenType);
			if (reader.TokenType is JsonTokenType.Null) return default;

			Boolean isEmpty = (reader.HasValueSequence ? reader.ValueSequence.Length : reader.ValueSpan.Length) <= 0;
			return isEmpty ? CString.Empty : new(reader);
		}
#if !NET7_0_OR_GREATER
		/// <summary>
		/// Escapes the string in the buffer.
		/// </summary>
		/// <param name="buffer">A UTF-8 unescaped buffer.</param>
		/// <returns>Number of bytes of escape adjustment.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
						// Only a UTF-8 unit is removed.
						adjustment += 1;
						buffer = buffer[..^1];
						break;
					case (Byte)'u':
					case (Byte)'U':
						adjustment += JsonConverter.EscapeUnicode(ref buffer, ref slashIdx);
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
#if NET5_0_OR_GREATER
		[SkipLocalsInit]
#endif
		private static void EscapeUnit(Span<Byte> buffer)
		{
			Int32 stackConsumed = 0;
			Byte[]? byteArray = default;
			try
			{
				Int32 nEscaped = buffer.Length - 2;
				Span<Byte> escaped = JsonConverter.BackupEscaped(
					JsonConverter.ConsumeStackBytes(nEscaped, ref stackConsumed) ?
						stackalloc Byte[nEscaped] :
						JsonConverter.RentArray(nEscaped, out byteArray, false), buffer[2..]);
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
			finally
			{
				JsonConverter.ReturnArray(byteArray);
				JsonConverter.ReleaseStackBytes(stackConsumed);
			}
		}
		/// <summary>
		/// Escapes a Unicode character in the buffer.
		/// </summary>
		/// <param name="buffer">A UTF-8 unescaped buffer.</param>
		/// <param name="escapeIndex">Index of escape begin.</param>
		/// <returns>The number of bytes that were not replaced.</returns>
#if NET5_0_OR_GREATER
		[SkipLocalsInit]
#endif
		private static Int32 EscapeUnicode(ref Span<Byte> buffer, ref Int32 escapeIndex)
		{
			Int32 stackConsumed = 0;
			Byte[]? byteArray = default;
			try
			{
				Char low = JsonConverter.GetUnicodeChar(buffer.Slice(escapeIndex + 2, 4));
				Int32 baseLength = 6; // Length of "\uXXXX" sequence.
				Int32 nEscaped = buffer.Length - escapeIndex - baseLength;
				Span<Byte> escaped = JsonConverter.BackupEscaped(
					JsonConverter.ConsumeStackBytes(nEscaped, ref stackConsumed) ?
						stackalloc Byte[nEscaped] :
						JsonConverter.RentArray(nEscaped, out byteArray, false), buffer[(escapeIndex + baseLength)..]);
#if NETCOREAPP
				Rune rune = JsonConverter.GetEscapeRune(buffer, ref escapeIndex, low, ref baseLength);
				Int32 nBytes = rune.EncodeToUtf8(buffer[escapeIndex..]);
#else
				UInt32 rune = JsonConverter.GetEscapeRune(buffer, ref escapeIndex, low, ref baseLength);
				Int32 nBytes = RuneCompat.EncodeToUtf8(rune, buffer[escapeIndex..]);
#endif
				Int32 offset = escapeIndex + nBytes;
				Int32 result = baseLength - nBytes;

				escaped.CopyTo(buffer[offset..]);
				buffer.Slice(offset + nEscaped, result).Clear(); // Clear the rest of the buffer.
				buffer = buffer[..^result];
				return result;
			}
			finally
			{
				JsonConverter.ReturnArray(byteArray);
				JsonConverter.ReleaseStackBytes(stackConsumed);
			}
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
				buffer.Slice(escapeIndex - 6, 2).SequenceEqual(CString.UnicodePrefix()); // Check for "\u" prefix.
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
#endif