namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class SegmentTest : ValueRegionTestBase
{
    [Fact]
    internal void BooleanTest() => Test<Boolean>();
    [Fact]
    internal void ByteTest() => Test<Byte>();
    [Fact]
    internal void Int16Test() => Test<Int16>();
    [Fact]
    internal void CharTest() => Test<Char>();
    [Fact]
    internal void Int32Test() => Test<Int32>();
    [Fact]
    internal void Int64Test() => Test<Int64>();
    [Fact]
    internal void Int128Test() => Test<Int128>();
    [Fact]
    internal void GuidTest() => Test<Guid>();
    [Fact]
    internal void SingleTest() => Test<Single>();
    [Fact]
    internal void HalfTest() => Test<Half>();
    [Fact]
    internal void DoubleTest() => Test<Double>();
    [Fact]
    internal void DecimalTest() => Test<Decimal>();
    [Fact]
    internal void DateTimeTest() => Test<DateTime>();
    [Fact]
    internal void TimeOnlyTest() => Test<TimeOnly>();
    [Fact]
    internal void TimeSpanTest() => Test<TimeSpan>();

    private static unsafe void Test<T>() where T : unmanaged
    {
        List<GCHandle> handles = new();
        T[] values0 = fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
        T[] values1 = fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
        T[] values2 = fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
        T[] values3 = fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();

        try
        {
            InvalidTest<T>(handles);
            AssertRegionSegment(values0, handles);
            AssertRegionSegment(values0, handles);
            AssertRegionSegment(values1, handles);
            AssertRegionSegment(values1, handles);
            AssertRegionSegment(values2, handles);
            AssertRegionSegment(values2, handles);
            AssertRegionSegment(values3, handles);
            AssertRegionSegment(values3, handles);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static void InvalidTest<T>(ICollection<GCHandle> handles) where T : unmanaged
    {
        AssertInvalidSegment(Create(Array.Empty<T>(), handles));
        AssertInvalidSegment(Create(Array.Empty<T>(), handles));
        AssertInvalidSegment(Create(Array.Empty<T>(), handles));
        AssertInvalidSegment(Create(Array.Empty<T>(), handles));
        AssertInvalidSegment(Create(Array.Empty<T>(), handles));
        AssertInvalidSegment(Create(Array.Empty<T>(), handles));
    }

    private static void AssertInvalidSegment<T>(ValueRegion<T> emptyRegion) where T : unmanaged
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(-1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(0, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(0, 1));

        if ((T[]?)emptyRegion is T[] arr)
            Assert.Same(emptyRegion.ToArray(), arr);
    }

    private static unsafe void AssertRegionSegment<T>(T[] values, ICollection<GCHandle> handles) where T : unmanaged
    {
        ValueRegion<T> region = Create(values, handles, out Boolean isReference);
        AssertRegionSegment(values, region, isReference);
    }

    private static unsafe void AssertRegionSegment<T>(T[] values, ValueRegion<T> region, Boolean isReference, Int32 initial = 0) where T : unmanaged
    {
        Int32 length = region.AsSpan().Length;
        for (Int32 i = 0; i < length; i++)
        {
            Int32 start = Random.Shared.Next(i, length);
            Int32 count = Random.Shared.Next(start, length) - start;
            ValueRegion<T> segment = region.Slice(start, count);

            AssertSegment(new SegmentedRegionState<T>()
            {
                Initial = initial,
                Values = values,
                Region = region,
                IsReference = isReference,
                Length = length,
                Start = start,
                Count = count,
                Segment = segment,
            });

            if (!region.IsSegmented)
                AssertRegionSegment(values, segment, isReference, initial + start);
        }

        AssertSegment(new SegmentedRegionState<T>()
        {
            Initial = initial,
            Values = values,
            Region = region,
            IsReference = isReference,
            Length = length,
            Start = 0,
            Count = length,
            Segment = region.Slice(0),
        });

    }

    private static unsafe void AssertSegment<T>(SegmentedRegionState<T> state) where T : unmanaged
    {
        ReadOnlySpan<T> span = state.Segment;
        T[]? array = (T[]?)state.Segment;
        T[] newArray = state.Segment.ToArray();

        Assert.Equal(state.Count, span.Length);
        Assert.Equal(state.Count != state.Length && !state.IsReference || state.Region.IsSegmented, state.Segment.IsSegmented);

        for (Int32 j = 0; j < state.Count; j++)
        {
            Int32 arrayOffset = state.GetArrayOffset(j);

            Assert.Equal(state.Region[state.GetRegionOffset(j)], state.Segment[j]);
            Assert.Equal(state.Values[arrayOffset], state.Segment[j]);
            if (!state.IsReference)
                Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(span[j]), ref state.Values[arrayOffset]));
            else
                Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(span[j]), ref state.Values.AsSpan()[arrayOffset..][0]));
        }

        if (state.Segment.IsSegmented || state.IsReference)
            Assert.Null(array);
        else
            Assert.Equal((T[]?)state.Segment, array);

        if (array is not null)
        {
            Assert.Same(state.Values, array);
            if (state.Values.Length > 0)
                Assert.NotSame(state.Values, newArray);
            else
                Assert.Same(state.Values, newArray);
        }
        else if (state.IsReference && state.Count == 0)
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
                Assert.Equal(IntPtr.Zero, new(ptr));

        Assert.Equal(state.Values.Skip(state.SkipArray()).Take(state.Count), newArray);
    }
}

