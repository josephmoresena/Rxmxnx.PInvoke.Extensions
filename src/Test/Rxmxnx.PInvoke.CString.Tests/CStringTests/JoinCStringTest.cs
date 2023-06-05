namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class JoinCStringTest
{
    [Fact]
    internal void Test()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        try
        {
            CString?[] values = new CString[strings.Length];
            for (Int32 i = 0; i < values.Length; i++)
                values[i] = TestSet.GetCString(indices[i], handles);

            ArrayTest(GetCStringSeparator(handles), strings, values);
            ArrayRangeTest(GetCStringSeparator(handles), strings, values);
            EnumerableTest(GetCStringSeparator(handles), strings, values);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Fact]
    internal async Task TestAsync()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        try
        {
            CString?[] values = new CString[strings.Length];
            for (Int32 i = 0; i < values.Length; i++)
                values[i] = TestSet.GetCString(indices[i], handles);

            await ArrayTestAsync(GetCStringSeparator(handles), strings, values);
            await ArrayRangeTestAsync(GetCStringSeparator(handles), strings, values);
            await EnumerableTestAsync(GetCStringSeparator(handles), strings, values);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static void ArrayTest(CString? separator, String?[] strings, CString?[] values)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static void EnumerableTest(CString? separator, String?[] strings, IEnumerable<CString?> values)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static void ArrayRangeTest(CString? separator, String?[] strings, CString?[] values)
    {
        Int32 startIndex = Random.Shared.Next(0, strings.Length);
        Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings, startIndex, count);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Join(separator, values, startIndex, count);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }

    private static async Task ArrayTestAsync(CString? separator, String?[] strings, CString?[] values)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task EnumerableTestAsync(CString? separator, String?[] strings, IEnumerable<CString?> values)
    {
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task ArrayRangeTestAsync(CString? separator, String?[] strings, CString?[] values)
    {
        Int32 startIndex = Random.Shared.Next(0, strings.Length);
        Int32 count = Random.Shared.Next(startIndex, strings.Length) - startIndex;
        String? strSeparator = separator?.ToString();
        String expectedCString = String.Join(strSeparator, strings, startIndex, count);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.JoinAsync(separator, values, startIndex, count);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static CString? GetCStringSeparator(ICollection<GCHandle> handles)
    {
        Int32 result = Random.Shared.Next(-3, TestSet.Utf16Text.Count);
        return TestSet.GetCString(result, handles);
    }
}
