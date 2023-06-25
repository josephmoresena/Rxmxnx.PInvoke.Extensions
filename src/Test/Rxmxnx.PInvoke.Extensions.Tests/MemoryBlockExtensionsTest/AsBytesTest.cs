namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsBytesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void BooleanTest() => AsBytesTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => AsBytesTest.Test<Byte>();
	[Fact]
	internal void CharTest() => AsBytesTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => AsBytesTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => AsBytesTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => AsBytesTest.Test<Double>();
	[Fact]
	internal void GuidTest() => AsBytesTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => AsBytesTest.Test<Half>();
	[Fact]
	internal void Int16Test() => AsBytesTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => AsBytesTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => AsBytesTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => AsBytesTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => AsBytesTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => AsBytesTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => AsBytesTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => AsBytesTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T[] values = AsBytesTest.fixture.CreateMany<T>(10).ToArray();
		Span<T> span = values;
		ReadOnlySpan<T> readOnlySpan = span;

		Span<Byte> spanBytes = MemoryMarshal.AsBytes(span);
		Byte[] bytes = spanBytes.ToArray();

		Assert.Equal(bytes, span.AsBytes().ToArray());
		Assert.Equal(bytes, readOnlySpan.AsBytes().ToArray());
		Assert.True(Unsafe.AreSame(ref spanBytes[0], ref MemoryMarshal.GetReference(span.AsBytes())));
		Assert.True(Unsafe.AreSame(ref spanBytes[0], ref MemoryMarshal.GetReference(readOnlySpan.AsBytes())));
	}
}