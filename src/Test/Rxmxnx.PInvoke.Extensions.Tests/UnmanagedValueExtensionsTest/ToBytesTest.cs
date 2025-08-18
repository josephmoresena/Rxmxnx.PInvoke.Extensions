#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToBytesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void BooleanTest() => ToBytesTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => ToBytesTest.Test<Byte>();
	[Fact]
	public void CharTest() => ToBytesTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => ToBytesTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => ToBytesTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => ToBytesTest.Test<Double>();
	[Fact]
	public void GuidTest() => ToBytesTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => ToBytesTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => ToBytesTest.Test<Int16>();
	[Fact]
	public void Int32Test() => ToBytesTest.Test<Int32>();
	[Fact]
	public void Int64Test() => ToBytesTest.Test<Int64>();
	[Fact]
	public void SByteTest() => ToBytesTest.Test<SByte>();
	[Fact]
	public void SingleTest() => ToBytesTest.Test<Single>();
	[Fact]
	public void UInt16Test() => ToBytesTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => ToBytesTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => ToBytesTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T value = ToBytesTest.fixture.Create<T>();
		T[] values = ToBytesTest.fixture.CreateMany<T>(10).ToArray();
		Byte[] bytes = ToBytesTest.GetBytes(ref value);
		Byte[] bytesValues = MemoryMarshal.AsBytes(values.AsSpan()).ToArray();

		PInvokeAssert.Equal(bytes, value.ToBytes());
		PInvokeAssert.Equal(bytesValues, values.ToBytes());
		PInvokeAssert.Equal(Array.Empty<Byte>(), Array.Empty<T>().ToBytes());

		T[]? nullValues = default;
		PInvokeAssert.Null(nullValues.ToBytes());
	}
	private static unsafe Byte[] GetBytes<T>(ref T refValue) where T : unmanaged
	{
		fixed (void* ptr = &refValue)
			return new ReadOnlySpan<Byte>(ptr, sizeof(T)).ToArray();
	}
}