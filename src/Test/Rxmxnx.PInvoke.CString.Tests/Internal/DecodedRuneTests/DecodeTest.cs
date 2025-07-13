#if !NETCOREAPP
using Rune = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuneCompat;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[ExcludeFromCodeCoverage]
public sealed class DecodeTest
{
	[Fact]
	internal void Utf16EmptyTest()
	{
		ReadOnlySpan<Char> source = ReadOnlySpan<Char>.Empty;
		DecodedRune? decodedRune = DecodedRune.Decode(source);

		Assert.Null(decodedRune);
	}

	[Fact]
	internal void Utf8EmptyTest()
	{
		ReadOnlySpan<Byte> source = ReadOnlySpan<Byte>.Empty;
		DecodedRune? decodedRune = DecodedRune.Decode(source);

		Assert.Null(decodedRune);
	}

	[Theory]
	[InlineData("Hello")]
	[InlineData("Привет")]
	[InlineData("こんにちは")]
	internal void Utf16Test(String input)
	{
		ReadOnlySpan<Char> source = input.AsSpan();
		DecodedRune? decodedRune = DecodedRune.Decode(source);
		Int32 rawValue = default;
		_ = Rune.DecodeFromUtf16(source, out _, out Int32 expectedCharsConsumed);

		Assert.NotNull(decodedRune);
		Assert.Equal(source[0], DecodeTest.CreateString(decodedRune.Value.Value)[0]);
		Assert.Equal(expectedCharsConsumed, decodedRune.Value.CharsConsumed);
		MemoryMarshal.AsBytes(source[..decodedRune.Value.CharsConsumed])
		             .CopyTo(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref rawValue, 1)));
		Assert.Equal(decodedRune.Value.RawValue, rawValue);
	}

	[Theory]
	[InlineData("Hello")]
	[InlineData("Привет")]
	[InlineData("こんにちは")]
	public void Utf8Test(String input)
	{
		ReadOnlySpan<Byte> source = Encoding.UTF8.GetBytes(input);
		DecodedRune? decodedRune = DecodedRune.Decode(source);
		Int32 rawValue = default;
		_ = Rune.DecodeFromUtf8(source, out _, out Int32 expectedCharsConsumed);

		Assert.NotNull(decodedRune);
		Assert.Equal(input[0], DecodeTest.CreateString(decodedRune.Value.Value)[0]);
		Assert.Equal(expectedCharsConsumed, decodedRune.Value.CharsConsumed);
		MemoryMarshal.AsBytes(source[..decodedRune.Value.CharsConsumed])
		             .CopyTo(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref rawValue, 1)));
		Assert.Equal(decodedRune.Value.RawValue, rawValue);
	}

#if NETCOREAPP
	private static String CreateString(in Int32 value) => Unsafe.As<Int32, Rune>(ref Unsafe.AsRef(in value)).ToString();
#else
	private static unsafe String CreateString(in Int32 value)
	{
		fixed (void* ptr = &value)
			return Encoding.UTF32.GetString((Byte*)ptr, sizeof(Int32));
	}
#endif
}