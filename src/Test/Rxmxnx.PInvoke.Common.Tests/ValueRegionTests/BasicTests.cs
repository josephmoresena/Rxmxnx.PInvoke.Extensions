namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[ExcludeFromCodeCoverage]
public sealed class BasicTests : ValueRegionTestBase
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
            AssertRegion(values0, handles);
            AssertRegion(values0, handles);
            AssertRegion(values1, handles);
            AssertRegion(values1, handles);
            AssertRegion(values2, handles);
            AssertRegion(values2, handles);
            AssertRegion(values3, handles);
            AssertRegion(values3, handles);
        }
        finally
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
    }

    private static unsafe void AssertRegion<T>(T[] values, ICollection<GCHandle> handles) where T : unmanaged
    {
        Int32 initialCount = handles.Count;
        ValueRegion<T> region = Create(values, handles, out Boolean isReference);
        ReadOnlySpan<T> span = region;
        T[]? array = (T[]?)region;
        T[] newArray = region.ToArray();

        Assert.False(region.IsSegmented);
        for (Int32 i = 0; i < values.Length; i++)
        {
            Assert.Equal(values[i], region[i]);
            Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(span[i]), ref values[i]));
        }

        if (array is not null)
            Assert.Same(values, array);
        else if (isReference && values.Length == 0)
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
                Assert.Equal(IntPtr.Zero, new(ptr));

        Assert.Equal(values, newArray);
        if (values.Length > 0)
            Assert.NotSame(values, newArray);
        else
            Assert.Same(values, newArray);
    }
}
