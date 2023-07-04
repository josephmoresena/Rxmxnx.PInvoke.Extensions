namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[ExcludeFromCodeCoverage]
public sealed class EqualsTest
{
	[Fact]
	internal void EqualityInstanceTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = decodedRune1;

		Assert.True(decodedRune1?.Equals(decodedRune2));
	}

	[Fact]
	internal void EqualityValueTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = DecodedRune.Decode(source);

		Assert.True(decodedRune1?.Equals(decodedRune2));
		Assert.True(decodedRune1?.Equals(decodedRune2?.Value));
	}

	[Fact]
	internal void InequalityTest()
	{
		DecodedRune? decodedRune1 = DecodedRune.Decode("A".AsSpan());
		DecodedRune? decodedRune2 = DecodedRune.Decode("B".AsSpan());

		Assert.False(decodedRune1?.Equals(decodedRune2));
		Assert.False(decodedRune1?.Equals(decodedRune2?.Value));
	}

	[Fact]
	internal void NullTest()
	{
		DecodedRune? decodedRune = DecodedRune.Decode("A".AsSpan());
		Assert.False(decodedRune?.Equals(null));
	}
}