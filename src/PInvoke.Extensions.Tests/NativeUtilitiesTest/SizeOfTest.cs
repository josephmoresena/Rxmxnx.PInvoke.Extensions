using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace PInvoke.Extensions.Tests.NativeUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public class SizeOfTest
    {
        [Fact]
        internal void BooleanTest() => Assert.Equal(1, NativeUtilities.SizeOf<Boolean>());
        [Fact]
        internal void ByteTest() => Assert.Equal(1, NativeUtilities.SizeOf<Byte>());
        [Fact]
        internal void CharTest() => Assert.Equal(2, NativeUtilities.SizeOf<Char>());
        [Fact]
        internal void DateTimeTest() => Assert.Equal(8, NativeUtilities.SizeOf<DateTime>());
        [Fact]
        internal void DecimalTest() => Assert.Equal(16, NativeUtilities.SizeOf<Decimal>());
        [Fact]
        internal void DoubleTest() => Assert.Equal(8, NativeUtilities.SizeOf<Double>());
        [Fact]
        internal void GuidTest() => Assert.Equal(16, NativeUtilities.SizeOf<Guid>());
        [Fact]
        internal void HalfTest() => Assert.Equal(2, NativeUtilities.SizeOf<Half>());
        [Fact]
        internal void Int16Test() => Assert.Equal(2, NativeUtilities.SizeOf<Int16>());
        [Fact]
        internal void Int32Test() => Assert.Equal(4, NativeUtilities.SizeOf<Int32>());
        [Fact]
        internal void Int64Test() => Assert.Equal(8, NativeUtilities.SizeOf<Int64>());
        [Fact]
        internal void IntPtrTest() => Assert.Equal(Environment.Is64BitProcess ? 8 : 4, NativeUtilities.SizeOf<IntPtr>());
        [Fact]
        internal void SByteTest() => Assert.Equal(1, NativeUtilities.SizeOf<SByte>());
        [Fact]
        internal void SingleTest() => Assert.Equal(4, NativeUtilities.SizeOf<Single>());
        [Fact]
        internal void UInt16Test() => Assert.Equal(2, NativeUtilities.SizeOf<UInt16>());
        [Fact]
        internal void UInt32Test() => Assert.Equal(4, NativeUtilities.SizeOf<UInt32>());
        [Fact]
        internal void UInt64Test() => Assert.Equal(8, NativeUtilities.SizeOf<UInt64>());
        [Fact]
        internal void UIntPtrTest() => Assert.Equal(Environment.Is64BitProcess ? 8 : 4, NativeUtilities.SizeOf<UIntPtr>());
    }
}
