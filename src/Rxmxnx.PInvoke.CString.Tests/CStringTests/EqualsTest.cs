using System;

namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class EqualsTest
{
    [Fact]
    internal void Test()
    {
        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            EqualityTest(i);
    }

    private void EqualityTest(Int32 index)
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

    private static void StringTest(CString cstr, String str, String strLower, String strUpper)
    {
        Assert.True(cstr.Equals(str, StringComparison.Ordinal));
        Assert.True(cstr.Equals(str, StringComparison.CurrentCulture));
        Assert.True(cstr.Equals(str, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(str, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(str, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(str, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(str.Equals(strLower), cstr.Equals(strLower, StringComparison.Ordinal));
        Assert.Equal(str.Equals(strLower), cstr.Equals(strLower, StringComparison.CurrentCulture));
        Assert.Equal(str.Equals(strLower), cstr.Equals(strLower, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(strLower, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(strLower, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(strLower, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(str.Equals(strUpper), cstr.Equals(strUpper, StringComparison.Ordinal));
        Assert.Equal(str.Equals(strUpper), cstr.Equals(strUpper, StringComparison.CurrentCulture));
        Assert.Equal(str.Equals(strUpper), cstr.Equals(strUpper, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(strUpper, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(strUpper, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(strUpper, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void CStringTest(CString cstr, String str, CString cstrLower, CString cstrUpper)
    {
        Assert.Equal(cstrLower.Equals(str), cstr.Equals(cstrLower, StringComparison.Ordinal));
        Assert.Equal(cstrLower.Equals(str), cstr.Equals(cstrLower, StringComparison.CurrentCulture));
        Assert.Equal(cstrLower.Equals(str), cstr.Equals(cstrLower, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(cstrLower, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(cstrLower, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(cstrLower, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(cstrUpper.Equals(str), cstr.Equals(cstrUpper, StringComparison.Ordinal));
        Assert.Equal(cstrUpper.Equals(str), cstr.Equals(cstrUpper, StringComparison.CurrentCulture));
        Assert.Equal(cstrUpper.Equals(str), cstr.Equals(cstrUpper, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(cstrUpper, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(cstrUpper, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(cstrUpper, StringComparison.InvariantCultureIgnoreCase));
    }
}

