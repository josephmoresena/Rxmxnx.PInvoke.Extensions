#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.ReferenceExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class TransformTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void BooleanTest() => TransformTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => TransformTest.Test<Byte>();
	[Fact]
	public void CharTest() => TransformTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => TransformTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => TransformTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => TransformTest.Test<Double>();
	[Fact]
	public void GuidTest() => TransformTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => TransformTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => TransformTest.Test<Int16>();
	[Fact]
	public void Int32Test() => TransformTest.Test<Int32>();
	[Fact]
	public void Int64Test() => TransformTest.Test<Int64>();
	[Fact]
	public void SByteTest() => TransformTest.Test<SByte>();
	[Fact]
	public void SingleTest() => TransformTest.Test<Single>();
	[Fact]
	public void UInt16Test() => TransformTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => TransformTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => TransformTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T value = TransformTest.fixture.Create<T>();

		TransformTest.BinaryTest(ref value);
		TransformTest.Test<T, Boolean>(ref value);
		TransformTest.Test<T, Byte>(ref value);
		TransformTest.Test<T, Char>(ref value);
		TransformTest.Test<T, DateTime>(ref value);
		TransformTest.Test<T, Decimal>(ref value);
		TransformTest.Test<T, Double>(ref value);
		TransformTest.Test<T, Guid>(ref value);
#if NET5_0_OR_GREATER
		TransformTest.Test<T, Half>(ref value);
#endif
		TransformTest.Test<T, Int16>(ref value);
		TransformTest.Test<T, Int32>(ref value);
		TransformTest.Test<T, Int64>(ref value);
		TransformTest.Test<T, SByte>(ref value);
		TransformTest.Test<T, Single>(ref value);
		TransformTest.Test<T, UInt16>(ref value);
		TransformTest.Test<T, UInt32>(ref value);
		TransformTest.Test<T, UInt64>(ref value);
	}

	private static unsafe void Test<T, T2>(ref T refValue) where T : unmanaged where T2 : unmanaged
	{
		try
		{
			ref T2 refValue2 = ref refValue.Transform<T, T2>();
			PInvokeAssert.Equal(sizeof(T), sizeof(T2));
			fixed (void* ptr1 = &refValue)
			fixed (void* ptr2 = &refValue2)
				PInvokeAssert.Equal(new(ptr1), new IntPtr(ptr2));

			if (typeof(T) == typeof(T2))
			{
				PInvokeAssert.Equal((Object)refValue, refValue2);
			}
			else
			{
				Span<Byte> bytes1 = refValue.AsBytes();
				Span<Byte> bytes2 = refValue2.AsBytes();
				PInvokeAssert.Equal(bytes1.ToArray(), bytes2.ToArray());
			}
		}
		catch (Exception ex)
		{
			PInvokeAssert.IsType<InvalidOperationException>(ex);
			PInvokeAssert.NotEqual(sizeof(T), sizeof(T2));
		}
	}

	private static void BinaryTest<T>(ref T refValue) where T : unmanaged
	{
		Byte[] bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref refValue, 1)).ToArray();
		Span<Byte> span = refValue.AsBytes();
		Span<T> spanT = MemoryMarshal.Cast<Byte, T>(span);

		PInvokeAssert.Equal(bytes, span.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref refValue, ref spanT[0]));
	}
}