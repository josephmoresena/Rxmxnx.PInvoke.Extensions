using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rxmxnx.PInvoke.Json
{
	[ExcludeFromCodeCoverage]
	public class CStringJsonConverter : JsonConverter<CString>
	{
		public override CString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null) return default;
			if (reader.TokenType != JsonTokenType.String)
				throw new JsonException(
					$"Unexpected token type: {reader.TokenType}. Expected token type: {JsonTokenType.String}.");
			return reader.HasValueSequence ?
				CString.Unescape(reader.ValueSequence) :
				CString.Unescape(reader.ValueSpan);
		}
		public override void Write(Utf8JsonWriter writer, CString? value, JsonSerializerOptions options)
		{
			Boolean isNull = value is null || value.IsZero;
			Boolean writeNull = isNull && !options.IgnoreNullValues;
			if (writeNull)
			{
				writer.WriteNullValue();
				return;
			}
			writer.WriteStringValue(value);
		}
	}
}