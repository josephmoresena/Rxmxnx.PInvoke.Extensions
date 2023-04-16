namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal sealed record ComparisonTestResult
{
    public Int32 Normal { get; private init; }
    public Int32 CaseSensitive { get; private init; }
    public Int32 CaseInsensitive { get; private init; }
    public IReadOnlyDictionary<StringComparison, Int32> Comparisons { get; private init; }

    private ComparisonTestResult()
    {
        this.Comparisons = default!;
    }

    public static async Task<ComparisonTestResult> CompareAsync(String strA, String strB, CancellationToken token)
    {
        Task<Int32> normalTask = Task.Run<Int32>(() => strA.CompareTo(strB), token);
        Task<Int32> ciTask = Task.Run<Int32>(() => String.Compare(strA, strB, true), token);
        Task<Int32> csTask = Task.Run<Int32>(() => String.Compare(strA, strB, false), token);
        Task<Dictionary<StringComparison, Int32>> cTask = GetComparisonsAsync(strA, strB, token);

        return new()
        {
            Normal = await normalTask.ConfigureAwait(false),
            CaseInsensitive = await ciTask.ConfigureAwait(false),
            CaseSensitive = await csTask.ConfigureAwait(false),
            Comparisons = await cTask.ConfigureAwait(false),
        };
    }

    private static async Task<Dictionary<StringComparison, Int32>> GetComparisonsAsync(String strA, String strB, CancellationToken token)
    {
        ConcurrentDictionary<StringComparison, Int32> result = new();
        StringComparison[] values = Enum.GetValues<StringComparison>();
        Task[] tasks = new Task[values.Length];

        for (Int32 i = 0; i < tasks.Length; i++)
        {
            StringComparison comparisonType = values[i];
            tasks[i] = Task.Run(() => _ = result.TryAdd(comparisonType, String.Compare(strA, strB, comparisonType)), token);
        }

        await Task.WhenAll(tasks).ConfigureAwait(false);
        return new(result);
    }
}
