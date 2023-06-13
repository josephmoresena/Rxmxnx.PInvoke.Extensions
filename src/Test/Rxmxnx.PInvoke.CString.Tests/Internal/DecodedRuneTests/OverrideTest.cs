namespace Rxmxnx.PInvoke.Tests.Internal.DecodedRuneTests;

[ExcludeFromCodeCoverage]
public sealed partial class OverrideTest
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

        Assert.Equal(source.ToString(), decodedRune?.ToString());
    }
}
