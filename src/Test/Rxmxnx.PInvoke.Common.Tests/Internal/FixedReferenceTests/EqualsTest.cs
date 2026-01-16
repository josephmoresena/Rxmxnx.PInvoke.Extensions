namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class EqualsTest : FixedReferenceTestsBase
{
	[Fact]
	public void BooleanTest() => EqualsTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => EqualsTest.Test<Byte>();
	[Fact]
	public void Int16Test() => EqualsTest.Test<Int16>();
	[Fact]
	public void CharTest() => EqualsTest.Test<Char>();
	[Fact]
	public void Int32Test() => EqualsTest.Test<Int32>();
	[Fact]
	public void Int64Test() => EqualsTest.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => EqualsTest.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => EqualsTest.Test<Guid>();
	[Fact]
	public void SingleTest() => EqualsTest.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => EqualsTest.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => EqualsTest.Test<Double>();
	[Fact]
	public void DecimalTest() => EqualsTest.Test<Decimal>();
	[Fact]
	public void DateTimeTest() => EqualsTest.Test<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => EqualsTest.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => EqualsTest.Test<TimeSpan>();
	private static void Test<T>()
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, EqualsTest.Test);
		FixedReferenceTestsBase.WithFixed(ref value, EqualsTest.ReadOnlyTest);
	}
	private static void Test<T>(FixedReference<T> fref, IntPtr _) { EqualsTest.Test(fref); }
	private static void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr _) { EqualsTest.Test(fref); }
	private static void Test<T>(FixedReference<T> fref)
	{
		EqualsTest.TransformationTest<T, Boolean>(fref);
		EqualsTest.TransformationTest<T, Byte>(fref);
		EqualsTest.TransformationTest<T, Int16>(fref);
		EqualsTest.TransformationTest<T, Char>(fref);
		EqualsTest.TransformationTest<T, Int32>(fref);
		EqualsTest.TransformationTest<T, Int64>(fref);
#if NET7_0_OR_GREATER
		EqualsTest.TransformationTest<T, Int128>(fref);
#endif
		EqualsTest.TransformationTest<T, Single>(fref);
#if NET5_0_OR_GREATER
		EqualsTest.TransformationTest<T, Half>(fref);
#endif
		EqualsTest.TransformationTest<T, Double>(fref);
		EqualsTest.TransformationTest<T, Decimal>(fref);
		EqualsTest.TransformationTest<T, DateTime>(fref);
#if NET6_0_OR_GREATER
		EqualsTest.TransformationTest<T, TimeOnly>(fref);
#endif
		EqualsTest.TransformationTest<T, TimeSpan>(fref);
	}
	private static void Test<T>(ReadOnlyFixedReference<T> fref)
	{
		EqualsTest.TransformationTest<T, Boolean>(fref);
		EqualsTest.TransformationTest<T, Byte>(fref);
		EqualsTest.TransformationTest<T, Int16>(fref);
		EqualsTest.TransformationTest<T, Char>(fref);
		EqualsTest.TransformationTest<T, Int32>(fref);
		EqualsTest.TransformationTest<T, Int64>(fref);
#if NET7_0_OR_GREATER
		EqualsTest.TransformationTest<T, Int128>(fref);
#endif
		EqualsTest.TransformationTest<T, Single>(fref);
#if NET5_0_OR_GREATER
		EqualsTest.TransformationTest<T, Half>(fref);
#endif
		EqualsTest.TransformationTest<T, Double>(fref);
		EqualsTest.TransformationTest<T, Decimal>(fref);
		EqualsTest.TransformationTest<T, DateTime>(fref);
#if NET6_0_OR_GREATER
		EqualsTest.TransformationTest<T, TimeOnly>(fref);
#endif
		EqualsTest.TransformationTest<T, TimeSpan>(fref);
	}
	private static unsafe void TransformationTest<T, T2>(FixedReference<T> fref) where T2 : unmanaged
	{
		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(in valueRef));
		Int32 binaryLength = sizeof(T);

		if (binaryLength < sizeof(T2)) return;
		ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);

		PInvokeAssert.Equal(binaryLength, fref.BinaryLength);
		FixedReferenceTestsBase.WithFixed(transformedRef, fref, EqualsTest.Test);
	}
	private static unsafe void TransformationTest<T, T2>(ReadOnlyFixedReference<T> fref) where T2 : unmanaged
	{
		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(in valueRef));
		Int32 binaryLength = sizeof(T);

		if (binaryLength < sizeof(T2)) return;
		ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);

		PInvokeAssert.Equal(binaryLength, fref.BinaryLength);
		FixedReferenceTestsBase.WithFixed(transformedRef, fref, EqualsTest.Test);
	}
	private static void Test<T, TInt>(FixedReference<TInt> fref2, FixedReference<T> fref)

	{
		Boolean equal = fref.IsReadOnly == fref2.IsReadOnly && typeof(TInt) == typeof(T);

		PInvokeAssert.Equal(equal, fref2.Equals(fref));
		PInvokeAssert.Equal(equal, fref2.Equals((Object)fref));
		PInvokeAssert.Equal(equal, fref2.Equals(fref as FixedReference<TInt>));
		PInvokeAssert.False(fref2.Equals(null));
		PInvokeAssert.False(fref2.Equals(new Object()));
		PInvokeAssert.False(fref2.IsFunction);
	}
	private static void Test<T, TInt>(ReadOnlyFixedReference<TInt> fref2, ReadOnlyFixedReference<T> fref)
	{
		Boolean equal = fref.IsReadOnly == fref2.IsReadOnly && typeof(TInt) == typeof(T);

		PInvokeAssert.Equal(equal, fref2.Equals(fref));
		PInvokeAssert.Equal(equal, fref2.Equals((Object)fref));
		PInvokeAssert.Equal(equal, fref2.Equals(fref as ReadOnlyFixedReference<TInt>));
		PInvokeAssert.False(fref2.Equals(null));
		PInvokeAssert.False(fref2.Equals(new Object()));
		PInvokeAssert.False(fref2.IsFunction);
	}
}
#pragma warning restore CS8500