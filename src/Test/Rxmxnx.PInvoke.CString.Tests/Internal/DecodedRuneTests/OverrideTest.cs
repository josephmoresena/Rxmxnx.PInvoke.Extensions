#if !NETCOREAPP
using Rune = System.UInt32;
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class OverrideTest
{
	[Fact]
	public void GetHashCodeTest()
	{
		DecodedRune? decodedRune1 = DecodedRune.Decode("A".AsSpan());
		DecodedRune? decodedRune2 = DecodedRune.Decode("A".AsSpan());

		PInvokeAssert.Equal(decodedRune1?.GetHashCode(), decodedRune2?.GetHashCode());
	}

	[Fact]
	public void ToStringTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune = DecodedRune.Decode(source);

		PInvokeAssert.Equal(source.ToString(), OverrideTest.CreateString(decodedRune?.Value));
	}

	private static String? CreateString(Int32? value)
	{
		if (!value.HasValue)
			return default;
#if NETCOREAPP
		Span<Rune> result = stackalloc Rune[1];
		Span<Int32> values = MemoryMarshal.Cast<Rune, Int32>(result);
		values[0] = value.Value;
		return result[0].ToString();
#else
		return Char.ConvertFromUtf32(value.Value);
#endif
	}
}