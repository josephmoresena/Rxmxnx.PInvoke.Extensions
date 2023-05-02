namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[ExcludeFromCodeCoverage]
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

        try
        {
            AssertRegionSegment(values0, handles);
            AssertRegionSegment(values1, handles);
            AssertRegionSegment(values2, handles);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static unsafe void AssertRegionSegment<T>(T[] values, ICollection<GCHandle> handles) where T : unmanaged
    {
        Int32 initialCount = handles.Count;
        ValueRegion<T> region = Create(values, handles);
        Boolean isReference = handles.Count > initialCount;

        AssertRegionSegment(values, region, isReference);
    }

    private static unsafe void AssertRegionSegment<T>(T[] values, ValueRegion<T> region, Boolean isReference, Int32 initial = 0) where T : unmanaged
    {
        Int32 length = region.AsSpan().Length;
        for (Int32 i = 0; i < length; i++)
        {
            Int32 start = Random.Shared.Next(i, length);
            Int32 count = Random.Shared.Next(start, length) - start;
            ValueRegion<T> segRegion = ValueRegion<T>.Create(region, start, count);
            ReadOnlySpan<T> span = segRegion;
            T[]? array = (T[]?)segRegion;
            T[] newArray = segRegion.ToArray();

            Assert.Equal(count, span.Length);
            Assert.Equal(count != length && !isReference || region.IsSegmented, segRegion.IsSegmented);

            fixed (void* _ = values)
                for (Int32 j = 0; j < count; j++)
                {
                    Assert.Equal(region[start + j], segRegion[j]);
                    Assert.Equal(values[initial + start + j], segRegion[j]);
                    Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(span[j]), ref values[initial + start + j]));
                }

            if (segRegion.IsSegmented || isReference)
                Assert.Null(array);
            else
                Assert.Equal((T[]?)segRegion, array);

            if (array is not null)
            {
                Assert.Same(values, array);
                Assert.Same(values, newArray);
            }
            else if (isReference && count == 0)
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    Assert.Equal(IntPtr.Zero, new(ptr));

            Assert.Equal(values.Skip(start + initial).Take(count), newArray);

            if (!region.IsSegmented && !isReference) //TODO: Support referenced
                AssertRegionSegment(values, segRegion, isReference, start);
        }
    }
}

