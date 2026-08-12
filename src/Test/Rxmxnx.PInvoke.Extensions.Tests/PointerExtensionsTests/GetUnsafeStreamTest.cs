namespace Rxmxnx.PInvoke.Tests.PointerExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafeStreamTest
{
	private readonly IFixture _fixture = new Fixture();

	[Fact]
	public void Test()
	{
		const Int32 length = 10;
		Span<Byte> bytes = stackalloc Byte[length];
		Byte[] source = this._fixture.CreateMany<Byte>(length).ToArray();
		using (Stream strm = bytes.GetUnsafeIntPtr().GetUnsafeStream(length))
			strm.Write(source, 0, source.Length);
		PInvokeAssert.True(bytes.SequenceEqual(source));
	}
	[Fact]
	public void UIntPtrTest()
	{
		const Int32 length = 10;
		Span<Byte> bytes = stackalloc Byte[length];
		Byte[] source = this._fixture.CreateMany<Byte>(length).ToArray();
		using (Stream strm = bytes.GetUnsafeUIntPtr().GetUnsafeStream(length))
			strm.Write(source, 0, source.Length);
		PInvokeAssert.True(bytes.SequenceEqual(source));
	}
	[Fact]
	public void InvalidSize() { PInvokeAssert.Throws<ArgumentException>(() => IntPtr.Zero.GetUnsafeStream(-1)); }
	[Fact]
	public void NullTest()
	{
		using Stream strm0 = IntPtr.Zero.GetUnsafeStream(0);
		using Stream strm1 = UIntPtr.Zero.GetUnsafeStream(0);
		PInvokeAssert.Same(strm0, strm1);
		PInvokeAssert.Same(Stream.Null, strm0);
	}
}