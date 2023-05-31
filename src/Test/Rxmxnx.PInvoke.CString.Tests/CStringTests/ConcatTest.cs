namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class ConcatTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void LocalEmptyTest(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        EmptyTest(values);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void ReferenceEmptyTest(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        EmptyTest(values);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NullEmptyTest(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        EmptyTest(values);
    }

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

            NormalTest(strings, values);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Fact]
    internal void BinaryTest()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        try
        {
            Byte[]?[] values = new Byte[]?[strings.Length];
            for (Int32 i = 0; i < values.Length; i++)
                if (TestSet.GetCString(indices[i], handles) is CString cstr)
                    values[i] = cstr.ToArray();

            NormalTest(strings, values);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Fact]
    internal void SpanTest()
    {
        IReadOnlyList<Int32> indices = TestSet.GetIndices(8);
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();

        Assert.Equal(String.Concat(strings[..2]),
            CString.Concat(
                TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1])).ToString());
        Assert.Equal(String.Concat(strings[..3]),
            CString.Concat(
                TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]), TestSet.GetSpan(indices[2])).ToString());
        Assert.Equal(String.Concat(strings[..4]),
            CString.Concat(
               TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]), TestSet.GetSpan(indices[2]),
               TestSet.GetSpan(indices[3])).ToString());
        Assert.Equal(String.Concat(strings[..5]),
            CString.Concat(
                TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]), TestSet.GetSpan(indices[2]),
                TestSet.GetSpan(indices[3]), TestSet.GetSpan(indices[4])).ToString());
        Assert.Equal(String.Concat(strings[..6]),
            CString.Concat(
                TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]), TestSet.GetSpan(indices[2]),
                TestSet.GetSpan(indices[3]), TestSet.GetSpan(indices[4]), TestSet.GetSpan(indices[5])).ToString());
        Assert.Equal(String.Concat(strings[..7]),
            CString.Concat(
                TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]), TestSet.GetSpan(indices[2]),
                TestSet.GetSpan(indices[3]), TestSet.GetSpan(indices[4]), TestSet.GetSpan(indices[5]),
                TestSet.GetSpan(indices[6])).ToString());
        Assert.Equal(String.Concat(strings),
            CString.Concat(
                TestSet.GetSpan(indices[0]), TestSet.GetSpan(indices[1]), TestSet.GetSpan(indices[2]),
                TestSet.GetSpan(indices[3]), TestSet.GetSpan(indices[4]), TestSet.GetSpan(indices[5]),
                TestSet.GetSpan(indices[6]), TestSet.GetSpan(indices[7])).ToString());

        Assert.Same(CString.Empty, CString.Concat(
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
        Assert.Same(CString.Empty, CString.Concat(
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
        Assert.Same(CString.Empty, CString.Concat(
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
            ReadOnlySpan<Byte>.Empty));
        Assert.Same(CString.Empty, CString.Concat(
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
        Assert.Same(CString.Empty, CString.Concat(
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
        Assert.Same(CString.Empty, CString.Concat(
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
            ReadOnlySpan<Byte>.Empty));
        Assert.Same(CString.Empty, CString.Concat(
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
            ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task LocalEmptyTestAsync(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        await EmptyTestAsync(values);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task ReferenceEmptyTestAsync(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        await EmptyTestAsync(values);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task NullEmptyTestAsync(Boolean emptyData)
    {
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        await EmptyTestAsync(values);
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

            await NormalTestAsync(strings, values);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Fact]
    internal async Task BinaryTestAsync()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i)).ToArray();
        try
        {
            Byte[]?[] values = new Byte[]?[strings.Length];
            for (Int32 i = 0; i < values.Length; i++)
                if (TestSet.GetCString(indices[i], handles) is CString cstr)
                    values[i] = cstr.ToArray();

            await NormalTestAsync(strings, values);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static async Task NormalTestAsync(String?[] strings, CString?[] values)
    {
        String expectedCString = String.Concat(strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.ConcatAsync(values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task NormalTestAsync(String?[] strings, Byte[]?[] values)
    {
        String expectedCString = String.Concat(strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = await CString.ConcatAsync(values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static async Task EmptyTestAsync(CString?[] values)
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
    private static void NormalTest(String?[] strings, CString?[] values)
    {
        String expectedCString = String.Concat(strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Concat(values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static void NormalTest(String?[] strings, Byte[]?[] values)
    {
        String expectedCString = String.Concat(strings);
        Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

        CString resultCString = CString.Concat(values);
        String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

        Assert.Equal(expectedCString, resultCStringCString);
        Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
    }
    private static void EmptyTest(CString?[] values)
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

