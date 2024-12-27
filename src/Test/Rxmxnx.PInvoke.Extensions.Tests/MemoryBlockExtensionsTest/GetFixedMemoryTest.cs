namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetFixedMemoryTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void BooleanTest() => GetFixedMemoryTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => GetFixedMemoryTest.Test<Byte>();
	[Fact]
	internal void CharTest() => GetFixedMemoryTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => GetFixedMemoryTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => GetFixedMemoryTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => GetFixedMemoryTest.Test<Double>();
	[Fact]
	internal void GuidTest() => GetFixedMemoryTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => GetFixedMemoryTest.Test<Half>();
	[Fact]
	internal void Int16Test() => GetFixedMemoryTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => GetFixedMemoryTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => GetFixedMemoryTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => GetFixedMemoryTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => GetFixedMemoryTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => GetFixedMemoryTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => GetFixedMemoryTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => GetFixedMemoryTest.Test<UInt64>();
	[Fact]
	internal void StringTest() => GetFixedMemoryTest.Test<String>();
	[Fact]
	internal void BooleanArrayTest() => GetFixedMemoryTest.Test<Boolean[]>();
	[Fact]
	internal void ByteArrayTest() => GetFixedMemoryTest.Test<Byte[]>();
	[Fact]
	internal void CharArrayTest() => GetFixedMemoryTest.Test<Char[]>();
	[Fact]
	internal void DateTimeArrayTest() => GetFixedMemoryTest.Test<DateTime[]>();
	[Fact]
	internal void DecimalArrayTest() => GetFixedMemoryTest.Test<Decimal[]>();
	[Fact]
	internal void DoubleArrayTest() => GetFixedMemoryTest.Test<Double[]>();
	[Fact]
	internal void GuidArrayTest() => GetFixedMemoryTest.Test<Guid[]>();
	[Fact]
	internal void HalfArrayTest() => GetFixedMemoryTest.Test<Half[]>();
	[Fact]
	internal void Int16ArrayTest() => GetFixedMemoryTest.Test<Int16[]>();
	[Fact]
	internal void Int32ArrayTest() => GetFixedMemoryTest.Test<Int32[]>();
	[Fact]
	internal void Int64ArrayTest() => GetFixedMemoryTest.Test<Int64[]>();
	[Fact]
	internal void SByteArrayTest() => GetFixedMemoryTest.Test<SByte[]>();
	[Fact]
	internal void SingleArrayTest() => GetFixedMemoryTest.Test<Single[]>();
	[Fact]
	internal void UInt16ArrayTest() => GetFixedMemoryTest.Test<UInt16[]>();
	[Fact]
	internal void UInt32ArrayTest() => GetFixedMemoryTest.Test<UInt32[]>();
	[Fact]
	internal void UInt64ArrayTest() => GetFixedMemoryTest.Test<UInt64[]>();
	[Fact]
	internal void StringArrayTest() => GetFixedMemoryTest.Test<String[]>();

	private static void Test<T>()
	{
		T[] array = GetFixedMemoryTest.fixture.CreateMany<T>().ToArray();
		Memory<T> mem = array.AsMemory();
		ReadOnlyMemory<T> rMem = mem;
		if (!typeof(T).IsValueType)
		{
			Assert.Throws<ArgumentException>(() => mem.GetFixedMemory());
			Assert.Throws<ArgumentException>(() => rMem.GetFixedMemory());
			return;
		}
		using IFixedMemory.IDisposable fMem = mem.GetFixedMemory();
		using IReadOnlyFixedMemory.IDisposable frMem = rMem.GetFixedMemory();
		Assert.IsAssignableFrom<IFixedContext<T>>(fMem);
		Assert.IsAssignableFrom<IReadOnlyFixedContext<T>>(frMem);
		Assert.Equal(array.Length * Unsafe.SizeOf<T>(), fMem.Bytes.Length);
		Assert.Equal(array.Length * Unsafe.SizeOf<T>(), frMem.Bytes.Length);
		Assert.True(Unsafe.AreSame(ref array[0], ref Unsafe.As<Byte, T>(ref fMem.Bytes[0])));
		Assert.True(Unsafe.AreSame(in fMem.Bytes[0], in frMem.Bytes[0]));
	}
}