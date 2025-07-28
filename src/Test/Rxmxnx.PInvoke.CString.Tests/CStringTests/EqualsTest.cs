#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class EqualsTest
{
	[Fact]
	public void Test()
	{
		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
			EqualsTest.CompleteTest(i, j);
	}

	[Fact]
	public void CulturalTest()
	{
		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
			EqualsTest.CompleteCulturalTest(i, j);
	}

	private static void CompleteTest(Int32 indexA, Int32 indexB)
	{
		String strA = TestSet.Utf16Text[indexA];
		CString cstrA = new(TestSet.Utf8Text[indexA]);

		String strB = TestSet.Utf16Text[indexB];
		String strLowerB = TestSet.Utf16TextLower[indexB];
		String strUpperB = TestSet.Utf16TextUpper[indexB];

		CString cstrB = new(TestSet.Utf8Text[indexB]);
		CString cstrLowerB = new(TestSet.Utf8TextLower[indexB]);
		CString cstrUpperB = new(TestSet.Utf8TextUpper[indexB]);

		ComparisonTestResult testAb = ComparisonTestResult.Compare(strA, strB);

		EqualsTest.StringTest(testAb, cstrA, strB);
		EqualsTest.CStringTest(testAb, cstrA, cstrB);
		EqualsTest.OperatorTest(testAb, cstrA, strA, cstrB, strB);
		EqualsTest.CompleteTest(ComparisonTestResult.Compare(strA, strLowerB), cstrA, strLowerB, cstrLowerB);
		EqualsTest.CompleteTest(ComparisonTestResult.Compare(strA, strUpperB), cstrA, strUpperB, cstrUpperB);
	}
	private static void CompleteTest(ComparisonTestResult results, CString cstrA, String strB, CString cstrB)
	{
		EqualsTest.StringTest(results, cstrA, strB);
		EqualsTest.CStringTest(results, cstrA, cstrB);
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

		EqualsTest.CompleteCulturalTest(strA, cstrA, strB, cstrB);
		EqualsTest.CompleteCulturalTest(strA, cstrA, strLowerB, cstrLowerB);
		EqualsTest.CompleteCulturalTest(strA, cstrA, strUpperB, cstrUpperB);
	}
	private static void CompleteCulturalTest(String strA, CString cstrA, String strB, CString cstrB)
	{
		CulturalComparisonTestResult results = CulturalComparisonTestResult.Compare(strA, strB);
		EqualsTest.StringTest(results, cstrA, strB);
		EqualsTest.CStringTest(results, cstrA, cstrB);

		PInvokeAssert.False(cstrA.Equals(results));
		PInvokeAssert.False(cstrA.Equals(default(Object)));
	}

	private static void StringTest(ComparisonTestResult results, CString cstrA, String strB)
	{
		PInvokeAssert.False(cstrA.Equals(default(String)));
		PInvokeAssert.False(cstrA.Equals(default(String), StringComparison.OrdinalIgnoreCase));

		PInvokeAssert.Equal(results.Normal == 0, cstrA.Equals(strB));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.Ordinal] == 0,
		                    cstrA.Equals(strB, StringComparison.Ordinal));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCulture] == 0,
		                    cstrA.Equals(strB, StringComparison.CurrentCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCulture] == 0,
		                    cstrA.Equals(strB, StringComparison.InvariantCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase] == 0,
		                    cstrA.Equals(strB, StringComparison.OrdinalIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase] == 0,
		                    cstrA.Equals(strB, StringComparison.CurrentCultureIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase] == 0,
		                    cstrA.Equals(strB, StringComparison.InvariantCultureIgnoreCase));
	}
	private static void StringTest(CulturalComparisonTestResult results, CString cstrA, String strB)
	{
		PInvokeAssert.Equal(results.CaseInsensitive == 0,
		                    StringUtf8Comparator.Create(true, results.Culture).TextEquals(cstrA, strB));
		PInvokeAssert.Equal(results.CaseSensitive == 0,
		                    StringUtf8Comparator.Create(false, results.Culture).TextEquals(cstrA, strB));
	}
	private static void CStringTest(ComparisonTestResult results, CString cstrA, CString cstrB)
	{
		PInvokeAssert.False(cstrA.Equals(default(CString)));
		PInvokeAssert.False(cstrA.Equals(default(CString), StringComparison.OrdinalIgnoreCase));

		PInvokeAssert.Equal(results.Normal == 0, cstrA.Equals(cstrB));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.Ordinal] == 0,
		                    cstrA.Equals(cstrB, StringComparison.Ordinal));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCulture] == 0,
		                    cstrA.Equals(cstrB, StringComparison.CurrentCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCulture] == 0,
		                    cstrA.Equals(cstrB, StringComparison.InvariantCulture));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase] == 0,
		                    cstrA.Equals(cstrB, StringComparison.OrdinalIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase] == 0,
		                    cstrA.Equals(cstrB, StringComparison.CurrentCultureIgnoreCase));
		PInvokeAssert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase] == 0,
		                    cstrA.Equals(cstrB, StringComparison.InvariantCultureIgnoreCase));
	}
	private static void CStringTest(CulturalComparisonTestResult results, CString cstrA, CString cstrB)
	{
		PInvokeAssert.Equal(results.CaseInsensitive == 0,
		                    CStringUtf8Comparator.Create(true, results.Culture).TextEquals(cstrA, cstrB));
		PInvokeAssert.Equal(results.CaseSensitive == 0,
		                    CStringUtf8Comparator.Create(false, results.Culture).TextEquals(cstrA, cstrB));
	}
	private static void OperatorTest(ComparisonTestResult results, CString cstrA, String strA, CString cstrB,
		String strB)
	{
		Boolean equals = results.Normal == 0;
		Boolean lower = results.Normal < 0;
		Boolean upper = results.Normal > 0;

		PInvokeAssert.True(strA == cstrA);
		PInvokeAssert.True(cstrB == strB);
		PInvokeAssert.Equal(equals, cstrA == cstrB);
		PInvokeAssert.Equal(equals, strA == cstrB);
		PInvokeAssert.Equal(equals, cstrA == strB);
		PInvokeAssert.Equal(!equals, cstrA != cstrB);
		PInvokeAssert.Equal(!equals, strA != cstrB);
		PInvokeAssert.Equal(!equals, cstrA != strB);

		PInvokeAssert.Equal(lower, strA < cstrB);
		PInvokeAssert.Equal(lower, cstrA < cstrB);
		PInvokeAssert.Equal(lower, cstrA < strB);

		PInvokeAssert.Equal(lower || equals, strA <= cstrB);
		PInvokeAssert.Equal(lower || equals, cstrA <= cstrB);
		PInvokeAssert.Equal(lower || equals, cstrA <= strB);

		PInvokeAssert.Equal(upper, strA > cstrB);
		PInvokeAssert.Equal(upper, cstrA > cstrB);
		PInvokeAssert.Equal(upper, cstrA > strB);

		PInvokeAssert.Equal(upper || equals, strA >= cstrB);
		PInvokeAssert.Equal(upper || equals, cstrA >= cstrB);
		PInvokeAssert.Equal(upper || equals, cstrA >= strB);
	}
}