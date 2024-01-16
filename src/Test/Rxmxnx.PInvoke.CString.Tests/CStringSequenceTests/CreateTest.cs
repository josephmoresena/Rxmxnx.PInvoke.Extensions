namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class CreateTest
{
	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	internal void Test(Int32? length)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
		CString?[] values = TestSet.GetValues(indices, handle);
		CStringSequence seq =
			CStringSequence.Create(values, CreateTest.CreationMethod, values.Select(x => x?.Length).ToArray());
		CreateTest.AssertSequence(seq, values);
		Assert.Equal(seq, values.Select(c => c ?? CString.Zero));
		GC.Collect();
		Assert.Equal(seq.Where(c => !CString.IsNullOrEmpty(c)), values.Where(c => !CString.IsNullOrEmpty(c)));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void SpanTest(Boolean useFirstEmpty)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(8);
		CString[] values = TestSet.GetValues(indices, handle).Select(c => c ?? CString.Empty).ToArray();

		if (useFirstEmpty) values[0] = CString.Empty;

		ReadOnlySpan<Byte> span0 = values[0];
		CStringSequence seq0 = new(span0);
		CStringSequence seq1 = new(span0, values[1]);
		CStringSequence seq2 = new(span0, values[1], values[2]);
		CStringSequence seq3 = new(span0, values[1], values[2], values[3]);
		CStringSequence seq4 = new(span0, values[1], values[2], values[3], values[4]);
		CStringSequence seq5 = new(span0, values[1], values[2], values[3], values[4], values[5]);
		CStringSequence seq6 = new(span0, values[1], values[2], values[3], values[4], values[5], values[6]);
		CStringSequence seq7 = new(span0, values[1], values[2], values[3], values[4], values[5], values[6], values[7]);

		CreateTest.AssertSequence(seq0, values);
		CreateTest.AssertSequence(seq1, values);
		CreateTest.AssertSequence(seq2, values);
		CreateTest.AssertSequence(seq3, values);
		CreateTest.AssertSequence(seq4, values);
		CreateTest.AssertSequence(seq5, values);
		CreateTest.AssertSequence(seq6, values);
		CreateTest.AssertSequence(seq7, values);

		Assert.Equal(seq0, values.Take(1));
		Assert.Equal(seq1, values.Take(2));
		Assert.Equal(seq2, values.Take(3));
		Assert.Equal(seq3, values.Take(4));
		Assert.Equal(seq4, values.Take(5));
		Assert.Equal(seq5, values.Take(6));
		Assert.Equal(seq6, values.Take(7));
		Assert.Equal(seq7, values);
	}

	private static void CreationMethod(Span<Byte> span, Int32 index, IReadOnlyList<CString?> values)
	{
		CString? value = values[index];
		Assert.False(CString.IsNullOrEmpty(value));
		Assert.Equal(span.Length, value.Length);

		value.AsSpan().CopyTo(span);
	}
	private static void AssertSequence(CStringSequence seq, IReadOnlyList<CString?> values)
	{
		for (Int32 i = 0; i < seq.Count; i++)
		{
			if (seq[i].Length != 0 || !seq[i].IsReference)
				Assert.Equal(values[i], seq[i]);
			else
				Assert.Null(values[i]);
		}
	}
}