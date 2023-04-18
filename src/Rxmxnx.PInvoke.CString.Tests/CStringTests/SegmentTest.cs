namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class SegmentTests
{
    [Fact]
    internal void InvalidTest()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[-1..]);
        Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[1..]);
        Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..-1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..1]);
    }

    [Fact]
    internal async Task TestAsync()
    {
        CancellationTokenSource source = new();
        List<Task> testTasks = new();

        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
        {
            testTasks.Add(SegmentTestAsync(TestSet.Utf16Text[i], (CString)TestSet.Utf16Text[i], source.Token));
            testTasks.Add(SegmentTestAsync(TestSet.Utf16Text[i], new(TestSet.Utf8Text[i]), source.Token));
            testTasks.Add(SegmentTestAsync(TestSet.Utf16Text[i], TestSet.Utf8Bytes[i], source.Token));
            testTasks.Add(SegmentTestAsync(TestSet.Utf16Text[i], TestSet.Utf8NullTerminatedBytes[i], source.Token));
            testTasks.Add(FixedSegmentAsync(TestSet.Utf16Text[i], TestSet.Utf8Bytes[i], source.Token));
            testTasks.Add(FixedSegmentAsync(TestSet.Utf16Text[i], TestSet.Utf8NullTerminatedBytes[i], source.Token));
        }
        try
        {
            await Task.WhenAll(testTasks);
        }
        catch (Exception)
        {
            source.Cancel();
            throw;
        }
    }

    private static Task SegmentTestAsync(String str, CString cstr, CancellationToken token)
        => Task.Run(() => Test(str, cstr), token);
    private static Task FixedSegmentAsync(String str, Byte[] bytes, CancellationToken token)
        => Task.Run(() => FixedTest(str, bytes), token);
    private unsafe static void FixedTest(String str, Byte[] bytes)
    {
        fixed (void* ptr = bytes)
        {
            Test(str, new(new IntPtr(ptr), bytes.Length));
        }
    }
    private static void Test(String str, CString cstr)
    {
        IReadOnlyList<Int32> strIndex = DecodedRune.GetIndices(str);
        IReadOnlyList<Int32> cstrIndex = DecodedRune.GetIndices(cstr);
        Int32 count = strIndex.Count;

        Assert.Equal(count, cstrIndex.Count);
        for (Int32 i = 0; i < count; i++)
        {
            Int32 start = Random.Shared.Next(i, count);
            Int32 end = Random.Shared.Next(start, count + 1);

            Int32 strStart = strIndex[start];
            Int32 strEnd = end < strIndex.Count ? strIndex[end] : str.Length;
            Int32 cstrStart = cstrIndex[start];
            Int32 cstrEnd = end < cstrIndex.Count ? cstrIndex[end] : cstr.Length;

            String strSeg = str[strStart..strEnd];
            CString cstrSeg = cstr[cstrStart..cstrEnd];

            Assert.Equal(strSeg, cstrSeg.ToString());
            Test(cstr, cstrSeg, cstrStart, cstrEnd);
            Test(cstr, cstr.Slice(cstrStart), cstrStart, cstr.Length);

            if (!cstr.IsSegmented && !cstr.IsReference)
                Test(strSeg, cstrSeg);
        }
    }
    private static void Test(CString cstr, CString cstrSeg, Int32 cstrStart, Int32 cstrEnd)
    {
        if (cstrSeg.Length == 0)
        {
            Assert.Same(CString.Empty, cstrSeg);
            Assert.False(cstrSeg.IsFunction);
            Assert.False(cstrSeg.IsReference);
            Assert.True(cstrSeg.IsNullTerminated);
        }
        else
        {
            Assert.Equal(cstr.IsReference, cstrSeg.IsReference);
            Assert.Equal(cstr.IsFunction, cstrSeg.IsFunction);
            Assert.Equal(cstr.IsSegmented || !cstr.IsReference && (cstrStart != 0 || cstrEnd != cstr.Length), cstrSeg.IsSegmented);
            Assert.Equal(cstrEnd == cstr.Length && cstr.IsNullTerminated, cstrSeg.IsNullTerminated);

            for (Int32 i = 0; i < cstrSeg.Length; i++)
                Assert.Equal(cstr[i + cstrStart], cstrSeg[i]);

            if (!cstr.IsSegmented && !cstr.IsFunction && !cstr.IsReference)
                if (!cstrSeg.IsSegmented)
                    Assert.Equal(CString.GetBytes(cstr), CString.GetBytes(cstrSeg));
                else
                    Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstrSeg));
        }
        CloneTest(cstrSeg);
    }
    private static void CloneTest(CString cstrSeg)
    {
        CString cloneSeg = (CString)cstrSeg.Clone();
        Assert.Equal(cstrSeg.ToString(), cloneSeg.ToString());
        Assert.Equal(cstrSeg.Length, cloneSeg.Length);
        Assert.False(cloneSeg.IsFunction);
        Assert.False(cloneSeg.IsReference);
        Assert.False(cloneSeg.IsSegmented);
        Assert.True(cloneSeg.IsNullTerminated);

        Assert.Equal(cstrSeg.Length + 1, CString.GetBytes(cloneSeg).Length);
    }
}