namespace Rxmxnx.PInvoke.Tests.CStringTests;

public static class JsonEncoderStandard
{
	public static Byte[] EncodeToUtf8Bytes(ReadOnlySpan<Byte> input)
	{
		Byte[] buffer = ArrayPool<Byte>.Shared.Rent(input.Length * 12);
		Int32 pos = 0;

		try
		{
			Int32 i = 0;
			while (i < input.Length)
			{
				Byte b = input[i];
				if (b < 128)
				{
					switch (b)
					{
						case (Byte)'\"': JsonEncoderStandard.WriteEscape(buffer, ref pos, '\"'); break;
						case (Byte)'\\': JsonEncoderStandard.WriteEscape(buffer, ref pos, '\\'); break;
						case (Byte)'\b': JsonEncoderStandard.WriteEscape(buffer, ref pos, 'b'); break;
						case (Byte)'\f': JsonEncoderStandard.WriteEscape(buffer, ref pos, 'f'); break;
						case (Byte)'\n': JsonEncoderStandard.WriteEscape(buffer, ref pos, 'n'); break;
						case (Byte)'\r': JsonEncoderStandard.WriteEscape(buffer, ref pos, 'r'); break;
						case (Byte)'\t': JsonEncoderStandard.WriteEscape(buffer, ref pos, 't'); break;
						case (Byte)'<': JsonEncoderStandard.WriteHexEscape(buffer, ref pos, 0x3C); break;
						case (Byte)'>': JsonEncoderStandard.WriteHexEscape(buffer, ref pos, 0x3E); break;
						case (Byte)'&': JsonEncoderStandard.WriteHexEscape(buffer, ref pos, 0x26); break;
						case (Byte)'\'': JsonEncoderStandard.WriteHexEscape(buffer, ref pos, 0x27); break;
						case (Byte)'+': JsonEncoderStandard.WriteHexEscape(buffer, ref pos, 0x2B); break;
						default:
							if (b < 32) JsonEncoderStandard.WriteHexEscape(buffer, ref pos, b);
							else buffer[pos++] = b;
							break;
					}
					i++;
				}
				else
				{
					DecodedRune? rune = DecodedRune.Decode(input[i..]);
					if (rune.HasValue)
					{
						if (rune.Value.Value <= 0xFFFF)
						{
							JsonEncoderStandard.WriteHexEscape(buffer, ref pos, rune.Value.Value);
						}
						else
						{
							Int32 high = (rune.Value.Value - 0x10000) / 0x400 + 0xD800;
							Int32 low = (rune.Value.Value - 0x10000) % 0x400 + 0xDC00;
							JsonEncoderStandard.WriteHexEscape(buffer, ref pos, high);
							JsonEncoderStandard.WriteHexEscape(buffer, ref pos, low);
						}
						i += rune.Value.CharsConsumed;
					}
					else
					{
						// Invalid unit.
						JsonEncoderStandard.WriteHexEscape(buffer, ref pos, 0xFFFD);
						i++;
					}
				}
			}

			Byte[] result = new Byte[pos];
			Array.Copy(buffer, 0, result, 0, pos);
			return result;
		}
		finally
		{
			ArrayPool<Byte>.Shared.Return(buffer);
		}
	}

	private static void WriteEscape(Byte[] buffer, ref Int32 pos, Char val)
	{
		buffer[pos++] = (Byte)'\\';
		buffer[pos++] = (Byte)val;
	}
	private static void WriteHexEscape(Byte[] buffer, ref Int32 pos, Int32 value)
	{
		buffer[pos++] = (Byte)'\\';
		buffer[pos++] = (Byte)'u';
		buffer[pos++] = JsonEncoderStandard.ToHexChar(value >> 12);
		buffer[pos++] = JsonEncoderStandard.ToHexChar((value >> 8) & 0x0F);
		buffer[pos++] = JsonEncoderStandard.ToHexChar((value >> 4) & 0x0F);
		buffer[pos++] = JsonEncoderStandard.ToHexChar(value & 0x0F);
	}
	private static Byte ToHexChar(Int32 value) => (Byte)(value < 10 ? value + '0' : value - 10 + 'A');
}