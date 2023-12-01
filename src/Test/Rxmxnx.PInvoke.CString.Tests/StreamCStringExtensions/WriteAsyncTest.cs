namespace Rxmxnx.PInvoke.Tests.StreamCStringExtensions;

[ExcludeFromCodeCoverage]
public sealed class WriteAsyncTest
{
	[Fact]
	internal async Task BasicTestAsync()
	{
		using TestMemoryHandle handle = new();
		List<WrittenCString> values = new();
		using MemoryStream strm = new();
		foreach (Task task in TestSet.GetIndices(10)
		                             .Select(i => WriteAsyncTest.AppendWrittenTask(
			                                     WrittenCString.Create(TestSet.GetCString(i, handle)), values, strm)))
			await task;

		WrittenCString.AssertWrite(strm, values, false);
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal async Task TestAsync(Boolean writeNullTermination)
	{
		using TestMemoryHandle handle = new();
		List<WrittenCString> values = new();
		using MemoryStream strm = new();
		foreach (Task task in TestSet.GetIndices(10)
		                             .Select(i => WriteAsyncTest.AppendWrittenTask(
			                                     WrittenCString.Create(TestSet.GetCString(i, handle)), values, strm,
			                                     writeNullTermination)))
			await task;

		WrittenCString.AssertWrite(strm, values, writeNullTermination);
	}

	[Fact]
	internal async Task RangeTestAsync()
	{
		using TestMemoryHandle handle = new();
		List<WrittenCString> values = new();
		using MemoryStream strm = new();
		foreach (Task task in TestSet.GetIndices(10)
		                             .Select(i => WriteAsyncTest.AppendWrittenTask(
			                                     WrittenCString.Create(TestSet.GetCString(i, handle), false), values,
			                                     strm, null)))
			await task;

		WrittenCString.AssertWrite(strm, values, false);
	}

	private static Task AppendWrittenTask(WrittenCString? written, ICollection<WrittenCString> values, MemoryStream strm)
	{
		if (written is null) return Task.CompletedTask;
		values.Add(written);
		return strm.WriteAsync(written.Value);
	}
	private static Task AppendWrittenTask(WrittenCString? written, ICollection<WrittenCString> values, Stream strm,
		Boolean? writeNullTermination)
	{
		if (written is null) return Task.CompletedTask;
		values.Add(written);
		return writeNullTermination.HasValue ?
			strm.WriteAsync(written.Value, writeNullTermination.Value) :
			strm.WriteAsync(written.Value, written.Start, written.Count);
	}
}