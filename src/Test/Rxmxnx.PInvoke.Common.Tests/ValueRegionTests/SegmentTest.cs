namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class SegmentTest : ValueRegionTestBase
{
	[Fact]
	internal void BooleanTest() => SegmentTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => SegmentTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => SegmentTest.Test<Int16>();
	[Fact]
	internal void CharTest() => SegmentTest.Test<Char>();
	[Fact]
	internal void Int32Test() => SegmentTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => SegmentTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => SegmentTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => SegmentTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => SegmentTest.Test<Single>();
	[Fact]
	internal void HalfTest() => SegmentTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => SegmentTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => SegmentTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => SegmentTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => SegmentTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => SegmentTest.Test<TimeSpan>();

	private static void Test<T>() where T : unmanaged
	{
		List<GCHandle> handles = new();
		T[] values0 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values1 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values2 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values3 = ValueRegionTestBase.fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();

		try
		{
			SegmentTest.InvalidTest<T>(handles);
			SegmentTest.AssertRegionSegment(values0, handles);
			SegmentTest.AssertRegionSegment(values0, handles);
			SegmentTest.AssertRegionSegment(values1, handles);
			SegmentTest.AssertRegionSegment(values1, handles);
			SegmentTest.AssertRegionSegment(values2, handles);
			SegmentTest.AssertRegionSegment(values2, handles);
			SegmentTest.AssertRegionSegment(values3, handles);
			SegmentTest.AssertRegionSegment(values3, handles);
		}
		finally
		{
			foreach (GCHandle handle in handles)
				handle.Free();
		}
	}

	private static void InvalidTest<T>(ICollection<GCHandle> handles) where T : unmanaged
	{
		SegmentTest.AssertInvalidSegment(ValueRegionTestBase.Create(Array.Empty<T>(), handles));
		SegmentTest.AssertInvalidSegment(ValueRegionTestBase.Create(Array.Empty<T>(), handles));
		SegmentTest.AssertInvalidSegment(ValueRegionTestBase.Create(Array.Empty<T>(), handles));
		SegmentTest.AssertInvalidSegment(ValueRegionTestBase.Create(Array.Empty<T>(), handles));
		SegmentTest.AssertInvalidSegment(ValueRegionTestBase.Create(Array.Empty<T>(), handles));
		SegmentTest.AssertInvalidSegment(ValueRegionTestBase.Create(Array.Empty<T>(), handles));
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

	private static void AssertRegionSegment<T>(T[] values, ICollection<GCHandle> handles) where T : unmanaged
	{
		ValueRegion<T> region = ValueRegionTestBase.Create(values, handles, out Boolean isReference);
		SegmentTest.AssertRegionSegment(values, region, isReference);
	}

	private static void AssertRegionSegment<T>(T[] values, ValueRegion<T> region, Boolean isReference,
		Int32 initial = 0) where T : unmanaged
	{
		Int32 length = region.AsSpan().Length;
		for (Int32 i = 0; i < length; i++)
		{
			Int32 start = Random.Shared.Next(i, length);
			Int32 count = Random.Shared.Next(start, length) - start;
			ValueRegion<T> segment = region.Slice(start, count);

			SegmentTest.AssertSegment(new SegmentedRegionState<T>
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

			if (!region.IsMemorySlice)
				SegmentTest.AssertRegionSegment(values, segment, isReference, initial + start);
		}

		SegmentTest.AssertSegment(new SegmentedRegionState<T>
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
		Assert.Equal((state.Count != state.Length && !state.IsReference) || state.Region.IsMemorySlice,
		             state.Segment.IsMemorySlice);

		for (Int32 j = 0; j < state.Count; j++)
		{
			Int32 arrayOffset = state.GetArrayOffset(j);

			Assert.Equal(state.Region[state.GetRegionOffset(j)], state.Segment[j]);
			Assert.Equal(state.Values[arrayOffset], state.Segment[j]);
			if (!state.IsReference)
				Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(in span[j]), ref state.Values[arrayOffset]));
			else
				Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(in span[j]), ref state.Values.AsSpan()[arrayOffset..][0]));
		}

		if (state.Segment.IsMemorySlice || state.IsReference)
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
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				Assert.Equal(IntPtr.Zero, new(ptr));
		}

		Assert.Equal(state.Values.Skip(state.SkipArray()).Take(state.Count), newArray);
	}
}