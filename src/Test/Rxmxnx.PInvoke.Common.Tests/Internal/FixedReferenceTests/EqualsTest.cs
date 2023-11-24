namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class EqualsTest : FixedReferenceTestsBase
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

	private void Test<T>() where T : unmanaged
	{
		T value = FixedMemoryTestsBase.fixture.Create<T>();
		this.WithFixed(ref value, EqualsTest.Test);
		this.WithFixed(ref value, EqualsTest.ReadOnlyTest);
	}

	private static void Test<T>(FixedReference<T> fref, IntPtr _) where T : unmanaged { EqualsTest.Test(fref); }
	private static void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr _) where T : unmanaged
	{
		EqualsTest.Test(fref);
	}

	private static void Test<T>(FixedReference<T> fref) where T : unmanaged
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
	private static void Test<T>(ReadOnlyFixedReference<T> fref) where T : unmanaged
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
	private static unsafe void TransformationTest<T, T2>(FixedReference<T> fref)
		where T : unmanaged where T2 : unmanaged
	{
		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(in valueRef));
		Int32 binaryLength = sizeof(T);

		if (binaryLength >= sizeof(T2))
		{
			ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);

			Assert.Equal(binaryLength, fref.BinaryLength);
			FixedReferenceTestsBase.WithFixed(transformedRef, fref, EqualsTest.Test);
		}
	}
	private static unsafe void TransformationTest<T, T2>(ReadOnlyFixedReference<T> fref)
		where T : unmanaged where T2 : unmanaged
	{
		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(in valueRef));
		Int32 binaryLength = sizeof(T);

		if (binaryLength >= sizeof(T2))
		{
			ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);

			Assert.Equal(binaryLength, fref.BinaryLength);
			FixedReferenceTestsBase.WithFixed(transformedRef, fref, EqualsTest.Test);
		}
	}
	private static void Test<T, TInt>(FixedReference<TInt> fref2, FixedReference<T> fref)
		where T : unmanaged where TInt : unmanaged
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
		where T : unmanaged where TInt : unmanaged
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