namespace Rxmxnx.PInvoke.Tests.UnmanagedValueExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class ToValuesTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void ByteTest() => ToValuesTest.Test<Byte>();
	[Fact]
	internal void CharTest() => ToValuesTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => ToValuesTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => ToValuesTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => ToValuesTest.Test<Double>();
	[Fact]
	internal void GuidTest() => ToValuesTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => ToValuesTest.Test<Half>();
	[Fact]
	internal void Int16Test() => ToValuesTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => ToValuesTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => ToValuesTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => ToValuesTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => ToValuesTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => ToValuesTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => ToValuesTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => ToValuesTest.Test<UInt64>();

	private static void Test<T>() where T : unmanaged
	{
		T[] values = ToValuesTest.fixture.CreateMany<T>(10).ToArray();
		ToValuesTest.Test<T, Byte>(values);
		ToValuesTest.Test<T, Char>(values);
		ToValuesTest.Test<T, DateTime>(values);
		ToValuesTest.Test<T, Decimal>(values);
		ToValuesTest.Test<T, Double>(values);
		ToValuesTest.Test<T, Guid>(values);
		ToValuesTest.Test<T, Half>(values);
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

		Assert.Equal(valuesT2, values.ToValues<T, T2>());
		Assert.Equal(valuesT2, values.ToValues<T, T2>(out Byte[] residual));
		Assert.Equal(resiudalBytes, residual);

		Assert.Equal(Array.Empty<T2>(), Array.Empty<T>().ToValues<T, T2>());
		Assert.Equal(Array.Empty<T2>(), Array.Empty<T>().ToValues<T, T2>(out Byte[] residualEmpty));
		Assert.Equal(Array.Empty<Byte>(), residualEmpty);

		T[]? nullValues = default;
		Assert.Null(nullValues.ToValues<T, T2>());
		Assert.Null(nullValues.ToValues<T, T2>(out Byte[]? residualNull));
		Assert.Null(residualNull);
	}
}