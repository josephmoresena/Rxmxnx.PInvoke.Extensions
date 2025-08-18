#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetFixedMemoryTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void BooleanTest() => GetFixedMemoryTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => GetFixedMemoryTest.Test<Byte>();
	[Fact]
	public void CharTest() => GetFixedMemoryTest.Test<Char>();
#if NET7_0_OR_GREATER
	[Fact]
	internal void DateTimeTest() => GetFixedMemoryTest.Test<DateTime>();
#endif
	[Fact]
	public void DecimalTest() => GetFixedMemoryTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => GetFixedMemoryTest.Test<Double>();
	[Fact]
	public void GuidTest() => GetFixedMemoryTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetFixedMemoryTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => GetFixedMemoryTest.Test<Int16>();
	[Fact]
	public void Int32Test() => GetFixedMemoryTest.Test<Int32>();
	[Fact]
	public void Int64Test() => GetFixedMemoryTest.Test<Int64>();
	[Fact]
	public void SByteTest() => GetFixedMemoryTest.Test<SByte>();
	[Fact]
	public void SingleTest() => GetFixedMemoryTest.Test<Single>();
	[Fact]
	public void UInt16Test() => GetFixedMemoryTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => GetFixedMemoryTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => GetFixedMemoryTest.Test<UInt64>();
	[Fact]
	public void StringTest() => GetFixedMemoryTest.Test<String>();
	[Fact]
	public void BooleanArrayTest() => GetFixedMemoryTest.Test<Boolean[]>();
	[Fact]
	public void ByteArrayTest() => GetFixedMemoryTest.Test<Byte[]>();
	[Fact]
	public void CharArrayTest() => GetFixedMemoryTest.Test<Char[]>();
	[Fact]
	public void DateTimeArrayTest() => GetFixedMemoryTest.Test<DateTime[]>();
	[Fact]
	public void DecimalArrayTest() => GetFixedMemoryTest.Test<Decimal[]>();
	[Fact]
	public void DoubleArrayTest() => GetFixedMemoryTest.Test<Double[]>();
	[Fact]
	public void GuidArrayTest() => GetFixedMemoryTest.Test<Guid[]>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfArrayTest() => GetFixedMemoryTest.Test<Half[]>();
#endif
	[Fact]
	public void Int16ArrayTest() => GetFixedMemoryTest.Test<Int16[]>();
	[Fact]
	public void Int32ArrayTest() => GetFixedMemoryTest.Test<Int32[]>();
	[Fact]
	public void Int64ArrayTest() => GetFixedMemoryTest.Test<Int64[]>();
	[Fact]
	public void SByteArrayTest() => GetFixedMemoryTest.Test<SByte[]>();
	[Fact]
	public void SingleArrayTest() => GetFixedMemoryTest.Test<Single[]>();
	[Fact]
	public void UInt16ArrayTest() => GetFixedMemoryTest.Test<UInt16[]>();
	[Fact]
	public void UInt32ArrayTest() => GetFixedMemoryTest.Test<UInt32[]>();
	[Fact]
	public void UInt64ArrayTest() => GetFixedMemoryTest.Test<UInt64[]>();
	[Fact]
	public void StringArrayTest() => GetFixedMemoryTest.Test<String[]>();

	private static void Test<T>()
	{
		T[] array = GetFixedMemoryTest.fixture.CreateMany<T>().ToArray();
		Memory<T> mem = array.AsMemory();
		ReadOnlyMemory<T> rMem = mem;
		Memory<T> eMemory = default;
		ReadOnlyMemory<T> erMemory = default;

		using (IFixedMemory.IDisposable eMem = eMemory.GetFixedMemory())
		using (IReadOnlyFixedMemory.IDisposable erMem = erMemory.GetFixedMemory())
		{
			PInvokeAssert.True(eMem.Bytes.IsEmpty);
			PInvokeAssert.True(eMem.Objects.IsEmpty);
			PInvokeAssert.True(erMem.Bytes.IsEmpty);
			PInvokeAssert.True(erMem.Objects.IsEmpty);
			PInvokeAssert.NotSame(eMem, erMem);
		}

		if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
#if NETCOREAPP
			Assert.Throws<ArgumentException>(() => mem.GetFixedMemory());
			Assert.Throws<ArgumentException>(() => rMem.GetFixedMemory());
#endif
			return;
		}
		using IFixedMemory.IDisposable fMem = mem.GetFixedMemory();
		using IReadOnlyFixedMemory.IDisposable frMem = rMem.GetFixedMemory();
		PInvokeAssert.IsType<IFixedContext<T>>(fMem, false);
		PInvokeAssert.IsType<IReadOnlyFixedContext<T>>(frMem, false);
		PInvokeAssert.Equal(array.Length * Unsafe.SizeOf<T>(), fMem.Bytes.Length);
		PInvokeAssert.Equal(array.Length * Unsafe.SizeOf<T>(), frMem.Bytes.Length);
		PInvokeAssert.True(Unsafe.AreSame(ref array[0], ref Unsafe.As<Byte, T>(ref fMem.Bytes[0])));
#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(in fMem.Bytes[0], in frMem.Bytes[0]));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in fMem.Bytes[0]), ref Unsafe.AsRef(in frMem.Bytes[0])));
#endif
	}
}