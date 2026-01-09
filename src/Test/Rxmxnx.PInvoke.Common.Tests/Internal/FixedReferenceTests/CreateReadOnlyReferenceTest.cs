namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class CreateReadOnlyReferenceTest : FixedReferenceTestsBase
{
	[Fact]
	public void BooleanTest() => CreateReadOnlyReferenceTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => CreateReadOnlyReferenceTest.Test<Byte>();
	[Fact]
	public void Int16Test() => CreateReadOnlyReferenceTest.Test<Int16>();
	[Fact]
	public void CharTest() => CreateReadOnlyReferenceTest.Test<Char>();
	[Fact]
	public void Int32Test() => CreateReadOnlyReferenceTest.Test<Int32>();
	[Fact]
	public void Int64Test() => CreateReadOnlyReferenceTest.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => CreateReadOnlyReferenceTest.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => CreateReadOnlyReferenceTest.Test<Guid>();
	[Fact]
	public void SingleTest() => CreateReadOnlyReferenceTest.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => CreateReadOnlyReferenceTest.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => CreateReadOnlyReferenceTest.Test<Double>();
	[Fact]
	public void DecimalTest() => CreateReadOnlyReferenceTest.Test<Decimal>();
	[Fact]
	public void DateTimeTest() => CreateReadOnlyReferenceTest.Test<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => CreateReadOnlyReferenceTest.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => CreateReadOnlyReferenceTest.Test<TimeSpan>();
	private static void Test<T>()
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, CreateReadOnlyReferenceTest.Test);
		FixedReferenceTestsBase.WithFixed(ref value, CreateReadOnlyReferenceTest.ReadOnlyTest);
	}
	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T>());
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		PInvokeAssert.Equal(ptr2, ptr);
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		PInvokeAssert.Equal(sizeof(T), fref.BinaryLength);
		PInvokeAssert.Equal(0, fref.BinaryOffset);
		PInvokeAssert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif

			CreateReadOnlyReferenceTest.TestSize<T, Boolean>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Byte>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Int16>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Char>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Int32>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Int64>(fref);
#if NET7_0_OR_GREATER
			CreateReadOnlyReferenceTest.TestSize<T, Int128>(fref);
#endif
			CreateReadOnlyReferenceTest.TestSize<T, Guid>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Single>(fref);
#if NET5_0_OR_GREATER
			CreateReadOnlyReferenceTest.TestSize<T, Half>(fref);
#endif
			CreateReadOnlyReferenceTest.TestSize<T, Double>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Decimal>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, DateTime>(fref);
#if NET6_0_OR_GREATER
			CreateReadOnlyReferenceTest.TestSize<T, TimeOnly>(fref);
#endif
			CreateReadOnlyReferenceTest.TestSize<T, TimeSpan>(fref);

			if (sizeof(T) < sizeof(ManagedStruct))
			{
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, ManagedStruct>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));
			}

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
			}
		}
		catch (ArgumentException)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);

				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, WrapperStruct<String>>(fref));

				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = PInvokeAssert.Throws<InvalidOperationException>(() => fref.CreateReadOnlyReference<T>());
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T>());
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		PInvokeAssert.Equal(ptr2, ptr);
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		PInvokeAssert.Equal(sizeof(T), fref.BinaryLength);
		PInvokeAssert.Equal(0, fref.BinaryOffset);
		PInvokeAssert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif

			CreateReadOnlyReferenceTest.TestSize<T, Boolean>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Byte>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Int16>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Char>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Int32>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Int64>(fref);
#if NET7_0_OR_GREATER
			CreateReadOnlyReferenceTest.TestSize<T, Int128>(fref);
#endif
			CreateReadOnlyReferenceTest.TestSize<T, Guid>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Single>(fref);
#if NET5_0_OR_GREATER
			CreateReadOnlyReferenceTest.TestSize<T, Half>(fref);
#endif
			CreateReadOnlyReferenceTest.TestSize<T, Double>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, Decimal>(fref);
			CreateReadOnlyReferenceTest.TestSize<T, DateTime>(fref);
#if NET6_0_OR_GREATER
			CreateReadOnlyReferenceTest.TestSize<T, TimeOnly>(fref);
#endif
			CreateReadOnlyReferenceTest.TestSize<T, TimeSpan>(fref);

			if (sizeof(T) < sizeof(ManagedStruct))
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, ManagedStruct>(fref));

			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
			}
		}
		catch (ArgumentException)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReadOnlyReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReadOnlyReferenceTest.TestSize<T, ManagedStruct>(fref);

				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest.TestSize<T, String>(
					                                                fref));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReadOnlyReferenceTest
					                                                .TestSize<T, WrapperStruct<String>>(fref));

				CreateReadOnlyReferenceTest.TestSize<T, String>(fref);
				CreateReadOnlyReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = PInvokeAssert.Throws<InvalidOperationException>(() => fref.CreateReadOnlyReference<T>());
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void TestSize<T, T2>(FixedReference<T> fref)
	{
		Int32 size = sizeof(T);
		Int32 size2 = sizeof(T2);

		if (size < size2)
		{
			Exception invalidSize =
				PInvokeAssert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			PInvokeAssert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T>());
			ref T2 value2 = ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T2>());
			Byte[] bytes1 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size).ToArray();
			Byte[] bytes2 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size2).ToArray();

			PInvokeAssert.Equal(bytes1[..size2], bytes2);

			if (typeof(T) == typeof(T2))
				PInvokeAssert.Equal(value, (Object)value2!);
		}

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void TestSize<T, T2>(ReadOnlyFixedReference<T> fref)
	{
		Int32 size = sizeof(T);
		Int32 size2 = sizeof(T2);

		if (size < size2)
		{
			Exception invalidSize =
				PInvokeAssert.Throws<InsufficientMemoryException>(() => fref.CreateReadOnlyReference<T2>());
			PInvokeAssert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T>());
			ref T2 value2 = ref Unsafe.AsRef(in fref.CreateReadOnlyReference<T2>());
			Byte[] bytes1 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size).ToArray();
			Byte[] bytes2 = new ReadOnlySpan<Byte>(Unsafe.AsPointer(ref value), size2).ToArray();

			PInvokeAssert.Equal(bytes1[..size2], bytes2);

			if (typeof(T) == typeof(T2))
				PInvokeAssert.Equal(value, (Object)value2!);
		}

		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
}
#pragma warning restore CS8500