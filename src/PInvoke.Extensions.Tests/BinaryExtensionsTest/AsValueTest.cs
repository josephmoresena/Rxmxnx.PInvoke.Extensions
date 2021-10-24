using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace PInvoke.Extensions.Tests.BinaryExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsValueTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void BooleanTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Boolean>();
            else
                ExceptionTest<Boolean>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void ByteTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Byte>();
            else
                ExceptionTest<Byte>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void CharTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Char>();
            else
                ExceptionTest<Char>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void DateTimeTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<DateTime>();
            else
                ExceptionTest<DateTime>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void DecimalTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Decimal>();
            else
                ExceptionTest<Decimal>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void DoubleTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Double>();
            else
                ExceptionTest<Double>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void GuidTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Guid>();
            else
                ExceptionTest<Guid>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void HalfTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Half>();
            else
                ExceptionTest<Half>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void Int16Test(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Int16>();
            else
                ExceptionTest<Int16>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void Int32Test(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Int32>();
            else
                ExceptionTest<Int32>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void Int64Test(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Int64>();
            else
                ExceptionTest<Int64>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void IntPtrTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<IntPtr>();
            else
                ExceptionTest<IntPtr>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void SByteTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<SByte>();
            else
                ExceptionTest<SByte>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void SingleTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<Single>();
            else
                ExceptionTest<Single>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void UInt16Test(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<UInt16>();
            else
                ExceptionTest<UInt16>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void UInt32Test(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<UInt32>();
            else
                ExceptionTest<UInt32>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void UInt64Test(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<UInt64>();
            else
                ExceptionTest<UInt64>();
        }
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void UIntPtrTest(Boolean exceptionTest)
        {
            if (!exceptionTest)
                NormalTest<UIntPtr>();
            else
                ExceptionTest<UIntPtr>();
        }

        private static void ExceptionTest<T>()
            where T : unmanaged
        {
            unsafe
            {
                Byte[] bytes = TestUtilities.SharedFixture.CreateMany<Byte>(sizeof(T) + 1).ToArray();
                Assert.Throws<ArgumentException>(() => bytes.AsValue<T>());
            }
        }

        private static void NormalTest<T>()
            where T : unmanaged
        {
            T value = TestUtilities.SharedFixture.Create<T>();
            unsafe
            {
                ReadOnlySpan<Byte> readOnlySpan = new(Unsafe.AsPointer(ref value), sizeof(T));
                Assert.Equal(value, readOnlySpan.ToArray().AsValue<T>());
            }
        }
    }
}
