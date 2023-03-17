namespace Rxmxnx.PInvoke.Tests.Internal.BinaryConcatenatorTests;

public sealed class BinaryTest
{
    private static readonly IFixture fixture = new Fixture();

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void Test(Boolean nullSeparator)
    {
        Byte[] sourceBytes = Encoding.UTF8.GetBytes(fixture.Create<String>());
        Byte? separator = !nullSeparator ? fixture.Create<Byte>() : default(Byte?);
        using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();

        foreach (Byte b in sourceBytes)
            helper.Write(b);

        AssertTest(helper, sourceBytes);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task TestAsync(Boolean nullSeparator)
    {
        Byte[] sourceBytes = Encoding.UTF8.GetBytes(fixture.Create<String>());
        Byte? separator = !nullSeparator ? fixture.Create<Byte>() : default(Byte?);
        using BinaryConcatenator helper = separator.HasValue ? new(separator.Value) : new();

        try
        {
            foreach (Byte b in sourceBytes)
                await helper.WriteAsync(b);

            AssertTest(helper, sourceBytes);
        }
        finally
        {
            await helper.DisposeAsync();
        }
    }

    private static void AssertTest(BinaryConcatenator helper, Byte[] sourceBytes)
    {
        Byte[]? outputBytes1 = helper.ToArray(false);
        Byte[]? outputBytes2 = helper.ToArray(true);

        Assert.NotNull(outputBytes1);
        Assert.NotNull(outputBytes2);
        if (!helper.Separator.HasValue)
            Assert.Equal(sourceBytes, outputBytes1);
        else
            AssertWithSeparator(helper, sourceBytes, outputBytes1);
        Assert.Equal(outputBytes1, outputBytes2[..outputBytes1.Length]);
        Assert.Equal(outputBytes1.Length, outputBytes2.Length - 1);
        Assert.Equal(default, outputBytes2.Last());
    }
    private static void AssertWithSeparator(BinaryConcatenator helper, Byte[] sourceBytes, Byte[] outputBytes1)
    {
        for (Int32 i = 0; i < sourceBytes.Length; i++)
        {
            Int32 iByte = i * 2;
            Int32 iSeparator = iByte + 1;

            Assert.Equal(sourceBytes[i], outputBytes1[iByte]);
            if (iSeparator < outputBytes1.Length)
                Assert.Equal(helper.Separator, outputBytes1[iSeparator]);
        }
    }
}

