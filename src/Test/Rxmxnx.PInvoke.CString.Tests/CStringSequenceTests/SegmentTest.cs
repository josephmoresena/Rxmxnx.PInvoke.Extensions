#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class SegmentTest
{
	[Fact]
	public void EmptyTest()
	{
		const Int32 zero = 0;
		Int32 varIndex = 1;
		CStringSequence seq = new(Array.Empty<CString>());

		PInvokeAssert.Empty(seq);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq[varIndex]);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq[varIndex..]);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq[..varIndex]);

		varIndex = -1;
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq[varIndex]);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq[varIndex..]);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq[..varIndex]);

		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq.Slice(varIndex, zero));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq.Slice(zero, 1));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => seq.Slice(varIndex, 1));
	}

	[Fact]
	public void Test()
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
				PInvokeAssert.Equal(seq[j + start], subSeq[j]);
		}
	}

	[Fact]
	[SuppressMessage("Style", "IDE0057")]
	public void SliceTest()
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
				PInvokeAssert.Equal(seq[j + start], subSeq[j]);
		}
	}

	private static CStringSequence CreateSequence(TestMemoryHandle handle, IReadOnlyList<Int32> indices)
	{
		CString?[] values = TestSet.GetValues(indices, handle);
		CStringSequence seq = new(values);
		return seq;
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}