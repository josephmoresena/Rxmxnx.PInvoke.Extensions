namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class SegmentTests
{
	[Fact]
	public void InvalidTest()
	{
		const Int32 zero = 0;
		Int32 varIndex = 1;

		// ReSharper disable once AccessToModifiedClosure
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[varIndex..]);
		// ReSharper disable once AccessToModifiedClosure
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..varIndex]);

		varIndex = -1;
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[varIndex..]);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..varIndex]);

		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty.Slice(varIndex, zero));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty.Slice(zero, 1));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty.Slice(varIndex, 1));
		return;
	}

	[Fact]
	public void Test()
	{
		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		{
			SegmentTests.SegmentTest(TestSet.Utf16Text[i], (CString)TestSet.Utf16Text[i]);
			SegmentTests.SegmentTest(TestSet.Utf16Text[i], new(TestSet.Utf8Text[i]));
			SegmentTests.SegmentTest(TestSet.Utf16Text[i], TestSet.Utf8Bytes[i]);
			SegmentTests.SegmentTest(TestSet.Utf16Text[i], TestSet.Utf8NullTerminatedBytes[i]);
			SegmentTests.FixedTest(TestSet.Utf16Text[i], TestSet.Utf8Bytes[i]);
			SegmentTests.FixedTest(TestSet.Utf16Text[i], TestSet.Utf8NullTerminatedBytes[i]);
		}
	}

	private static unsafe void FixedTest(String str, Byte[] bytes)
	{
		fixed (void* ptr = bytes)
			SegmentTests.SegmentTest(str, CString.CreateUnsafe(new(ptr), bytes.Length));
	}
	[SuppressMessage("Style", "IDE0057")]
	private static void SegmentTest(String str, CString cstr)
	{
		ReadOnlySpan<Int32> strIndex = SegmentTests.GetIndices(str);
		ReadOnlySpan<Int32> cstrIndex = SegmentTests.GetIndices(cstr);
		Int32 count = strIndex.Length;

		PInvokeAssert.Equal(count, cstrIndex.Length);

		for (Int32 i = 0; i < count; i++)
		{
			Int32 start = PInvokeRandom.Shared.Next(i, count);
			Int32 end = PInvokeRandom.Shared.Next(start, count + 1);

			Int32 strStart = strIndex[start];
			Int32 strEnd = end < strIndex.Length ? strIndex[end] : str.Length;
			Int32 cstrStart = cstrIndex[start];
			Int32 cstrEnd = end < cstrIndex.Length ? cstrIndex[end] : cstr.Length;

			String strSeg = str[strStart..strEnd];
			CString cstrSeg = cstr[cstrStart..cstrEnd];

			PInvokeAssert.Equal(strSeg, cstrSeg.ToString());
			SegmentTests.AssertSegment(cstr, cstrSeg, cstrStart, cstrEnd);
			// ReSharper disable once ReplaceSliceWithRangeIndexer
			SegmentTests.AssertSegment(cstr, cstr.Slice(cstrStart), cstrStart, cstr.Length);

			if (cstr is { IsSegmented: false, IsReference: false, })
				SegmentTests.SegmentTest(strSeg, cstrSeg);

			CompareTest.CompleteTest(str, cstr, strSeg, cstrSeg);
		}
	}
	private static void AssertSegment(CString cstr, CString cstrSeg, Int32 cstrStart, Int32 cstrEnd)
	{
		if (cstrSeg.Length == 0)
		{
			PInvokeAssert.Same(CString.Empty, cstrSeg);
			PInvokeAssert.False(cstrSeg.IsReference);
			PInvokeAssert.True(cstrSeg.IsNullTerminated);
		}
		else
		{
			PInvokeAssert.Equal(cstr.IsReference, cstrSeg.IsReference);
			PInvokeAssert.Equal(cstr.IsFunction, cstrSeg.IsFunction);
			PInvokeAssert.Equal(cstr.IsSegmented || (!cstr.IsReference && (cstrStart != 0 || cstrEnd != cstr.Length)),
			                    cstrSeg.IsSegmented);
			PInvokeAssert.Equal(cstrEnd == cstr.Length && cstr.IsNullTerminated, cstrSeg.IsNullTerminated);

			for (Int32 i = 0; i < cstrSeg.Length; i++)
				PInvokeAssert.Equal(cstr[i + cstrStart], cstrSeg[i]);

			if (cstr is { IsFunction: false, } or { IsReference: false, }) return;

			if (!cstrSeg.IsSegmented)
				PInvokeAssert.Equal(CString.GetBytes(cstr), CString.GetBytes(cstrSeg));
		}
		SegmentTests.AssertClone(cstrSeg);
	}
	private static void AssertClone(CString cstrSeg)
	{
		CString cloneSeg = (CString)cstrSeg.Clone();
		PInvokeAssert.Equal(cstrSeg.ToString(), cloneSeg.ToString());
		PInvokeAssert.Equal(cstrSeg.Length, cloneSeg.Length);
		PInvokeAssert.False(cloneSeg.IsFunction);
		PInvokeAssert.False(cloneSeg.IsReference);
		PInvokeAssert.False(cloneSeg.IsSegmented);
		PInvokeAssert.True(cloneSeg.IsNullTerminated);

		PInvokeAssert.Equal(cstrSeg.Length + 1, CString.GetBytes(cloneSeg).Length);
	}
	private static ReadOnlySpan<Int32> GetIndices(ReadOnlySpan<Byte> source)
	{
		List<Int32> result = new(Encoding.UTF8.GetCharCount(source));
		Int32 length = default;

		while (length < source.Length)
		{
			if (Rune.DecodeFromUtf8(source[length..], out Rune _, out Int32 consumed) is not OperationStatus.Done)
				break;
			result.Add(length);
			length += consumed;
		}

#if NET5_0_OR_GREATER
		return CollectionsMarshal.AsSpan(result);
#else
		return result.ToArray().AsSpan();
#endif
	}
	private static ReadOnlySpan<Int32> GetIndices(ReadOnlySpan<Char> source)
	{
		List<Int32> result = new(source.Length);
		Int32 length = default;

		while (length < source.Length)
		{
			if (Rune.DecodeFromUtf16(source[length..], out Rune _, out Int32 consumed) is not OperationStatus.Done)
				break;
			result.Add(length);
			length += consumed;
		}

#if NET5_0_OR_GREATER
		return CollectionsMarshal.AsSpan(result);
#else
		return result.ToArray().AsSpan();
#endif
	}
}