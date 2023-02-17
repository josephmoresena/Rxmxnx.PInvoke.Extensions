using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.UnmanagedValueExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsBytesTest
    {
        [Fact]
        internal void BooleanTest() => ValueTest<Boolean>();
        [Fact]
        internal void ByteTest() => ValueTest<Byte>();
        [Fact]
        internal void CharTest() => ValueTest<Char>();
        [Fact]
        internal void DateTimeTest() => ValueTest<DateTime>();
        [Fact]
        internal void DecimalTest() => ValueTest<Decimal>();
        [Fact]
        internal void DoubleTest() => ValueTest<Double>();
        [Fact]
        internal void GuidTest() => ValueTest<Guid>();
        [Fact]
        internal void HalfTest() => ValueTest<Half>();
        [Fact]
        internal void Int16Test() => ValueTest<Int16>();
        [Fact]
        internal void Int32Test() => ValueTest<Int32>();
        [Fact]
        internal void Int64Test() => ValueTest<Int64>();
        [Fact]
        internal void IntPtrTest() => ValueTest<IntPtr>();
        [Fact]
        internal void SByteTest() => ValueTest<SByte>();
        [Fact]
        internal void SingleTest() => ValueTest<Single>();
        [Fact]
        internal void UInt16Test() => ValueTest<UInt16>();
        [Fact]
        internal void UInt32Test() => ValueTest<UInt32>();
        [Fact]
        internal void UInt64Test() => ValueTest<UInt64>();
        [Fact]
        internal void UIntPtrTest() => ValueTest<UIntPtr>();

        private static void ValueTest<T>()
            where T : unmanaged
        {
            T value = TestUtilities.SharedFixture.Create<T>();
            Byte[] valueAsByte = value.AsBytes();
            unsafe
            {
                Assert.Equal(sizeof(T), valueAsByte.Length);
                fixed (void* ptr = valueAsByte)
                    Assert.Equal(value, Unsafe.AsRef<T>(ptr));
            }
        }
    }
}
