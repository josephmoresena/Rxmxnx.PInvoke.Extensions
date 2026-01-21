#if NETCOREAPP
namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// JSON converter for <see cref="CStringSequence"/> class.
	/// </summary>
	public sealed class JsonConverter : JsonConverter<CStringSequence>
	{
		/// <inheritdoc/>
#pragma warning disable CS8764
		public override CStringSequence? Read(ref Utf8JsonReader reader, Type typeToConvert,
#pragma warning restore CS8764
			JsonSerializerOptions options)
		{
			ValidationUtilities.ThrowIfNotArray(reader.TokenType);
			if (reader.TokenType is JsonTokenType.Null) return default;
			List<Int32?> lengths = [];
			StringBuilder strBuilder = new();
			Boolean leadingNull = false;

			StackAllocationHelper.InitStackBytes();
			while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
				JsonConverter.ReadString(reader, strBuilder, lengths, ref leadingNull);

			if (leadingNull)
				strBuilder.Append('\0'); // Append a null character if the last string was not null-terminated.
			return lengths.Count == 0 ? CStringSequence.Empty : new(strBuilder.ToString(), lengths.ToArray());
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, CStringSequence? value, JsonSerializerOptions options)
		{
			Boolean writeNull = value is null &&
#if NETCOREAPP3_1_OR_GREATER
				options.DefaultIgnoreCondition switch
				{
					JsonIgnoreCondition.WhenWritingNull => false,
					JsonIgnoreCondition.WhenWritingDefault => false,
					_ => true,
				};
#else
				!options.IgnoreNullValues;
#endif
			if (writeNull)
			{
				writer.WriteNullValue();
				return;
			}

			Int32 item = 0;
			Utf8View view = new(value, true);
			ReadOnlySpan<Int32?> lengths = value?._lengths;

			writer.WriteStartArray();
			foreach (ReadOnlySpan<Byte> utf8Text in view)
			{
				CString.JsonConverter.Write(writer, utf8Text, !lengths[item].HasValue, false);
				item++;
			}
			writer.WriteEndArray();
		}

		/// <summary>
		/// Reads a string from the JSON reader and appends it to the building sequence.
		/// </summary>
		/// <param name="reader">A <see cref="Utf8JsonReader"/> instance.</param>
		/// <param name="buffer">Buffer for UTF-8 data.</param>
		/// <param name="lengths">Text length collection.</param>
		/// <param name="leadingNull">Ref. Leading null-character flag.</param>
#if NET7_0_OR_GREATER
		[SkipLocalsInit]
#endif
		private static void ReadString(Utf8JsonReader reader, StringBuilder buffer, List<Int32?> lengths,
			ref Boolean leadingNull)
		{
			ValidationUtilities.ThrowIfNotString(reader.TokenType);
			if (reader.TokenType is JsonTokenType.Null)
			{
				lengths.Add(default);
				return;
			}

			Int32 length = CString.JsonConverter.GetLength(reader);
			if (length == 0)
			{
				lengths.Add(0);
				return;
			}

			Int32 stackConsumed = 0;
			Char[]? charArray = default;
			try
			{
				Int32 nChar = length / sizeof(Char) + 1;
				// Since .NET 7.0, the resulting charSpan is left uninitialized.
				Span<Char> charSpan = StackAllocationHelper.ConsumeStackBytes(nChar * 2, ref stackConsumed) ?
					stackalloc Char[nChar] :
					StackAllocationHelper.RentArray(nChar, out charArray, CString.JsonConverter.ClearArray);
				Span<Byte> bytes = MemoryMarshal.AsBytes(charSpan);

				if (leadingNull)
				{
					bytes[0] = default; // Add leading null character in the string.
					bytes = bytes[1..]; // Skip leading null.
				}
				charSpan[^1] = default; // Ensure the last character is null.
				length += CString.JsonConverter.ReadBytes(reader, bytes[..length], false);

				Int32 nBytes = length + (leadingNull ? 1 : 0);
				Int32 charsCount = nBytes / sizeof(Char) + nBytes % sizeof(Char);

				// If the last byte is not null, we need to add a null character.
				leadingNull = nBytes % sizeof(Char) == 0 || bytes[length] != default;
				buffer.Append(charSpan[..charsCount]);
				lengths.Add(length);
			}
			finally
			{
				StackAllocationHelper.ReturnArray(charArray);
				StackAllocationHelper.ReleaseStackBytes(stackConsumed);
			}
		}
	}
}
#endif