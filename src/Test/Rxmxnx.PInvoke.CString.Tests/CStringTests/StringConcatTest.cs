namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class StringConcatTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void LocalEmptyTest(Boolean emptyData)
    {
        String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : Array.Empty<String>();
        EmptyTest(values);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NullEmptyTest(Boolean emptyData)
    {
        String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : Array.Empty<String>();
        EmptyTest(values);
    }

    [Fact]
    internal void Test()
    {
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        NormalTest(strings);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task LocalEmptyTestAsync(Boolean emptyData)
    {
        String?[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : Array.Empty<String>();
        await EmptyTestAsync(values);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task NullEmptyTestAsync(Boolean emptyData)
    {
        String?[] values = !emptyData ? Enumerable.Repeat<String?>(default, 3).ToArray() : Array.Empty<String>();
        await EmptyTestAsync(values);
    }

    [Fact]
    internal async Task TestAsync()
    {
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        await NormalTestAsync(strings);
    }

    private static async Task NormalTestAsync(String?[] strings)
    {
        String expectedCString = String.Concat(strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.ConcatAsync(strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task EmptyTestAsync(String?[] values)
    {
        CString resultCString = await CString.ConcatAsync(values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static void NormalTest(String?[] strings)
    {
        String expectedCString = String.Concat(strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Concat(strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static void EmptyTest(String?[] values)
    {
        CString resultCString = CString.Concat(values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
}

