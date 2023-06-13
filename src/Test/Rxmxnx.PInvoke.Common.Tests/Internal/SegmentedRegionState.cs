namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
internal sealed record SegmentedRegionState<T> where T : unmanaged
{
    public Int32 Initial { get; init; }
    public T[] Values { get; init; }
    public ValueRegion<T> Region { get; init; }
    public Boolean IsReference { get; init; }
    public Int32 Length { get; init; }
    public Int32 Start { get; init; }
    public Int32 Count { get; init; }
    public ValueRegion<T> Segment { get; init; }

    public Int32 GetRegionOffset(Int32 index) => this.Start + index;
    public Int32 GetArrayOffset(Int32 index) => this.Initial + this.Start + index;
    public Int32 SkipArray() => this.Initial + this.Start;

    public SegmentedRegionState()
    {
        this.Values = default!;
        this.Region = default!;
        this.Segment = default!;
    }
}
