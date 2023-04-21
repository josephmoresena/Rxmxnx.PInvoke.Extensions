namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

public sealed class BasicTests
{
    [Fact]
    internal void Test()
    {
        List<GCHandle> handles = new();
        IReadOnlyList<Int32> indices = TestSet.GetIndices();
        String?[] strings = indices.Select(i => TestSet.GetString(i, true)).ToArray();
        CStringSequence seqRef = new(strings);
        try
        {
            CString?[] values = new CString[strings.Length];
            for (Int32 i = 0; i < values.Length; i++)
                values[i] = TestSet.GetCString(indices[i], handles);
            CStringSequence seq = new(values);

            Assert.Equal(seqRef, seq);
            Assert.Equal(strings.Select(c => (CString?)c ?? CString.Zero), seq);
            Assert.Equal(seqRef.Count, seq.Count);
            Assert.Equal(seqRef.ToString(), seq.ToString());
            Assert.Equal(seqRef.ToString().GetHashCode(), seq.GetHashCode());

            AssertSequence(seq, strings, values);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static unsafe void AssertSequence(CStringSequence seq, String?[] strings, CString?[] values)
    {
        for (Int32 i = 0; i < seq.Count; i++)
            if (seq[i].Length != 0 || !seq[i].IsReference)
            {
                Assert.Equal(values[i], seq[i]);
                Assert.Equal(strings[i], seq[i].ToString());
            }
            else
            {
                Assert.Same(seq[i], CString.Zero);
                Assert.Equal(String.Empty, seq[i].ToString());
                if (values[i] is not null)
                    fixed (void* ptr = values[i]!.AsSpan())
                        Assert.Equal(IntPtr.Zero, new(ptr));
                Assert.Null(strings[i]);
            }
    }
}

