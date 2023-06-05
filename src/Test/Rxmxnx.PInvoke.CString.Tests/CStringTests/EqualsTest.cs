namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class EqualsTest
{
    [Fact]
    internal void Test()
    {
        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
                CompleteTest(i, j);
    }

    [Fact]
    internal void CulturalTest()
    {
        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
                CompleteCulturalTest(i, j);
    }

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

        ComparisonTestResult testAB = ComparisonTestResult.Compare(strA, strB);

        StringTest(testAB, cstrA, strB);
        CStringTest(testAB, cstrA, cstrB);
        OperatorTest(testAB, cstrA, strA, cstrB, strB);
        CompleteTest(ComparisonTestResult.Compare(strA, strLowerB), cstrA, strLowerB, cstrLowerB);
        CompleteTest(ComparisonTestResult.Compare(strA, strUpperB), cstrA, strUpperB, cstrUpperB);
    }
    private static void CompleteTest(ComparisonTestResult results, CString cstrA, String strB, CString cstrB)
    {
        StringTest(results, cstrA, strB);
        CStringTest(results, cstrA, cstrB);
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

        CompleteCulturalTest(strA, cstrA, strB, cstrB);
        CompleteCulturalTest(strA, cstrA, strLowerB, cstrLowerB);
        CompleteCulturalTest(strA, cstrA, strUpperB, cstrUpperB);
    }
    private static void CompleteCulturalTest(String strA, CString cstrA, String strB, CString cstrB)
    {
        CulturalComparisonTestResult results = CulturalComparisonTestResult.Compare(strA, strB);
        StringTest(results, cstrA, strB);
        CStringTest(results, cstrA, cstrB);

        Assert.False(cstrA.Equals(results));
        Assert.False(cstrA.Equals(default(Object)));
    }

    private static void StringTest(ComparisonTestResult results, CString cstrA, String strB)
    {
        Assert.False(cstrA.Equals(default(String)));
        Assert.False(cstrA.Equals(default(String), StringComparison.OrdinalIgnoreCase));

        Assert.Equal(results.Normal == 0, cstrA.Equals(strB));
        Assert.Equal(results.Comparisons[StringComparison.Ordinal] == 0, cstrA.Equals(strB, StringComparison.Ordinal));
        Assert.Equal(results.Comparisons[StringComparison.CurrentCulture] == 0, cstrA.Equals(strB, StringComparison.CurrentCulture));
        Assert.Equal(results.Comparisons[StringComparison.InvariantCulture] == 0, cstrA.Equals(strB, StringComparison.InvariantCulture));
        Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase] == 0, cstrA.Equals(strB, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase] == 0, cstrA.Equals(strB, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase] == 0, cstrA.Equals(strB, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void StringTest(CulturalComparisonTestResult results, CString cstrA, String strB)
    {
        Assert.Equal(results.CaseInsensitive == 0, StringUtf8Comparator.Create(true, results.Culture).TextEquals(cstrA, strB));
        Assert.Equal(results.CaseSensitive == 0, StringUtf8Comparator.Create(false, results.Culture).TextEquals(cstrA, strB));
    }
    private static void CStringTest(ComparisonTestResult results, CString cstrA, CString cstrB)
    {
        Assert.False(cstrA.Equals(default(CString)));
        Assert.False(cstrA.Equals(default(CString), StringComparison.OrdinalIgnoreCase));

        Assert.Equal(results.Normal == 0, cstrA.Equals(cstrB));
        Assert.Equal(results.Comparisons[StringComparison.Ordinal] == 0, cstrA.Equals(cstrB, StringComparison.Ordinal));
        Assert.Equal(results.Comparisons[StringComparison.CurrentCulture] == 0, cstrA.Equals(cstrB, StringComparison.CurrentCulture));
        Assert.Equal(results.Comparisons[StringComparison.InvariantCulture] == 0, cstrA.Equals(cstrB, StringComparison.InvariantCulture));
        Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase] == 0, cstrA.Equals(cstrB, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase] == 0, cstrA.Equals(cstrB, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase] == 0, cstrA.Equals(cstrB, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void CStringTest(CulturalComparisonTestResult results, CString cstrA, CString cstrB)
    {
        Assert.Equal(results.CaseInsensitive == 0, CStringUtf8Comparator.Create(true, results.Culture).TextEquals(cstrA, cstrB));
        Assert.Equal(results.CaseSensitive == 0, CStringUtf8Comparator.Create(false, results.Culture).TextEquals(cstrA, cstrB));
    }
    private static void OperatorTest(ComparisonTestResult results, CString cstrA, String strA, CString cstrB, String strB)
    {
        Boolean equals = results.Normal == 0;
        Boolean lower = results.Normal < 0;
        Boolean upper = results.Normal > 0;

        Assert.True(strA == cstrA);
        Assert.True(cstrB == strB);
        Assert.Equal(equals, cstrA == cstrB);
        Assert.Equal(equals, strA == cstrB);
        Assert.Equal(equals, cstrA == strB);
        Assert.Equal(!equals, cstrA != cstrB);
        Assert.Equal(!equals, strA != cstrB);
        Assert.Equal(!equals, cstrA != strB);

        Assert.Equal(lower, strA < cstrB);
        Assert.Equal(lower, cstrA < cstrB);
        Assert.Equal(lower, cstrA < strB);

        Assert.Equal(lower || equals, strA <= cstrB);
        Assert.Equal(lower || equals, cstrA <= cstrB);
        Assert.Equal(lower || equals, cstrA <= strB);

        Assert.Equal(upper, strA > cstrB);
        Assert.Equal(upper, cstrA > cstrB);
        Assert.Equal(upper, cstrA > strB);

        Assert.Equal(upper || equals, strA >= cstrB);
        Assert.Equal(upper || equals, cstrA >= cstrB);
        Assert.Equal(upper || equals, cstrA >= strB);
    }
}