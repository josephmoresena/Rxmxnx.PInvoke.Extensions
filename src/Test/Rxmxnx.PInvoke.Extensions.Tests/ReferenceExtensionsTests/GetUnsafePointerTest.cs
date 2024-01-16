namespace Rxmxnx.PInvoke.Tests.ReferenceExtensionsTests;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafePointerTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	internal void BooleanTest() => GetUnsafePointerTest.Test<Boolean>();
	[Fact]
	internal void ByteTest() => GetUnsafePointerTest.Test<Byte>();
	[Fact]
	internal void CharTest() => GetUnsafePointerTest.Test<Char>();
	[Fact]
	internal void DateTimeTest() => GetUnsafePointerTest.Test<DateTime>();
	[Fact]
	internal void DecimalTest() => GetUnsafePointerTest.Test<Decimal>();
	[Fact]
	internal void DoubleTest() => GetUnsafePointerTest.Test<Double>();
	[Fact]
	internal void GuidTest() => GetUnsafePointerTest.Test<Guid>();
	[Fact]
	internal void HalfTest() => GetUnsafePointerTest.Test<Half>();
	[Fact]
	internal void Int16Test() => GetUnsafePointerTest.Test<Int16>();
	[Fact]
	internal void Int32Test() => GetUnsafePointerTest.Test<Int32>();
	[Fact]
	internal void Int64Test() => GetUnsafePointerTest.Test<Int64>();
	[Fact]
	internal void SByteTest() => GetUnsafePointerTest.Test<SByte>();
	[Fact]
	internal void SingleTest() => GetUnsafePointerTest.Test<Single>();
	[Fact]
	internal void UInt16Test() => GetUnsafePointerTest.Test<UInt16>();
	[Fact]
	internal void UInt32Test() => GetUnsafePointerTest.Test<UInt32>();
	[Fact]
	internal void UInt64Test() => GetUnsafePointerTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		T value = GetUnsafePointerTest.fixture.Create<T>();
		ref T refValue = ref value;
		fixed (void* ptr = &refValue)
		{
			Assert.Equal((IntPtr)ptr, refValue.GetUnsafeIntPtr());
			Assert.Equal((UIntPtr)ptr, refValue.GetUnsafeUIntPtr());
			Assert.Equal((ValPtr<T>)(IntPtr)ptr, refValue.GetUnsafeValPtr());
		}
	}
}