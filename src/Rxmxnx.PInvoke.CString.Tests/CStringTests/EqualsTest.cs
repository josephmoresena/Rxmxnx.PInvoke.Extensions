namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class EqualsTest
{
    [Fact]
    internal async Task TestAsync()
    {
        CancellationTokenSource source = new();
        List<Task> testTasks = new();

        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
                testTasks.Add(CompleteTestAsync(i, j, source.Token));

        try
        {
            await Task.WhenAll(testTasks).ConfigureAwait(false);
        }
        catch (Exception)
        {
            source.Cancel();
            throw;
        }
    }

    [Fact]
    internal async Task CulturalTestAsync()
    {
        CancellationTokenSource source = new();
        List<Task> testTasks = new();

        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            for (Int32 j = i; j < TestSet.Utf16Text.Count; j++)
                testTasks.Add(CompleteCulturalTestAsync(i, j, source.Token));
        try
        {
            await Task.WhenAll(testTasks).ConfigureAwait(false);
        }
        catch (Exception)
        {
            source.Cancel();
            throw;
        }
    }

    private static async Task CompleteTestAsync(Int32 indexA, Int32 indexB, CancellationToken token)
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

        List<Task> equalizationTasks = new();
        ComparisonTestResult testAB = await ComparisonTestResult.CompareAsync(strA, strB, token).ConfigureAwait(false);
        await Task.WhenAll(
            StringTestAsync(testAB, cstrA, strB, token),
            CStringTestAsync(testAB, cstrA, cstrB, token),
            OperatorTestAsync(testAB, cstrA, strA, cstrB, strB, token),
            CompleteTestAsync(ComparisonTestResult.CompareAsync(strA, strLowerB, token), cstrA, strLowerB, cstrLowerB, token),
            CompleteTestAsync(ComparisonTestResult.CompareAsync(strA, strUpperB, token), cstrA, strUpperB, cstrUpperB, token)
        ).ConfigureAwait(false);
    }
    private static async Task CompleteTestAsync(Task<ComparisonTestResult> resultsTask, CString cstrA, String strB, CString cstrB, CancellationToken token)
    {
        ComparisonTestResult result = await resultsTask;
        await StringTestAsync(result, cstrA, strB, token).ConfigureAwait(false);
        await CStringTestAsync(result, cstrA, cstrB, token).ConfigureAwait(false);
    }
    private static async Task CompleteCulturalTestAsync(Int32 indexA, Int32 indexB, CancellationToken token)
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

        await Task.WhenAll(
            CompleteCulturalTestAsync(strA, cstrA, strB, cstrB, token),
            CompleteCulturalTestAsync(strA, cstrA, strLowerB, cstrLowerB, token),
            CompleteCulturalTestAsync(strA, cstrA, strUpperB, cstrUpperB, token)
        ).ConfigureAwait(false);
    }
    private static async Task CompleteCulturalTestAsync(String strA, CString cstrA, String strB, CString cstrB, CancellationToken token)
    {
        CulturalComparisonTestResult results = await CulturalComparisonTestResult.CompareAsync(strA, strB, token).ConfigureAwait(false);
        await StringTestAsync(results, cstrA, strB, token);
        await CStringTestAsync(results, cstrA, cstrB, token);
    }

    private static Task StringTestAsync(ComparisonTestResult results, CString cstrA, String strB, CancellationToken token)
        => Task.WhenAll
        (
            Task.Run(() => Assert.Equal(results.Normal == 0, cstrA.Equals(strB)), token),
            Task.Run(() => Assert.Equal(results.Normal == 0, CString.Equals(cstrA, strB)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.Ordinal] == 0, cstrA.Equals(strB, StringComparison.Ordinal)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCulture] == 0, cstrA.Equals(strB, StringComparison.CurrentCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCulture] == 0, cstrA.Equals(strB, StringComparison.InvariantCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase] == 0, cstrA.Equals(strB, StringComparison.OrdinalIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase] == 0, cstrA.Equals(strB, StringComparison.CurrentCultureIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase] == 0, cstrA.Equals(strB, StringComparison.InvariantCultureIgnoreCase)), token)
        );
    private static Task StringTestAsync(CulturalComparisonTestResult results, CString cstrA, String strB, CancellationToken token)
        => Task.WhenAll
        (
            Task.Run(() => Assert.Equal(results.CaseInsensitive == 0, StringUtf8Comparator.Create(true, results.Culture).TextEquals(cstrA, strB)), token),
            Task.Run(() => Assert.Equal(results.CaseSensitive == 0, StringUtf8Comparator.Create(false, results.Culture).TextEquals(cstrA, strB)), token)
        );
    private static Task CStringTestAsync(ComparisonTestResult results, CString cstrA, CString cstrB, CancellationToken token)
        => Task.WhenAll
        (
            Task.Run(() => Assert.Equal(results.Normal == 0, cstrA.Equals(cstrB)), token),
            Task.Run(() => Assert.Equal(results.Normal == 0, CString.Equals(cstrA, cstrB)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.Ordinal] == 0, cstrA.Equals(cstrB, StringComparison.Ordinal)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCulture] == 0, cstrA.Equals(cstrB, StringComparison.CurrentCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCulture] == 0, cstrA.Equals(cstrB, StringComparison.InvariantCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase] == 0, cstrA.Equals(cstrB, StringComparison.OrdinalIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase] == 0, cstrA.Equals(cstrB, StringComparison.CurrentCultureIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase] == 0, cstrA.Equals(cstrB, StringComparison.InvariantCultureIgnoreCase)), token)
        );
    private static Task CStringTestAsync(CulturalComparisonTestResult results, CString cstrA, CString cstrB, CancellationToken token)
        => Task.WhenAll
        (
            Task.Run(() => Assert.Equal(results.CaseInsensitive == 0, CStringUtf8Comparator.Create(true, results.Culture).TextEquals(cstrA, cstrB)), token),
            Task.Run(() => Assert.Equal(results.CaseSensitive == 0, CStringUtf8Comparator.Create(false, results.Culture).TextEquals(cstrA, cstrB)), token)
        );
    private static Task OperatorTestAsync(ComparisonTestResult results, CString cstrA, String strA, CString cstrB, String strB, CancellationToken token)
    {
        Boolean equals = results.Normal == 0;

        return Task.WhenAll(
            Task.Run(() => Assert.True(strA == cstrA), token),
            Task.Run(() => Assert.True(cstrB == strB), token),
            Task.Run(() => Assert.Equal(equals, cstrA == cstrB), token),
            Task.Run(() => Assert.Equal(equals, strA == cstrB), token),
            Task.Run(() => Assert.Equal(equals, cstrA == strB), token),
            Task.Run(() => Assert.Equal(!equals, cstrA != cstrB), token),
            Task.Run(() => Assert.Equal(!equals, strA != cstrB), token),
            Task.Run(() => Assert.Equal(!equals, cstrA != strB), token)
        );
    }
}