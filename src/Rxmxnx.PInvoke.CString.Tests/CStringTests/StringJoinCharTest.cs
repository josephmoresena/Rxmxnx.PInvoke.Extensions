namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class StringJoinCharTest
{
    private static IFixture fixture = new Fixture();

    [Fact]
    internal void Test()
    {
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        ArrayTest(GetByteSeparator(), strings);
        ArrayRangeTest(GetByteSeparator(), strings);
        EnumerableTest(GetByteSeparator(), strings);
    }

    [Fact]
    internal async Task TestAsync()
    {
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        await ArrayTestAsync(GetByteSeparator(), strings);
        await ArrayRangeTestAsync(GetByteSeparator(), strings);
        await EnumerableTestAsync(GetByteSeparator(), strings);
    }

    private static void ArrayTest(Char separator, String?[] strings)
    {
        String strSeparator = new(new Char[] { separator });
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
        Assert.Same(CString.Empty, CString.Join(separator, Array.Empty<String?>()));
    }
    private static void EnumerableTest(Char separator, IEnumerable<String?> strings)
    {
        String strSeparator = new(new Char[] { separator });
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
        Assert.Same(CString.Empty, CString.Join(separator, Array.Empty<String?>().ToList()));
    }
    private static void ArrayRangeTest(Char separator, String?[] strings)
    {
        Int32 startIndex = Random.Shared.Next(0, strings.Length);
        Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
        String strSeparator = new(new Char[] { separator });
        String expectedCString = String.Join(strSeparator, strings, startIndex, count);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, strings, startIndex, count);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
        Assert.Same(CString.Empty, CString.Join(separator, strings, 0, 0));
    }

    private static async Task ArrayTestAsync(Char separator, String?[] strings)
    {
        String strSeparator = new (new Char[] { separator });
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task EnumerableTestAsync(Char separator, IEnumerable<String?> strings)
    {
        String strSeparator = new(new Char[] { separator });
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, strings);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task ArrayRangeTestAsync(Char separator, String?[] strings)
    {
        Int32 startIndex = Random.Shared.Next(0, strings.Length);
        Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
        String strSeparator = new(new Char[] { separator });
        String expectedCString = String.Join(strSeparator, strings, startIndex, count);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, strings, startIndex, count);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static Char GetByteSeparator() => fixture.Create<Char>();
}

