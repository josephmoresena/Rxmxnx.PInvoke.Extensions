namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class GetHashCodeTest : FixedContextTestsBase
{
	[Fact]
	internal void BooleanTest() => GetHashCodeTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => GetHashCodeTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => GetHashCodeTest.Test<Int16>();
	[Fact]
	internal void CharTest() => GetHashCodeTest.Test<Char>();
	[Fact]
	internal void Int32Test() => GetHashCodeTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => GetHashCodeTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => GetHashCodeTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => GetHashCodeTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => GetHashCodeTest.Test<Single>();
	[Fact]
	internal void HalfTest() => GetHashCodeTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => GetHashCodeTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => GetHashCodeTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => GetHashCodeTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => GetHashCodeTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => GetHashCodeTest.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => GetHashCodeTest.Test<ManagedStruct>();
	[Fact]
	internal void StringTest() => GetHashCodeTest.Test<String>();
	private static unsafe void Test<T>()
	{
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>(sizeof(Int128) * 3 / sizeof(T)).ToArray();
		FixedContextTestsBase.WithFixed(values, GetHashCodeTest.Test);
		FixedContextTestsBase.WithFixed(values, GetHashCodeTest.ReadOnlyTest);
	}
	private static unsafe void Test<T>(FixedContext<T> ctx, T[] values)
	{
		fixed (T* ptrValue = values)
		{
			Int32 binaryLength = values.Length * sizeof(T);
			HashCode hash = new();
			HashCode hashReadOnly = new();

			hash.Add(new IntPtr(ptrValue));
			hash.Add(0);
			hash.Add(binaryLength);
			hash.Add(false);
			hash.Add(typeof(T));
			hashReadOnly.Add(new IntPtr(ptrValue));
			hashReadOnly.Add(0);
			hashReadOnly.Add(binaryLength);
			hashReadOnly.Add(true);
			hashReadOnly.Add(typeof(T));

			Assert.True(hash.ToHashCode().Equals(ctx.GetHashCode()));
			Assert.False(hashReadOnly.ToHashCode().Equals(ctx.GetHashCode()));

			try
			{
				GCHandle.Alloc(values, GCHandleType.Pinned).Free();

				GetHashCodeTest.TransformationTest<T, Boolean>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Byte>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int16>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Char>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int32>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int64>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int128>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Single>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Half>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Double>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Decimal>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, DateTime>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, TimeOnly>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, TimeSpan>(ctx, values.Length);

				GetHashCodeTest.TransformationTest<T, WrapperStruct<Boolean>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Byte>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int16>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Char>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int32>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int64>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int128>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Single>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Half>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Double>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Decimal>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<DateTime>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<TimeOnly>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<TimeSpan>>(ctx, values.Length);
			}
			catch (Exception)
			{
				if (typeof(T).IsValueType)
				{
					GetHashCodeTest.TransformationTest<T, ManagedStruct>(ctx, values.Length);
					GetHashCodeTest.TransformationTest<T, WrapperStruct<ManagedStruct>>(ctx, values.Length);
				}
				else
				{
					GetHashCodeTest.TransformationTest<T, String>(ctx, values.Length);
					GetHashCodeTest.TransformationTest<T, Object>(ctx, values.Length);
				}
			}
		}
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] values)
	{
		fixed (T* ptrValue = values)
		{
			Int32 binaryLength = values.Length * sizeof(T);
			HashCode hash = new();
			HashCode hashReadOnly = new();

			hash.Add(new IntPtr(ptrValue));
			hash.Add(0);
			hash.Add(binaryLength);
			hash.Add(false);
			hash.Add(typeof(T));
			hashReadOnly.Add(new IntPtr(ptrValue));
			hashReadOnly.Add(0);
			hashReadOnly.Add(binaryLength);
			hashReadOnly.Add(true);
			hashReadOnly.Add(typeof(T));

			Assert.False(hash.ToHashCode().Equals(ctx.GetHashCode()));
			Assert.True(hashReadOnly.ToHashCode().Equals(ctx.GetHashCode()));

			try
			{
				GCHandle.Alloc(values, GCHandleType.Pinned).Free();

				GetHashCodeTest.TransformationTest<T, Boolean>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Byte>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int16>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Char>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int32>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int64>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Int128>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Single>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Half>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Double>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, Decimal>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, DateTime>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, TimeOnly>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, TimeSpan>(ctx, values.Length);

				GetHashCodeTest.TransformationTest<T, WrapperStruct<Boolean>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Byte>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int16>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Char>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int32>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int64>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Int128>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Single>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Half>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Double>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<Decimal>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<DateTime>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<TimeOnly>>(ctx, values.Length);
				GetHashCodeTest.TransformationTest<T, WrapperStruct<TimeSpan>>(ctx, values.Length);
			}
			catch (Exception)
			{
				if (typeof(T).IsValueType)
				{
					GetHashCodeTest.TransformationTest<T, ManagedStruct>(ctx, values.Length);
					GetHashCodeTest.TransformationTest<T, WrapperStruct<ManagedStruct>>(ctx, values.Length);
				}
				else
				{
					GetHashCodeTest.TransformationTest<T, String>(ctx, values.Length);
					GetHashCodeTest.TransformationTest<T, Object>(ctx, values.Length);
				}
			}
		}
	}
	private static unsafe void TransformationTest<T, T2>(FixedContext<T> ctx, Int32 length)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(length);
		void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
		ref T2 refValue = ref Unsafe.AsRef<T2>(ptr);
		ReadOnlySpan<T2> transformedSpan =
			MemoryMarshal.CreateReadOnlySpan(ref refValue, length * sizeof(T) / sizeof(T2));
		FixedContextTestsBase.WithFixed(transformedSpan, ctx, GetHashCodeTest.Test);
	}
	private static unsafe void TransformationTest<T, T2>(ReadOnlyFixedContext<T> ctx, Int32 length)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(length);
		void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
		ref T2 refValue = ref Unsafe.AsRef<T2>(ptr);
		ReadOnlySpan<T2> transformedSpan =
			MemoryMarshal.CreateReadOnlySpan(ref refValue, length * sizeof(T) / sizeof(T2));
		FixedContextTestsBase.WithFixed(transformedSpan, ctx, GetHashCodeTest.Test);
	}
	private static void Test<T, TInt>(FixedContext<TInt> ctx2, FixedContext<T> ctx)
		=> Assert.Equal(typeof(TInt) == typeof(T), ctx2.GetHashCode().Equals(ctx.GetHashCode()));
	private static void Test<T, TInt>(ReadOnlyFixedContext<TInt> ctx2, ReadOnlyFixedContext<T> ctx)
		=> Assert.Equal(typeof(TInt) == typeof(T), ctx2.GetHashCode().Equals(ctx.GetHashCode()));
}
#pragma warning restore CS8500