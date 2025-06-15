﻿namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[ExcludeFromCodeCoverage]
public sealed class OverrideTest
{
	[Fact]
	public void GetHashCodeTest()
	{
		DecodedRune? decodedRune1 = DecodedRune.Decode("A".AsSpan());
		DecodedRune? decodedRune2 = DecodedRune.Decode("A".AsSpan());

		Assert.Equal(decodedRune1?.GetHashCode(), decodedRune2?.GetHashCode());
	}

	[Fact]
	public void ToStringTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune = DecodedRune.Decode(source);

		Assert.Equal(source.ToString(), OverrideTest.CreateString(decodedRune?.Value));
	}

	private static String? CreateString(Int32? value)
	{
		if (!value.HasValue)
			return default;

		Span<Rune> result = stackalloc Rune[1];
		Span<Int32> values = MemoryMarshal.Cast<Rune, Int32>(result);
		values[0] = value.Value;
		return result[0].ToString();
	}
}