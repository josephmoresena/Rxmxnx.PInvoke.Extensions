namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[ExcludeFromCodeCoverage]
public sealed class AsHexStringTest
{
    private static IFixture fixture = new Fixture();

    [Fact]
    internal void NormalTest()
    {
        Byte[] input = fixture.Create<Byte[]>();
        StringBuilder strBuild = new();
        foreach (Byte value in input)
        {
            Assert.Equal(value.ToString("X2").ToLower(), value.AsHexString());
            strBuild.Append(value.ToString("X2"));
        }
        Assert.Equal(strBuild.ToString().ToLower(), input.AsHexString());
    }
}

