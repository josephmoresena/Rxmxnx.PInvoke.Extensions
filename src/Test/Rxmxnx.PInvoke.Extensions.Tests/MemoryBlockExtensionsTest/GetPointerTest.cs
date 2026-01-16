namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetPointerTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void ByteTest() => GetPointerTest.Test<Byte>();
	[Fact]
	public void CharTest() => GetPointerTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => GetPointerTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => GetPointerTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => GetPointerTest.Test<Double>();
	[Fact]
	public void GuidTest() => GetPointerTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetPointerTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => GetPointerTest.Test<Int16>();
	[Fact]
	public void Int32Test() => GetPointerTest.Test<Int32>();
	[Fact]
	public void Int64Test() => GetPointerTest.Test<Int64>();
	[Fact]
	public void SByteTest() => GetPointerTest.Test<SByte>();
	[Fact]
	public void SingleTest() => GetPointerTest.Test<Single>();
	[Fact]
	public void UInt16Test() => GetPointerTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => GetPointerTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => GetPointerTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		T[] values = GetPointerTest.fixture.CreateMany<T>(10).ToArray();
		Span<T> span = values;
		ReadOnlySpan<T> readOnlySpan = span;

		fixed (void* ptr = &MemoryMarshal.GetReference(readOnlySpan))
		{
			IntPtr intPtr = (IntPtr)ptr;
			UIntPtr uintPtr = (UIntPtr)ptr;

			PInvokeAssert.Equal(intPtr, span.GetUnsafeIntPtr());
			PInvokeAssert.Equal(uintPtr, span.GetUnsafeUIntPtr());
			PInvokeAssert.Equal((ValPtr<T>)intPtr, span.GetUnsafeValPtr());
			PInvokeAssert.Equal(intPtr, readOnlySpan.GetUnsafeIntPtr());
			PInvokeAssert.Equal(uintPtr, readOnlySpan.GetUnsafeUIntPtr());
			PInvokeAssert.Equal((ReadOnlyValPtr<T>)intPtr, readOnlySpan.GetUnsafeValPtr());
		}
	}
}