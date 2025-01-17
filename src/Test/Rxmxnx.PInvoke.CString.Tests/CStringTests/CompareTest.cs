﻿namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CompareTest
{
	[Fact]
	internal void EmptyTest()
	{
		Assert.Equal(0, CString.Compare(default, default(CString)));
		Assert.Equal(0, CString.Compare(default, default(CString), StringComparison.Ordinal));
		Assert.Equal(0, CString.Compare(default, default(CString), false));
		Assert.Equal(0, CString.Compare(default, default(CString), false, default));
		Assert.Equal(0, CString.Compare(default, default(String)));
		Assert.Equal(0, CString.Compare(default, default(String), StringComparison.Ordinal));
		Assert.Equal(0, CString.Compare(default, default(String), false));
		Assert.Equal(0, CString.Compare(default, default(String), false, default));
	}

	[Fact]
	internal void Test()
	{
		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		for (Int32 j = 0; j < TestSet.Utf16Text.Count; j++)
			CompareTest.CompleteTest(i, j);
	}

	[Fact]
	internal void CulturalTest()
	{
		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
			CompareTest.CompleteCulturalTest(i, j);
	}

	internal static void CompleteTest(String strA, CString cstrA, String strB, CString cstrB)
		=> CompareTest.CompleteTest(ComparisonTestResult.Compare(strA, strB), cstrA, strB, cstrB);

	private static void CompleteTest(Int32 indexA, Int32 indexB)
	{
		String strA = TestSet.Utf16Text[indexA];
		String strLowerA = TestSet.Utf16TextLower[indexA];
		String strUpperA = TestSet.Utf16TextUpper[indexA];

		CString cstrA = new(TestSet.Utf8Text[indexA]);
		CString cstrLowerA = new(TestSet.Utf8TextLower[indexA]);
		CString cstrUpperA = new(TestSet.Utf8TextUpper[indexA]);

		String strB = TestSet.Utf16Text[indexB];
		String strLowerB = TestSet.Utf16TextLower[indexB];
		String strUpperB = TestSet.Utf16TextUpper[indexB];

		CString cstrB = new(TestSet.Utf8Text[indexB]);
		CString cstrLowerB = new(TestSet.Utf8TextLower[indexB]);
		CString cstrUpperB = new(TestSet.Utf8TextUpper[indexB]);

		ComparisonTestResult testAb = ComparisonTestResult.Compare(strA, strB);
		CompareTest.StringTest(testAb, cstrA, strB);
		CompareTest.CStringTest(testAb, cstrA, cstrB);
		CompareTest.CompleteTest(ComparisonTestResult.Compare(strA, strLowerB), cstrA, strLowerB, cstrLowerB);
		CompareTest.CompleteTest(ComparisonTestResult.Compare(strA, strUpperB), cstrA, strUpperB, cstrUpperB);

		if (testAb.Normal != 0)
		{
			CompareTest.CompleteTest(ComparisonTestResult.Compare(strB, strA), cstrB, strA, cstrA);
			CompareTest.CompleteTest(ComparisonTestResult.Compare(strB, strLowerA), cstrB, strLowerA, cstrLowerA);
			CompareTest.CompleteTest(ComparisonTestResult.Compare(strB, strUpperA), cstrB, strUpperA, cstrUpperA);
		}
		Assert.Equal(1, cstrA.CompareTo(default(Object)));
	}
	private static void CompleteTest(ComparisonTestResult results, CString cstrA, String strB, CString cstrB)
	{
		CompareTest.StringTest(results, cstrA, strB);
		CompareTest.CStringTest(results, cstrA, cstrB);
	}
	private static void CompleteCulturalTest(Int32 indexA, Int32 indexB)
	{
		String strA = TestSet.Utf16Text[indexA];
		String strLowerA = TestSet.Utf16TextLower[indexA];
		String strUpperA = TestSet.Utf16TextUpper[indexA];

		CString cstrA = new(TestSet.Utf8Text[indexA]);
		CString cstrLowerA = new(TestSet.Utf8TextLower[indexA]);
		CString cstrUpperA = new(TestSet.Utf8TextUpper[indexA]);

		String strB = TestSet.Utf16Text[indexB];
		String strLowerB = TestSet.Utf16TextLower[indexB];
		String strUpperB = TestSet.Utf16TextUpper[indexB];

		CString cstrB = new(TestSet.Utf8Text[indexB]);
		CString cstrLowerB = new(TestSet.Utf8TextLower[indexB]);
		CString cstrUpperB = new(TestSet.Utf8TextUpper[indexB]);

		CompareTest.CompleteCulturalTest(strA, cstrA, strB, cstrB);
		CompareTest.CompleteCulturalTest(strA, cstrA, strLowerB, cstrLowerB);
		CompareTest.CompleteCulturalTest(strA, cstrA, strUpperB, cstrUpperB);

		if (Object.ReferenceEquals(strA, strB)) return;
		CompareTest.CompleteCulturalTest(strB, cstrB, strA, cstrA);
		CompareTest.CompleteCulturalTest(strB, cstrB, strLowerA, cstrLowerA);
		CompareTest.CompleteCulturalTest(strB, cstrB, strUpperA, cstrUpperA);
	}
	private static void CompleteCulturalTest(String strA, CString cstrA, String strB, CString cstrB)
	{
		CulturalComparisonTestResult results = CulturalComparisonTestResult.Compare(strA, strB);
		CompareTest.StringTest(results, cstrA, strB);
		CompareTest.CStringTest(results, cstrA, cstrB);
	}

	private static void StringTest(ComparisonTestResult results, CString cstrA, String strB)
	{
		Assert.Equal(1, cstrA.CompareTo(default(String)));
		Assert.Equal(1, CString.Compare(cstrA, default(String)));
		Assert.Equal(1, CString.Compare(cstrA, default(String), StringComparison.Ordinal));
		Assert.Equal(1, CString.Compare(cstrA, default(String), false));
		Assert.Equal(-1, CString.Compare(default, strB));
		Assert.Equal(-1, CString.Compare(default, strB, StringComparison.Ordinal));
		Assert.Equal(-1, CString.Compare(default, strB, false));

		Assert.Equal(results.Normal, cstrA.CompareTo(strB));
		Assert.Equal(results.Normal, CString.Compare(cstrA, strB));
		Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, strB, true));
		Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, strB, false));
		Assert.Equal(results.Comparisons[StringComparison.Ordinal],
		             CString.Compare(cstrA, strB, StringComparison.Ordinal));
		Assert.Equal(results.Comparisons[StringComparison.CurrentCulture],
		             CString.Compare(cstrA, strB, StringComparison.CurrentCulture));
		Assert.Equal(results.Comparisons[StringComparison.InvariantCulture],
		             CString.Compare(cstrA, strB, StringComparison.InvariantCulture));
		Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase],
		             CString.Compare(cstrA, strB, StringComparison.OrdinalIgnoreCase));
		Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase],
		             CString.Compare(cstrA, strB, StringComparison.CurrentCultureIgnoreCase));
		Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase],
		             CString.Compare(cstrA, strB, StringComparison.InvariantCultureIgnoreCase));
	}
	private static void StringTest(CulturalComparisonTestResult results, CString cstrA, String strB)
	{
		Assert.Equal(1, CString.Compare(cstrA, default(String), false, results.Culture));
		Assert.Equal(-1, CString.Compare(default, strB, false, results.Culture));

		Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, strB, true, results.Culture));
		Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, strB, false, results.Culture));
	}
	private static void CStringTest(ComparisonTestResult results, CString cstrA, CString cstrB)
	{
		Assert.Equal(1, cstrA.CompareTo(default(CString)));
		Assert.Equal(1, CString.Compare(cstrA, default(CString)));
		Assert.Equal(1, CString.Compare(cstrA, default(CString), StringComparison.Ordinal));
		Assert.Equal(1, CString.Compare(cstrA, default(CString), false));
		Assert.Equal(-1, CString.Compare(default, cstrB));
		Assert.Equal(-1, CString.Compare(default, cstrB, StringComparison.Ordinal));
		Assert.Equal(-1, CString.Compare(default, cstrB, false));

		Assert.Equal(results.Normal, cstrA.CompareTo(cstrB));
		Assert.Equal(results.Normal, CString.Compare(cstrA, cstrB));
		Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, cstrB, true));
		Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, cstrB, false));
		Assert.Equal(results.Comparisons[StringComparison.Ordinal],
		             CString.Compare(cstrA, cstrB, StringComparison.Ordinal));
		Assert.Equal(results.Comparisons[StringComparison.CurrentCulture],
		             CString.Compare(cstrA, cstrB, StringComparison.CurrentCulture));
		Assert.Equal(results.Comparisons[StringComparison.InvariantCulture],
		             CString.Compare(cstrA, cstrB, StringComparison.InvariantCulture));
		Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase],
		             CString.Compare(cstrA, cstrB, StringComparison.OrdinalIgnoreCase));
		Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase],
		             CString.Compare(cstrA, cstrB, StringComparison.CurrentCultureIgnoreCase));
		Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase],
		             CString.Compare(cstrA, cstrB, StringComparison.InvariantCultureIgnoreCase));
	}
	private static void CStringTest(CulturalComparisonTestResult results, CString cstrA, CString cstrB)
	{
		Assert.Equal(1, CString.Compare(cstrA, default(CString), false, results.Culture));
		Assert.Equal(-1, CString.Compare(default, cstrB, false, results.Culture));

		Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, cstrB, true, results.Culture));
		Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, cstrB, false, results.Culture));
	}
}