namespace Rxmxnx.PInvoke;

public partial class CString
{
	internal sealed partial class Chunk
	{
		/// <summary>
		/// Structure containing metadata required to allocate UTF-8 chunks from a char span.
		/// </summary>
		private readonly ref struct CharSpanUtf8Split
		{
			/// <summary>
			/// Left char span.
			/// </summary>
			public ReadOnlySpan<Char> Left { get; }
			/// <summary>
			/// Right char span.
			/// </summary>
			public ReadOnlySpan<Char> Right { get; }

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="chars">A char read-only span.</param>
			/// <param name="charsByteCount">UTF-8 bytes required to encode <paramref name="chars"/>.</param>
			/// <param name="maxLeftByteCount">Maximum UTF-8 bytes available to store left char span.</param>
			public CharSpanUtf8Split(ReadOnlySpan<Char> chars, Int32 charsByteCount, Int32 maxLeftByteCount)
			{
				if (chars.IsEmpty) return;

				Int32 utf8Count = 0;
				Int32 leftCharCount =
					CharSpanUtf8Split.GetInitialCharCount(chars.Length, charsByteCount, maxLeftByteCount);
				while (leftCharCount < chars.Length)
				{
					Int32 runeUtf8Bytes = CharSpanUtf8Split.DecodeRune(chars[leftCharCount..], out Int32 charsConsumed);
					if (utf8Count + runeUtf8Bytes > maxLeftByteCount)
						break;

					utf8Count += runeUtf8Bytes;
					leftCharCount += charsConsumed;
				}

				this.Left = chars[..leftCharCount];
				this.Right = chars[leftCharCount..];
			}

			/// <summary>
			/// Decodes the rune from <paramref name="slice"/>.
			/// </summary>
			/// <param name="slice">A char read-only span.</param>
			/// <param name="charsConsumed">Output. UTF-16 chars consumed by the rune.</param>
			/// <returns>UTF-8 units consumed by the run.</returns>
			private static Int32 DecodeRune(ReadOnlySpan<Char> slice, out Int32 charsConsumed)
			{
				DecodedRune? dr = DecodedRune.Decode(slice);
				if (!dr.HasValue)
				{
					charsConsumed = 1;
					return 2;
				}
				charsConsumed = dr.Value.CharsConsumed;
				return dr.Value.Value switch
				{
					<= 0x7F => 1,
					<= 0x7FF => 2,
					<= 0xFFFF => 3,
					_ => 4,
				};
			}
			/// <summary>
			/// Retrieves the initial length of left char span.
			/// </summary>
			/// <param name="charsLength">Char span length.</param>
			/// <param name="charsByteCount">UTF-8 bytes required to encode the char span.</param>
			/// <param name="maxLeftByteCount">Maximum UTF-8 bytes available to store left char span.</param>
			/// <returns>The initial length of left char span.</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static Int32 GetInitialCharCount(Int64 charsLength, Int32 charsByteCount, Int32 maxLeftByteCount)
			{
				Int32 leftCharCount = 0;
				if (charsByteCount <= 0 || maxLeftByteCount <= 0) return leftCharCount;
				Int32 approx = (Int32)(charsLength * maxLeftByteCount / charsByteCount);
				leftCharCount = (UInt32)approx <= (UInt32)charsLength ? approx : (Int32)charsLength;
				return leftCharCount;
			}
		}
	}
}