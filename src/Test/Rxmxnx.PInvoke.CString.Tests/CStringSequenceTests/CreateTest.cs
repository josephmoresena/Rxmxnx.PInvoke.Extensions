namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class CreateTest
{
	[Fact]
	internal void Test()
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices();
		CString?[] values = TestSet.GetValues(indices, handle);
		CStringSequence seq =
			CStringSequence.Create(values, CreateTest.CreationMethod, values.Select(x => x?.Length).ToArray());
		CreateTest.AssertSequence(seq, values);
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
	}

	private static void CreationMethod(Span<Byte> span, Int32 index, IReadOnlyList<CString?> values)
	{
		CString? value = values[index];
		Assert.False(CString.IsNullOrEmpty(value));
		Assert.Equal(span.Length, value.Length);

		value.AsSpan().CopyTo(span);
	}
	private static void AssertSequence(CStringSequence seq, CString?[] values)
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