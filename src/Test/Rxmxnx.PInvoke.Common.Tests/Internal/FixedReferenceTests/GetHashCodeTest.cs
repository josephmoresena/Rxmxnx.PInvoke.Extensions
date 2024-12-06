namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetHashCodeTest : FixedReferenceTestsBase
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

	private static void Test<T>() where T : unmanaged
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, GetHashCodeTest.Test);
		FixedReferenceTestsBase.WithFixed(ref value, GetHashCodeTest.ReadOnlyTest);
	}

	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		Boolean isReadOnly = fref.IsReadOnly;

		Int32 binaryLength = sizeof(T);
		HashCode hash = new();
		HashCode hashReadOnly = new();

		hash.Add(ptr);
		hash.Add(0);
		hash.Add(binaryLength);
		hash.Add(false);
		hash.Add(typeof(T));
		hashReadOnly.Add(ptr);
		hashReadOnly.Add(0);
		hashReadOnly.Add(binaryLength);
		hashReadOnly.Add(true);
		hashReadOnly.Add(typeof(T));

		Assert.Equal(!isReadOnly, hash.ToHashCode().Equals(fref.GetHashCode()));
		Assert.Equal(isReadOnly, hashReadOnly.ToHashCode().Equals(fref.GetHashCode()));

		GetHashCodeTest.TransformationTest<T, Boolean>(fref);
		GetHashCodeTest.TransformationTest<T, Byte>(fref);
		GetHashCodeTest.TransformationTest<T, Int16>(fref);
		GetHashCodeTest.TransformationTest<T, Char>(fref);
		GetHashCodeTest.TransformationTest<T, Int32>(fref);
		GetHashCodeTest.TransformationTest<T, Int64>(fref);
		GetHashCodeTest.TransformationTest<T, Int128>(fref);
		GetHashCodeTest.TransformationTest<T, Single>(fref);
		GetHashCodeTest.TransformationTest<T, Half>(fref);
		GetHashCodeTest.TransformationTest<T, Double>(fref);
		GetHashCodeTest.TransformationTest<T, Decimal>(fref);
		GetHashCodeTest.TransformationTest<T, DateTime>(fref);
		GetHashCodeTest.TransformationTest<T, TimeOnly>(fref);
		GetHashCodeTest.TransformationTest<T, TimeSpan>(fref);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		Boolean isReadOnly = fref.IsReadOnly;

		Int32 binaryLength = sizeof(T);
		HashCode hash = new();
		HashCode hashReadOnly = new();

		hash.Add(ptr);
		hash.Add(0);
		hash.Add(binaryLength);
		hash.Add(false);
		hash.Add(typeof(T));
		hashReadOnly.Add(ptr);
		hashReadOnly.Add(0);
		hashReadOnly.Add(binaryLength);
		hashReadOnly.Add(true);
		hashReadOnly.Add(typeof(T));

		Assert.Equal(!isReadOnly, hash.ToHashCode().Equals(fref.GetHashCode()));
		Assert.Equal(isReadOnly, hashReadOnly.ToHashCode().Equals(fref.GetHashCode()));

		GetHashCodeTest.TransformationTest<T, Boolean>(fref);
		GetHashCodeTest.TransformationTest<T, Byte>(fref);
		GetHashCodeTest.TransformationTest<T, Int16>(fref);
		GetHashCodeTest.TransformationTest<T, Char>(fref);
		GetHashCodeTest.TransformationTest<T, Int32>(fref);
		GetHashCodeTest.TransformationTest<T, Int64>(fref);
		GetHashCodeTest.TransformationTest<T, Int128>(fref);
		GetHashCodeTest.TransformationTest<T, Single>(fref);
		GetHashCodeTest.TransformationTest<T, Half>(fref);
		GetHashCodeTest.TransformationTest<T, Double>(fref);
		GetHashCodeTest.TransformationTest<T, Decimal>(fref);
		GetHashCodeTest.TransformationTest<T, DateTime>(fref);
		GetHashCodeTest.TransformationTest<T, TimeOnly>(fref);
		GetHashCodeTest.TransformationTest<T, TimeSpan>(fref);
	}
	private static unsafe void TransformationTest<T, T2>(FixedReference<T> fref)
		where T : unmanaged where T2 : unmanaged
	{
		if (sizeof(T2) > fref.BinaryLength)
			return;

		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in valueRef));
		ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);
		FixedReferenceTestsBase.WithFixed(transformedRef, fref, GetHashCodeTest.Test);
	}
	private static unsafe void TransformationTest<T, T2>(ReadOnlyFixedReference<T> fref)
		where T : unmanaged where T2 : unmanaged
	{
		if (sizeof(T2) > fref.BinaryLength)
			return;

		ref readonly T valueRef = ref fref.CreateReadOnlyReference<T>();
		void* ptr = Unsafe.AsPointer(ref UnsafeLegacy.AsRef(in valueRef));
		ref readonly T2 transformedRef = ref Unsafe.AsRef<T2>(ptr);
		FixedReferenceTestsBase.WithFixed(transformedRef, fref, GetHashCodeTest.Test);
	}
	private static void Test<T, TInt>(FixedReference<TInt> fref2, FixedReference<T> fref)
		where T : unmanaged where TInt : unmanaged
		=> Assert.Equal(typeof(TInt) == typeof(T), fref2.GetHashCode().Equals(fref.GetHashCode()));
	private static void Test<T, TInt>(ReadOnlyFixedReference<TInt> fref2, ReadOnlyFixedReference<T> fref)
		where T : unmanaged where TInt : unmanaged
		=> Assert.Equal(typeof(TInt) == typeof(T), fref2.GetHashCode().Equals(fref.GetHashCode()));
}