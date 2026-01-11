namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class OperatorsTest
{
	[Fact]
	public void EqualityInstanceTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = decodedRune1;

		PInvokeAssert.True(decodedRune1 == decodedRune2);
		PInvokeAssert.False(decodedRune1 != decodedRune2);
	}

	[Fact]
	public void EqualityValueTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = DecodedRune.Decode(source);

		PInvokeAssert.True(decodedRune1 == decodedRune2);
		PInvokeAssert.False(decodedRune1 != decodedRune2);
		PInvokeAssert.False(default == decodedRune1);
		PInvokeAssert.False(decodedRune1 == default);
		PInvokeAssert.True(default == default(DecodedRune?));
	}

	[Fact]
	public void InequalityTest()
	{
		DecodedRune? decodedRune1 = DecodedRune.Decode("A".AsSpan());
		DecodedRune? decodedRune2 = DecodedRune.Decode("B".AsSpan());

		PInvokeAssert.False(decodedRune1 == decodedRune2);
		PInvokeAssert.True(decodedRune1 != decodedRune2);
	}
}