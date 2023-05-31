namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[ExcludeFromCodeCoverage]
public sealed class GetIndicesTest
{
    [Fact]
    public void Utf16EmptyTest()
    {
        ReadOnlySpan<Char> source = ReadOnlySpan<Char>.Empty;
        IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

        Assert.Empty(indices);
    }

    [Fact]
    public void Utf8EmptyTest()
    {
        ReadOnlySpan<Byte> source = ReadOnlySpan<Byte>.Empty;
        IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

        Assert.Empty(indices);
    }

    [Theory]
    [InlineData("Hello", new[] { 0, 1, 2, 3, 4 })]
    [InlineData("Привет", new[] { 0, 1, 2, 3, 4, 5 })]
    [InlineData("こんにちは", new[] { 0, 1, 2, 3, 4 })]
    internal void Utf16Test(String input, Int32[] expectedIndices)
    {
        ReadOnlySpan<Char> source = input.AsSpan();
        IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

        Assert.Equal(expectedIndices.Length, indices.Count);
        for (Int32 i = 0; i < expectedIndices.Length; i++)
        {
            Assert.Equal(expectedIndices[i], indices[i]);
        }
    }

    [Theory]
    [InlineData("Hello", new[] { 0, 1, 2, 3, 4 })]
    [InlineData("Привет", new[] { 0, 2, 4, 6, 8, 10 })]
    [InlineData("こんにちは", new[] { 0, 3, 6, 9, 12 })]
    public void Utf8Test(String input, Int32[] expectedIndices)
    {
        ReadOnlySpan<Byte> source = Encoding.UTF8.GetBytes(input);
        IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

        Assert.Equal(expectedIndices.Length, indices.Count);
        for (Int32 i = 0; i < expectedIndices.Length; i++)
        {
            Assert.Equal(expectedIndices[i], indices[i]);
        }
    }
}
