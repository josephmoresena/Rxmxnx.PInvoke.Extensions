#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
#pragma warning disable CS8500
public sealed class CreateReferenceTest : FixedReferenceTestsBase
{
	[Fact]
	public void BooleanTest() => CreateReferenceTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => CreateReferenceTest.Test<Byte>();
	[Fact]
	public void Int16Test() => CreateReferenceTest.Test<Int16>();
	[Fact]
	public void CharTest() => CreateReferenceTest.Test<Char>();
	[Fact]
	public void Int32Test() => CreateReferenceTest.Test<Int32>();
	[Fact]
	public void Int64Test() => CreateReferenceTest.Test<Int64>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void Int128Test() => CreateReferenceTest.Test<Int128>();
#endif
	[Fact]
	public void GuidTest() => CreateReferenceTest.Test<Guid>();
	[Fact]
	public void SingleTest() => CreateReferenceTest.Test<Single>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => CreateReferenceTest.Test<Half>();
#endif
	[Fact]
	public void DoubleTest() => CreateReferenceTest.Test<Double>();
	[Fact]
	public void DecimalTest() => CreateReferenceTest.Test<Decimal>();
	[Fact]
	public void DateTimeTest() => CreateReferenceTest.Test<DateTime>();
#if NET6_0_OR_GREATER
	[Fact]
	internal void TimeOnlyTest() => CreateReferenceTest.Test<TimeOnly>();
#endif
	[Fact]
	public void TimeSpanTest() => CreateReferenceTest.Test<TimeSpan>();
	[Fact]
	public void ManagedStructTest() => CreateReferenceTest.Test<ManagedStruct>();
	[Fact]
	public void StringTest() => CreateReferenceTest.Test<String>();
	private static void Test<T>()
	{
		T value = FixedMemoryTestsBase.Fixture.Create<T>();
		FixedReferenceTestsBase.WithFixed(ref value, CreateReferenceTest.Test);
		Exception readOnly =
			PInvokeAssert.Throws<InvalidOperationException>(() => FixedReferenceTestsBase.WithFixed(
				                                                ref value, CreateReferenceTest.ReadOnlyTest));
		PInvokeAssert.Equal(FixedMemoryTestsBase.ReadOnlyError, readOnly.Message);
	}
	private static unsafe void Test<T>(FixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref fref.CreateReference<T>();
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		PInvokeAssert.Equal(ptr2, ptr);
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		PInvokeAssert.Equal(sizeof(T), fref.BinaryLength);
		PInvokeAssert.Equal(0, fref.BinaryOffset);
		PInvokeAssert.Equal(typeof(T), fref.Type);
		PInvokeAssert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif

			CreateReferenceTest.TestSize<T, Boolean>(fref);
			CreateReferenceTest.TestSize<T, Byte>(fref);
			CreateReferenceTest.TestSize<T, Int16>(fref);
			CreateReferenceTest.TestSize<T, Char>(fref);
			CreateReferenceTest.TestSize<T, Int32>(fref);
			CreateReferenceTest.TestSize<T, Int64>(fref);
#if NET7_0_OR_GREATER
			CreateReferenceTest.TestSize<T, Int128>(fref);
#endif
			CreateReferenceTest.TestSize<T, Guid>(fref);
			CreateReferenceTest.TestSize<T, Single>(fref);
#if NET5_0_OR_GREATER
			CreateReferenceTest.TestSize<T, Half>(fref);
#endif
			CreateReferenceTest.TestSize<T, Double>(fref);
			CreateReferenceTest.TestSize<T, Decimal>(fref);
			CreateReferenceTest.TestSize<T, DateTime>(fref);
#if NET6_0_OR_GREATER
			CreateReferenceTest.TestSize<T, TimeOnly>(fref);
#endif
			CreateReferenceTest.TestSize<T, TimeSpan>(fref);

			if (sizeof(T) < sizeof(ManagedStruct))
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, ManagedStruct>(
					                                                fref));

			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
		}
		catch (ArgumentException)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);

				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest
					                                                .TestSize<T, WrapperStruct<String>>(fref));

				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = PInvokeAssert.Throws<InvalidOperationException>(() => fref.CreateReference<T>());
		PInvokeAssert.Equal(FixedMemoryTestsBase.InvalidError, invalid.Message);
		Exception functionException = PInvokeAssert.Throws<InvalidOperationException>(fref.CreateDelegate<Action>);
		PInvokeAssert.Equal(FixedMemoryTestsBase.IsNotFunction, functionException.Message);
	}
	private static unsafe void ReadOnlyTest<T>(ReadOnlyFixedReference<T> fref, IntPtr ptr)
	{
		ref T refValue = ref fref.CreateReference<T>();
		IntPtr ptr2 = new(Unsafe.AsPointer(ref refValue));
		PInvokeAssert.Equal(ptr2, ptr);
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef<T>(ptr.ToPointer()), ref refValue));
		PInvokeAssert.Equal(sizeof(T), fref.BinaryLength);
		PInvokeAssert.Equal(0, fref.BinaryOffset);
		PInvokeAssert.Equal(typeof(T), fref.Type);
		PInvokeAssert.False(fref.IsFunction);

		try
		{
			GCHandle.Alloc(Array.Empty<T>(), GCHandleType.Pinned).Free();
#if !NETCOREAPP
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
				throw new ArgumentException(); // Required for Mono?
#endif

			CreateReferenceTest.TestSize<T, Boolean>(fref);
			CreateReferenceTest.TestSize<T, Byte>(fref);
			CreateReferenceTest.TestSize<T, Int16>(fref);
			CreateReferenceTest.TestSize<T, Char>(fref);
			CreateReferenceTest.TestSize<T, Int32>(fref);
			CreateReferenceTest.TestSize<T, Int64>(fref);
#if NET7_0_OR_GREATER
			CreateReferenceTest.TestSize<T, Int128>(fref);
#endif
			CreateReferenceTest.TestSize<T, Guid>(fref);
			CreateReferenceTest.TestSize<T, Single>(fref);
#if NET5_0_OR_GREATER
			CreateReferenceTest.TestSize<T, Half>(fref);
#endif
			CreateReferenceTest.TestSize<T, Double>(fref);
			CreateReferenceTest.TestSize<T, Decimal>(fref);
			CreateReferenceTest.TestSize<T, DateTime>(fref);
#if NET6_0_OR_GREATER
			CreateReferenceTest.TestSize<T, TimeOnly>(fref);
#endif
			CreateReferenceTest.TestSize<T, TimeSpan>(fref);

			if (sizeof(T) < sizeof(ManagedStruct))
			{
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, ManagedStruct>(
					                                                fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));
			}

			if (sizeof(T) < IntPtr.Size)
			{
				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
		}
		catch (ArgumentException)
		{
			if (sizeof(T) < sizeof(WrapperStruct<ManagedStruct>))
				CreateReferenceTest.TestSize<T, WrapperStruct<ManagedStruct>>(fref);
			else
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest
					                                                .TestSize<T, WrapperStruct<ManagedStruct>>(fref));

			if (typeof(T).IsValueType)
			{
				CreateReferenceTest.TestSize<T, ManagedStruct>(fref);

				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, String>(fref));
			}
			else
			{
				PInvokeAssert.Throws<InvalidOperationException>(() => CreateReferenceTest.TestSize<T, ManagedStruct>(
					                                                fref));

				CreateReferenceTest.TestSize<T, String>(fref);
				CreateReferenceTest.TestSize<T, Object>(fref);
			}
		}

		fref.Unload();
		Exception invalid = PInvokeAssert.Throws<InvalidOperationException>(() => fref.CreateReference<T>());
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
			Exception invalidSize = PInvokeAssert.Throws<InsufficientMemoryException>(() => fref.CreateReference<T2>());
			PInvokeAssert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref fref.CreateReference<T>();
			ref T2 value2 = ref fref.CreateReference<T2>();
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
			Exception invalidSize = PInvokeAssert.Throws<InsufficientMemoryException>(() => fref.CreateReference<T2>());
			PInvokeAssert.Equal(String.Format(FixedMemoryTestsBase.InvalidSizeFormat, typeof(T2)), invalidSize.Message);
		}
		else
		{
			ref T value = ref fref.CreateReference<T>();
			ref T2 value2 = ref fref.CreateReference<T2>();
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