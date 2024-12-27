namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class EqualsTest : FixedReferenceTestsBase
{
	[Fact]
	internal void BooleanTest() => EqualsTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => EqualsTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => EqualsTest.Test<Int16>();
	[Fact]
	internal void CharTest() => EqualsTest.Test<Char>();
	[Fact]
	internal void Int32Test() => EqualsTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => EqualsTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => EqualsTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => EqualsTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => EqualsTest.Test<Single>();
	[Fact]
	internal void HalfTest() => EqualsTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => EqualsTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => EqualsTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => EqualsTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => EqualsTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => EqualsTest.Test<TimeSpan>();
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
		EqualsTest.TransformationTest<T, Int128>(fref);
		EqualsTest.TransformationTest<T, Single>(fref);
		EqualsTest.TransformationTest<T, Half>(fref);
		EqualsTest.TransformationTest<T, Double>(fref);
		EqualsTest.TransformationTest<T, Decimal>(fref);
		EqualsTest.TransformationTest<T, DateTime>(fref);
		EqualsTest.TransformationTest<T, TimeOnly>(fref);
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
		EqualsTest.TransformationTest<T, Int128>(fref);
		EqualsTest.TransformationTest<T, Single>(fref);
		EqualsTest.TransformationTest<T, Half>(fref);
		EqualsTest.TransformationTest<T, Double>(fref);
		EqualsTest.TransformationTest<T, Decimal>(fref);
		EqualsTest.TransformationTest<T, DateTime>(fref);
		EqualsTest.TransformationTest<T, TimeOnly>(fref);
		EqualsTest.TransformationTest<T, TimeSpan>(fref);
	}
	private static unsafe void TransformationTest<T, T2>(FixedReference<T> fref) where T2 : unmanaged
	{
		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in valueRef));
		Int32 binaryLength = sizeof(T);

		if (binaryLength < sizeof(T2)) return;
		ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);

		Assert.Equal(binaryLength, fref.BinaryLength);
		FixedReferenceTestsBase.WithFixed(transformedRef, fref, EqualsTest.Test);
	}
	private static unsafe void TransformationTest<T, T2>(ReadOnlyFixedReference<T> fref) where T2 : unmanaged
	{
		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in valueRef));
		Int32 binaryLength = sizeof(T);

		if (binaryLength < sizeof(T2)) return;
		ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);

		Assert.Equal(binaryLength, fref.BinaryLength);
		FixedReferenceTestsBase.WithFixed(transformedRef, fref, EqualsTest.Test);
	}
	private static void Test<T, TInt>(FixedReference<TInt> fref2, FixedReference<T> fref)

	{
		Boolean equal = fref.IsReadOnly == fref2.IsReadOnly && typeof(TInt) == typeof(T);

		Assert.Equal(equal, fref2.Equals(fref));
		Assert.Equal(equal, fref2.Equals((Object)fref));
		Assert.Equal(equal, fref2.Equals(fref as FixedReference<TInt>));
		Assert.False(fref2.Equals(null));
		Assert.False(fref2.Equals(new Object()));
		Assert.False(fref2.IsFunction);
	}
	private static void Test<T, TInt>(ReadOnlyFixedReference<TInt> fref2, ReadOnlyFixedReference<T> fref)
	{
		Boolean equal = fref.IsReadOnly == fref2.IsReadOnly && typeof(TInt) == typeof(T);

		Assert.Equal(equal, fref2.Equals(fref));
		Assert.Equal(equal, fref2.Equals((Object)fref));
		Assert.Equal(equal, fref2.Equals(fref as ReadOnlyFixedReference<TInt>));
		Assert.False(fref2.Equals(null));
		Assert.False(fref2.Equals(new Object()));
		Assert.False(fref2.IsFunction);
	}
}
#pragma warning restore CS8500