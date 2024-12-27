namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal sealed record ComparisonTestResult
{
	public Int32 Normal { get; private init; }
	public Int32 CaseSensitive { get; private init; }
	public Int32 CaseInsensitive { get; private init; }
	public IReadOnlyDictionary<StringComparison, Int32> Comparisons { get; private init; }
	private ComparisonTestResult() => this.Comparisons = default!;

	public static ComparisonTestResult Compare(String strA, String strB)
	{
		Int32 normal = strA.CompareTo(strB);
		Int32 caseInsensitive = String.Compare(strA, strB, true);
		Int32 caseSensitive = String.Compare(strA, strB, false);
		return new()
		{
			Normal = normal,
			CaseInsensitive = caseInsensitive,
			CaseSensitive = caseSensitive,
			Comparisons = ComparisonTestResult.GetComparisons(strA, strB),
		};
	}

	private static Dictionary<StringComparison, Int32> GetComparisons(String strA, String strB)
	{
		ConcurrentDictionary<StringComparison, Int32> result = new();
		StringComparison[] values = Enum.GetValues<StringComparison>();
		foreach (StringComparison comparisonType in values.AsSpan())
			result.TryAdd(comparisonType, String.Compare(strA, strB, comparisonType));

		return new(result);
	}
}