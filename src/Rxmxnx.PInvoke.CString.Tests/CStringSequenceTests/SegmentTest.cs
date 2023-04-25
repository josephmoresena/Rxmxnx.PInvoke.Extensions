namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class SegmentTest
{
    [Fact]
    internal void EmptyTest()
    {
        CStringSequence seq = new(Array.Empty<CString>());
        Assert.Empty(seq);
        Assert.Throws<ArgumentOutOfRangeException>(() => seq[-1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => seq[1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => seq[-1..]);
        Assert.Throws<ArgumentOutOfRangeException>(() => seq[1..]);
        Assert.Throws<ArgumentOutOfRangeException>(() => seq[..-1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => seq[..1]);
    }

    [Fact]
    internal void Test()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i, true)).ToArray();
        try
        {
            CStringSequence seq = CreateSequence(handles, indices);
            Int32 count = seq.Count;
            for (Int32 i = 0; i < count; i++)
            {
                Int32 start = Random.Shared.Next(i, count);
                Int32 end = Random.Shared.Next(start, count + 1);
                CStringSequence subSeq = seq[start..end];
                for (Int32 j = 0; j < subSeq.Count; j++)
                    Assert.Equal(seq[j + start], subSeq[j]);
            }
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Fact]
    internal void SliceTest()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i, true)).ToArray();
        try
        {
            CStringSequence seq = CreateSequence(handles, indices);
            Int32 count = seq.Count;
            for (Int32 i = 0; i < count; i++)
            {
                Int32 start = Random.Shared.Next(i);
                CStringSequence subSeq = seq.Slice(start);
                for (Int32 j = 0; j < subSeq.Count; j++)
                    Assert.Equal(seq[j + start], subSeq[j]);
            }
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static CStringSequence CreateSequence(ICollection<GCHandle> handles, IReadOnlyList<Int32> indices)
    {
        CString?[] values = new CString[indices.Count];
        for (Int32 i = 0; i < values.Length; i++)
            values[i] = TestSet.GetCString(indices[i], handles);
        CStringSequence seq = new(values);
        return seq;
    }
}

