namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[ExcludeFromCodeCoverage]
public sealed class CompatTest
{
	[Theory]
	[InlineData(0x1F409)]
	[InlineData(0x1F004)]
	[InlineData(0x1F9D9)]
	[InlineData(0x1F9E0)]
	[InlineData(0x1F6F8)]
	[InlineData(0x1F5FF)]
	internal void EmojiTest(UInt32 value)
	{
		String text = new Rune(value).ToString();
		Span<Byte> utf8 = stackalloc Byte[5];
		Int32 l8 = RuneCompat.EncodeToUtf8(value, utf8);

		Assert.Equal(text, Encoding.UTF8.GetString(utf8[..l8]));
		Assert.Equal(OperationStatus.Done, RuneCompat.DecodeFromUtf8(utf8[..l8], out UInt32 value8, out Int32 bytes));
		Assert.Equal(OperationStatus.Done, RuneCompat.DecodeFromUtf16(text, out UInt32 value16, out Int32 chars));

		Assert.Equal(value, value8);
		Assert.Equal(bytes, l8);
		Assert.Equal(value, value16);
		Assert.Equal(chars, text.Length);
	}
	[Fact]
	internal void Test()
	{
		using TestMemoryHandle handle = new();
		foreach (Int32 index in TestSet.GetIndices().Where(i => i > 0))
		{
			ReadOnlySpan<Char> utf16 = TestSet.GetString(index);
			ReadOnlySpan<Byte> utf8 = TestSet.GetCString(index, handle);
			while (utf16.Length > 0 && utf8.Length > 0 &&
			       RuneCompat.DecodeFromUtf8(utf8, out UInt32 value8, out Int32 bytes) ==
			       RuneCompat.DecodeFromUtf16(utf16, out UInt32 value16, out Int32 chars))
			{
				Assert.True(Rune.DecodeFromUtf8(utf8, out Rune rune8, out Int32 bytes2) ==
				            Rune.DecodeFromUtf16(utf16, out Rune rune16, out Int32 chars2));
				unchecked
				{
					Assert.Equal(rune8, rune16);
					Assert.Equal(rune8.Value, (Int32)value8);
					Assert.Equal(rune16.Value, (Int32)value16);
					Assert.Equal(value8, value16);
					Assert.Equal(bytes2, bytes);
					Assert.Equal(chars2, chars);
				}
				utf8 = utf8[bytes..];
				utf16 = utf16[chars..];
			}
		}
	}
}