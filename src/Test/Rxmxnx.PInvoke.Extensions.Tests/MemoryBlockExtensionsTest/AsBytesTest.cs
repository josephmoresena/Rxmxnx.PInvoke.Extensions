namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsBytesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void BooleanTest() => AsBytesTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => AsBytesTest.Test<Byte>();
	[Fact]
	public void CharTest() => AsBytesTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => AsBytesTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => AsBytesTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => AsBytesTest.Test<Double>();
	[Fact]
	public void GuidTest() => AsBytesTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => AsBytesTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => AsBytesTest.Test<Int16>();
	[Fact]
	public void Int32Test() => AsBytesTest.Test<Int32>();
	[Fact]
	public void Int64Test() => AsBytesTest.Test<Int64>();
	[Fact]
	public void SByteTest() => AsBytesTest.Test<SByte>();
	[Fact]
	public void SingleTest() => AsBytesTest.Test<Single>();
	[Fact]
	public void UInt16Test() => AsBytesTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => AsBytesTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => AsBytesTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T[] values = AsBytesTest.fixture.CreateMany<T>(10).ToArray();
		Span<T> span = values;
		ReadOnlySpan<T> readOnlySpan = span;

		Span<Byte> spanBytes = MemoryMarshal.AsBytes(span);
		Byte[] bytes = spanBytes.ToArray();

		PInvokeAssert.Equal(bytes, span.AsBytes().ToArray());
		PInvokeAssert.Equal(bytes, readOnlySpan.AsBytes().ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref spanBytes[0], ref MemoryMarshal.GetReference(span.AsBytes())));
		PInvokeAssert.True(Unsafe.AreSame(ref spanBytes[0], ref MemoryMarshal.GetReference(readOnlySpan.AsBytes())));
	}
}