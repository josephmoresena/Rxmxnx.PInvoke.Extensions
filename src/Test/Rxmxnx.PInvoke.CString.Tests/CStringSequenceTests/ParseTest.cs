#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class ParseTest
{
	[Fact]
	public void EmptyTest()
	{
		PInvokeAssert.Null(CStringSequence.Parse(null));

		String nullString = new(Enumerable.Repeat('\0', Random.Shared.Next(0, Byte.MaxValue)).ToArray());
		CStringSequence seq0 = CStringSequence.Parse(String.Empty);
		CStringSequence seq1 = CStringSequence.Create(Array.Empty<Char>());
		CStringSequence seq2 = CStringSequence.Parse(nullString);
		CStringSequence seq3 = CStringSequence.Create(nullString);
		CStringSequence seq4 = CStringSequence.Create("\0\0\0"u8);
		CStringSequence seq5 = CStringSequence.Parse("\0\0\0");

		PInvokeAssert.Empty(seq0);
		PInvokeAssert.Equal(0, seq0.ToString().Length);
		PInvokeAssert.Empty(seq1);
		PInvokeAssert.Equal(0, seq1.ToString().Length);
		PInvokeAssert.Empty(seq2);
		PInvokeAssert.Equal(0, seq2.ToString().Length);
		PInvokeAssert.Empty(seq3);
		PInvokeAssert.Equal(0, seq3.ToString().Length);
		PInvokeAssert.Empty(seq4);
		PInvokeAssert.Equal(0, seq4.ToString().Length);
		PInvokeAssert.Empty(seq5);
		PInvokeAssert.Equal(0, seq5.ToString().Length);
	}

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
		CStringSequence seq = new(values);
		CString[] nonEmpty = values.Where(c => !CString.IsNullOrEmpty(c) && c.All(b => b != 0x0)).ToArray()!;
		ParseTest.ExactParseTest(nonEmpty, seq.ToString());
		ParseTest.RandomParseTest(nonEmpty);
	}

	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void SpanTest(Int32? length)
	{
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
		CStringBuilder csb = new();
		using (IEnumerator<Int32> enumerator = indices.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				String? val = TestSet.GetString(enumerator.Current);
				if (String.IsNullOrEmpty(val)) continue;
				csb.Append(TestSet.GetString(enumerator.Current));
				break;
			}
			while (enumerator.MoveNext())
			{
				String? val = TestSet.GetString(enumerator.Current);
				if (String.IsNullOrEmpty(val)) continue;
				csb.Append((Byte)'\0');
				csb.Append(TestSet.GetString(enumerator.Current));
			}
		}

		CString value = csb.ToCString(false);
		CStringSequence seq = CStringSequence.Create(value);

		csb.Clear().AppendJoin("\0"u8, seq.CreateView(false));

		PInvokeAssert.Equal(value, csb.ToCString());
	}

	[Theory]
	[InlineData(null)]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void ZeroTest(Int32? length)
	{
#pragma warning disable CA1859
		IReadOnlyList<Int32> indices = TestSet.GetIndices(length);
#pragma warning restore CA1859
		CString[] zeros = new CString[indices.Count];
		CStringBuilder csb = new();
		for (Int32 i = 0; i < indices.Count; i++)
		{
			String? str = TestSet.GetString(indices[i]);
			zeros[i] = CString.Create(Enumerable.Repeat((Byte)0, i).ToArray());
			if (String.IsNullOrEmpty(str)) str = i.ToString();
			csb.Append((Byte)0);
			csb.Append(zeros[i]);
			csb.Append(str);
			zeros[i] += str;
		}

		CString value = csb.ToCString(false);
		CStringSequence seq = CStringSequence.Create(value);
		CStringSequence seq2 = CStringSequence.Parse(seq.ToString());

		for (Int32 index = 0; index < seq.Count; index++)
		{
			PInvokeAssert.Equal(zeros[index], seq[index]);
			PInvokeAssert.Equal(zeros[index], seq2[index]);
		}

		PInvokeAssert.Equal(value, csb.ToCString());
	}

	private static void ExactParseTest(CString[] values, String buffer)
	{
		String nullEndBuffer = buffer +
			new String(Enumerable.Repeat('\0', Random.Shared.Next(0, Byte.MaxValue)).ToArray());
		CStringSequence seq0 = CStringSequence.Parse(buffer);
		CStringSequence seq1 = CStringSequence.Parse(nullEndBuffer);
		CStringSequence seq2 = CStringSequence.Create(buffer);
		CStringSequence seq3 = new(values);

		PInvokeAssert.Equal(values, seq0);
		PInvokeAssert.Equal(values, seq1);
		PInvokeAssert.Equal(values, seq2);
		PInvokeAssert.True(Object.ReferenceEquals(buffer, seq0.ToString()));
		PInvokeAssert.True(Object.ReferenceEquals(nullEndBuffer, seq1.ToString()));
		PInvokeAssert.False(Object.ReferenceEquals(buffer, seq2.ToString()));
		PInvokeAssert.Equal(seq3.ToString(), seq0.ToString());
		PInvokeAssert.Equal(nullEndBuffer.Length, seq1.ToString().Length);
		PInvokeAssert.Equal(seq3.ToString(), seq2.ToString());
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

		PInvokeAssert.Equal(values, seq0);
		PInvokeAssert.Equal(values, seq2);
		PInvokeAssert.Equal(offset == 0, Object.ReferenceEquals(buffer, seq0.ToString()));
		PInvokeAssert.False(Object.ReferenceEquals(buffer, seq2.ToString()));
		PInvokeAssert.InRange(seq0.ToString().Length, values.Select(c => c.Length).Sum() / 2, totalChars);
		PInvokeAssert.Equal(offset != 0 ? seq3.ToString() : buffer, seq0.ToString());
		PInvokeAssert.Equal(seq3.ToString(), seq2.ToString());
	}
	private static void RandomCreate(Span<Char> span, (Int32 offset, CString[] values) arg)
	{
		CStringSequence seq = new(arg.values);
		Span<Byte> buffer = MemoryMarshal.AsBytes(span);
		ReadOnlySpan<Byte> source = MemoryMarshal.AsBytes(seq.ToString().AsSpan());
		Int32 count = Math.Min(buffer.Length - arg.offset, source.Length);
		Span<Byte> destination = buffer[arg.offset..];
		buffer[..arg.offset].Clear();
		source[..count].CopyTo(destination);
		destination[count..].Clear();
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}