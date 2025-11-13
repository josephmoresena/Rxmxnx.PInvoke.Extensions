namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Retrieves the UTF-8 \u prefix for JSON decoding.
	/// </summary>
	/// <returns>The UTF-8 \u prefix.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	public static ReadOnlySpan<Byte> UnicodePrefix()
#else
	private static ReadOnlySpan<Byte> UnicodePrefix()
#endif
		=> "\\u"u8;
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using a <typeparamref name="TState"/> instance.
	/// </summary>
	/// <typeparam name="TState">Type of the state object.</typeparam>
	/// <param name="state">Function state parameter.</param>
	/// <param name="getSpan">Function to retrieve utf-8 span from the state.</param>
	/// <param name="isNullTerminated">Indicates whether resulting UTF-8 text is null-terminated.</param>
	/// <param name="length">UTF-8 text length.</param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	public static CString Create<TState>(TState state, ReadOnlySpanFunc<Byte, TState> getSpan, Boolean isNullTerminated,
		Int32 length)
#else
	private static CString Create<TState>(TState state, ReadOnlySpanFunc<Byte, TState> getSpan,
		Boolean isNullTerminated, Int32 length)
#endif
		where TState : struct
	{
		ValueRegion<Byte> data = ValueRegion<Byte>.Create(state, getSpan);
		return new(data, true, isNullTerminated, length);
	}

	/// <summary>
	/// Retrieves an instance of the <see cref="EqualsDelegate"/> optimized for the current
	/// process' bitness.
	/// </summary>
	/// <returns>An instance of the <see cref="EqualsDelegate"/>.</returns>
	/// <remarks>
	/// This method selects the appropriate version of the Equals method to compare UTF-8
	/// strings in a manner that is optimized for the current machine's architecture
	/// (32 or 64 bit).
	/// </remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static EqualsDelegate GetEquals()
		=> Environment.Is64BitProcess ? CString.Equals<Int64> : CString.Equals<Int32>;
	/// <summary>
	/// Copies the byte data from the source array into the destination character span,
	/// treating the byte data as UTF-8 encoded text.
	/// </summary>
	/// <param name="destination">
	/// The destination <see cref="Span{Char}"/> where the byte data will be copied to.
	/// </param>
	/// <param name="source">The source byte array from which the data will be copied.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void CopyBytes(Span<Char> destination, Byte[] source)
	{
		//Converts binary span into source char span.
		ReadOnlySpan<Char> sourceChars = MemoryMarshal.Cast<Byte, Char>(source);
		//Gets the binary size of source char span.
		Int32 offset = sourceChars.Length * sizeof(Char);
		//Creates the remaining bytes from source.
		ReadOnlySpan<Byte> remSource = source.AsSpan()[offset..];
		//Gets the remaining binary destination into destination span.
		Span<Byte> remDestination = MemoryMarshal.AsBytes(destination[sourceChars.Length..]);

		//Copies the source char span into destination span.
		sourceChars.CopyTo(destination);
		//Copies the remaining binary span into UTF8 destination span.
		remSource.CopyTo(remDestination);
	}
	/// <summary>
	/// Compares two ReadOnlySpan instances for equality, treating their byte data as
	/// <typeparamref name="TInteger"/> for faster comparison.
	/// </summary>
	/// <typeparam name="TInteger">
	/// A ValueType which is used to interpret the byte data for comparison, such as
	/// <see cref="Int32"/> or <see cref="Int64"/>.
	/// </typeparam>
	/// <param name="current">The first ReadOnlySpan instance to compare.</param>
	/// <param name="other">The second ReadOnlySpan instance to compare.</param>
	/// <returns>
	/// <see langword="true"/> if both read-only instances have the same length and contain the same
	/// byte data, otherwise <see langword="false"/>.
	/// </returns>
	private static unsafe Boolean Equals<TInteger>(ReadOnlySpan<Byte> current, ReadOnlySpan<Byte> other)
		where TInteger : unmanaged, IEquatable<TInteger>
	{
		if (current.Length != other.Length) return false;
		ReadOnlySpan<TInteger> currentIntegers = MemoryMarshal.Cast<Byte, TInteger>(current);
		ReadOnlySpan<TInteger> otherIntegers = MemoryMarshal.Cast<Byte, TInteger>(other);

		if (!currentIntegers.SequenceEqual(otherIntegers)) return false;
		Int32 binaryOffset = currentIntegers.Length * sizeof(TInteger);
		return current[binaryOffset..].SequenceEqual(other[binaryOffset..]);
	}
	/// <summary>
	/// Determines if a given read-only span of bytes represents a null-terminated UTF-8 string.
	/// </summary>
	/// <param name="data">The read-only span of bytes to check.</param>
	/// <param name="textLength">
	/// When this method returns, contains the length of the text within the read-only span, excluding
	/// the null terminator, if one was found; otherwise, the total length of the read-only span.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the read-only span represents a null-terminated UTF-8 string; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsNullTerminatedSpan(ReadOnlySpan<Byte> data, out Int32 textLength)
	{
		textLength = data.Length;
		while (textLength > 0 && data[textLength - 1] == default)
			textLength--;
		return textLength < data.Length;
	}
	/// <summary>
	/// Creates a null-terminated UTF-8 string that consists of a given ReadOnlySpan of bytes repeated
	/// a specified number of times.
	/// </summary>
	/// <param name="seq">The read-only span of bytes to repeat.</param>
	/// <param name="count">The number of times to repeat the read-only span.</param>
	/// <returns>
	/// A <see cref="Byte"/> array that represents a null-terminated UTF-8 string composed of the
	/// read-only span repeated the specified number of times.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Byte[] CreateRepeatedSequence(ReadOnlySpan<Byte> seq, Int32 count)
	{
		Byte[] result = new Byte[seq.Length * count + 1];
		for (Int32 i = 0; i < count; i++)
			seq.CopyTo(result.AsSpan()[(seq.Length * i)..]);
		return result;
	}
	/// <summary>
	/// Creates a <see cref="String"/> that consists of a single UTF-16 character.
	/// </summary>
	/// <param name="separator">The character to make up the <see cref="String"/>.</param>
	/// <returns>A <see cref="String"/> that consists of a single instance of the specified character.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static String CreateSeparator(Char separator) => String.Create(1, separator, CString.SetSeparator);
	/// <summary>
	/// Sets the value of the specified UTF-16 character in a Span of characters.
	/// </summary>
	/// <param name="spanSeparator">The span of UTF-16 characters to set the character value.</param>
	/// <param name="separator">The UTF-16 character to set.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void SetSeparator(Span<Char> spanSeparator, Char separator) => spanSeparator[0] = separator;
	/// <summary>
	/// Calls to <see cref="CString.Write(Stream, Int32, Int32)"/> in async context.
	/// </summary>
	/// <param name="writer">A <see cref="SyncAsyncWriter"/> value.</param>
	private static async Task WriteSyncAsync(SyncAsyncWriter writer)
	{
		await Task.Yield();
		writer.Write();
	}
