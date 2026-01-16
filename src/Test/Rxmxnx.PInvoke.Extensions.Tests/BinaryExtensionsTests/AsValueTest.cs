namespace Rxmxnx.PInvoke.Tests.BinaryExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class AsValueTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ByteTest() => AsValueTest.Test<Byte>();
	[Fact]
	public void CharTest() => AsValueTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => AsValueTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => AsValueTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => AsValueTest.Test<Double>();
	[Fact]
	public void GuidTest() => AsValueTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => AsValueTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => AsValueTest.Test<Int16>();
	[Fact]
	public void Int32Test() => AsValueTest.Test<Int32>();
	[Fact]
	public void Int64Test() => AsValueTest.Test<Int64>();
	[Fact]
	public void SByteTest() => AsValueTest.Test<SByte>();
	[Fact]
	public void SingleTest() => AsValueTest.Test<Single>();
	[Fact]
	public void UInt16Test() => AsValueTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => AsValueTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => AsValueTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		Span<Byte> bytes = AsValueTest.fixture.CreateMany<Byte>(sizeof(T)).ToArray();
		ReadOnlySpan<Byte> readOnlyBytes = bytes;
		T expected = AsValueTest.GetValue<T>(bytes);

		ref T refValue = ref bytes.AsValue<T>();
		ref readonly T refReadOnlyValue = ref readOnlyBytes.AsValue<T>();

#if NET8_0_OR_GREATER
		Assert.True(Unsafe.AreSame(ref refValue, in refReadOnlyValue));
#else
		PInvokeAssert.True(Unsafe.AreSame(ref refValue, ref Unsafe.AsRef(in refReadOnlyValue)));
#endif
		PInvokeAssert.Equal(expected, refValue);

		PInvokeAssert.Throws<InsufficientMemoryException>(() => AsValueTest.InvalidTest<T>(true));
		PInvokeAssert.Throws<InvalidCastException>(() => AsValueTest.InvalidTest<T>(false));
		PInvokeAssert.Throws<InsufficientMemoryException>(() => AsValueTest.InvalidReadOnlyTest<T>(true));
		PInvokeAssert.Throws<InvalidCastException>(() => AsValueTest.InvalidReadOnlyTest<T>(false));
	}
	private static unsafe T GetValue<T>(ReadOnlySpan<Byte> bytes) where T : unmanaged
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(bytes))
			return Unsafe.Read<T>(ptr);
	}
	private static unsafe void InvalidTest<T>(Boolean empty) where T : unmanaged
	{
		Span<Byte> bytes = empty ? [] : AsValueTest.fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
		_ = ref bytes.AsValue<T>();
	}
	private static unsafe void InvalidReadOnlyTest<T>(Boolean empty) where T : unmanaged
	{
		ReadOnlySpan<Byte> bytes = empty ? [] : AsValueTest.fixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
		_ = ref bytes.AsValue<T>();
	}
}