using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rxmxnx.PInvoke.Json
{
	[ExcludeFromCodeCoverage]
	public class CStringSequenceJsonConverter : JsonConverter<CStringSequence>
	{
		public override CStringSequence? Read(ref Utf8JsonReader reader, Type typeToConvert,
			JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null) return default;
			if (reader.TokenType != JsonTokenType.StartArray)
				throw new JsonException(
					$"Unexpected token type: {reader.TokenType}. Expected token type: {JsonTokenType.StartArray}.");
			CStringSequence.Builder builder = CStringSequence.CreateBuilder();
			while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
			{
				if (reader.TokenType == JsonTokenType.Null)
					builder.Append(CString.Zero);
				else if (reader.TokenType != JsonTokenType.String)
					throw new JsonException(
						$"Unexpected token type: {reader.TokenType}. Expected token type: {JsonTokenType.String}.");
				else
					builder = reader.HasValueSequence ?
						builder.AppendEscaped(reader.ValueSequence) :
						builder.AppendEscaped(reader.ValueSpan);
			}
			return builder.Build();
		}
		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, CStringSequence? value, JsonSerializerOptions options)
		{
			Boolean writeNull = value is null && !options.IgnoreNullValues;
			if (writeNull)
			{
				writer.WriteNullValue();
				return;
			}

			CStringSequence.Utf8View view = value.CreateView();

			writer.WriteStartArray();
			foreach (ReadOnlySpan<Byte> utf8Text in view)
			{
				if (utf8Text.Length > 0)
					writer.WriteStringValue(utf8Text);
				else if (Unsafe.IsNullRef(ref MemoryMarshal.GetReference(utf8Text)))
					writer.WriteNullValue();
				else
					writer.WriteStringValue(ReadOnlySpan<Byte>.Empty);
			}
			writer.WriteEndArray();
		}
	}
}