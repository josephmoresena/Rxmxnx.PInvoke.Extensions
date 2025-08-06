#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
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
	public void Test(Int32? length)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
		CString?[] values = TestSet.GetValues(indices, handle);
		CStringSequence seq = CStringSequence.Create(values, CreateTest.CreationMethod, values.Select(x =>
		{
			if (x is null || x.IsZero)
				return default(Int32?);
			return x.Length;
		}).ToArray());
		CreateTest.AssertSequence(seq, values);
		PInvokeAssert.Equal(seq, values.Select(c => c ?? CString.Zero));
		GC.Collect();
		PInvokeAssert.Equal(seq.Where(c => !CString.IsNullOrEmpty(c)), values.Where(c => !CString.IsNullOrEmpty(c)));
	}
	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void SpanTest(Boolean useFirstEmpty)
	{
		using TestMemoryHandle handle = new();
		IReadOnlyList<Int32> indices = TestSet.GetIndices(8);
		CString[] values = TestSet.GetValues(indices, handle).Select(c => !CString.IsNullOrEmpty(c) ? c : CString.Empty)
		                          .ToArray();

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

		PInvokeAssert.Equal(seq0, values.Take(1));
		PInvokeAssert.Equal(seq1, values.Take(2));
		PInvokeAssert.Equal(seq2, values.Take(3));
		PInvokeAssert.Equal(seq3, values.Take(4));
		PInvokeAssert.Equal(seq4, values.Take(5));
		PInvokeAssert.Equal(seq5, values.Take(6));
		PInvokeAssert.Equal(seq6, values.Take(7));
		PInvokeAssert.Equal(seq7, values);
	}

	private static void CreationMethod(Span<Byte> span, Int32 index, IReadOnlyList<CString?> values)
	{
		CString? value = values[index];
		PInvokeAssert.False(CString.IsNullOrEmpty(value));
		PInvokeAssert.Equal(span.Length, value!.Length);

		value.AsSpan().CopyTo(span);
	}
	private static void AssertSequence(CStringSequence seq, CString?[] values)
	{
		for (Int32 i = 0; i < seq.Count; i++)
		{
			if (seq[i].Length != 0)
				PInvokeAssert.Equal(values[i], seq[i]);
			else if (!seq[i].IsReference)
				PInvokeAssert.Equal(CString.Empty, values[i]);
			else
				PInvokeAssert.True(values[i]?.IsZero ?? true); // Null or Zero
		}
	}
}