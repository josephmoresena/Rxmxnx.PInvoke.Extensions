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
        _ = Rune.DecodeFromUtf16(source, out _, out Int32 expectedCharsConsumed);

        Assert.NotNull(decodedRune);
        Assert.Equal(source[0], decodedRune.Value.ToString()[0]);
        Assert.Equal(expectedCharsConsumed, decodedRune.CharsConsumed);
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("Привет")]
    [InlineData("こんにちは")]
    public void Utf8Test(String input)
    {
        ReadOnlySpan<Byte> source = Encoding.UTF8.GetBytes(input);
        DecodedRune? decodedRune = DecodedRune.Decode(source);
        _ = Rune.DecodeFromUtf8(source, out _, out Int32 expectedCharsConsumed);

        Assert.NotNull(decodedRune);
        Assert.Equal(input[0], decodedRune.Value.ToString()[0]);
        Assert.Equal(expectedCharsConsumed, decodedRune.CharsConsumed);
    }
}
