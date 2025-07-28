#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.ValueRegionTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class SegmentTest : ValueRegionTestBase
{
	[Fact]
	public void BooleanTest() => SegmentTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => SegmentTest.Test<Byte>();
	[Fact]
	public void Int16Test() => SegmentTest.Test<Int16>();
	[Fact]
	public void CharTest() => SegmentTest.Test<Char>();
	[Fact]
	public void Int32Test() => SegmentTest.Test<Int32>();
	[Fact]
	public void Int64Test() => SegmentTest.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => SegmentTest.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => SegmentTest.Test<Guid>();
	[Fact]
	public void SingleTest() => SegmentTest.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => SegmentTest.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => SegmentTest.Test<Double>();
	[Fact]
	public void DecimalTest() => SegmentTest.Test<Decimal>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void DateTimeTest() => SegmentTest.Test<DateTime>();
#endif
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => SegmentTest.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => SegmentTest.Test<TimeSpan>();

	private static void Test<T>() where T : unmanaged
	{
		List<GCHandle> handles = [];
		T[] values0 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values1 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values2 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();
		T[] values3 = ValueRegionTestBase.Fixture.CreateMany<T>(Random.Shared.Next(default, 100)).ToArray();

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
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(-1, 0));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(1, 0));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(0, -1));
		PInvokeAssert.Throws<ArgumentOutOfRangeException>(() => emptyRegion.Slice(0, 1));

		if ((T[]?)emptyRegion is { } arr)
			PInvokeAssert.Same(emptyRegion.ToArray(), arr);
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

		PInvokeAssert.Equal(state.Count, span.Length);
		PInvokeAssert.Equal((state.Count != state.Length && !state.IsReference) || state.Region.IsMemorySlice,
		                    state.Segment.IsMemorySlice);

		for (Int32 j = 0; j < state.Count; j++)
		{
			Int32 arrayOffset = state.GetArrayOffset(j);

			PInvokeAssert.Equal(state.Region[state.GetRegionOffset(j)], state.Segment[j]);
			PInvokeAssert.Equal(state.Values[arrayOffset], state.Segment[j]);
#if NET8_0_OR_GREATER
			Assert.True(!state.IsReference ?
				            Unsafe.AreSame(in span[j], ref state.Values[arrayOffset]) :
				            Unsafe.AreSame(in span[j], ref state.Values.AsSpan()[arrayOffset..][0]));
#else
			PInvokeAssert.True(!state.IsReference ?
				            Unsafe.AreSame(ref Unsafe.AsRef(in span[j]), ref state.Values[arrayOffset]) :
				            Unsafe.AreSame(ref Unsafe.AsRef(in span[j]), ref state.Values.AsSpan()[arrayOffset..][0]));
#endif
		}

		if (state.Segment.IsMemorySlice || state.IsReference)
			PInvokeAssert.Null(array);
		else
			PInvokeAssert.Equal((T[]?)state.Segment, array);

		if (array is not null)
		{
			PInvokeAssert.Same(state.Values, array);
			if (state.Values.Length > 0)
				PInvokeAssert.NotSame(state.Values, newArray);
			else
				PInvokeAssert.Same(Array.Empty<T>(), newArray);
		}
		else if (state is { IsReference: true, Count: 0, })
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				PInvokeAssert.Equal(IntPtr.Zero, new(ptr));
		}

		PInvokeAssert.Equal(state.Values.Skip(state.SkipArray()).Take(state.Count), newArray);
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}