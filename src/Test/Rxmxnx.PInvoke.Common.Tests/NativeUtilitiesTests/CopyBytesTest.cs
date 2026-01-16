namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class CopyBytesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ExceptionTest()
	{
		PInvokeAssert.Throws<InsufficientMemoryException>(() =>
		{
			Span<Byte> bytes = stackalloc Byte[2];
			NativeUtilities.CopyBytes(CopyBytesTest.fixture.Create<Decimal>(), bytes);
		});
	}

	[Fact]
	public void NormalTest()
	{
		CopyBytesTest.CopyTest<Boolean>();
		CopyBytesTest.CopyTest<Byte>();
		CopyBytesTest.CopyTest<Int16>();
		CopyBytesTest.CopyTest<Int32>();
		CopyBytesTest.CopyTest<Int64>();
		CopyBytesTest.CopyTest<Single>();
		CopyBytesTest.CopyTest<Double>();
		CopyBytesTest.CopyTest<Decimal>();
	}

	private static unsafe void CopyTest<T>() where T : unmanaged
	{
		T[] values = CopyBytesTest.fixture.CreateMany<T>().ToArray();
		Span<Byte> span = stackalloc Byte[values.Length * sizeof(T)];
		for (Int32 i = 0; i < values.Length; i++)
			NativeUtilities.CopyBytes(values[i], span, i * sizeof(T));
		Span<T> spanValues = MemoryMarshal.Cast<Byte, T>(span);
		PInvokeAssert.Equal(values, spanValues.ToArray());
	}
}