#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class BasicTests : ValueRegionTestBase
{
#pragma warning disable CS8500
	[Fact]
	public void BooleanTest() => BasicTests.Test<Boolean>();
	[Fact]
	public void ByteTest() => BasicTests.Test<Byte>();
	[Fact]
	public void Int16Test() => BasicTests.Test<Int16>();
	[Fact]
	public void CharTest() => BasicTests.Test<Char>();
	[Fact]
	public void Int32Test() => BasicTests.Test<Int32>();
	[Fact]
	public void Int64Test() => BasicTests.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => BasicTests.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => BasicTests.Test<Guid>();
	[Fact]
	public void SingleTest() => BasicTests.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => BasicTests.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => BasicTests.Test<Double>();
	[Fact]
	public void DecimalTest() => BasicTests.Test<Decimal>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void DateTimeTest() => BasicTests.Test<DateTime>();
#endif
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => BasicTests.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => BasicTests.Test<TimeSpan>();

	[Fact]
	public void StringTest() => BasicTests.Test<String>();

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
		Boolean spanTest = true;
		try
		{
			ValueRegion<T> region = ValueRegionTestBase.Create(values, handles, out isReference);
			array = (T[]?)region;
			span = BasicTests.AssertRegion(values, region);
		}
		catch (ArgumentException)
		{
			PInvokeAssert.True(RuntimeHelpers.IsReferenceOrContainsReferences<T>());
			fixed (void* ptr = values)
			{
				ValueRegion<T> region = ValueRegion<T>.Create(new(ptr), values.Length);
				BasicTests.AssertRegion(values, region);
			}
			spanTest = false;
		}

		if (array is not null)
			PInvokeAssert.Same(values, array);
		else if (spanTest && isReference && values.Length == 0)
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				PInvokeAssert.Equal(IntPtr.Zero, new(ptr));
	}
	private static ReadOnlySpan<T> AssertRegion<T>(T[] values, ValueRegion<T> region)
	{
		ReadOnlySpan<T> span = region;
		T[] newArray = region.ToArray();

		PInvokeAssert.False(region.IsMemorySlice);
		for (Int32 i = 0; i < values.Length; i++)
		{
			PInvokeAssert.Equal(values[i], region[i]);
#if NET8_0_OR_GREATER
			Assert.True(Unsafe.AreSame(in span[i], ref values[i]));
#else
			PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in span[i]), ref values[i]));
#endif
		}

		PInvokeAssert.Equal(values, newArray);
		if (values.Length > 0)
			PInvokeAssert.NotSame(values, newArray);
		else
			PInvokeAssert.Same(Array.Empty<T>(), newArray);

		Boolean isAllocated = region.TryAlloc(GCHandleType.Pinned, out GCHandle handle);
		PInvokeAssert.Equal(isAllocated, handle.IsAllocated);

#if NETCOREAPP
		if (!region.GetType().Name.Contains("Managed")) return span;
		Assert.Equal(!RuntimeHelpers.IsReferenceOrContainsReferences<T>(), isAllocated);
#endif
		return span;
	}
#pragma warning restore CS8500
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}