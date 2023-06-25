namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToBytesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void BooleanTest() => ToBytesTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => ToBytesTest.Test<Byte>();
	[Fact]
	internal void CharTest() => ToBytesTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => ToBytesTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => ToBytesTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => ToBytesTest.Test<Double>();
	[Fact]
	internal void GuidTest() => ToBytesTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => ToBytesTest.Test<Half>();
	[Fact]
	internal void Int16Test() => ToBytesTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => ToBytesTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => ToBytesTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => ToBytesTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => ToBytesTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => ToBytesTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => ToBytesTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => ToBytesTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T value = ToBytesTest.fixture.Create<T>();
		T[] values = ToBytesTest.fixture.CreateMany<T>(10).ToArray();
		Byte[] bytes = ToBytesTest.GetBytes(ref value);
		Byte[] bytesValues = MemoryMarshal.AsBytes(values.AsSpan()).ToArray();

		Assert.Equal(bytes, value.ToBytes());
		Assert.Equal(bytesValues, values.ToBytes());
		Assert.Equal(Array.Empty<Byte>(), Array.Empty<T>().ToBytes());

		T[]? nullValues = default;
		Assert.Null(nullValues.ToBytes());
	}
	private static unsafe Byte[] GetBytes<T>(ref T refValue) where T : unmanaged
	{
		fixed (void* ptr = &refValue)
			return new ReadOnlySpan<Byte>(ptr, sizeof(T)).ToArray();
	}
}