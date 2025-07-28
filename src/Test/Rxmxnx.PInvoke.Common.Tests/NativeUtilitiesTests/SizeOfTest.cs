#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class SizeOfTest
{
	[Fact]
	public void BooleanTest() => PInvokeAssert.Equal(1, NativeUtilities.SizeOf<Boolean>());
	[Fact]
	public void ByteTest() => PInvokeAssert.Equal(1, NativeUtilities.SizeOf<Byte>());
	[Fact]
	public void CharTest() => PInvokeAssert.Equal(2, NativeUtilities.SizeOf<Char>());
	[Fact]
	public void DateTimeTest() => PInvokeAssert.Equal(8, NativeUtilities.SizeOf<DateTime>());
	[Fact]
	public void DecimalTest() => PInvokeAssert.Equal(16, NativeUtilities.SizeOf<Decimal>());
	[Fact]
	public void DoubleTest() => PInvokeAssert.Equal(8, NativeUtilities.SizeOf<Double>());
	[Fact]
	public void GuidTest() => PInvokeAssert.Equal(16, NativeUtilities.SizeOf<Guid>());
#if NET5_0_OR_GREATER
	[Fact]
	internal void HalfTest() => Assert.Equal(2, NativeUtilities.SizeOf<Half>());
#endif
	[Fact]
	public void Int16Test() => PInvokeAssert.Equal(2, NativeUtilities.SizeOf<Int16>());
	[Fact]
	public void Int32Test() => PInvokeAssert.Equal(4, NativeUtilities.SizeOf<Int32>());
	[Fact]
	public void Int64Test() => PInvokeAssert.Equal(8, NativeUtilities.SizeOf<Int64>());
	[Fact]
	public void IntPtrTest()
	{
		PInvokeAssert.Equal(Environment.Is64BitProcess ? 8 : 4, NativeUtilities.SizeOf<IntPtr>());
		PInvokeAssert.Equal(NativeUtilities.SizeOf<IntPtr>(), NativeUtilities.PointerSize);
	}
	[Fact]
	public void SByteTest() => PInvokeAssert.Equal(1, NativeUtilities.SizeOf<SByte>());
	[Fact]
	public void SingleTest() => PInvokeAssert.Equal(4, NativeUtilities.SizeOf<Single>());
	[Fact]
	public void UInt16Test() => PInvokeAssert.Equal(2, NativeUtilities.SizeOf<UInt16>());
	[Fact]
	public void UInt32Test() => PInvokeAssert.Equal(4, NativeUtilities.SizeOf<UInt32>());
	[Fact]
	public void UInt64Test() => PInvokeAssert.Equal(8, NativeUtilities.SizeOf<UInt64>());
	[Fact]
	public void UIntPtrTest()
		=> PInvokeAssert.Equal(Environment.Is64BitProcess ? 8 : 4, NativeUtilities.SizeOf<UIntPtr>());
}