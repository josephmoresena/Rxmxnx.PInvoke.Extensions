namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsValueTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void ByteTest() => AsValueTest.Test<Byte>();
	[Fact]
	internal void CharTest() => AsValueTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => AsValueTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => AsValueTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => AsValueTest.Test<Double>();
	[Fact]
	internal void GuidTest() => AsValueTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => AsValueTest.Test<Half>();
	[Fact]
	internal void Int16Test() => AsValueTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => AsValueTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => AsValueTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => AsValueTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => AsValueTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => AsValueTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => AsValueTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => AsValueTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		Span<Byte> bytes = AsValueTest.fixture.CreateMany<Byte>(sizeof(T)).ToArray();
		ReadOnlySpan<Byte> readOnlyBytes = bytes;
		T expected = AsValueTest.GetValue<T>(bytes);

		ref T refValue = ref bytes.AsValue<T>();
		ref readonly T refReadOnlyValue = ref readOnlyBytes.AsValue<T>();

		Assert.True(Unsafe.AreSame(ref refValue, in refReadOnlyValue));
		Assert.Equal(expected, refValue);

		Assert.Throws<InsufficientMemoryException>(() => AsValueTest.InvalidTest<T>(true));
		Assert.Throws<InvalidCastException>(() => AsValueTest.InvalidTest<T>(false));
		Assert.Throws<InsufficientMemoryException>(() => AsValueTest.InvalidReadOnlyTest<T>(true));
		Assert.Throws<InvalidCastException>(() => AsValueTest.InvalidReadOnlyTest<T>(false));
	}
	private static unsafe T GetValue<T>(ReadOnlySpan<Byte> bytes) where T : unmanaged
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(bytes))
			return Unsafe.Read<T>(ptr);
	}
	private static unsafe void InvalidTest<T>(Boolean empty) where T : unmanaged
	{
		Span<Byte> bytes = empty ? Array.Empty<Byte>() : AsValueTest.fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
		_ = ref bytes.AsValue<T>();
	}
	private static unsafe void InvalidReadOnlyTest<T>(Boolean empty) where T : unmanaged
	{
		ReadOnlySpan<Byte> bytes =
			empty ? Array.Empty<Byte>() : AsValueTest.fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
		_ = ref bytes.AsValue<T>();
	}
}