namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class JoinEmptyTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void LocalEmptySpanTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        ReadOnlySpan<Byte> separator = default;
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void ReferenceEmptySpanTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        ReadOnlySpan<Byte> separator = default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NullEmptySpanTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        ReadOnlySpan<Byte> separator = default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void LocalEmptyTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = !emptyData? CString.Empty : default;
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void ReferenceEmptyTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = !emptyData ? CString.Empty : default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void NullEmptyTest(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = !emptyData ? CString.Empty : default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        try
        {
            Test(separator, values);
            Test(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task LocalEmptyTestAsync(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = !emptyData ? CString.Empty : default;
        CString?[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : Array.Empty<CString>();
        try
        {
            await TestAsync(separator, values);
            await TestAsync(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task ReferenceEmptyTestAsync(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = !emptyData ? CString.Empty : default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString>(new(IntPtr.Zero, default), 3).ToArray() : Array.Empty<CString>();
        try
        {
            await TestAsync(separator, values);
            await TestAsync(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal async Task NullEmptyTestAsync(Boolean emptyData)
    {
        List<GCHandle> handles = new();
        CString? separator = !emptyData ? CString.Empty : default;
        CString?[] values = !emptyData ? Enumerable.Repeat<CString?>(default, 3).ToArray() : Array.Empty<CString>();
        try
        {
            await TestAsync(separator, values);
            await TestAsync(separator, values.ToList());
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static void Test(ReadOnlySpan<Byte> separator, CString?[] values)
    {
        CString resultCString = CString.Join(separator, values);
        CString resultCString2 = CString.Join(separator, values, 0, 1);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);

        Assert.NotNull(resultCString2);
        Assert.Equal(CString.Empty, resultCString2);
        Assert.Same(CString.Empty, resultCString2);
        Assert.True(resultCString2.IsNullTerminated);
        Assert.False(resultCString2.IsReference);
        Assert.False(resultCString2.IsSegmented);
        Assert.False(resultCString2.IsFunction);
    }
    private static void Test(ReadOnlySpan<Byte> separator, IEnumerable<CString?> values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static void Test(CString? separator, CString?[] values)
    {
        CString resultCString = CString.Join(separator, values);
        CString resultCString2 = CString.Join(separator, values, 0, 1);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);

        Assert.NotNull(resultCString2);
        Assert.Equal(CString.Empty, resultCString2);
        Assert.Same(CString.Empty, resultCString2);
        Assert.True(resultCString2.IsNullTerminated);
        Assert.False(resultCString2.IsReference);
        Assert.False(resultCString2.IsSegmented);
        Assert.False(resultCString2.IsFunction);
    }
    private static void Test(CString? separator, IEnumerable<CString?> values)
    {
        CString resultCString = CString.Join(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
    private static async Task TestAsync(CString? separator, CString?[] values)
    {
        CString resultCString = await CString.JoinAsync(separator, values);
        CString resultCString2 = await CString.JoinAsync(separator, values, 0, 1);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);

        Assert.NotNull(resultCString2);
        Assert.Equal(CString.Empty, resultCString2);
        Assert.Same(CString.Empty, resultCString2);
        Assert.True(resultCString2.IsNullTerminated);
        Assert.False(resultCString2.IsReference);
        Assert.False(resultCString2.IsSegmented);
        Assert.False(resultCString2.IsFunction);
    }
    private static async Task TestAsync(CString? separator, IEnumerable<CString?> values)
    {
        CString resultCString = await CString.JoinAsync(separator, values);
        Assert.NotNull(resultCString);
        Assert.Equal(CString.Empty, resultCString);
        Assert.Same(CString.Empty, resultCString);
        Assert.True(resultCString.IsNullTerminated);
        Assert.False(resultCString.IsReference);
        Assert.False(resultCString.IsSegmented);
        Assert.False(resultCString.IsFunction);
    }
}
