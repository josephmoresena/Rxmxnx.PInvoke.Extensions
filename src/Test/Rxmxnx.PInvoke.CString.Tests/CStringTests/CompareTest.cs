#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CompareTest
{
	[Fact]
	public void EmptyTest()
	{
		PInvokeAssert.Equal(0, CString.Compare(default, default(CString)));
		PInvokeAssert.Equal(0, CString.Compare(default, default(CString), StringComparison.Ordinal));
		PInvokeAssert.Equal(0, CString.Compare(default, default(CString), false));
		PInvokeAssert.Equal(0, CString.Compare(default, default(CString), false, default));
		PInvokeAssert.Equal(0, CString.Compare(default, default(String)));
		PInvokeAssert.Equal(0, CString.Compare(default, default(String), StringComparison.Ordinal));
		PInvokeAssert.Equal(0, CString.Compare(default, default(String), false));
		PInvokeAssert.Equal(0, CString.Compare(default, default(String), false, default));
	}

	[Fact]
	public void Test()
	{
		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		for (Int32 j = 0; j < TestSet.Utf16Text.Count; j++)
			CompareTest.CompleteTest(i, j);
	}

	[Fact]
	public void CulturalTest()
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
		PInvokeAssert.Equal(1, cstrA.CompareTo(default(Object)));
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
		PInvokeAssert.Equal(1, cstrA.CompareTo(default(String)));
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(String)));
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(String), StringComparison.Ordinal));
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(String), false));
		PInvokeAssert.Equal(-1, CString.Compare(default, strB));
		PInvokeAssert.Equal(-1, CString.Compare(default, strB, StringComparison.Ordinal));
		PInvokeAssert.Equal(-1, CString.Compare(default, strB, false));

		PInvokeAssert.Equal(results.Normal, cstrA.CompareTo(strB));
		PInvokeAssert.Equal(results.Normal, CString.Compare(cstrA, strB));
		PInvokeAssert.Equal(results.CaseInsensitive, CString.Compare(cstrA, strB, true));
		PInvokeAssert.Equal(results.CaseSensitive, CString.Compare(cstrA, strB, false));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.Ordinal],
		                    CString.Compare(cstrA, strB, StringComparison.Ordinal));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCulture],
		                    CString.Compare(cstrA, strB, StringComparison.CurrentCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCulture],
		                    CString.Compare(cstrA, strB, StringComparison.InvariantCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase],
		                    CString.Compare(cstrA, strB, StringComparison.OrdinalIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase],
		                    CString.Compare(cstrA, strB, StringComparison.CurrentCultureIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase],
		                    CString.Compare(cstrA, strB, StringComparison.InvariantCultureIgnoreCase));
	}
	private static void StringTest(CulturalComparisonTestResult results, CString cstrA, String strB)
	{
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(String), false, results.Culture));
		PInvokeAssert.Equal(-1, CString.Compare(default, strB, false, results.Culture));

		PInvokeAssert.Equal(results.CaseInsensitive, CString.Compare(cstrA, strB, true, results.Culture));
		PInvokeAssert.Equal(results.CaseSensitive, CString.Compare(cstrA, strB, false, results.Culture));
	}
	private static void CStringTest(ComparisonTestResult results, CString cstrA, CString cstrB)
	{
		PInvokeAssert.Equal(1, cstrA.CompareTo(default(CString)));
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(CString)));
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(CString), StringComparison.Ordinal));
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(CString), false));
		PInvokeAssert.Equal(-1, CString.Compare(default, cstrB));
		PInvokeAssert.Equal(-1, CString.Compare(default, cstrB, StringComparison.Ordinal));
		PInvokeAssert.Equal(-1, CString.Compare(default, cstrB, false));

		PInvokeAssert.Equal(results.Normal, cstrA.CompareTo(cstrB));
		PInvokeAssert.Equal(results.Normal, CString.Compare(cstrA, cstrB));
		PInvokeAssert.Equal(results.CaseInsensitive, CString.Compare(cstrA, cstrB, true));
		PInvokeAssert.Equal(results.CaseSensitive, CString.Compare(cstrA, cstrB, false));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.Ordinal],
		                    CString.Compare(cstrA, cstrB, StringComparison.Ordinal));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCulture],
		                    CString.Compare(cstrA, cstrB, StringComparison.CurrentCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCulture],
		                    CString.Compare(cstrA, cstrB, StringComparison.InvariantCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase],
		                    CString.Compare(cstrA, cstrB, StringComparison.OrdinalIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase],
		                    CString.Compare(cstrA, cstrB, StringComparison.CurrentCultureIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase],
		                    CString.Compare(cstrA, cstrB, StringComparison.InvariantCultureIgnoreCase));
	}
	private static void CStringTest(CulturalComparisonTestResult results, CString cstrA, CString cstrB)
	{
		PInvokeAssert.Equal(1, CString.Compare(cstrA, default(CString), false, results.Culture));
		PInvokeAssert.Equal(-1, CString.Compare(default, cstrB, false, results.Culture));

		PInvokeAssert.Equal(results.CaseInsensitive, CString.Compare(cstrA, cstrB, true, results.Culture));
		PInvokeAssert.Equal(results.CaseSensitive, CString.Compare(cstrA, cstrB, false, results.Culture));
	}
}