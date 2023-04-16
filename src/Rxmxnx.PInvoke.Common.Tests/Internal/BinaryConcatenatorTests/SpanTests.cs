namespace Rxmxnx.PInvoke.Tests.Internal.BinaryConcatenatorTests;

[ExcludeFromCodeCoverage]
public sealed class SpanTests
{
    private static readonly IFixture fixture = new Fixture();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void SingleSpanTest(Boolean nullSeparator)
    {
        Byte[] sourceBytes = Encoding.UTF8.GetBytes(fixture.Create<String>());
        Byte? separator = !nullSeparator ? fixture.Create<Byte>() : default(Byte?);
        using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();

        helper.Write(sourceBytes);
        SingleSpanAssert(helper, sourceBytes);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void MultipleSpanTest(Boolean nullSeparator)
    {
        Byte[] sourceBytes1 = Encoding.UTF8.GetBytes(fixture.Create<String>());
        Byte[] sourceBytes2 = Encoding.UTF8.GetBytes(fixture.Create<String>());
        Byte? separator = !nullSeparator ? fixture.Create<Byte>() : default(Byte?);
        using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();

        helper.Write(sourceBytes1);
        helper.Write(sourceBytes2);
        MultipleSpanAssert(sourceBytes1, sourceBytes2, helper);
    }

    private static void SingleSpanAssert(BinaryConcatenator helper, Byte[] sourceBytes)
    {
        Boolean separatorInBytes = sourceBytes.Any(b => b == helper.Separator);
        Byte[]? outputBytes1 = helper.ToArray(false);
        Byte[]? outputBytes2 = helper.ToArray(true);

        Assert.NotNull(outputBytes1);
        Assert.NotNull(outputBytes2);

        Assert.Equal(sourceBytes, outputBytes1);
        Assert.Equal(sourceBytes, outputBytes2[..sourceBytes.Length]);
        Assert.Equal(outputBytes1.Length, outputBytes2.Length - 1);
        Assert.Equal(default, outputBytes2.Last());

        Assert.Equal(separatorInBytes, outputBytes1!.Any(b => b == helper.Separator));
    }

    private static void MultipleSpanAssert(Byte[] sourceBytes1, Byte[] sourceBytes2, BinaryConcatenator helper)
    {
        Int32 output2Start = sourceBytes1.Length + (helper.Separator.HasValue ? 1 : 0);
        Int32 finalLength = output2Start + sourceBytes2.Length;
        Byte[]? outputBytes1 = helper.ToArray(false);
        Byte[]? outputBytes2 = helper.ToArray(true);

        Assert.NotNull(outputBytes1);
        Assert.NotNull(outputBytes2);
        Assert.Equal(sourceBytes1, outputBytes1![..sourceBytes1.Length]);
        if (helper.Separator.HasValue)
            Assert.Equal(helper.Separator, outputBytes1![sourceBytes1.Length]);
        Assert.Equal(sourceBytes2, outputBytes1![output2Start..]);
        if (helper.Separator.HasValue)
            Assert.Contains(helper.Separator.Value, outputBytes1);

        Assert.Equal(outputBytes1, outputBytes2[..finalLength]);
        Assert.Equal(outputBytes1.Length, outputBytes2.Length - 1);
        Assert.Equal(default, outputBytes2.Last());
    }
}

