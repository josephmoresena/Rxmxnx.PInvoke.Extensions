namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateReadOnlyReferenceTest : FixedReferenceTestsBase
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
		this.WithFixed(ref value, CreateReadOnlyReferenceTest.Test);
		this.WithFixed(ref value, CreateReadOnlyReferenceTest.ReadOnlyTest);
	}

	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		ref T refValue = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T>());
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.False(fref.IsFunction);

		CreateReadOnlyReferenceTest.TestSize<T, Boolean>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Byte>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int16>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Char>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int32>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int64>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int128>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Guid>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Single>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Half>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Double>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Decimal>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, DateTime>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, TimeOnly>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, TimeSpan>(fref);

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReadOnlyReference<T>());
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = Assert.Throws<InvalidOperationException>(() => fref.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		ref T refValue = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T>());
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.False(fref.IsFunction);

		CreateReadOnlyReferenceTest.TestSize<T, Boolean>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Byte>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int16>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Char>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int32>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int64>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Int128>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Guid>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Single>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Half>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Double>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, Decimal>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, DateTime>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, TimeOnly>(fref);
		CreateReadOnlyReferenceTest.TestSize<T, TimeSpan>(fref);

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReadOnlyReference<T>());
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = Assert.Throws<InvalidOperationException>(() => fref.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}

	private static unsafe void TestSize<T, T2>(FixedReference<T> fref) where T : unmanaged where T2 : unmanaged
	{
		Int32 size = sizeof(T);
		Int32 size2 = sizeof(T2);

		if (size < size2)
		{
			Exception invalidSize =
				Assert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			Assert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T>());
			ref T2 value2 = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T2>());
			Byte[] bytes1 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size).ToArray();
			Byte[] bytes2 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size2).ToArray();

			Assert.Equal(bytes1[..size2], bytes2);

			if (typeof(T) == typeof(T2))
				Assert.Equal(value, (Object)value2);
		}

		Exception functionException = Assert.Throws<InvalidOperationException>(() => fref.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void TestSize<T, T2>(ReadOnlyFixedReference<T> fref) where T : unmanaged where T2 : unmanaged
	{
		Int32 size = sizeof(T);
		Int32 size2 = sizeof(T2);

		if (size < size2)
		{
			Exception invalidSize =
				Assert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			Assert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T>());
			ref T2 value2 = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T2>());
			Byte[] bytes1 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size).ToArray();
			Byte[] bytes2 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size2).ToArray();

			Assert.Equal(bytes1[..size2], bytes2);

			if (typeof(T) == typeof(T2))
				Assert.Equal(value, (Object)value2);
		}

		Exception functionException = Assert.Throws<InvalidOperationException>(() => fref.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
}