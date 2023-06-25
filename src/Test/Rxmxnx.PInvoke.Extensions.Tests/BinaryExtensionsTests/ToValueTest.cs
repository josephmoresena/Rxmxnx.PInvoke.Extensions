namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToValueTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void ByteTest() => ToValueTest.Test<Byte>();
	[Fact]
	internal void CharTest() => ToValueTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => ToValueTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => ToValueTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => ToValueTest.Test<Double>();
	[Fact]
	internal void GuidTest() => ToValueTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => ToValueTest.Test<Half>();
	[Fact]
	internal void Int16Test() => ToValueTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => ToValueTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => ToValueTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => ToValueTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => ToValueTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => ToValueTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => ToValueTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => ToValueTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		Byte[] bytes = ToValueTest.fixture.CreateMany<Byte>(sizeof(T)).ToArray();
		Span<Byte> byteSpan = bytes;
		ReadOnlySpan<Byte> readOnlyBytes = bytes;
		T expected = ToValueTest.GetValue<T>(bytes);

		Assert.Equal(expected, bytes.ToValue<T>());
		Assert.Equal(expected, byteSpan.ToValue<T>());
		Assert.Equal(expected, readOnlyBytes.ToValue<T>());

		bytes = ToValueTest.fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
		byteSpan = bytes;
		readOnlyBytes = bytes;
		expected = ToValueTest.GetValue<T>(bytes);

		Assert.Equal(expected, bytes.ToValue<T>());
		Assert.Equal(expected, byteSpan.ToValue<T>());
		Assert.Equal(expected, readOnlyBytes.ToValue<T>());

		bytes = ToValueTest.fixture.CreateMany<Byte>(sizeof(T) - 1).ToArray();
		byteSpan = bytes;
		readOnlyBytes = bytes;
		expected = ToValueTest.GetValue<T>(bytes);

		Assert.Equal(expected, bytes.ToValue<T>());
		Assert.Equal(expected, byteSpan.ToValue<T>());
		Assert.Equal(expected, readOnlyBytes.ToValue<T>());

		bytes = Array.Empty<Byte>();
		byteSpan = bytes;
		readOnlyBytes = bytes;

		Assert.Equal(default, bytes.ToValue<T>());
		Assert.Equal(default, byteSpan.ToValue<T>());
		Assert.Equal(default, readOnlyBytes.ToValue<T>());
	}
	private static unsafe T GetValue<T>(ReadOnlySpan<Byte> bytes) where T : unmanaged
	{
		if (bytes.Length >= sizeof(T))
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(bytes))
				return Unsafe.Read<T>(ptr);
		}
		Span<Byte> result = stackalloc Byte[sizeof(T)];
		bytes.CopyTo(result);
		fixed (void* ptr = &MemoryMarshal.GetReference(result))
			return Unsafe.Read<T>(ptr);
	}
}