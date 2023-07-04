namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetPointerTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void ByteTest() => GetPointerTest.Test<Byte>();
	[Fact]
	internal void CharTest() => GetPointerTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => GetPointerTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => GetPointerTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => GetPointerTest.Test<Double>();
	[Fact]
	internal void GuidTest() => GetPointerTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => GetPointerTest.Test<Half>();
	[Fact]
	internal void Int16Test() => GetPointerTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => GetPointerTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => GetPointerTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => GetPointerTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => GetPointerTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => GetPointerTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => GetPointerTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => GetPointerTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		T[] values = GetPointerTest.fixture.CreateMany<T>(10).ToArray();
		Span<T> span = values;
		ReadOnlySpan<T> readOnlySpan = span;

		fixed (void* ptr = &MemoryMarshal.GetReference(readOnlySpan))
		{
			IntPtr intPtr = (IntPtr)ptr;
			UIntPtr uintPtr = (UIntPtr)ptr;

			Assert.Equal(intPtr, span.GetUnsafeIntPtr());
			Assert.Equal(uintPtr, span.GetUnsafeUIntPtr());
			Assert.Equal(intPtr, readOnlySpan.GetUnsafeIntPtr());
			Assert.Equal(uintPtr, readOnlySpan.GetUnsafeUIntPtr());
		}
	}
}