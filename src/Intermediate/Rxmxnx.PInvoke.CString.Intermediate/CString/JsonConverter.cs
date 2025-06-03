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
		{
			ValidationUtilities.ThrowIfNotString(reader.TokenType);
			if (reader.TokenType is JsonTokenType.Null) return default;

			Boolean isEmpty = (reader.HasValueSequence ? reader.ValueSequence.Length : reader.ValueSpan.Length) <= 0;
			return isEmpty ? CString.Empty : new(reader);
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, CString? value, JsonSerializerOptions options)
			=> JsonConverter.Write(writer, value, value is null, options);

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
			if (reader.HasValueSequence)
				reader.ValueSequence.CopyTo(buffer);
			else
				reader.ValueSpan.CopyTo(buffer);
			return buffer[^1] == 0 ? -1 : 0;
		}
		/// <summary>
		/// Writes a UTF-8 text bytes as JSON string.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="value">UTF-8 text bytes.</param>
		/// <param name="isNull">Indicates whether UTF-8 text is null.</param>
		/// <param name="options">An object that specifies serialization options to use.</param>
		internal static void Write(Utf8JsonWriter writer, ReadOnlySpan<Byte> value, Boolean isNull,
			JsonSerializerOptions options)
		{
			if (isNull)
			{
				if (!options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingNull))
				{
					writer.WriteNullValue();
					return;
				}
				if (options.DefaultIgnoreCondition.HasFlag(JsonIgnoreCondition.WhenWritingDefault))
					return;
			}
			writer.WriteStringValue(value);
		}
	}
}