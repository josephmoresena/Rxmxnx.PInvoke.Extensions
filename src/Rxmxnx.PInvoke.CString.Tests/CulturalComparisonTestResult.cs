namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal sealed record CulturalComparisonTestResult
{
    private const Int32 count = 10;
    private static readonly CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

    public CultureInfo Culture { get; private init; }
    public Int32 CaseSensitive { get; private init; }
    public Int32 CaseInsensitive { get; private init; }

    private CulturalComparisonTestResult(CultureInfo culture)
    {
        this.Culture = culture;
    }

    public static async Task<CulturalComparisonTestResult> CompareAsync(String strA, String strB, CancellationToken token)
    {
        CultureInfo culture = cultures[Random.Shared.Next(0, cultures.Length)];
        HashSet<CultureInfo> currentCultures = new();
        Task<Int32> ciTask = Task.Run(() => String.Compare(strA, strB, true, culture), token);
        Task<Int32> csTask = Task.Run(() => String.Compare(strA, strB, false, culture), token);

        return new(culture)
        {
            CaseInsensitive = await ciTask.ConfigureAwait(false),
            CaseSensitive = await csTask.ConfigureAwait(false),
        };
    }
}