#if !PACKAGE || NETCOREAPP
	/// <summary>
	/// Reads a UTF-8 string from the specified <see cref="Utf8JsonReader"/> and returns its length.
	/// </summary>
	/// <param name="reader">A <see cref="Utf8JsonReader"/> instance.</param>
	/// <param name="data">Output. A <see cref="ValueRegion{Byte}"/> instance.</param>
	/// <returns>Read UTF-8 text length.</returns>
	private static Int32 Read(Utf8JsonReader reader, out ValueRegion<Byte> data)
	{
		Int32 stackConsumed = 0;
		Byte[]? byteArray = default;
		try
		{
			Int32 length = JsonConverter.GetLength(reader);
			Span<Byte> span = JsonConverter.ConsumeStackBytes(length, ref stackConsumed) ?
				stackalloc Byte[length] :
				JsonConverter.RentArray(length, out byteArray);

			length += JsonConverter.ReadBytes(reader, span);
			data = ValueRegion<Byte>.Create(span[..length].ToArray());
			return length;
		}
		finally
		{
			JsonConverter.ReturnArray(byteArray);
			JsonConverter.ReleaseStackBytes(stackConsumed);
		}
	}
#endif
	/// <summary>
	/// Creates a non-null-terminated <see cref="CString"/> instance that contains a single
	/// <paramref name="c"/> character.
	/// </summary>
	/// <param name="c">A UTF-8 byte.</param>
	/// <returns>
	/// A non-null-terminated <see cref="CString"/> instance that contains a single <paramref name="c"/> character.
	/// </returns>
	private static CString Create(Byte c) => new([c,], false);
}