using System;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace PInvoke.Extensions.Tests.PointerExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public class AsReadOnlySpanTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        internal void IntPtrZeroTest(Int32 length)
        {
            if (length >= 0)
            {
                ReadOnlySpan<Byte> result = IntPtr.Zero.AsReadOnlySpan<Byte>(length);
                Assert.True(result.IsEmpty);
            }
            else
                Assert.Throws<ArgumentException>(() => IntPtr.Zero.AsReadOnlySpan<Byte>(length));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        internal void UIntPtrZeroTest(Int32 length)
        {
            if (length >= 0)
            {
                ReadOnlySpan<Byte> result = UIntPtr.Zero.AsReadOnlySpan<Byte>(length);
                Assert.True(result.IsEmpty);
            }
            else
                Assert.Throws<ArgumentException>(() => UIntPtr.Zero.AsReadOnlySpan<Byte>(length));
        }

        [Fact]
        internal void IntPtrTest()
        {
            Byte[] input = TestUtilities.SharedFixture.Create<Byte[]>();
            Byte[] result = default;
            unsafe
            {
                fixed (void* ptr = input)
                    result = new IntPtr(ptr).AsReadOnlySpan<Byte>(input.Length).ToArray();
            }
            Assert.Equal(input, result);
        }

        [Fact]
        internal void UIntPtrTest()
        {
            Byte[] input = TestUtilities.SharedFixture.Create<Byte[]>();
            Byte[] result = default;
            unsafe
            {
                fixed (void* ptr = input)
                    result = new UIntPtr(ptr).AsReadOnlySpan<Byte>(input.Length).ToArray();
            }
            Assert.Equal(input, result);
        }
    }
}
