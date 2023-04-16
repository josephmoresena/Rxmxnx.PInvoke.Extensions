namespace Rxmxnx.PInvoke.Tests.CStringTests;

public sealed class CompareTest
{
    [Fact]
    internal async Task TestAsync()
    {
        CancellationTokenSource source = new();
        List<Task> testTasks = new();

        for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
            for (Int32 j = 0; j < TestSet.Utf16Text.Count; j++)
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

        ComparisonTestResult testAB = await ComparisonTestResult.CompareAsync(strA, strB, token).ConfigureAwait(false);

        List<Task> comparisionTasks = new(6)
        {
            StringTestAsync(testAB, cstrA, strB, token),
            CStringTestAsync(testAB, cstrA, cstrB, token),
            CompleteTestAsync(ComparisonTestResult.CompareAsync(strA, strLowerB, token), cstrA, strLowerB, cstrLowerB, token),
            CompleteTestAsync(ComparisonTestResult.CompareAsync(strA, strUpperB, token), cstrA, strUpperB, cstrUpperB, token),
        };

        if (testAB.Normal != 0)
        {
            comparisionTasks.Add(CompleteTestAsync(ComparisonTestResult.CompareAsync(strB, strA, token), cstrB, strA, cstrA, token));
            comparisionTasks.Add(CompleteTestAsync(ComparisonTestResult.CompareAsync(strB, strLowerA, token), cstrB, strLowerA, cstrLowerA, token));
            comparisionTasks.Add(CompleteTestAsync(ComparisonTestResult.CompareAsync(strB, strUpperA, token), cstrB, strUpperA, cstrUpperA, token));
        }

        await Task.WhenAll(comparisionTasks).ConfigureAwait(false);
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

        List<Task> tasks = new(6)
        {
            CompleteCulturalTestAsync(strA, cstrA, strB, cstrB, token),
            CompleteCulturalTestAsync(strA, cstrA, strLowerB, cstrLowerB, token),
            CompleteCulturalTestAsync(strA, cstrA, strUpperB, cstrUpperB, token),
        };

        if (!Object.ReferenceEquals(strA, strB))
        {
            tasks.Add(CompleteCulturalTestAsync(strB, cstrB, strA, cstrA, token));
            tasks.Add(CompleteCulturalTestAsync(strB, cstrB, strLowerA, cstrLowerA, token));
            tasks.Add(CompleteCulturalTestAsync(strB, cstrB, strUpperA, cstrUpperA, token));
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
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
            Task.Run(() => Assert.Equal(results.Normal, cstrA.CompareTo(strB)), token),
            Task.Run(() => Assert.Equal(results.Normal, CString.Compare(cstrA, strB)), token),
            Task.Run(() => Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, strB, true)), token),
            Task.Run(() => Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, strB, false)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.Ordinal], CString.Compare(cstrA, strB, StringComparison.Ordinal)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCulture], CString.Compare(cstrA, strB, StringComparison.CurrentCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCulture], CString.Compare(cstrA, strB, StringComparison.InvariantCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase], CString.Compare(cstrA, strB, StringComparison.OrdinalIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase], CString.Compare(cstrA, strB, StringComparison.CurrentCultureIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase], CString.Compare(cstrA, strB, StringComparison.InvariantCultureIgnoreCase)), token)
        );
    private static Task StringTestAsync(CulturalComparisonTestResult results, CString cstrA, String strB, CancellationToken token)
        => Task.WhenAll
        (
            Task.Run(() => Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, strB, true, results.Culture)), token),
            Task.Run(() => Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, strB, false, results.Culture)), token)
        );
    private static Task CStringTestAsync(ComparisonTestResult results, CString cstrA, CString cstrB, CancellationToken token)
        => Task.WhenAll
        (
            Task.Run(() => Assert.Equal(results.Normal, cstrA.CompareTo(cstrB)), token),
            Task.Run(() => Assert.Equal(results.Normal, CString.Compare(cstrA, cstrB)), token),
            Task.Run(() => Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, cstrB, true)), token),
            Task.Run(() => Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, cstrB, false)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.Ordinal], CString.Compare(cstrA, cstrB, StringComparison.Ordinal)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCulture], CString.Compare(cstrA, cstrB, StringComparison.CurrentCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCulture], CString.Compare(cstrA, cstrB, StringComparison.InvariantCulture)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.OrdinalIgnoreCase], CString.Compare(cstrA, cstrB, StringComparison.OrdinalIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.CurrentCultureIgnoreCase], CString.Compare(cstrA, cstrB, StringComparison.CurrentCultureIgnoreCase)), token),
            Task.Run(() => Assert.Equal(results.Comparisons[StringComparison.InvariantCultureIgnoreCase], CString.Compare(cstrA, cstrB, StringComparison.InvariantCultureIgnoreCase)), token)
        );
    private static Task CStringTestAsync(CulturalComparisonTestResult results, CString cstrA, CString cstrB, CancellationToken token)
        => Task.WhenAll
        (
            Task.Run(() => Assert.Equal(results.CaseInsensitive, CString.Compare(cstrA, cstrB, true, results.Culture)), token),
            Task.Run(() => Assert.Equal(results.CaseSensitive, CString.Compare(cstrA, cstrB, false, results.Culture)), token)
        );

}
