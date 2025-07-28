#if !NETCOREAPP
using Rune = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuneCompat;
using Fact = NUnit.Framework.TestAttribute;
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class DecodeTest
{
	[Fact]
	public void Utf16EmptyTest()
	{
		ReadOnlySpan<Char> source = ReadOnlySpan<Char>.Empty;
		DecodedRune? decodedRune = DecodedRune.Decode(source);

		PInvokeAssert.Null(decodedRune);
	}

	[Fact]
	public void Utf8EmptyTest()
	{
		ReadOnlySpan<Byte> source = ReadOnlySpan<Byte>.Empty;
		DecodedRune? decodedRune = DecodedRune.Decode(source);

		PInvokeAssert.Null(decodedRune);
	}

	[Theory]
	[InlineData("Hello")]
	[InlineData("Привет")]
	[InlineData("こんにちは")]
	public void Utf16Test(String input)
	{
		ReadOnlySpan<Char> source = input.AsSpan();
		DecodedRune? decodedRune = DecodedRune.Decode(source);
		Int32 rawValue = default;
		_ = Rune.DecodeFromUtf16(source, out _, out Int32 expectedCharsConsumed);

		PInvokeAssert.NotNull(decodedRune);
		PInvokeAssert.Equal(source[0], DecodeTest.CreateString(decodedRune.Value.Value)[0]);
		PInvokeAssert.Equal(expectedCharsConsumed, decodedRune.Value.CharsConsumed);
		MemoryMarshal.AsBytes(source[..decodedRune.Value.CharsConsumed])
		             .CopyTo(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref rawValue, 1)));
		PInvokeAssert.Equal(decodedRune.Value.RawValue, rawValue);
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

		PInvokeAssert.NotNull(decodedRune);
		PInvokeAssert.Equal(input[0], DecodeTest.CreateString(decodedRune.Value.Value)[0]);
		PInvokeAssert.Equal(expectedCharsConsumed, decodedRune.Value.CharsConsumed);
		MemoryMarshal.AsBytes(source[..decodedRune.Value.CharsConsumed])
		             .CopyTo(MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref rawValue, 1)));
		PInvokeAssert.Equal(decodedRune.Value.RawValue, rawValue);
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