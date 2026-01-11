namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("roslyn", "CA1861")]
public sealed class GetIndicesTest
{
	[Fact]
	public void Utf16EmptyTest()
	{
		ReadOnlySpan<Char> source = ReadOnlySpan<Char>.Empty;
		IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

		PInvokeAssert.Empty(indices);
	}

	[Fact]
	public void Utf8EmptyTest()
	{
		ReadOnlySpan<Byte> source = ReadOnlySpan<Byte>.Empty;
		IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

		PInvokeAssert.Empty(indices);
	}

	[Theory]
	[InlineData("Hello", new[] { 0, 1, 2, 3, 4, })]
	[InlineData("Привет", new[] { 0, 1, 2, 3, 4, 5, })]
	[InlineData("こんにちは", new[] { 0, 1, 2, 3, 4, })]
	public void Utf16Test(String input, Int32[] expectedIndices)
	{
		ReadOnlySpan<Char> source = input.AsSpan();
		IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

		PInvokeAssert.Equal(expectedIndices.Length, indices.Count);
		for (Int32 i = 0; i < expectedIndices.Length; i++) PInvokeAssert.Equal(expectedIndices[i], indices[i]);
	}

	[Theory]
	[InlineData("Hello", new[] { 0, 1, 2, 3, 4, })]
	[InlineData("Привет", new[] { 0, 2, 4, 6, 8, 10, })]
	[InlineData("こんにちは", new[] { 0, 3, 6, 9, 12, })]
	public void Utf8Test(String input, Int32[] expectedIndices)
	{
		ReadOnlySpan<Byte> source = Encoding.UTF8.GetBytes(input);
		IReadOnlyList<Int32> indices = DecodedRune.GetIndices(source);

		PInvokeAssert.Equal(expectedIndices.Length, indices.Count);
		for (Int32 i = 0; i < expectedIndices.Length; i++) PInvokeAssert.Equal(expectedIndices[i], indices[i]);
	}
}