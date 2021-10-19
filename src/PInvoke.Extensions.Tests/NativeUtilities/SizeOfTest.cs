using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

using Utilities = PInvoke.Extensions.NativeUtilities;

namespace PInvoke.Extensions.Tests.NativeUtilities
{
    [ExcludeFromCodeCoverage]
    public class SizeOfTest
    {
        [Fact]
        internal void BooleanTest() => Assert.Equal(1, Utilities.SizeOf<Boolean>());
        [Fact]
        internal void ByteTest() => Assert.Equal(1, Utilities.SizeOf<Byte>());
        [Fact]
        internal void CharTest() => Assert.Equal(2, Utilities.SizeOf<Char>());
        [Fact]
        internal void DateTimeTest() => Assert.Equal(8, Utilities.SizeOf<DateTime>());
        [Fact]
        internal void DecimalTest() => Assert.Equal(16, Utilities.SizeOf<Decimal>());
        [Fact]
        internal void DoubleTest() => Assert.Equal(8, Utilities.SizeOf<Double>());
        [Fact]
        internal void GuidTest() => Assert.Equal(16, Utilities.SizeOf<Guid>());
        [Fact]
        internal void HalfTest() => Assert.Equal(2, Utilities.SizeOf<Half>());
        [Fact]
        internal void Int16Test() => Assert.Equal(2, Utilities.SizeOf<Int16>());
        [Fact]
        internal void Int32Test() => Assert.Equal(4, Utilities.SizeOf<Int32>());
        [Fact]
        internal void Int64Test() => Assert.Equal(8, Utilities.SizeOf<Int64>());
        [Fact]
        internal void IntPtrTest() => Assert.Equal(Environment.Is64BitProcess ? 8 : 4, Utilities.SizeOf<IntPtr>());
        [Fact]
        internal void SByteTest() => Assert.Equal(1, Utilities.SizeOf<SByte>());
        [Fact]
        internal void SingleTest() => Assert.Equal(4, Utilities.SizeOf<Single>());
        [Fact]
        internal void UInt16Test() => Assert.Equal(2, Utilities.SizeOf<UInt16>());
        [Fact]
        internal void UInt32Test() => Assert.Equal(4, Utilities.SizeOf<UInt32>());
        [Fact]
        internal void UInt64Test() => Assert.Equal(8, Utilities.SizeOf<UInt64>());
        [Fact]
        internal void UIntPtrTest() => Assert.Equal(Environment.Is64BitProcess ? 8 : 4, Utilities.SizeOf<UIntPtr>());
    }
}
