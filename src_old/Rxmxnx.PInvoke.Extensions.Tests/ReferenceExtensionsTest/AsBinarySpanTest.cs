using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.ReferenceExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsBinarySpanTest
    {
        [Fact]
        internal void BooleanTest() => NormalTest<Boolean>();
        [Fact]
        internal void ByteTest() => NormalTest<Byte>();
        [Fact]
        internal void ShortTest() => NormalTest<Int16>();
        [Fact]
        internal void CharTest() => NormalTest<Char>();
        [Fact]
        internal void IntegerTest() => NormalTest<Int32>();
        [Fact]
        internal void LongTest() => NormalTest<Int64>();
        [Fact]
        internal void DecimalTest() => NormalTest<Decimal>();

        private static void NormalTest<TValue>() where TValue : unmanaged, IConvertible
        {
            unsafe
            {
                Byte[] bytes = new Byte[sizeof(TValue)];
                TValue value = default;

                ref TValue refValue = ref value;
                Random.Shared.NextBytes(bytes);

                Span<Byte> span = refValue.AsBinarySpan();
                bytes.CopyTo(span);

                fixed (void* ptr = bytes)
                    Assert.Equal(new ReadOnlySpan<TValue>(ptr, 1)[0], value);
            }
        }
    }
}
