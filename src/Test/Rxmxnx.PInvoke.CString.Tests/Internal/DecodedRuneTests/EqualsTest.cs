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
		try
		{
			Assert.True(decodedRune1?.Equals(EqualsTest.CreateRune(decodedRune2?.Value)));
		}
		catch (Exception)
		{
			// NetStandard
			Assert.True(decodedRune1?.Equals(decodedRune2?.Value));
		}
	}

	[Fact]
	internal void InequalityTest()
	{
		DecodedRune? decodedRune1 = DecodedRune.Decode("A".AsSpan());
		DecodedRune? decodedRune2 = DecodedRune.Decode("B".AsSpan());

		Assert.False(decodedRune1?.Equals(decodedRune2));
		try
		{
			Assert.False(decodedRune1?.Equals(EqualsTest.CreateRune(decodedRune2?.Value)));
		}
		catch (Exception)
		{
			// NetStandard
			Assert.False(decodedRune1?.Equals(decodedRune2?.Value));
		}
	}

	[Fact]
	internal void NullTest()
	{
		DecodedRune? decodedRune = DecodedRune.Decode("A".AsSpan());
		Assert.False(decodedRune?.Equals(null));
	}

	private static Rune? CreateRune(Int32? value)
	{
		if (!value.HasValue)
			return default;

		Span<Rune> result = stackalloc Rune[1];
		Span<Int32> values = MemoryMarshal.Cast<Rune, Int32>(result);
		values[0] = value.Value;
		return result[0];
	}
}