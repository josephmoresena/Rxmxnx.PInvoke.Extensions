namespace Rxmxnx.PInvoke.Tests.StreamCStringExtensions;

[ExcludeFromCodeCoverage]
public sealed class WriteTest
{
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void Test(Boolean writeNullTermination)
	{
		using TestMemoryHandle handle = new();
		List<WrittenCString> values = new();
		using MemoryStream strm = new();
		TestSet.GetIndices(10)
		       .ForEach(i => WriteTest.AppendWritten(WrittenCString.Create(TestSet.GetCString(i, handle)), values, strm,
		                                             writeNullTermination));
		WrittenCString.AssertWrite(strm, values, writeNullTermination);
	}

	[Fact]
	internal void RangeTest()
	{
		using TestMemoryHandle handle = new();
		List<WrittenCString> values = new();
		using MemoryStream strm = new();
		TestSet.GetIndices(10)
		       .ForEach(i => WriteTest.AppendWritten(WrittenCString.Create(TestSet.GetCString(i, handle), false),
		                                             values, strm));
		WrittenCString.AssertWrite(strm, values, false);
	}

	private static void AppendWritten(WrittenCString? written, ICollection<WrittenCString> values, Stream strm,
		Boolean? writeNullTermination = default)
	{
		if (written is null) return;
		values.Add(written);
		if (writeNullTermination.HasValue)
			strm.Write(written.Value, writeNullTermination.Value);
		else
			strm.Write(written.Value, written.Start, written.Count);
	}
}