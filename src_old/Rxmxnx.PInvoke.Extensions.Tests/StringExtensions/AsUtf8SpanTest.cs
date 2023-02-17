using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.StringExtensions
{
    [ExcludeFromCodeCoverage]
    public sealed class AsUtf8SpanTest
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void EmptyTest(Boolean emptyString)
        {
            String str = emptyString ? String.Empty : default;
            ReadOnlySpan<Byte> result = str.AsUtf8Span();
            Assert.True(result.IsEmpty);
            Assert.Equal(IntPtr.Zero, GetIntPtr(result));
            Assert.Equal(0, result.Length);
        }

        [Fact]
        internal void NormalTest()
        {
            String str = TestUtilities.SharedFixture.Create<String>();
            Byte[] utfEncode = Encoding.UTF8.GetBytes(str);
            ReadOnlySpan<Byte> result = str.AsUtf8Span();
            Assert.False(result.IsEmpty);
            Assert.Equal(utfEncode.Length, result.Length);
            Assert.NotEqual(IntPtr.Zero, GetIntPtr(result));
            Assert.NotEqual(GetIntPtr(utfEncode), GetIntPtr(result));
            for (Int32 i = 0; i < result.Length; i++)
                Assert.Equal(utfEncode[i], result[i]);
        }

        private static IntPtr GetIntPtr(Byte[] array)
            => GetIntPtr(array.AsSpan());

        private static IntPtr GetIntPtr(ReadOnlySpan<Byte> span)
        {
            unsafe
            {
                return new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)));
            }
        }
    }
}
