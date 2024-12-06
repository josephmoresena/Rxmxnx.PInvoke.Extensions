namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class CreateReferenceTest : FixedReferenceTestsBase
{
	[Fact]
	internal void BooleanTest() => CreateReferenceTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => CreateReferenceTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => CreateReferenceTest.Test<Int16>();
	[Fact]
	internal void CharTest() => CreateReferenceTest.Test<Char>();
	[Fact]
	internal void Int32Test() => CreateReferenceTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => CreateReferenceTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => CreateReferenceTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => CreateReferenceTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => CreateReferenceTest.Test<Single>();
	[Fact]
	internal void HalfTest() => CreateReferenceTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => CreateReferenceTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => CreateReferenceTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => CreateReferenceTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => CreateReferenceTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => CreateReferenceTest.Test<TimeSpan>();
	[Fact]
	internal void ManagedStructTest() => CreateReferenceTest.Test<ManagedStruct>();
	[Fact]
	internal void StringTest() => CreateReferenceTest.Test<String>();
	private static void Test<T>()
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, CreateReferenceTest.Test);
		Exception readOnly =
			Assert.Throws<InvalidOperationException>(
				() => FixedReferenceTestsBase.WithFixed(ref value, CreateReferenceTest.ReadOnlyTest));
		Assert.Equal(FixedMemoryTestsBase.ReadOnlyError, readOnly.Message);
	}
	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref fref.CreateReference<T>();
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.Equal(typeof(T), fref.Type);
		Assert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();

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

			if (sizeof(T) < sizeof(ManagedStruct))
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);
			else
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, ManagedStruct>(fref));

			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				Assert.Throws<InvalidOperationException>(
					() => CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
		}
		catch (Exception)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				Assert.Throws<InvalidOperationException>(
					() => CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);

				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
			else
			{
				Assert.Throws<InvalidOperationException>(
					() => CreateReferenceTest.TestSize<T, WrapperStruct<String>>(fref));

				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReference<T>());
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = Assert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref fref.CreateReference<T>();
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.Equal(typeof(T), fref.Type);
		Assert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();

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

			if (sizeof(T) < sizeof(ManagedStruct))
			{
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, ManagedStruct>(fref));
				Assert.Throws<InvalidOperationException>(
					() => CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));
			}

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
		}
		catch (Exception)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				Assert.Throws<InvalidOperationException>(
					() => CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);

				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, ManagedStruct>(fref));

				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReference<T>());
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = Assert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void TestSize<T, T2>(FixedReference<T> fref)
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
				Assert.Equal(value, (Object)value2!);
		}
		Exception functionException = Assert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void TestSize<T, T2>(ReadOnlyFixedReference<T> fref)
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
				Assert.Equal(value, (Object)value2!);
		}
		Exception functionException = Assert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
}
#pragma warning restore CS8500