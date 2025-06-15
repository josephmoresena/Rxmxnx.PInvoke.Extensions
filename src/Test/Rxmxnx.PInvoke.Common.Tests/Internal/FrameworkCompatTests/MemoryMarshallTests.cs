namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
public sealed unsafe class MemoryMarshallTests
{
	[Theory]
	[InlineData(new Byte[] { 0x68, 0x6F, 0x6C, 0x61, 0x00, 0x6D, 0x75, 0x6E, 0x64, 0x6F, 0x00, })] // "hola\0mundo\0"
	[InlineData(new Byte[]
	{
		0xE4, 0xBD, 0xA0, 0xE5, 0xA5, 0xBD, 0x00, 0xE4, 0xB8, 0x96, 0xE7, 0x95, 0x8C, 0x00,
	})] // "你好\0世界\0"
	[InlineData(new Byte[]
	{
		0xE3, 0x81, 0x93, 0xE3, 0x82, 0x93, 0xE3, 0x81, 0xAB, 0xE3, 0x81, 0xA1, 0xE3, 0x81, 0xAF, 0x00,
	})] // "こんにちは\0"
	[InlineData(new Byte[] { 0xD8, 0xB3, 0xD9, 0x84, 0xD8, 0xA7, 0xD9, 0x85, 0x00, })] // "سلام\0"
	[InlineData(new Byte[]
	{
		0x44, 0x6F, 0x62, 0x72, 0x79, 0x64, 0xE9, 0x6E, 0x00, 0x77, 0x69, 0x74, 0x61, 0x6D, 0x69, 0x6E, 0x61, 0x00,
	})] // 
	[InlineData(new Byte[]
	{
		0xCE, 0x95, 0xCE, 0xBB, 0xCE, 0xBB, 0xCE, 0xB7, 0xCE, 0xBD, 0xCE, 0xB9, 0xCE, 0xBA, 0xCE, 0xAC, 0x00,
	})] // "Ελληνικά\0""Dobrydén\0witamina\0"
	[InlineData(new Byte[]
	{
		0xE0, 0xA4, 0xA8, 0xE0, 0xA4, 0xAE, 0xE0, 0xA4, 0xB8, 0xE0, 0xA5, 0x8D, 0xE0, 0xA4, 0xA4, 0xE0, 0xA5, 0x87,
		0x00,
	})] // "नमस्ते\0"
	[InlineData(new Byte[]
	{
		0x48, 0x65, 0x6A, 0x00, 0x64, 0x61, 0x72, 0x6C, 0x69, 0x6E, 0x67, 0x00,
	})] // "Hej\0darling\0"
	[InlineData(new Byte[] { 0x46, 0x72, 0x61, 0x6E, 0xE7, 0x61, 0x69, 0x73, 0x00, })] // "Français\0"
	[InlineData(new Byte[] { 0x44, 0x61, 0x6E, 0x6B, 0x65, 0x00, 0x00, })] // "Danke\0\0"
	[InlineData(new Byte[]
	{
		0xE6, 0xAC, 0xA2, 0xE8, 0xBF, 0x8E, 0x00, 0xE4, 0xBD, 0xA0, 0xE5, 0xA5, 0xBD, 0x00,
	})] // "欢迎\0你好\0"
	[InlineData(new Byte[]
	{
		0xE0, 0xA6, 0x86, 0xE0, 0xA6, 0xAE, 0xE0, 0xA6, 0xBF, 0xE0, 0xA6, 0xA4, 0x00,
	})] // "আমিৎ\0"
	[InlineData(new Byte[] { 0x4D, 0x65, 0x72, 0x68, 0x61, 0x62, 0x61, 0x00, })] // "Merhaba\0"
	[InlineData(new Byte[] { 0x42, 0x6F, 0x6E, 0x6A, 0x6F, 0x75, 0x72, 0x00, })] // "Bonjour\0"
	[InlineData(new Byte[]
	{
		0xD0, 0x97, 0xD0, 0xB4, 0xD1, 0x80, 0xD0, 0xB0, 0xD0, 0xB2, 0xD1, 0x81, 0xD1, 0x82, 0xD0, 0xB2, 0xD1, 0x83,
		0xD0, 0xB9, 0x00,
	})] // "Здравствуйте\0"
	[InlineData(new Byte[] { 0xD7, 0xA9, 0xD7, 0x9C, 0xD7, 0x95, 0xD7, 0x9D, 0x00, })] // "שלום\0"
	[InlineData(new Byte[] { 0xC2, 0xA1, 0x48, 0x6F, 0x6C, 0x61, 0x00, })] // ¡Hola\0
	[InlineData(new Byte[]
	{
		0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x00, 0x77, 0x6F, 0x72, 0x6C, 0x64, 0x00,
	})] // "Hello\0world\0"
	[InlineData(new Byte[] { 0x54, 0x61, 0x6C, 0x6F, 0x6B, 0x00, 0x50, 0x6F, 0x6E, 0x67, 0x00, })] // Talok\0Pong\0
	internal void ByteTest(Byte[] array)
	{
		for (Int32 offset = 0; offset < array.Length - 1; offset++)
		{
			ReadOnlySpan<Byte> data = array.AsSpan()[offset..];
			fixed (Byte* ptr = data)
			{
				ReadOnlySpan<Byte> original = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr);
				ReadOnlySpan<Byte> span = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(ptr);

				Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(original),
				                           ref MemoryMarshal.GetReference(span)));
				Assert.Equal(original.IsEmpty, span.IsEmpty);
				Assert.Equal(original.Length, span.Length);
				Assert.Equal(original.Length,
				             MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference(original)));
				for (Int32 i = 0; i < original.Length; i++)
				{
					Assert.True(Unsafe.AreSame(in data[i], in span[i]));
					Assert.True(Unsafe.AreSame(in original[i], in span[i]));
				}
			}
		}
	}
	[Theory]
	[InlineData(new[] { 'h', 'o', 'l', 'a', '\0', 'm', 'u', 'n', 'd', 'o', '\0', })]
	[InlineData(new[] { '你', '好', '\0', '世', '界', '\0', })]
	[InlineData(new[] { 'こ', 'ん', 'に', 'ち', 'は', '\0', })]
	[InlineData(new[] { 'س', 'ل', 'ا', 'م', '\0', })]
	[InlineData(new[] { 'D', 'o', 'b', 'r', 'y', 'd', 'é', 'n', '\0', 'w', 'i', 't', 'a', 'm', 'i', 'n', 'a', '\0', })]
	[InlineData(new[] { 'Ε', 'λ', 'λ', 'η', 'ν', 'ι', 'κ', 'ά', '\0', })]
	[InlineData(new[] { 'न', 'म', 'स', '्', 'त', 'े', '\0', })]
	[InlineData(new[] { 'H', 'e', 'j', '\0', 'd', 'a', 'r', 'l', 'i', 'n', 'g', '\0', })]
	[InlineData(new[] { 'F', 'r', 'a', 'n', 'ç', 'a', 'i', 's', '\0', })]
	[InlineData(new[] { 'D', 'a', 'n', 'k', 'e', '\0', '\0', })]
	[InlineData(new[] { '欢', '迎', '\0', '你', '好', '\0', })]
	[InlineData(new[] { 'আ', 'ম', 'ি', 'ত', '\0', })]
	[InlineData(new[] { 'M', 'e', 'r', 'h', 'a', 'b', 'a', '\0', })]
	[InlineData(new[] { 'B', 'o', 'n', 'j', 'o', 'u', 'r', '\0', })]
	[InlineData(new[] { 'З', 'д', 'р', 'а', 'в', 'с', 'т', 'в', 'у', 'й', 'т', 'е', '\0', })]
	[InlineData(new[] { 'ש', 'ל', 'ו', 'ם', '\0', })]
	[InlineData(new[] { 'H', 'e', 'l', 'l', 'o', '\0', 'w', 'o', 'r', 'l', 'd', '\0', })]
	[InlineData(new[] { 'ስ', 'ለ', 'ዚ', 'ህ', '\0', })]
	[InlineData(new[] { '¡', 'H', 'o', 'l', 'a', '\0', })]
	[InlineData(new[] { 'T', 'a', 'l', 'o', 'k', '\0', 'P', 'o', 'n', 'g', '\0', })]
	internal void CharTest(Char[] array)
	{
		for (Int32 offset = 0; offset < array.Length - 1; offset++)
		{
			ReadOnlySpan<Char> data = array.AsSpan()[offset..];
			fixed (Char* ptr = data)
			{
				ReadOnlySpan<Char> original = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr);
				ReadOnlySpan<Char> span = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(ptr);

				Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(original),
				                           ref MemoryMarshal.GetReference(span)));
				Assert.Equal(original.IsEmpty, span.IsEmpty);
				Assert.Equal(original.Length, span.Length);
				Assert.Equal(original.Length,
				             MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference(original)));
				for (Int32 i = 0; i < original.Length; i++)
				{
					Assert.True(Unsafe.AreSame(in data[i], in span[i]));
					Assert.True(Unsafe.AreSame(in original[i], in span[i]));
				}
			}
		}
	}
	[Fact]
	internal void ByteNullTest()
	{
		Byte* ptr = null;
		ReadOnlySpan<Byte> original = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr);
		ReadOnlySpan<Byte> span = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(ptr);
		Assert.True(span.IsEmpty);
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(original), ref MemoryMarshal.GetReference(span)));
		Assert.Equal(original.IsEmpty, span.IsEmpty);
		Assert.Equal(original.Length, span.Length);
		Assert.Equal(original.Length, MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference(original)));
	}
	[Fact]
	internal void CharNullTest()
	{
		Char* ptr = null;
		ReadOnlySpan<Char> original = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr);
		ReadOnlySpan<Char> span = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(ptr);
		Assert.True(span.IsEmpty);
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(original), ref MemoryMarshal.GetReference(span)));
		Assert.Equal(original.IsEmpty, span.IsEmpty);
		Assert.Equal(original.Length, span.Length);
		Assert.Equal(original.Length, MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference(original)));
	}
	[Fact]
	internal void ByteEmptyTest()
	{
		ReadOnlySpan<Byte> data = [0,];
		fixed (Byte* ptr = data)
		{
			ReadOnlySpan<Byte> original = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr);
			ReadOnlySpan<Byte> span = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(ptr);
			Assert.True(span.IsEmpty);
			Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(original), ref MemoryMarshal.GetReference(span)));
			Assert.Equal(original.IsEmpty, span.IsEmpty);
			Assert.Equal(original.Length, span.Length);
			Assert.Equal(original.Length, MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference(original)));
		}
	}
	[Fact]
	internal void CharEmptyTest()
	{
		ReadOnlySpan<Char> data = ['\0',];
		fixed (Char* ptr = data)
		{
			ReadOnlySpan<Char> original = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr);
			ReadOnlySpan<Char> span = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(ptr);
			Assert.True(span.IsEmpty);
			Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(original), ref MemoryMarshal.GetReference(span)));
			Assert.Equal(original.IsEmpty, span.IsEmpty);
			Assert.Equal(original.Length, span.Length);
			Assert.Equal(original.Length, MemoryMarshalCompat.IndexOfNull(ref MemoryMarshal.GetReference(original)));
		}
	}
}