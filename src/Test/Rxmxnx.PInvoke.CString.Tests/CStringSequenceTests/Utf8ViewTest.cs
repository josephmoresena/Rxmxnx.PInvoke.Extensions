namespace Rxmxnx.PInvoke.Tests.CStringSequenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed unsafe class Utf8ViewTest
{
	[Fact]
	public void NullTest()
	{
		CStringSequence.Utf8View enumerable = default;
		CStringSequence.Utf8View.Enumerator enumerator = enumerable.GetEnumerator();

		PInvokeAssert.Null(enumerable.ToString());
		PInvokeAssert.Equal(0, enumerable.Count);
		PInvokeAssert.Empty(enumerable.ToArray());
		PInvokeAssert.False(enumerator.MoveNext());
		PInvokeAssert.Throws<InvalidOperationException>(() =>
		{
			_ = default(CStringSequence.Utf8View.Enumerator).Current;
		});
	}

	[Theory]
	[InlineData(true)]
	[InlineData(false)]
	public void EmptyTest(Boolean empty)
	{
		CStringSequence.Utf8View enumerable = (empty ?
				CStringSequence.Empty :
				new(ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
				    ReadOnlySpan<Byte>.Empty,
				    ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty, ReadOnlySpan<Byte>.Empty,
				    ReadOnlySpan<Byte>.Empty))
			.CreateView(false);
		CStringSequence.Utf8View.Enumerator enumerator = enumerable.GetEnumerator();

		PInvokeAssert.Equal(String.Empty, enumerable.ToString());
		PInvokeAssert.Equal(0, enumerable.Count);
		PInvokeAssert.Empty(enumerable.ToArray());
		PInvokeAssert.False(enumerator.MoveNext());
		PInvokeAssert.Throws<InvalidOperationException>(() =>
		{
			_ = default(CStringSequence.Utf8View.Enumerator).Current;
		});
	}

	[Fact]
	public void EnumeratorTest()
	{
		CStringSequence sequence = new(Enumerable.Range(0, 10).Select(i => i % 2 == 0 ? String.Empty : default)
		                                         .Concat(TestSet.GetIndices(20).Select(i => TestSet.GetString(i, true)))
		                                         .Concat(Enumerable.Range(0, 10)
		                                                           .Select(i => i % 2 == 0 ? String.Empty : default)));
		CStringSequence.Utf8View enumerable = sequence.CreateView(false);
		CStringSequence.Utf8View.Enumerator enumerator = enumerable.GetEnumerator();

		PInvokeAssert.Equal(sequence.NonEmptyCount, enumerable.Count);

#if NET9_0_OR_GREATER
		ref CStringSequence.Utf8View.Enumerator refEnumerator = ref enumerator;
		IntPtr ptrEnumerator = (IntPtr)Unsafe.AsPointer(ref refEnumerator);

		Assert.Throws<InvalidOperationException>(() =>
		{
			ref CStringSequence.Utf8View.Enumerator rE =
				ref Unsafe.AsRef<CStringSequence.Utf8View.Enumerator>(ptrEnumerator.ToPointer());
			_ = rE.Current;
		});
		Utf8ViewTest.AssertNonEmpty(sequence, enumerator);
		Assert.Throws<InvalidOperationException>(() =>
		{
			ref CStringSequence.Utf8View.Enumerator rE =
				ref Unsafe.AsRef<CStringSequence.Utf8View.Enumerator>(ptrEnumerator.ToPointer());
			_ = rE.Current;
		});
#endif

		enumerator.Reset();
		Utf8ViewTest.AssertNonEmpty(sequence, enumerator);
	}
	[Fact]
	public void EnumerableTest()
	{
		CStringSequence sequence = new(Enumerable.Range(0, 10).Select(i => i % 2 == 0 ? String.Empty : default)
		                                         .Concat(TestSet.GetIndices(20).Select(i => TestSet.GetString(i, true)))
		                                         .Concat(Enumerable.Range(0, 10)
		                                                           .Select(i => i % 2 == 0 ? String.Empty : default)));
		CStringSequence.Utf8View enumerable = sequence.CreateView();
		using IEnumerator<CString> seqEnumerator = (sequence as IEnumerable<CString>).GetEnumerator();
		foreach (ReadOnlySpan<Byte> span in enumerable)
		{
			seqEnumerator.MoveNext();
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference<Byte>(seqEnumerator.Current),
			                                  ref MemoryMarshal.GetReference(span)));
		}
		PInvokeAssert.Equal(sequence.Count, enumerable.Count);
	}

	private static void AssertNonEmpty(CStringSequence sequence, CStringSequence.Utf8View.Enumerator enumerator)
	{
		Int32 index = 0;
		Span<Int32> offsets = stackalloc Int32[sequence.NonEmptyCount];
		fixed (Byte* ptr = &MemoryMarshal.GetReference(MemoryMarshal.AsBytes<Char>(sequence.ToString())))
		{
			ref Byte ref0 = ref *ptr;
			sequence.GetOffsets(offsets);
			while (enumerator.MoveNext())
			{
#if NET7_0_OR_GREATER
				Assert.True(Unsafe.AreSame(ref Unsafe.AddByteOffset(ref ref0, offsets[index]),
				                           ref MemoryMarshal.GetReference(enumerator.Current)));
#else
				PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AddByteOffset(ref ref0, (IntPtr)offsets[index]),
				                                  ref MemoryMarshal.GetReference(enumerator.Current)));
#endif
				index++;
			}
		}
		PInvokeAssert.Equal(sequence.NonEmptyCount, index);
	}
}