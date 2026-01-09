namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToValuesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ByteTest() => ToValuesTest.Test<Byte>();
	[Fact]
	public void CharTest() => ToValuesTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => ToValuesTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => ToValuesTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => ToValuesTest.Test<Double>();
	[Fact]
	public void GuidTest() => ToValuesTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => ToValuesTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => ToValuesTest.Test<Int16>();
	[Fact]
	public void Int32Test() => ToValuesTest.Test<Int32>();
	[Fact]
	public void Int64Test() => ToValuesTest.Test<Int64>();
	[Fact]
	public void SByteTest() => ToValuesTest.Test<SByte>();
	[Fact]
	public void SingleTest() => ToValuesTest.Test<Single>();
	[Fact]
	public void UInt16Test() => ToValuesTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => ToValuesTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => ToValuesTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T[] values = ToValuesTest.fixture.CreateMany<T>(10).ToArray();
		ToValuesTest.Test<T, Byte>(values);
		ToValuesTest.Test<T, Char>(values);
		ToValuesTest.Test<T, DateTime>(values);
		ToValuesTest.Test<T, Decimal>(values);
		ToValuesTest.Test<T, Double>(values);
		ToValuesTest.Test<T, Guid>(values);
#if NET5_0_OR_GREATER
		ToValuesTest.Test<T, Half>(values);
#endif
		ToValuesTest.Test<T, Int16>(values);
		ToValuesTest.Test<T, Int32>(values);
		ToValuesTest.Test<T, Int64>(values);
		ToValuesTest.Test<T, SByte>(values);
		ToValuesTest.Test<T, Single>(values);
		ToValuesTest.Test<T, UInt16>(values);
		ToValuesTest.Test<T, UInt32>(values);
		ToValuesTest.Test<T, UInt64>(values);
	}
	private static unsafe void Test<T, T2>(T[] values) where T : unmanaged where T2 : unmanaged
	{
		ReadOnlySpan<T> span = values;
		ReadOnlySpan<Byte> byteSpan = MemoryMarshal.AsBytes(span);
		ReadOnlySpan<T2> spanT2 = MemoryMarshal.Cast<Byte, T2>(byteSpan);
		ReadOnlySpan<Byte> spanResidual = byteSpan[(spanT2.Length * sizeof(T2))..];

		T2[] valuesT2 = spanT2.ToArray();
		Byte[] resiudalBytes = spanResidual.ToArray();

		PInvokeAssert.Equal(valuesT2, values.ToValues<T, T2>());
		PInvokeAssert.Equal(valuesT2, values.ToValues<T, T2>(out Byte[] residual));
		PInvokeAssert.Equal(resiudalBytes, residual);

		PInvokeAssert.Equal([], Array.Empty<T>().ToValues<T, T2>());
		PInvokeAssert.Equal([], Array.Empty<T>().ToValues<T, T2>(out Byte[] residualEmpty));
		PInvokeAssert.Equal(Array.Empty<Byte>(), residualEmpty);

		T[]? nullValues = default;
		PInvokeAssert.Null(nullValues.ToValues<T, T2>());
		PInvokeAssert.Null(nullValues.ToValues<T, T2>(out Byte[]? residualNull));
		PInvokeAssert.Null(residualNull);
	}
}