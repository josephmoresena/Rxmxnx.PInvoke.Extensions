#if !NETCOREAPP
using Rune = System.UInt32;
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class EqualsTest
{
	private static readonly Boolean useSystemRune = (Object?)default(DecodedRune) is IEquatable<Rune>;

	[Fact]
	public void EqualityInstanceTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = decodedRune1;

		PInvokeAssert.True(decodedRune1?.Equals(decodedRune2));
	}

	[Fact]
	public void EqualityValueTest()
	{
		ReadOnlySpan<Char> source = "A".AsSpan();
		DecodedRune? decodedRune1 = DecodedRune.Decode(source);
		DecodedRune? decodedRune2 = DecodedRune.Decode(source);

		PInvokeAssert.True(decodedRune1?.Equals(decodedRune2));
		PInvokeAssert.True(EqualsTest.useSystemRune ?
			                   decodedRune1?.Equals(EqualsTest.CreateRune(decodedRune2?.Value)) :
			                   decodedRune1?.Equals((UInt32?)decodedRune2?.Value));
	}

	[Fact]
	public void InequalityTest()
	{
		DecodedRune? decodedRune1 = DecodedRune.Decode("A".AsSpan());
		DecodedRune? decodedRune2 = DecodedRune.Decode("B".AsSpan());

		PInvokeAssert.False(decodedRune1?.Equals(decodedRune2));
		PInvokeAssert.False(EqualsTest.useSystemRune ?
			                    decodedRune1?.Equals(EqualsTest.CreateRune(decodedRune2?.Value)) :
			                    decodedRune1?.Equals((UInt32?)decodedRune2?.Value));
	}

	[Fact]
	public void NullTest()
	{
		DecodedRune? decodedRune = DecodedRune.Decode("A".AsSpan());
		PInvokeAssert.False(decodedRune?.Equals(null));
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