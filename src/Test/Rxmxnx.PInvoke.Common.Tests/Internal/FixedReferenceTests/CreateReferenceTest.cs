namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CreateReferenceTest : FixedReferenceTestsBase
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
		this.WithFixed(ref value, CreateReferenceTest.Test);
		Exception readOnly =
			Assert.Throws<InvalidOperationException>(
				() => this.WithFixed(ref value, CreateReferenceTest.ReadOnlyTest));
		Assert.Equal(FixedMemoryTestsBase.ReadOnlyError, readOnly.Message);
	}

	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		ref T refValue = ref fref.CreateReference<T>();
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.Equal(typeof(T), fref.Type);
		Assert.False(fref.IsFunction);

		CreateReferenceTest.TestSize<T, Boolean>(fref);
		CreateReferenceTest.TestSize<T, Byte>(fref);
		CreateReferenceTest.TestSize<T, Int16>(fref);
		CreateReferenceTest.TestSize<T, Char>(fref);
		CreateReferenceTest.TestSize<T, Int32>(fref);
		CreateReferenceTest.TestSize<T, Int64>(fref);
		CreateReferenceTest.TestSize<T, Int128>(fref);
		CreateReferenceTest.TestSize<T, Guid>(fref);
		CreateReferenceTest.TestSize<T, Single>(fref);
		CreateReferenceTest.TestSize<T, Half>(fref);
		CreateReferenceTest.TestSize<T, Double>(fref);
		CreateReferenceTest.TestSize<T, Decimal>(fref);
		CreateReferenceTest.TestSize<T, DateTime>(fref);
		CreateReferenceTest.TestSize<T, TimeOnly>(fref);
		CreateReferenceTest.TestSize<T, TimeSpan>(fref);

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReference<T>());
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = Assert.Throws<InvalidOperationException>(() => fref.CreateDelegate<Action>());
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr) where T : unmanaged
	{
		ref T refValue = ref fref.CreateReference<T>();
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.Equal(typeof(T), fref.Type);
		Assert.False(fref.IsFunction);

		CreateReferenceTest.TestSize<T, Boolean>(fref);
		CreateReferenceTest.TestSize<T, Byte>(fref);
		CreateReferenceTest.TestSize<T, Int16>(fref);
		CreateReferenceTest.TestSize<T, Char>(fref);
		CreateReferenceTest.TestSize<T, Int32>(fref);
		CreateReferenceTest.TestSize<T, Int64>(fref);
		CreateReferenceTest.TestSize<T, Int128>(fref);
		CreateReferenceTest.TestSize<T, Guid>(fref);
		CreateReferenceTest.TestSize<T, Single>(fref);
		CreateReferenceTest.TestSize<T, Half>(fref);
		CreateReferenceTest.TestSize<T, Double>(fref);
		CreateReferenceTest.TestSize<T, Decimal>(fref);
		CreateReferenceTest.TestSize<T, DateTime>(fref);
		CreateReferenceTest.TestSize<T, TimeOnly>(fref);
		CreateReferenceTest.TestSize<T, TimeSpan>(fref);

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReference<T>());
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
			Exception invalidSize = Assert.Throws<InsufficientMemoryException>(() => fref.CreateReference<T2>());
			Assert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref fref.CreateReference<T>();
			ref T2 value2 = ref fref.CreateReference<T2>();
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
			Exception invalidSize = Assert.Throws<InsufficientMemoryException>(() => fref.CreateReference<T2>());
			Assert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref fref.CreateReference<T>();
			ref T2 value2 = ref fref.CreateReference<T2>();
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