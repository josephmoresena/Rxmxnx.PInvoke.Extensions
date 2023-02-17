using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.MemoryBlockExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsPointerTest
    {
        [Fact]
        internal void IntPtrSpanEmptyTest()
        {
            Byte[] bytes = default;
            Span<Byte> sBytes = bytes.AsSpan();
            Assert.Equal(IntPtr.Zero, sBytes.AsIntPtr());
        }

        [Fact]
        internal void UIntPtrSpanEmptyTest()
        {
            Byte[] bytes = default;
            Span<Byte> sBytes = bytes.AsSpan();
            Assert.Equal(UIntPtr.Zero, sBytes.AsUIntPtr());
        }


        [Fact]
        internal void IntPtrReadOnlySpanEmptyTest()
        {
            String text = default;
            ReadOnlySpan<Char> sChar = text.AsSpan();
            Assert.Equal(IntPtr.Zero, sChar.AsIntPtr());
        }

        [Fact]
        internal void UIntPtrReadOnlySpanEmptyTest()
        {
            String text = default;
            ReadOnlySpan<Char> sChar = text.AsSpan();
            Assert.Equal(UIntPtr.Zero, sChar.AsUIntPtr());
        }

        [Fact]
        internal void IntPtrSpanNormalTest()
        {
            Byte[] bytes = TestUtilities.SharedFixture.Create<Byte[]>();
            Span<Byte> sBytes = bytes.AsSpan();
            IntPtr result = sBytes.AsIntPtr();
            Assert.NotEqual(IntPtr.Zero, result);
            for (Int32 i = 0; i < bytes.Length; i++)
                Assert.Equal(bytes[i], Marshal.ReadByte(result + i));
        }

        [Fact]
        internal void UIntPtrSpanNormalTest()
        {
            Byte[] bytes = TestUtilities.SharedFixture.Create<Byte[]>();
            Span<Byte> sBytes = bytes.AsSpan();
            UIntPtr result = sBytes.AsUIntPtr();
            Assert.NotEqual(UIntPtr.Zero, result);
            unsafe
            {
                for (Int32 i = 0; i < bytes.Length; i++)
                    Assert.Equal(bytes[i], Marshal.ReadByte(new IntPtr(result.ToPointer()) + i));
            }
        }

        [Fact]
        internal void IntPtrReadOnlySpanNormalTest()
        {
            String text = TestUtilities.SharedFixture.Create<String>();
            ReadOnlySpan<Char> sChar = text.AsSpan();
            IntPtr result = sChar.AsIntPtr();
            Assert.NotEqual(IntPtr.Zero, result);
            for (Int32 i = 0; i < text.Length; i++)
                Assert.Equal((Int16)text[i], Marshal.ReadInt16(result + (2 * i)));
        }

        [Fact]
        internal void UIntPtrReadOnlySpanNormalTest()
        {
            String text = TestUtilities.SharedFixture.Create<String>();
            ReadOnlySpan<Char> sChar = text.AsSpan();
            UIntPtr result = sChar.AsUIntPtr();
            Assert.NotEqual(UIntPtr.Zero, result);
            unsafe
            {
                for (Int32 i = 0; i < text.Length; i++)
                    Assert.Equal((Int16)text[i], Marshal.ReadInt16(new IntPtr(result.ToPointer()) + (2 * i)));
            }
        }
    }
}
