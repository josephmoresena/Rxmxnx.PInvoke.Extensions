using System;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.PointerExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsStringTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        internal void IntPtrZeroTest(Int32 length)
        {
            if (length >= 0)
                Assert.Null(IntPtr.Zero.AsString(length));
            else
                Assert.Throws<ArgumentException>(() => IntPtr.Zero.AsString(length));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        internal void UIntPtrZeroTest(Int32 length)
        {
            if (length >= 0)
                Assert.Null(UIntPtr.Zero.AsString(length));
            else
                Assert.Throws<ArgumentException>(() => UIntPtr.Zero.AsString(length));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void IntPtrTest(Boolean fixedLength)
        {
            String input = TestUtilities.SharedFixture.Create<String>();
            String result = default;
            unsafe
            {
                fixed (void* ptr = input)
                    result = new IntPtr(ptr).AsString(fixedLength ? input.Length : default);
            }
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void UIntPtrTest(Boolean fixedLength)
        {
            String input = TestUtilities.SharedFixture.Create<String>();
            String result = default;
            unsafe
            {
                fixed (void* ptr = input)
                    result = new UIntPtr(ptr).AsString(fixedLength ? input.Length : default);
            }
            Assert.Equal(input, result);
        }
    }
}
