namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class ParseTest
{
	[Fact]
	internal void EmptyTest()
	{
		Assert.Null(CStringSequence.Parse(null));
		Assert.Null(CStringSequence.Parse(null, out Boolean sameInstance));
		Assert.True(sameInstance);
		Assert.Empty(CStringSequence.Parse(String.Empty));
		Assert.Empty(CStringSequence.Parse(String.Empty, out sameInstance));
		Assert.False(sameInstance);
		Assert.Empty(CStringSequence.Create(Array.Empty<Char>()));
	}

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
		CStringSequence seq = new(values);
		CString[] nonEmpty = values.Where(c => !CString.IsNullOrEmpty(c)).ToArray()!;
		ParseTest.ExactParseTest(nonEmpty, seq.ToString());
		ParseTest.RandomParseTest(nonEmpty);
	}

	private static void ExactParseTest(CString[] values, String buffer)
	{
		CStringSequence seq0 = CStringSequence.Parse(buffer);
		CStringSequence seq1 = CStringSequence.Parse(buffer, out Boolean sameInstance);
		CStringSequence seq2 = CStringSequence.Create(buffer);
		CStringSequence seq3 = new(values);

		Assert.Equal(values, seq0);
		Assert.Equal(values, seq1);
		Assert.Equal(values, seq2);
		Assert.True(Object.ReferenceEquals(buffer, seq0.ToString()));
		Assert.True(Object.ReferenceEquals(buffer, seq1.ToString()));
		Assert.False(Object.ReferenceEquals(buffer, seq2.ToString()));
		Assert.True(sameInstance);
		Assert.Equal(seq3.ToString(), seq0.ToString());
		Assert.Equal(seq3.ToString(), seq2.ToString());
	}
	private static void RandomParseTest(CString[] values)
	{
		Int32 offset = Random.Shared.Next(0, Byte.MaxValue);
		Int32 padding = Random.Shared.Next(1, Byte.MaxValue);
		Int32 length = values.Select(c => c.Length).Sum() + values.Length;
		Int32 bufferLength = (length + padding + offset) / 2;
		String buffer = String.Create(bufferLength, (offset, values), ParseTest.RandomCreate);
		CStringSequence seq0 = CStringSequence.Parse(buffer);
		CStringSequence seq1 = CStringSequence.Parse(buffer, out Boolean sameInstance);
		CStringSequence seq2 = CStringSequence.Create(buffer);
		CStringSequence seq3 = new(values);

		Assert.Equal(values, seq0);
		Assert.Equal(values, seq1);
		Assert.Equal(values, seq2);
		Assert.False(sameInstance);
		Assert.False(Object.ReferenceEquals(buffer, seq0.ToString()));
		Assert.False(Object.ReferenceEquals(buffer, seq1.ToString()));
		Assert.False(Object.ReferenceEquals(buffer, seq2.ToString()));
		Assert.InRange(seq0.ToString().Length, values.Select(c => c.Length).Sum() / 2, bufferLength);
		Assert.Equal(seq3.ToString(), seq0.ToString());
		Assert.Equal(seq3.ToString(), seq1.ToString());
		Assert.Equal(seq3.ToString(), seq2.ToString());
	}
	private static void RandomCreate(Span<Char> span, (Int32 offset, CString[] values) arg)
	{
		Span<Byte> buffer = MemoryMarshal.AsBytes(span);
		MemoryMarshal.AsBytes(new CStringSequence(arg.values).ToString().AsSpan()).CopyTo(buffer[arg.offset..]);
	}
}