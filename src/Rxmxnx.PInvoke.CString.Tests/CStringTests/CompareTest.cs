namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class CompareTest
{
    [Fact]
    internal void Test()
    {
        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            EqualityTest(i);

        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
        {
            Int32 ix = Random.Shared.Next(0, TestSet.Utf16Text.Count);
            Int32 iy = Random.Shared.Next(0, TestSet.Utf16Text.Count);

            InequalityTest(ix, iy);
        }
    }

    private static void EqualityTest(Int32 index)
    {
        String str = TestSet.Utf16Text[index];
        String strLower = TestSet.Utf16TextLower[index];
        String strUpper = TestSet.Utf16TextUpper[index];

        CString cstr = new(TestSet.Utf8Text[index]);
        CString cstrLower = new(TestSet.Utf8TextLower[index]);
        CString cstrUpper = new(TestSet.Utf8TextUpper[index]);

        StringTest(cstr, str, strLower, strUpper);
        CStringTest(cstr, str, cstrLower, cstrUpper);
    }
    private static void InequalityTest(Int32 indexA, Int32 indexB)
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

        StringTest(cstrA, strA, strB);
        StringTest(cstrA, strA, strLowerB);
        StringTest(cstrA, strA, strUpperB);

        StringTest(cstrB, strB, strA);
        StringTest(cstrB, strB, strLowerA);
        StringTest(cstrB, strB, strUpperA);

        CStringTest(cstrA, strA, cstrB);
        CStringTest(cstrA, strA, cstrLowerB);
        CStringTest(cstrA, strA, cstrUpperB);

        CStringTest(cstrB, strB, cstrA);
        CStringTest(cstrB, strB, cstrLowerA);
        CStringTest(cstrB, strB, cstrUpperA);
    }
    private static void StringTest(CString cstrA, String strA, String strB)
    {
        Assert.Equal(strA.CompareTo(strB), cstrA.CompareTo(strB));
        Assert.Equal(String.Compare(strA, strB, true), CString.Compare(cstrA, strB, true));
        Assert.Equal(String.Compare(strA, strB, false), CString.Compare(cstrA, strB, false));
        Assert.Equal(String.Compare(strA, strB, StringComparison.Ordinal), CString.Compare(cstrA, strB, StringComparison.Ordinal));
        Assert.Equal(String.Compare(strA, strB, StringComparison.CurrentCulture), CString.Compare(cstrA, strB, StringComparison.CurrentCulture));
        Assert.Equal(String.Compare(strA, strB, StringComparison.InvariantCulture), CString.Compare(cstrA, strB, StringComparison.InvariantCulture));
        Assert.Equal(String.Compare(strA, strB, StringComparison.OrdinalIgnoreCase), CString.Compare(cstrA, strB, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(String.Compare(strA, strB, StringComparison.CurrentCultureIgnoreCase), CString.Compare(cstrA, strB, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(String.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase), CString.Compare(cstrA, strB, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void StringTest(CString cstr, String str, String strLower, String strUpper)
    {

        Assert.Equal(0, CString.Compare(cstr, str));
        Assert.Equal(0, CString.Compare(cstr, str, true));
        Assert.Equal(0, CString.Compare(cstr, str, false));
        Assert.Equal(0, CString.Compare(cstr, str, StringComparison.Ordinal));
        Assert.Equal(0, CString.Compare(cstr, str, StringComparison.CurrentCulture));
        Assert.Equal(0, CString.Compare(cstr, str, StringComparison.InvariantCulture));
        Assert.Equal(0, CString.Compare(cstr, str, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, str, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, str, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(String.Compare(str, strLower), CString.Compare(cstr, strLower));
        Assert.Equal(String.Compare(str, strLower, true), CString.Compare(cstr, strLower, true));
        Assert.Equal(String.Compare(str, strLower, false), CString.Compare(cstr, strLower, false));
        Assert.Equal(String.Compare(str, strLower, StringComparison.Ordinal), CString.Compare(cstr, strLower, StringComparison.Ordinal));
        Assert.Equal(String.Compare(str, strLower, StringComparison.CurrentCulture), CString.Compare(cstr, strLower, StringComparison.CurrentCulture));
        Assert.Equal(String.Compare(str, strLower, StringComparison.InvariantCulture), CString.Compare(cstr, strLower, StringComparison.InvariantCulture));
        Assert.Equal(0, CString.Compare(cstr, strLower, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, strLower, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, strLower, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(String.Compare(str, strUpper), CString.Compare(cstr, strUpper));
        Assert.Equal(String.Compare(str, strUpper, true), CString.Compare(cstr, strUpper, true));
        Assert.Equal(String.Compare(str, strUpper, false), CString.Compare(cstr, strUpper, false));
        Assert.Equal(String.Compare(str, strUpper, StringComparison.Ordinal), CString.Compare(cstr, strUpper, StringComparison.Ordinal));
        Assert.Equal(String.Compare(str, strUpper, StringComparison.CurrentCulture), CString.Compare(cstr, strUpper, StringComparison.CurrentCulture));
        Assert.Equal(String.Compare(str, strUpper, StringComparison.InvariantCulture), CString.Compare(cstr, strUpper, StringComparison.InvariantCulture));
        Assert.Equal(0, CString.Compare(cstr, strUpper, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, strUpper, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, strUpper, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void CStringTest(CString cstrA, String strA, CString cstrB)
    {
        String strB = cstrB.ToString();
        Assert.Equal(strA.CompareTo(strB), cstrA.CompareTo(cstrB));
        Assert.Equal(String.Compare(strA, strB, true), CString.Compare(cstrA, cstrB, true));
        Assert.Equal(String.Compare(strA, strB, false), CString.Compare(cstrA, cstrB, false));
        Assert.Equal(String.Compare(strA, strB, StringComparison.Ordinal), CString.Compare(cstrA, cstrB, StringComparison.Ordinal));
        Assert.Equal(String.Compare(strA, strB, StringComparison.CurrentCulture), CString.Compare(cstrA, cstrB, StringComparison.CurrentCulture));
        Assert.Equal(String.Compare(strA, strB, StringComparison.InvariantCulture), CString.Compare(cstrA, cstrB, StringComparison.InvariantCulture));
        Assert.Equal(String.Compare(strA, strB, StringComparison.OrdinalIgnoreCase), CString.Compare(cstrA, cstrB, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(String.Compare(strA, strB, StringComparison.CurrentCultureIgnoreCase), CString.Compare(cstrA, cstrB, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(String.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase), CString.Compare(cstrA, cstrB, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void CStringTest(CString cstr, String str, CString cstrLower, CString cstrUpper)
    {
        String strLower = cstrLower.ToString();
        String strUpper = cstrUpper.ToString();

        Assert.Equal(String.Compare(str, strLower), CString.Compare(cstr, cstrLower));
        Assert.Equal(String.Compare(str, strLower, true), CString.Compare(cstr, cstrLower, true));
        Assert.Equal(String.Compare(str, strLower, false), CString.Compare(cstr, cstrLower, false));
        Assert.Equal(String.Compare(str, strLower, StringComparison.Ordinal), CString.Compare(cstr, cstrLower, StringComparison.Ordinal));
        Assert.Equal(String.Compare(str, strLower, StringComparison.CurrentCulture), CString.Compare(cstr, cstrLower, StringComparison.CurrentCulture));
        Assert.Equal(String.Compare(str, strLower, StringComparison.InvariantCulture), CString.Compare(cstr, cstrLower, StringComparison.InvariantCulture));
        Assert.Equal(0, CString.Compare(cstr, cstrLower, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, cstrLower, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, cstrLower, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(String.Compare(str, strUpper), CString.Compare(cstr, cstrUpper));
        Assert.Equal(String.Compare(str, strUpper, true), CString.Compare(cstr, cstrUpper, true));
        Assert.Equal(String.Compare(str, strUpper, false), CString.Compare(cstr, cstrUpper, false));
        Assert.Equal(String.Compare(str, strUpper, StringComparison.Ordinal), CString.Compare(cstr, cstrUpper, StringComparison.Ordinal));
        Assert.Equal(String.Compare(str, strUpper, StringComparison.CurrentCulture), CString.Compare(cstr, cstrUpper, StringComparison.CurrentCulture));
        Assert.Equal(String.Compare(str, strUpper, StringComparison.InvariantCulture), CString.Compare(cstr, cstrUpper, StringComparison.InvariantCulture));
        Assert.Equal(0, CString.Compare(cstr, cstrUpper, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, cstrUpper, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(0, CString.Compare(cstr, cstrUpper, StringComparison.InvariantCultureIgnoreCase));
    }
}

