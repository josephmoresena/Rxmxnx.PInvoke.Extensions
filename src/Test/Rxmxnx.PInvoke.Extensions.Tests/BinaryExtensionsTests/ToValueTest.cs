#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToValueTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ByteTest() => ToValueTest.Test<Byte>();
	[Fact]
	public void CharTest() => ToValueTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => ToValueTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => ToValueTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => ToValueTest.Test<Double>();
	[Fact]
	public void GuidTest() => ToValueTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => ToValueTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => ToValueTest.Test<Int16>();
	[Fact]
	public void Int32Test() => ToValueTest.Test<Int32>();
	[Fact]
	public void Int64Test() => ToValueTest.Test<Int64>();
	[Fact]
	public void SByteTest() => ToValueTest.Test<SByte>();
	[Fact]
	public void SingleTest() => ToValueTest.Test<Single>();
	[Fact]
	public void UInt16Test() => ToValueTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => ToValueTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => ToValueTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		Byte[] bytes = ToValueTest.fixture.CreateMany<Byte>(sizeof(T)).ToArray();
		Span<Byte> byteSpan = bytes;
		ReadOnlySpan<Byte> readOnlyBytes = bytes;
		T expected = ToValueTest.GetValue<T>(bytes);

		PInvokeAssert.Equal(expected, bytes.ToValue<T>());
		PInvokeAssert.Equal(expected, byteSpan.ToValue<T>());
		PInvokeAssert.Equal(expected, readOnlyBytes.ToValue<T>());

		bytes = ToValueTest.fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
		byteSpan = bytes;
		readOnlyBytes = bytes;
		expected = ToValueTest.GetValue<T>(bytes);

		PInvokeAssert.Equal(expected, bytes.ToValue<T>());
		PInvokeAssert.Equal(expected, byteSpan.ToValue<T>());
		PInvokeAssert.Equal(expected, readOnlyBytes.ToValue<T>());

		bytes = ToValueTest.fixture.CreateMany<Byte>(sizeof(T) - 1).ToArray();
		byteSpan = bytes;
		readOnlyBytes = bytes;
		expected = ToValueTest.GetValue<T>(bytes);

		PInvokeAssert.Equal(expected, bytes.ToValue<T>());
		PInvokeAssert.Equal(expected, byteSpan.ToValue<T>());
		PInvokeAssert.Equal(expected, readOnlyBytes.ToValue<T>());

		bytes = [];
		byteSpan = bytes;
		readOnlyBytes = bytes;

		PInvokeAssert.Equal(default, bytes.ToValue<T>());
		PInvokeAssert.Equal(default, byteSpan.ToValue<T>());
		PInvokeAssert.Equal(default, readOnlyBytes.ToValue<T>());
	}
	private static unsafe T GetValue<T>(ReadOnlySpan<Byte> bytes) where T : unmanaged
	{
		if (bytes.Length >= sizeof(T))
			fixed (void* ptr = &MemoryMarshal.GetReference(bytes))
				return Unsafe.Read<T>(ptr);
		Span<Byte> result = stackalloc Byte[sizeof(T)];
		bytes.CopyTo(result);
		fixed (void* ptr = &MemoryMarshal.GetReference(result))
			return Unsafe.Read<T>(ptr);
	}
}