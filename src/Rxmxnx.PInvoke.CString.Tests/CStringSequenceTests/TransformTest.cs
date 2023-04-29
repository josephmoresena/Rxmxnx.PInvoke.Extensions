namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class TransformTest
{
    [Fact]
    internal void EmptyTest()
    {
        FixedCStringSequence fseq = default;
        Assert.Empty(fseq.Values);
        Assert.Empty(fseq.ToArray());
        Assert.Null(fseq.ToString());

        fseq.Unload();
        Assert.Empty(fseq.Values);
        Assert.Empty(fseq.ToArray());
        Assert.Null(fseq.ToString());
    }

    [Fact]
    internal void BasicTest()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        try
        {
            CStringSequence seq = CreateSequence(handles, indices, out _);
            seq.Transform(AssertReference);
            Assert.Equal(seq, seq.Transform(CreateCopy));
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Fact]
    internal void Test()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        try
        {
            CStringSequence seq = CreateSequence(handles, indices, out CString?[] values);
            seq.Transform(values, AssertSequence);
            Assert.Equal(seq, seq.Transform(seq, CreateCopy));
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static void AssertReference(FixedCStringSequence fseq)
    {
        IReadOnlyList<CString> values = fseq.Values;
        IReadOnlyFixedMemory[] fValues = fseq.ToArray();

        for (Int32 i = 0; i < values.Count; i++)
            values[i].WithSafeFixed(fValues[i], AssertReference);
    }
    private static void AssertSequence(FixedCStringSequence fseq, IReadOnlyList<CString?> values)
    {
        for (Int32 i = 0; i < values.Count; i++)
            if (values[i] is CString value)
            {
                Assert.Equal(value, fseq.Values[i]);
                Assert.Equal(!fseq.Values[i].IsReference, ReferenceEquals(value, fseq.Values[i]));
            }
            else
                Assert.Equal(0, fseq.Values[i].Length);

        try
        {
            _ = fseq[-1];
        }
        catch (Exception ex)
        {
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        try
        {
            _ = fseq[values.Count];
        }
        catch (Exception ex)
        {
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }
    }
    private static void AssertReference(in IReadOnlyFixedMemory fcstr, IReadOnlyFixedMemory fValue)
        => Assert.Equal(fcstr, fValue);
    private static CStringSequence CreateCopy(FixedCStringSequence fseq)
    {
        CStringSequence seq = new(fseq.Values);
        IReadOnlyFixedMemory[] fmems = fseq.ToArray();

        for (Int32 i = 0; i < seq.Count; i++)
            Assert.Equal(seq[i], fmems[i].Bytes.ToArray());

        fseq.Unload();
        foreach (IReadOnlyFixedMemory mem in fmems)
            Assert.Throws<InvalidOperationException>(() => mem.Bytes.ToArray());

        return seq;
    }
    private static CStringSequence CreateCopy(FixedCStringSequence fseq, CStringSequence seq)
    {
        for (Int32 i = 0; i < seq.Count; i++)
            seq[i].WithSafeFixed(fseq[i], AssertReference);
        Assert.Equal(new CString(() => MemoryMarshal.AsBytes<Char>(seq.ToString())).ToString(), fseq.ToString());
        return new(fseq.Values);
    }
    private static CStringSequence CreateSequence(ICollection<GCHandle> handles, IReadOnlyList<Int32> indices, out CString?[] values)
    {
        values = new CString[indices.Count];
        for (Int32 i = 0; i < values.Length; i++)
            values[i] = TestSet.GetCString(indices[i], handles);
        CStringSequence seq = new(values);
        return seq;
    }
}
