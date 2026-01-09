namespace Rxmxnx.PInvoke.Tests.ReferenceExtensionsTests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class GetUnsafePointerTest
{
	private static readonly IFixture fixture = new Fixture();

	[Fact]
	public void BooleanTest() => GetUnsafePointerTest.Test<Boolean>();
	[Fact]
	public void ByteTest() => GetUnsafePointerTest.Test<Byte>();
	[Fact]
	public void CharTest() => GetUnsafePointerTest.Test<Char>();
	[Fact]
	public void DateTimeTest() => GetUnsafePointerTest.Test<DateTime>();
	[Fact]
	public void DecimalTest() => GetUnsafePointerTest.Test<Decimal>();
	[Fact]
	public void DoubleTest() => GetUnsafePointerTest.Test<Double>();
	[Fact]
	public void GuidTest() => GetUnsafePointerTest.Test<Guid>();
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => GetUnsafePointerTest.Test<Half>();
#endif
	[Fact]
	public void Int16Test() => GetUnsafePointerTest.Test<Int16>();
	[Fact]
	public void Int32Test() => GetUnsafePointerTest.Test<Int32>();
	[Fact]
	public void Int64Test() => GetUnsafePointerTest.Test<Int64>();
	[Fact]
	public void SByteTest() => GetUnsafePointerTest.Test<SByte>();
	[Fact]
	public void SingleTest() => GetUnsafePointerTest.Test<Single>();
	[Fact]
	public void UInt16Test() => GetUnsafePointerTest.Test<UInt16>();
	[Fact]
	public void UInt32Test() => GetUnsafePointerTest.Test<UInt32>();
	[Fact]
	public void UInt64Test() => GetUnsafePointerTest.Test<UInt64>();

	private static unsafe void Test<T>() where T : unmanaged
	{
		T value = GetUnsafePointerTest.fixture.Create<T>();
		ref T refValue = ref value;
		fixed (void* ptr = &refValue)
		{
			PInvokeAssert.Equal((IntPtr)ptr, refValue.GetUnsafeIntPtr());
			PInvokeAssert.Equal((UIntPtr)ptr, refValue.GetUnsafeUIntPtr());
			PInvokeAssert.Equal((ValPtr<T>)(IntPtr)ptr, refValue.GetUnsafeValPtr());
		}
	}
}