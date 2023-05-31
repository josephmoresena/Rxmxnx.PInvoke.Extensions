namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class WithSafeTransformTest
{
    [Fact]
    internal void EmptyTest()
    {
        FixedCStringSequence fseq = default;
        ReadOnlyFixedMemoryList fml = fseq;
        Assert.Empty(fseq.Values);
        Assert.Empty(fseq.ToArray());
        Assert.Null(fseq.ToString());

        Assert.Equal(0, fml.Count);
        Assert.True(fml.IsEmpty);
        Assert.Empty(fml.ToArray());

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
            seq.WithSafeTransform(AssertReference);
            seq.WithSafeFixed(AssertReference);
            Assert.Equal(seq, seq.WithSafeTransform(CreateCopy));
            Assert.Equal(seq.Where(c => !CString.IsNullOrEmpty(c)), seq.WithSafeFixed(GetNonEmptyValues));
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
            seq.WithSafeTransform(values, AssertSequence);
            seq.WithSafeFixed(seq, AssertSequence);
            Assert.Equal(seq, seq.WithSafeTransform(seq, CreateCopy));
            Assert.Equal(seq.ToString(), seq.WithSafeFixed(seq, CreateCopy).ToString());
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
    private static void AssertReference(ReadOnlyFixedMemoryList fml)
    {
        Int32 offset = 0;
        IntPtr ptr = IntPtr.Zero;

        for (Int32 i = 0; i < fml.Count; i++)
            if (!fml[i].Bytes.IsEmpty)
            {
                if (ptr == IntPtr.Zero)
                    ptr = fml[i].Pointer;

                Assert.Equal(ptr + offset, fml[i].Pointer);
                offset += fml[i].Bytes.Length + 1;
            }
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
    private static unsafe void AssertSequence(ReadOnlyFixedMemoryList fml, CStringSequence seq)
    {
        Int32 offset = 0;
        IntPtr ptr = IntPtr.Zero;

        for (Int32 i = 0; i < fml.Count; i++)
            if (!fml[i].Bytes.IsEmpty)
            {
                if (ptr == IntPtr.Zero)
                    ptr = fml[i].Pointer;

                Assert.Equal(seq[i].Length, fml[i].Bytes.Length);
                fixed (void* ptr2 = &MemoryMarshal.GetReference(seq[i].AsSpan()))
                    Assert.Equal(new(ptr2), ptr + offset);
                offset += fml[i].Bytes.Length + 1;
            }
            else
                Assert.True(CString.IsNullOrEmpty(seq[i]));

        try
        {
            _ = fml[-1];
        }
        catch (Exception ex)
        {
            Assert.IsType<ArgumentOutOfRangeException>(ex);
        }

        try
        {
            _ = fml[seq.Count];
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
    private static unsafe CStringSequence CreateCopy(ReadOnlyFixedMemoryList fml, CStringSequence seq)
    {
        Int32 offset = 0;
        IntPtr ptr = IntPtr.Zero;

        List<CString> cstr = new();
        for (Int32 i = 0; i < fml.Count; i++)
            if (!fml[i].Bytes.IsEmpty)
            {
                if (ptr == IntPtr.Zero)
                    ptr = fml[i].Pointer;

                Assert.Equal(seq[i].Length, fml[i].Bytes.Length);
                fixed (void* ptr2 = &MemoryMarshal.GetReference(seq[i].AsSpan()))
                    Assert.Equal(new(ptr2), ptr + offset);
                offset += fml[i].Bytes.Length + 1;
                cstr.Add(new(fml[i].Bytes));
            }
            else
                Assert.True(CString.IsNullOrEmpty(seq[i]));

        return new(cstr);
    }
    private static CStringSequence CreateSequence(ICollection<GCHandle> handles, IReadOnlyList<Int32> indices, out CString?[] values)
    {
        values = new CString[indices.Count];
        for (Int32 i = 0; i < values.Length; i++)
            values[i] = TestSet.GetCString(indices[i], handles);
        CStringSequence seq = new(values);
        return seq;
    }
    private static IEnumerable<CString> GetNonEmptyValues(ReadOnlyFixedMemoryList fml)
    {
        List<CString> result = new();
        foreach (IReadOnlyFixedMemory fmem in fml)
            if (!fmem.Bytes.IsEmpty)
                result.Add(new(fmem.Bytes));
        return result;
    }
}
