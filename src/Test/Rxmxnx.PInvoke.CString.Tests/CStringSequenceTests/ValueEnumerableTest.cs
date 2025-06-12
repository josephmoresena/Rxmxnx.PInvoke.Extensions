namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[ExcludeFromCodeCoverage]
public sealed unsafe class ValueSequenceTest
{
	[Fact]
	internal void NullTest()
	{
		CStringSequence.ValueSequence enumerable = default;
		CStringSequence.ValueSequence.Enumerator enumerator = enumerable.GetEnumerator();

		Assert.Null(enumerable.ToString());
		Assert.Equal(0, enumerable.Count);
		Assert.Empty(enumerable.ToArray());
		Assert.False(enumerator.MoveNext());
		Assert.Throws<InvalidOperationException>(() =>
		{
			_ = default(CStringSequence.ValueSequence.Enumerator).Current;
		});
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	internal void EmptyTest(Boolean empty)
	{
		CStringSequence.ValueSequence enumerable = (empty ?
				CStringSequence.Empty :
				new(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
				    ReadOnlySpan<Byte>.Empty,
				    ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
				    ReadOnlySpan<Byte>.Empty))
			.ToValueSequence(false);
		CStringSequence.ValueSequence.Enumerator enumerator = enumerable.GetEnumerator();

		Assert.Equal(String.Empty, enumerable.ToString());
		Assert.Equal(0, enumerable.Count);
		Assert.Empty(enumerable.ToArray());
		Assert.False(enumerator.MoveNext());
		Assert.Throws<InvalidOperationException>(() =>
		{
			_ = default(CStringSequence.ValueSequence.Enumerator).Current;
		});
	}

	[Fact]
	internal void EnumeratorTest()
	{
		CStringSequence sequence = new(Enumerable.Range(0, 10).Select(i => i % 2 == 0 ? String.Empty : default)
		                                         .Concat(TestSet.GetIndices(20).Select(i => TestSet.GetString(i, true)))
		                                         .Concat(Enumerable.Range(0, 10)
		                                                           .Select(i => i % 2 == 0 ? String.Empty : default)));
		CStringSequence.ValueSequence enumerable = sequence.ToValueSequence(false);
		CStringSequence.ValueSequence.Enumerator enumerator = enumerable.GetEnumerator();
		ref CStringSequence.ValueSequence.Enumerator refEnumerator = ref enumerator;
		IntPtr ptrEnumerator = (IntPtr)Unsafe.AsPointer(ref refEnumerator);

		Assert.Equal(sequence.NonEmptyCount, enumerable.Count);
		Assert.Throws<InvalidOperationException>(() =>
		{
			ref CStringSequence.ValueSequence.Enumerator rE =
				ref Unsafe.AsRef<CStringSequence.ValueSequence.Enumerator>(ptrEnumerator.ToPointer());
			_ = rE.Current;
		});
		ValueSequenceTest.AssertNonEmpty(sequence, enumerator);
		Assert.Throws<InvalidOperationException>(() =>
		{
			ref CStringSequence.ValueSequence.Enumerator rE =
				ref Unsafe.AsRef<CStringSequence.ValueSequence.Enumerator>(ptrEnumerator.ToPointer());
			_ = rE.Current;
		});

		enumerator.Reset();
		ValueSequenceTest.AssertNonEmpty(sequence, enumerator);
	}
	[Fact]
	internal void EnumerableTest()
	{
		CStringSequence sequence = new(Enumerable.Range(0, 10).Select(i => i % 2 == 0 ? String.Empty : default)
		                                         .Concat(TestSet.GetIndices(20).Select(i => TestSet.GetString(i, true)))
		                                         .Concat(Enumerable.Range(0, 10)
		                                                           .Select(i => i % 2 == 0 ? String.Empty : default)));
		CStringSequence.ValueSequence enumerable = sequence.ToValueSequence();
		using IEnumerator<CString> seqEnumerator = (sequence as IEnumerable<CString>).GetEnumerator();
		foreach (ReadOnlySpan<Byte> span in enumerable)
		{
			seqEnumerator.MoveNext();
			Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference<Byte>(seqEnumerator.Current),
			                           ref MemoryMarshal.GetReference(span)));
		}
		Assert.Equal(sequence.Count, enumerable.Count);
	}

	private static void AssertNonEmpty(CStringSequence sequence, CStringSequence.ValueSequence.Enumerator enumerator)
	{
		Int32 index = 0;
		Span<Int32> offsets = stackalloc Int32[sequence.NonEmptyCount];
		fixed (Byte* ptr = &MemoryMarshal.GetReference(MemoryMarshal.AsBytes<Char>(sequence.ToString())))
		{
			ref Byte ref0 = ref *ptr;
			sequence.GetOffsets(offsets);
			while (enumerator.MoveNext())
			{
				Assert.True(Unsafe.AreSame(ref Unsafe.AddByteOffset(ref ref0, offsets[index]),
				                           ref MemoryMarshal.GetReference(enumerator.Current)));
				index++;
			}
		}
		Assert.Equal(sequence.NonEmptyCount, index);
	}
}