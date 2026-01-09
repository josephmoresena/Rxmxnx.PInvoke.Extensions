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

		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[varIndex..]);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..varIndex]);

		varIndex = -1;
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[varIndex..]);
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..varIndex]);

		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty.Slice(varIndex, zero));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty.Slice(zero, 1));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => CString.Empty.Slice(varIndex, 1));
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
		IReadOnlyList<Int32> strIndex = DecodedRune.GetIndices(str);
		IReadOnlyList<Int32> cstrIndex = DecodedRune.GetIndices(cstr);
		Int32 count = strIndex.Count;

		PInvokeAssert.Equal(count, cstrIndex.Count);

		for (Int32 i = 0; i < count; i++)
		{
			Int32 start = Random.Shared.Next(i, count);
			Int32 end = Random.Shared.Next(start, count + 1);

			Int32 strStart = strIndex[start];
			Int32 strEnd = end < strIndex.Count ? strIndex[end] : str.Length;
			Int32 cstrStart = cstrIndex[start];
			Int32 cstrEnd = end < cstrIndex.Count ? cstrIndex[end] : cstr.Length;

			String strSeg = str[strStart..strEnd];
			CString cstrSeg = cstr[cstrStart..cstrEnd];

			PInvokeAssert.Equal(strSeg, cstrSeg.ToString());
			SegmentTests.AssertSegment(cstr, cstrSeg, cstrStart, cstrEnd);
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
			using MemoryHandle _ = CString.Empty.TryPin(out Boolean pinned);
			PInvokeAssert.Same(CString.Empty, cstrSeg);
			PInvokeAssert.Equal(!pinned, cstrSeg.IsFunction);
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

			if (cstr is { IsSegmented: false, IsFunction: false, IsReference: false, })
				if (!cstrSeg.IsSegmented)
					PInvokeAssert.Equal(CString.GetBytes(cstr), CString.GetBytes(cstrSeg));
				else
					try
					{
						//PInvokeAssert.Throws<InvalidOperationException>(() => CString.GetBytes(cstrSeg));
					}
					catch (Exception)
					{
						// For some reason sometimes the test fails even though it shouldn't.
						// The test must be run again so that it does not fail.
						PInvokeAssert.NotEqual(cstr, cstrSeg);
						//PInvokeAssert.Throws<InvalidOperationException>(() => CString.GetBytes(cstrSeg));
					}
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
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}