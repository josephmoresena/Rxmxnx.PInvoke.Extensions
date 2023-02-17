using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.NativeUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class BinaryCopyToTest
    {
        [Fact]
        internal void ExceptionTest()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Span<Byte> bytes = stackalloc Byte[2];
                NativeUtilities.BinaryCopyTo(TestUtilities.SharedFixture.Create<Decimal>(), bytes);
            });
        }

        [Fact]
        internal void NormalTest()
        {
            CopyTest<Boolean>();
            CopyTest<Byte>();
            CopyTest<Int16>();
            CopyTest<Int32>();
            CopyTest<Int64>();
            CopyTest<Single>();
            CopyTest<Double>();
            CopyTest<Decimal>();
        }

        private static void CopyTest<T>() where T : unmanaged
        {
            T[] values = TestUtilities.SharedFixture.CreateMany<T>().ToArray();
            unsafe
            {
                Span<Byte> span = stackalloc Byte[values.Length * sizeof(T)];
                for (Int32 i = 0; i < values.Length; i++)
                    NativeUtilities.BinaryCopyTo(values[i], span, i * sizeof(T));

                span.BinaryTransform<T, T[]>(values, (s, a, r) =>
                {
                    Assert.Equal(a, s.ToArray());
                    Assert.Equal(0, r.Length);
                });
            }
        }
    }
}
