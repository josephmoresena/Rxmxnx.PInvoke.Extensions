namespace Rxmxnx.PInvoke.Tests.Internal.BinaryConcatenatorTests;

[ExcludeFromCodeCoverage]
public sealed class EmptyTest
{
    private static readonly IFixture fixture = new Fixture();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void Test(Boolean nullSeparator)
    {
        Byte? separator = !nullSeparator ? fixture.Create<Byte>() : default(Byte?);
        using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();
        Assert.Null(helper.ToArray(false));
        Assert.Null(helper.ToArray(true));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task TestAsync(Boolean nullSeparator)
    {
        Byte? separator = !nullSeparator ? fixture.Create<Byte>() : default(Byte?);
        BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();
        try
        {
            Assert.Null(helper.ToArray(false));
            Assert.Null(helper.ToArray(true));
        }
        finally
        {
            await helper.DisposeAsync();
        }
    }
}
