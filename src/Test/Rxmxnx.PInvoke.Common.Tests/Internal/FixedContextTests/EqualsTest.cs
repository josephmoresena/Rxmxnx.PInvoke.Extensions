namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class EqualsTest : FixedContextTestsBase
{
	[Fact]
	internal void BooleanTest() => this.Test<Boolean>();
	[Fact]
	internal void ByteTest() => this.Test<Byte>();
	[Fact]
	internal void Int16Test() => this.Test<Int16>();
	[Fact]
	internal void CharTest() => this.Test<Char>();
	[Fact]
	internal void Int32Test() => this.Test<Int32>();
	[Fact]
	internal void Int64Test() => this.Test<Int64>();
	[Fact]
	internal void Int128Test() => this.Test<Int128>();
	[Fact]
	internal void GuidTest() => this.Test<Guid>();
	[Fact]
	internal void SingleTest() => this.Test<Single>();
	[Fact]
	internal void HalfTest() => this.Test<Half>();
	[Fact]
	internal void DoubleTest() => this.Test<Double>();
	[Fact]
	internal void DecimalTest() => this.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => this.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => this.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => this.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => this.Test<ManagedStruct>();
	[Fact]
	internal void StringTest() => this.Test<String>();
	
	private unsafe void Test<T>()
	{
		T[] values = FixedMemoryTestsBase.Fixture.CreateMany<T>(sizeof(Int128) * 3 / sizeof(T)).ToArray();
		Action<FixedContext<T>, T[]> action;
		Action<ReadOnlyFixedContext<T>, T[]> readonlyAction;
		try
		{
			GCHandle.Alloc(values, GCHandleType.Pinned).Free();
			// Unmanaged type
			action = EqualsTest.UnmanagedTest;
			readonlyAction = EqualsTest.UnmanagedReadOnlyTest;
		}
		catch (Exception)
		{
			// Managed type
			if (typeof(T).IsValueType)
			{
				action = (c, a) => EqualsTest.ManagedValueTypeTest(c, a.Length);
				readonlyAction = (c, a) => EqualsTest.ManagedValueTypeTest(c, a.Length);
			}
			else
			{
				action = (c, a) => EqualsTest.ReferenceTypeTest(c, a.Length);
				readonlyAction = (c, a) => EqualsTest.ReferenceTypeTest(c, a.Length);
			}
		}
		this.WithFixed(values, action);
		this.WithFixed(values, readonlyAction);
	}
	private static void Test<T, T2>(FixedContext<T2> ctx2, FixedContext<T> ctx)
	{
		Boolean equal = ctx.IsReadOnly == ctx2.IsReadOnly && typeof(T2) == typeof(T);

		Assert.Equal(equal, ctx2.Equals(ctx));
		Assert.Equal(equal, ctx2.Equals((Object)ctx));
		Assert.Equal(equal, ctx2.Equals(ctx as FixedContext<T2>));
		Assert.False(ctx2.Equals(null));
		Assert.False(ctx2.Equals(new Object()));
	}
	private static void Test<T, T2>(ReadOnlyFixedContext<T2> ctx2, ReadOnlyFixedContext<T> ctx)
	{
		Boolean equal = ctx.IsReadOnly == ctx2.IsReadOnly && typeof(T2) == typeof(T);

		Assert.Equal(equal, ctx2.Equals(ctx));
		Assert.Equal(equal, ctx2.Equals((Object)ctx));
		Assert.Equal(equal, ctx2.Equals(ctx as ReadOnlyFixedContext<T2>));
		Assert.False(ctx2.Equals(null));
		Assert.False(ctx2.Equals(new Object()));
	}
	private static unsafe void TransformationTest<T, T2>(FixedContext<T> ctx, Int32 length)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(length);
		void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
		ref T2 refT2 = ref Unsafe.AsRef<T2>(ptr);
		Int32 binaryLength = length * sizeof(T);
		ReadOnlySpan<T2> transformedSpan = MemoryMarshal.CreateReadOnlySpan(ref refT2, binaryLength / sizeof(T2));

		Assert.Equal(binaryLength, ctx.BinaryLength);
		FixedContextTestsBase.WithFixed(transformedSpan, ctx, EqualsTest.Test);
	}
	private static unsafe void TransformationTest<T, T2>(ReadOnlyFixedContext<T> ctx, Int32 length)
	{
		ReadOnlySpan<T> span = ctx.CreateReadOnlySpan<T>(length);
		void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
		ref T2 refT2 = ref Unsafe.AsRef<T2>(ptr);
		Int32 binaryLength = length * sizeof(T);
		ReadOnlySpan<T2> transformedSpan = MemoryMarshal.CreateSpan(ref refT2, binaryLength / sizeof(T2));

		Assert.Equal(binaryLength, ctx.BinaryLength);
		FixedContextTestsBase.WithFixed(transformedSpan, ctx, EqualsTest.Test);
	}
	private static unsafe void UnmanagedTest<T>(FixedContext<T> ctx, T[] values)
	{
		fixed (T* ptrValue = values)
		{
			EqualsTest.UnmanagedTest(ctx, values.Length);
			EqualsTest.UnmanagedFalseTest(ctx, values, ptrValue);
		}
	}
	private static unsafe void UnmanagedReadOnlyTest<T>(ReadOnlyFixedContext<T> ctx, T[] values)
	{
		fixed (T* ptrValue = values)
		{
			EqualsTest.UnmanagedTest(ctx, values.Length);
			EqualsTest.UnmanagedFalseTest(ctx, values, ptrValue);
		}
	}
	private static void UnmanagedTest<T>(FixedContext<T> ctx, Int32 length)
	{
		EqualsTest.TransformationTest<T, Boolean>(ctx, length);
		EqualsTest.TransformationTest<T, Byte>(ctx, length);
		EqualsTest.TransformationTest<T, Int16>(ctx, length);
		EqualsTest.TransformationTest<T, Char>(ctx, length);
		EqualsTest.TransformationTest<T, Int32>(ctx, length);
		EqualsTest.TransformationTest<T, Int64>(ctx, length);
		EqualsTest.TransformationTest<T, Int128>(ctx, length);
		EqualsTest.TransformationTest<T, Single>(ctx, length);
		EqualsTest.TransformationTest<T, Half>(ctx, length);
		EqualsTest.TransformationTest<T, Double>(ctx, length);
		EqualsTest.TransformationTest<T, Decimal>(ctx, length);
		EqualsTest.TransformationTest<T, DateTime>(ctx, length);
		EqualsTest.TransformationTest<T, TimeOnly>(ctx, length);
		EqualsTest.TransformationTest<T, TimeSpan>(ctx, length);

		EqualsTest.TransformationTest<T, WrapperStruct<Boolean>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Byte>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int16>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Char>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int32>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int64>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int128>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Single>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Half>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Double>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Decimal>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<DateTime>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<TimeOnly>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<TimeSpan>>(ctx, length);
	}
	private static void UnmanagedTest<T>(ReadOnlyFixedContext<T> ctx, Int32 length)
	{
		EqualsTest.TransformationTest<T, Boolean>(ctx, length);
		EqualsTest.TransformationTest<T, Byte>(ctx, length);
		EqualsTest.TransformationTest<T, Int16>(ctx, length);
		EqualsTest.TransformationTest<T, Char>(ctx, length);
		EqualsTest.TransformationTest<T, Int32>(ctx, length);
		EqualsTest.TransformationTest<T, Int64>(ctx, length);
		EqualsTest.TransformationTest<T, Int128>(ctx, length);
		EqualsTest.TransformationTest<T, Single>(ctx, length);
		EqualsTest.TransformationTest<T, Half>(ctx, length);
		EqualsTest.TransformationTest<T, Double>(ctx, length);
		EqualsTest.TransformationTest<T, Decimal>(ctx, length);
		EqualsTest.TransformationTest<T, DateTime>(ctx, length);
		EqualsTest.TransformationTest<T, TimeOnly>(ctx, length);
		EqualsTest.TransformationTest<T, TimeSpan>(ctx, length);

		EqualsTest.TransformationTest<T, WrapperStruct<Boolean>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Byte>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int16>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Char>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int32>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int64>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Int128>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Single>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Half>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Double>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<Decimal>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<DateTime>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<TimeOnly>>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<TimeSpan>>(ctx, length);
	}
	private static void ManagedValueTypeTest<T>(FixedContext<T> ctx, Int32 length)
	{
		EqualsTest.TransformationTest<T, ManagedStruct>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<ManagedStruct>>(ctx, length);
	}
	private static void ManagedValueTypeTest<T>(ReadOnlyFixedContext<T> ctx, Int32 length)
	{
		EqualsTest.TransformationTest<T, ManagedStruct>(ctx, length);
		EqualsTest.TransformationTest<T, WrapperStruct<ManagedStruct>>(ctx, length);
	}
	private static void ReferenceTypeTest<T>(FixedContext<T> ctx, Int32 length)
	{
		EqualsTest.TransformationTest<T, String>(ctx, length);
		EqualsTest.TransformationTest<T, Object>(ctx, length);
	}
	private static void ReferenceTypeTest<T>(ReadOnlyFixedContext<T> ctx, Int32 length)
	{
		EqualsTest.TransformationTest<T, String>(ctx, length);
		EqualsTest.TransformationTest<T, Object>(ctx, length);
	}
	private static unsafe void UnmanagedFalseTest<T>(FixedContext<T> ctx, T[] values, T* ptrValue)
	{
		ReadOnlyFixedContext<T> ctx1 = new(ptrValue, values.Length - 1);
		FixedContext<T> ctx2 = new(ptrValue, values.Length - 1);
		ReadOnlyFixedContext<T> ctx3 = new(ptrValue + 1, values.Length - 1);
		FixedContext<T> ctx4 = new(ptrValue + 1, values.Length - 1);

		Assert.False(ctx.Equals(ctx1));
		Assert.False(ctx.Equals(ctx2));
		Assert.False(ctx.Equals(ctx3));
		Assert.False(ctx.Equals(ctx4));
		Assert.False(ctx1.Equals(ctx2));
		Assert.False(ctx1.Equals(ctx3));
		Assert.False(ctx1.Equals(ctx4));
		Assert.False(ctx2.Equals(ctx3));
		Assert.False(ctx2.Equals(ctx4));
		Assert.False(ctx3.Equals(ctx4));
	}
	private static unsafe void UnmanagedFalseTest<T>(ReadOnlyFixedContext<T> ctx, T[] values, T* ptrValue)
	{
		ReadOnlyFixedContext<T> ctx1 = new(ptrValue, values.Length - 1);
		FixedContext<T> ctx2 = new(ptrValue, values.Length - 1);
		ReadOnlyFixedContext<T> ctx3 = new(ptrValue + 1, values.Length - 1);
		FixedContext<T> ctx4 = new(ptrValue + 1, values.Length - 1);

		Assert.False(ctx.Equals(ctx1));
		Assert.False(ctx.Equals(ctx2));
		Assert.False(ctx.Equals(ctx3));
		Assert.False(ctx.Equals(ctx4));
		Assert.False(ctx1.Equals(ctx2));
		Assert.False(ctx1.Equals(ctx3));
		Assert.False(ctx1.Equals(ctx4));
		Assert.False(ctx2.Equals(ctx3));
		Assert.False(ctx2.Equals(ctx4));
		Assert.False(ctx3.Equals(ctx4));
	}

	[StructLayout(LayoutKind.Sequential)]
	private readonly struct WrapperStruct<T>
	{
		public T Value { get; init; }
	}
}
#pragma warning restore CS8500