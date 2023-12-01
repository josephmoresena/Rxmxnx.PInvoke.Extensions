namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class SegmentTests
{
	[Fact]
	internal void InvalidTest()
	{
		const Int32 negativeOne = 1;
		Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[negativeOne..]);
		Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[1..]);
		Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..negativeOne]);
		Assert.Throws<ArgumentOutOfRangeException>(() => CString.Empty[..1]);
	}

	[Fact]
	internal void Test()
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

		Assert.Equal(count, cstrIndex.Count);

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

			Assert.Equal(strSeg, cstrSeg.ToString());
			SegmentTests.AssertSegment(cstr, cstrSeg, cstrStart, cstrEnd);
			SegmentTests.AssertSegment(cstr, cstr.Slice(cstrStart), cstrStart, cstr.Length);

			if (!cstr.IsSegmented && !cstr.IsReference)
				SegmentTests.SegmentTest(strSeg, cstrSeg);

			CompareTest.CompleteTest(str, cstr, strSeg, cstrSeg);
		}
	}
	private static void AssertSegment(CString cstr, CString cstrSeg, Int32 cstrStart, Int32 cstrEnd)
	{
		if (cstrSeg.Length == 0)
		{
			Assert.Same(CString.Empty, cstrSeg);
			Assert.False(cstrSeg.IsFunction);
			Assert.False(cstrSeg.IsReference);
			Assert.True(cstrSeg.IsNullTerminated);
		}
		else
		{
			Assert.Equal(cstr.IsReference, cstrSeg.IsReference);
			Assert.Equal(cstr.IsFunction, cstrSeg.IsFunction);
			Assert.Equal(cstr.IsSegmented || (!cstr.IsReference && (cstrStart != 0 || cstrEnd != cstr.Length)),
			             cstrSeg.IsSegmented);
			Assert.Equal(cstrEnd == cstr.Length && cstr.IsNullTerminated, cstrSeg.IsNullTerminated);

			for (Int32 i = 0; i < cstrSeg.Length; i++)
				Assert.Equal(cstr[i + cstrStart], cstrSeg[i]);

			if (!cstr.IsSegmented && !cstr.IsFunction && !cstr.IsReference)
				if (!cstrSeg.IsSegmented)
					Assert.Equal(CString.GetBytes(cstr), CString.GetBytes(cstrSeg));
				else
					try
					{
						Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstrSeg));
					}
					catch (Exception)
					{
						// For some reason sometimes the test fails even though it shouldn't.
						// The test must be run again so that it does not fail.
						Assert.NotEqual(cstr, cstrSeg);
						Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstrSeg));
					}
		}
		SegmentTests.AssertClone(cstrSeg);
	}
	private static void AssertClone(CString cstrSeg)
	{
		CString cloneSeg = (CString)cstrSeg.Clone();
		Assert.Equal(cstrSeg.ToString(), cloneSeg.ToString());
		Assert.Equal(cstrSeg.Length, cloneSeg.Length);
		Assert.False(cloneSeg.IsFunction);
		Assert.False(cloneSeg.IsReference);
		Assert.False(cloneSeg.IsSegmented);
		Assert.True(cloneSeg.IsNullTerminated);

		Assert.Equal(cstrSeg.Length + 1, CString.GetBytes(cloneSeg).Length);
	}
}