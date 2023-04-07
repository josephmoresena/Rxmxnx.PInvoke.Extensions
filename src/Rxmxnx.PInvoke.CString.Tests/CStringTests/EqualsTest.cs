namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class EqualsTest
{
    [Fact]
    internal async Task TestAsync()
    {
        CancellationTokenSource source = new();
        List<Task> testTasks = new();
        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            testTasks.Add(EqualityTestAsync(i, source.Token));

        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
            {
                testTasks.Add(InequalityTestAsync(i, j, source.Token));
                testTasks.Add(OperatorTestAsync(i, j, source.Token));
            }

        try
        {
            await Task.WhenAll(testTasks);
        }
        catch (Exception)
        {
            source.Cancel();
            throw;
        }
    }

    private static Task EqualityTestAsync(Int32 index, CancellationToken token) => Task.Run(() => EqualityTest(index), token);
    private static Task InequalityTestAsync(Int32 indexA, Int32 indexB, CancellationToken token) => Task.Run(() => InequalityTest(indexA, indexB), token);
    private static Task OperatorTestAsync(Int32 indexA, Int32 indexB, CancellationToken token)
    {
        String strA = TestSet.Utf16Text[indexA];
        String strB = TestSet.Utf16Text[indexB];

        CString cstrA = new(TestSet.Utf8Text[indexA]);
        CString cstrB = new(TestSet.Utf8Text[indexB]);

        return Task.Run(() => OperatorTest(cstrA, strA, strB, cstrB), token);
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
        CStringTest(cstr, str, strLower, cstrLower, strUpper, cstrUpper);
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

        CStringTest(cstrA, strA, strB, cstrB);
        CStringTest(cstrA, strA, strLowerB, cstrLowerB);
        CStringTest(cstrA, strA, strUpperB, cstrUpperB);

        CStringTest(cstrB, strB, strA, cstrA);
        CStringTest(cstrB, strB, strLowerA, cstrLowerA);
        CStringTest(cstrB, strB, strUpperA, cstrUpperA);
    }
    private static void StringTest(CString cstrA, String strA, String strB)
    {
        Assert.Equal(strA.Equals(strB), cstrA.Equals(strB));
        Assert.Equal(strA.Equals(strB, StringComparison.CurrentCulture), cstrA.Equals(strB, StringComparison.CurrentCulture));
        Assert.Equal(strA.Equals(strB, StringComparison.CurrentCultureIgnoreCase), cstrA.Equals(strB, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(strA.Equals(strB, StringComparison.InvariantCulture), cstrA.Equals(strB, StringComparison.InvariantCulture));
        Assert.Equal(strA.Equals(strB, StringComparison.InvariantCultureIgnoreCase), cstrA.Equals(strB, StringComparison.InvariantCultureIgnoreCase));
        Assert.Equal(strA.Equals(strB, StringComparison.Ordinal), cstrA.Equals(strB, StringComparison.Ordinal));
        Assert.Equal(strA.Equals(strB, StringComparison.OrdinalIgnoreCase), cstrA.Equals(strB, StringComparison.OrdinalIgnoreCase));
    }
    private static void StringTest(CString cstr, String str, String strLower, String strUpper)
    {
        Assert.True(cstr.Equals(str, StringComparison.Ordinal));
        Assert.True(cstr.Equals(str, StringComparison.CurrentCulture));
        Assert.True(cstr.Equals(str, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(str, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(str, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(str, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(str.Equals(strLower, StringComparison.Ordinal), cstr.Equals(strLower, StringComparison.Ordinal));
        Assert.Equal(str.Equals(strLower, StringComparison.CurrentCulture), cstr.Equals(strLower, StringComparison.CurrentCulture));
        Assert.Equal(str.Equals(strLower, StringComparison.InvariantCulture), cstr.Equals(strLower, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(strLower, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(strLower, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(strLower, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(str.Equals(strUpper, StringComparison.Ordinal), cstr.Equals(strUpper, StringComparison.Ordinal));
        Assert.Equal(str.Equals(strUpper, StringComparison.CurrentCulture), cstr.Equals(strUpper, StringComparison.CurrentCulture));
        Assert.Equal(str.Equals(strUpper, StringComparison.InvariantCulture), cstr.Equals(strUpper, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(strUpper, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(strUpper, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(strUpper, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void CStringTest(CString cstrA, String strA, String strB, CString cstrB)
    {
        Assert.Equal(strA.Equals(strB), cstrA.Equals(cstrB));
        Assert.Equal(strA.Equals(strB, StringComparison.CurrentCulture), cstrA.Equals(cstrB, StringComparison.CurrentCulture));
        Assert.Equal(strA.Equals(strB, StringComparison.CurrentCultureIgnoreCase), cstrA.Equals(cstrB, StringComparison.CurrentCultureIgnoreCase));
        Assert.Equal(strA.Equals(strB, StringComparison.InvariantCulture), cstrA.Equals(cstrB, StringComparison.InvariantCulture));
        Assert.Equal(strA.Equals(strB, StringComparison.InvariantCultureIgnoreCase), cstrA.Equals(cstrB, StringComparison.InvariantCultureIgnoreCase));
        Assert.Equal(strA.Equals(strB, StringComparison.Ordinal), cstrA.Equals(cstrB, StringComparison.Ordinal));
        Assert.Equal(strA.Equals(strB, StringComparison.OrdinalIgnoreCase), cstrA.Equals(cstrB, StringComparison.OrdinalIgnoreCase));
    }
    private static void CStringTest(CString cstr, String str, String strLower, CString cstrLower, String strUpper, CString cstrUpper)
    {
        Assert.Equal(str.Equals(strLower, StringComparison.Ordinal), cstr.Equals(cstrLower, StringComparison.Ordinal));
        Assert.Equal(str.Equals(strLower, StringComparison.CurrentCulture), cstr.Equals(cstrLower, StringComparison.CurrentCulture));
        Assert.Equal(str.Equals(strLower, StringComparison.InvariantCulture), cstr.Equals(cstrLower, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(cstrLower, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(cstrLower, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(cstrLower, StringComparison.InvariantCultureIgnoreCase));

        Assert.Equal(str.Equals(strUpper, StringComparison.Ordinal), cstr.Equals(cstrUpper, StringComparison.Ordinal));
        Assert.Equal(str.Equals(strUpper, StringComparison.CurrentCulture), cstr.Equals(cstrUpper, StringComparison.CurrentCulture));
        Assert.Equal(str.Equals(strUpper, StringComparison.InvariantCulture), cstr.Equals(cstrUpper, StringComparison.InvariantCulture));
        Assert.True(cstr.Equals(cstrUpper, StringComparison.OrdinalIgnoreCase));
        Assert.True(cstr.Equals(cstrUpper, StringComparison.CurrentCultureIgnoreCase));
        Assert.True(cstr.Equals(cstrUpper, StringComparison.InvariantCultureIgnoreCase));
    }
    private static void OperatorTest(CString cstrA, String strA, String strB, CString cstrB)
    {
        Boolean equals = strA == strB;

        Assert.True(strA == cstrA);
        Assert.True(cstrB == strB);
        Assert.Equal(equals, cstrA == cstrB);
        Assert.Equal(equals, strA == cstrB);
        Assert.Equal(equals, cstrA == strB);
        Assert.Equal(!equals, cstrA != cstrB);
        Assert.Equal(!equals, strA != cstrB);
        Assert.Equal(!equals, cstrA != strB);
    }
}