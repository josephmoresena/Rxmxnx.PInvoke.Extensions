namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class BasicTests : ValueRegionTestBase
{
	[Fact]
	internal void BooleanTest() => BasicTests.Test<Boolean>();
	[Fact]
	internal void ByteTest() => BasicTests.Test<Byte>();
	[Fact]
	internal void Int16Test() => BasicTests.Test<Int16>();
	[Fact]
	internal void CharTest() => BasicTests.Test<Char>();
	[Fact]
	internal void Int32Test() => BasicTests.Test<Int32>();
	[Fact]
	internal void Int64Test() => BasicTests.Test<Int64>();
	[Fact]
	internal void Int128Test() => BasicTests.Test<Int128>();
	[Fact]
	internal void GuidTest() => BasicTests.Test<Guid>();
	[Fact]
	internal void SingleTest() => BasicTests.Test<Single>();
	[Fact]
	internal void HalfTest() => BasicTests.Test<Half>();
	[Fact]
	internal void DoubleTest() => BasicTests.Test<Double>();
	[Fact]
	internal void DecimalTest() => BasicTests.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => BasicTests.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => BasicTests.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => BasicTests.Test<TimeSpan>();

	private static void Test<T>() where T : unmanaged
	{
		List<GCHandle> handles = new();
		T[] values0 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values1 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values2 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values3 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();

		try
		{
			BasicTests.AssertRegion(values0, handles);
			BasicTests.AssertRegion(values0, handles);
			BasicTests.AssertRegion(values1, handles);
			BasicTests.AssertRegion(values1, handles);
			BasicTests.AssertRegion(values2, handles);
			BasicTests.AssertRegion(values2, handles);
			BasicTests.AssertRegion(values3, handles);
			BasicTests.AssertRegion(values3, handles);
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
		ValueRegion<T> region = ValueRegionTestBase.Create(values, handles, out Boolean isReference);
		ReadOnlySpan<T> span = region;
		T[]? array = (T[]?)region;
		T[] newArray = region.ToArray();

		Assert.False(region.IsMemorySlice);
		for (Int32 i = 0; i < values.Length; i++)
		{
			Assert.Equal(values[i], region[i]);
			Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(in span[i]), ref values[i]));
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