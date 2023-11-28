namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class SegmentTest
{
	[Fact]
	internal void EmptyTest()
	{
		const Int32 negativeOne = 1;
		CStringSequence seq = new(Array.Empty<CString>());
		Assert.Empty(seq);
		Assert.Throws<ArgumentOutOfRangeException>(() => seq[-1]);
		Assert.Throws<ArgumentOutOfRangeException>(() => seq[1]);
		Assert.Throws<ArgumentOutOfRangeException>(() => seq[negativeOne..]);
		Assert.Throws<ArgumentOutOfRangeException>(() => seq[1..]);
		Assert.Throws<ArgumentOutOfRangeException>(() => seq[..negativeOne]);
		Assert.Throws<ArgumentOutOfRangeException>(() => seq[..1]);
	}

	[Fact]
	internal void Test()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = SegmentTest.CreateSequence(handle, indices);
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

	[Fact]
	[SuppressMessage("Style", "IDE0057")]
	internal void SliceTest()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		CStringSequence seq = SegmentTest.CreateSequence(handle, indices);
		Int32 count = seq.Count;
		for (Int32 i = 0; i < count; i++)
		{
			Int32 start = Random.Shared.Next(i);
			CStringSequence subSeq = seq.Slice(start);
			for (Int32 j = 0; j < subSeq.Count; j++)
				Assert.Equal(seq[j + start], subSeq[j]);
		}
	}

	private static CStringSequence CreateSequence(TestMemoryHandle handle, IReadOnlyList<Int32> indices)
	{
		CString?[] values = TestSet.GetValues(indices, handle);
		CStringSequence seq = new(values);
		return seq;
	}
}