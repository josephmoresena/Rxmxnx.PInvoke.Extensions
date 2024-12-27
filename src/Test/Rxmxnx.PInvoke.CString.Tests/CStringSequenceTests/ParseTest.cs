namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed class ParseTest
{
	[Fact]
	internal void EmptyTest()
	{
		Assert.Null(CStringSequence.Parse(null));

		String nullString = new(Enumerable.Repeat('\0', Random.Shared.Next(0, Byte.MaxValue)).ToArray());
		CStringSequence seq0 = CStringSequence.Parse(String.Empty);
		CStringSequence seq1 = CStringSequence.Create(Array.Empty<Char>());
		CStringSequence seq2 = CStringSequence.Parse(nullString);
		CStringSequence seq3 = CStringSequence.Create(nullString);

		Assert.Empty(seq0);
		Assert.Equal(0, seq0.ToString().Length);
		Assert.Empty(seq1);
		Assert.Equal(0, seq1.ToString().Length);
		Assert.Empty(seq2);
		Assert.Equal(0, seq2.ToString().Length);
		Assert.Empty(seq3);
		Assert.Equal(0, seq3.ToString().Length);
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
		CString[] nonEmpty = values.Where(c => !CString.IsNullOrEmpty(c) && c.All(b => b != 0x0)).ToArray()!;
		ParseTest.ExactParseTest(nonEmpty, seq.ToString());
		ParseTest.RandomParseTest(nonEmpty);
	}

	private static void ExactParseTest(CString[] values, String buffer)
	{
		String nullEndBuffer = buffer +
			new String(Enumerable.Repeat('\0', Random.Shared.Next(0, Byte.MaxValue)).ToArray());
		CStringSequence seq0 = CStringSequence.Parse(buffer);
		CStringSequence seq1 = CStringSequence.Parse(nullEndBuffer);
		CStringSequence seq2 = CStringSequence.Create(buffer);
		CStringSequence seq3 = new(values);

		Assert.Equal(values, seq0);
		Assert.Equal(values, seq1);
		Assert.Equal(values, seq2);
		Assert.True(Object.ReferenceEquals(buffer, seq0.ToString()));
		Assert.True(Object.ReferenceEquals(nullEndBuffer, seq1.ToString()));
		Assert.False(Object.ReferenceEquals(buffer, seq2.ToString()));
		Assert.Equal(seq3.ToString(), seq0.ToString());
		Assert.Equal(nullEndBuffer.Length, seq1.ToString().Length);
		Assert.Equal(seq3.ToString(), seq2.ToString());
	}
	private static void RandomParseTest(CString[] values)
	{
		Int32 offset = Random.Shared.Next(0, Byte.MaxValue);
		Int32 padding = Random.Shared.Next(-1, Byte.MaxValue);
		Int32 length = values.Select(c => c.Length + 1).Sum();
		Int32 totalBytes = length + padding + offset;
		Int32 totalChars = totalBytes / sizeof(Char) + totalBytes % sizeof(Char);
		String buffer = String.Create(totalChars, (offset, values), ParseTest.RandomCreate);
		CStringSequence seq0 = CStringSequence.Parse(buffer);
		CStringSequence seq2 = CStringSequence.Create(buffer);
		CStringSequence seq3 = new(values);

		Assert.Equal(values, seq0);
		Assert.Equal(values, seq2);
		Assert.Equal(offset == 0, Object.ReferenceEquals(buffer, seq0.ToString()));
		Assert.False(Object.ReferenceEquals(buffer, seq2.ToString()));
		Assert.InRange(seq0.ToString().Length, values.Select(c => c.Length).Sum() / 2, totalChars);
		Assert.Equal(offset != 0 ? seq3.ToString() : buffer, seq0.ToString());
		Assert.Equal(seq3.ToString(), seq2.ToString());
	}
	private static void RandomCreate(Span<Char> span, (Int32 offset, CString[] values) arg)
	{
		CStringSequence seq = new(arg.values);
		Span<Byte> buffer = MemoryMarshal.AsBytes(span);
		ReadOnlySpan<Byte> source = MemoryMarshal.AsBytes(seq.ToString().AsSpan());
		Int32 count = Math.Min(buffer.Length - arg.offset, source.Length);
		source[..count].CopyTo(buffer[arg.offset..]);
	}
}