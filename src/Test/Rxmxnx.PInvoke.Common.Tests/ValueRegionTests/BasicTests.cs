namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class BasicTests : ValueRegionTestBase
{
#pragma warning disable CS8500
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

	[Fact]
	internal void StringTest() => BasicTests.Test<String>();

	private static void Test<T>()
	{
		List<GCHandle> handles = [];
		T[] values0 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values1 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values2 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values3 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();

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

	private static unsafe void AssertRegion<T>(T[] values, ICollection<GCHandle> handles)
	{
		T[]? array = null;
		Boolean isReference = false;
		ReadOnlySpan<T> span = ReadOnlySpan<T>.Empty;
		try
		{
			ValueRegion<T> region = ValueRegionTestBase.Create(values, handles, out isReference);
			array = (T[]?)region;
			span = BasicTests.AssertRegion(values, region);
		}
		catch (ArgumentException)
		{
			Assert.True(RuntimeHelpers.IsReferenceOrContainsReferences<T>());
			fixed (void* ptr = values)
			{
				ValueRegion<T> region = ValueRegion<T>.Create(new(ptr), values.Length);
				BasicTests.AssertRegion(values, region);
			}
		}

		if (array is not null)
			Assert.Same(values, array);
		else if ((isReference && values.Length == 0) ||
		         (RuntimeHelpers.IsReferenceOrContainsReferences<T>() && span.IsEmpty))
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				Assert.Equal(IntPtr.Zero, new(ptr));
	}
	private static ReadOnlySpan<T> AssertRegion<T>(T[] values, ValueRegion<T> region)
	{
		ReadOnlySpan<T> span = region;
		T[] newArray = region.ToArray();

		Assert.False(region.IsMemorySlice);
		for (Int32 i = 0; i < values.Length; i++)
		{
			Assert.Equal(values[i], region[i]);
			Assert.True(Unsafe.AreSame(in span[i], ref values[i]));
		}

		Assert.Equal(values, newArray);
		if (values.Length > 0)
			Assert.NotSame(values, newArray);
		else
			Assert.Same(Array.Empty<T>(), newArray);

		Boolean isAllocated = region.TryAlloc(GCHandleType.Pinned, out GCHandle handle);
		Assert.Equal(isAllocated, handle.IsAllocated);

		if (region.GetType().Name.Contains("Managed"))
			Assert.Equal(!RuntimeHelpers.IsReferenceOrContainsReferences<T>(), isAllocated);

		return span;
	}
#pragma warning restore CS8500
}