namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class CreateReadOnlyReferenceTest : FixedReferenceTestsBase
{
	[Fact]
	internal void BooleanTest() => CreateReadOnlyReferenceTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => CreateReadOnlyReferenceTest.Test<Byte>();
	[Fact]
	internal void Int16Test() => CreateReadOnlyReferenceTest.Test<Int16>();
	[Fact]
	internal void CharTest() => CreateReadOnlyReferenceTest.Test<Char>();
	[Fact]
	internal void Int32Test() => CreateReadOnlyReferenceTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => CreateReadOnlyReferenceTest.Test<Int64>();
	[Fact]
	internal void Int128Test() => CreateReadOnlyReferenceTest.Test<Int128>();
	[Fact]
	internal void GuidTest() => CreateReadOnlyReferenceTest.Test<Guid>();
	[Fact]
	internal void SingleTest() => CreateReadOnlyReferenceTest.Test<Single>();
	[Fact]
	internal void HalfTest() => CreateReadOnlyReferenceTest.Test<Half>();
	[Fact]
	internal void DoubleTest() => CreateReadOnlyReferenceTest.Test<Double>();
	[Fact]
	internal void DecimalTest() => CreateReadOnlyReferenceTest.Test<Decimal>();
	[Fact]
	internal void DateTimeTest() => CreateReadOnlyReferenceTest.Test<DateTime>();
	[Fact]
	internal void TimeOnlyTest() => CreateReadOnlyReferenceTest.Test<TimeOnly>();
	[Fact]
	internal void TimeSpanTest() => CreateReadOnlyReferenceTest.Test<TimeSpan>();
	private static void Test<T>()
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, CreateReadOnlyReferenceTest.Test);
		FixedReferenceTestsBase.WithFixed(ref value, CreateReadOnlyReferenceTest.ReadOnlyTest);
	}
	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T>());
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();

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

			if (sizeof(T) < sizeof(ManagedStruct))
			{
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			}
			else
			{
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref));
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));
			}

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
			}
		}
		catch (Exception)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);

				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
			}
			else
			{
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<String>>(fref));

				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReadOnlyReference<T>());
		Assert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = Assert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref UnsafeLegacy.AsRef(in fref.CreateReadOnlyReference<T>());
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		Assert.Equal(ptr2, ptr);
		Assert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		Assert.Equal(sizeof(T), fref.BinaryLength);
		Assert.Equal(0, fref.BinaryOffset);
		Assert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();

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

			if (sizeof(T) < sizeof(ManagedStruct))
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);
			else
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref));

			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
			}
		}
		catch (Exception)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);

				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
				Assert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(fref));
			}
			else
			{
				Assert.Throws<InvalidOperationException>(
					() => CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<String>>(fref));

				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = Assert.Throws<InvalidOperationException>(() => fref.CreateReadOnlyReference<T>());
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
				Assert.Equal(value, (Object)value2!);
		}

		Exception functionException = Assert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		Assert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
}
#pragma warning restore CS8500