namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[ExcludeFromCodeCoverage]
public sealed class OperatorsTest
{
	[Fact]
	internal void EqualityInstanceTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = decodedRune1;

		Assert.True(decodedRune1 == decodedRune2);
		Assert.False(decodedRune1 != decodedRune2);
	}

	[Fact]
	internal void EqualityValueTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = DecodedRune.Decode(source);

		Assert.True(decodedRune1 == decodedRune2);
		Assert.False(decodedRune1 != decodedRune2);
		Assert.False(default == decodedRune1);
		Assert.False(decodedRune1 == default);
		Assert.True(default == default(DecodedRune?));
	}

	[Fact]
	internal void InequalityTest()
	{
		DecodedRune? decodedRune1 = DecodedRune.Decode("A".AsSpan());
		DecodedRune? decodedRune2 = DecodedRune.Decode("B".AsSpan());

		Assert.False(decodedRune1 == decodedRune2);
		Assert.True(decodedRune1 != decodedRune2);
	}
}