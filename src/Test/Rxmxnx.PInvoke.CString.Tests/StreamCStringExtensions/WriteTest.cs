namespace Rxmxnx.PInvoke.Tests.StreamCStringExtensions;

[ExcludeFromCodeCoverage]
public sealed class WriteTest
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    internal void Test(Boolean writeNullTermination)
    {
        List<GCHandle> handles = new();
        try
        {
            List<WritedCString> values = new();
            using MemoryStream strm = new();
            foreach (Int32 index in TestSet.GetIndices(10))
                if (WritedCString.Create(TestSet.GetCString(index, handles)) is WritedCString writed)
                {
                    values.Add(writed);
                    strm.Write(writed.Value, writeNullTermination);
                }

            WritedCString.AssertWrite(strm, values, writeNullTermination);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    [Fact]
    internal void RangeTest()
    {
        List<GCHandle> handles = new();
        try
        {
            List<WritedCString> values = new();
            using MemoryStream strm = new();
            foreach (Int32 index in TestSet.GetIndices(10))
                if (WritedCString.Create(TestSet.GetCString(index, handles), false) is WritedCString writed)
                {
                    values.Add(writed);
                    strm.Write(writed.Value, writed.Start, writed.Count);
                }

            WritedCString.AssertWrite(strm, values, false);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }
}