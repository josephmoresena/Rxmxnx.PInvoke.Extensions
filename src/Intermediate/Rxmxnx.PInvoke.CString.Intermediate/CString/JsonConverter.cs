#if NETCOREAPP
namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// JSON converter for <see cref="CString"/> class.
	/// </summary>
	public sealed class JsonConverter : JsonConverter<CString>
	{
#if NET6_0_OR_GREATER
		/// <summary>
		/// Specifies whether the rented span is cleared by default before use.
		/// </summary>
#if NET7_0_OR_GREATER
		internal const Boolean ClearArray = false;
#else
		internal const Boolean ClearArray = true;
#endif
#endif

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
		/// Reads and converts the JSON to type <see cref="CString"/>.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>The converted value.</returns>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		// ReSharper disable once MemberCanBePrivate.Global
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
		/// <param name="clearUnused">Indicates whether the current unused bytes should be cleared.</param>
		/// <returns>Adjustment value for text length.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static Int32 ReadBytes(Utf8JsonReader reader, Span<Byte> buffer, Boolean clearUnused)
		{
#if NET7_0_OR_GREATER
			Int32 unused = buffer.Length - reader.CopyString(buffer);
#else
			if (reader.HasValueSequence)
				reader.ValueSequence.CopyTo(buffer);
			else
				reader.ValueSpan.CopyTo(buffer);

			Int32 unused = TextUnescape.Unescape(buffer);
#endif
			return CString.FinalizeBuffer(buffer, unused, clearUnused);
		}
	}
}
#endif