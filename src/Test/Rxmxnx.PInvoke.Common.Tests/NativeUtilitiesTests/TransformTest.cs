namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class TransformTest
{
	private static readonly IFixture fixture = new Fixture();

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

		TransformTest.BinaryTest(value);
		TransformTest.Test<T, Byte>(value);
		TransformTest.Test<T, Char>(value);
		TransformTest.Test<T, DateTime>(value);
		TransformTest.Test<T, Decimal>(value);
		TransformTest.Test<T, Double>(value);
		TransformTest.Test<T, Guid>(value);
#if NET5_0_OR_GREATER
		TransformTest.Test<T, Half>(value);
#endif
		TransformTest.Test<T, Int16>(value);
		TransformTest.Test<T, Int32>(value);
		TransformTest.Test<T, Int64>(value);
		TransformTest.Test<T, SByte>(value);
		TransformTest.Test<T, Single>(value);
		TransformTest.Test<T, UInt16>(value);
		TransformTest.Test<T, UInt32>(value);
		TransformTest.Test<T, UInt64>(value);
	}

	private static unsafe void Test<T, T2>(in T refValue) where T : unmanaged where T2 : unmanaged
	{
		try
		{
			ref readonly T2 refValue2 = ref NativeUtilities.Transform<T, T2>(refValue);
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
				ReadOnlySpan<Byte> bytes1 =
					MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in refValue), 1));
				ReadOnlySpan<Byte> bytes2 =
					MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in refValue2), 1));
				PInvokeAssert.Equal(bytes1.ToArray(), bytes2.ToArray());
			}
		}
		catch (Exception ex)
		{
			PInvokeAssert.IsType<InvalidOperationException>(ex);
			PInvokeAssert.NotEqual(sizeof(T), sizeof(T2));
		}
	}

	private static void BinaryTest<T>(in T refValue) where T : unmanaged
	{
		Byte[] bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in refValue), 1))
		                            .ToArray();
		ReadOnlySpan<Byte> span = NativeUtilities.AsBytes(refValue);
		ReadOnlySpan<T> spanT = MemoryMarshal.Cast<Byte, T>(span);

		PInvokeAssert.Equal(bytes, NativeUtilities.ToBytes(refValue));
		PInvokeAssert.Equal(bytes, span.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in refValue), ref Unsafe.AsRef(in spanT[0])));
	}
}