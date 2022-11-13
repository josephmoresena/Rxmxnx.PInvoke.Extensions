using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.FixedContextTest
{
    [ExcludeFromCodeCoverage]
    public sealed class SpanTest
    {
        private static readonly Int32[] primes = new[]
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29,
            31, 37, 41, 43, 47, 53, 59, 61, 67,
            71, 73, 79, 83, 89, 97
        };

        [Fact]
        internal void ByteTest() => GenericTest<Byte>();
        [Fact]
        internal void Int16Test() => GenericTest<Int16>();
        [Fact]
        internal void Int32Test() => GenericTest<Int32>();
        [Fact]
        internal void Int64Test() => GenericTest<Int64>();
        [Fact]
        internal void GuidTest() => GenericTest<Guid>();

        private static void GenericTest<T>() where T : unmanaged
        {
            ContextTest<T, Byte>();
            ContextTest<T, Int16>();
            ContextTest<T, Int32>();
            ContextTest<T, Int64>();
            ContextTest<T, Guid>();
        }
        private static void ContextTest<TSource, TDestination>() where TSource : unmanaged where TDestination : unmanaged
        {
            TSource[] sources = new TSource[primes[Random.Shared.Next(0, primes.Length)]];
            Span<TSource> span = sources;
            span.WithSafeFixed(FixedTest);
            span.WithSafeFixed((in IFixedContext<TSource> src_ctx) =>
            {
                Int32 count = primes[Random.Shared.Next(0, primes.Length)];
                foreach (ref TSource source in src_ctx.Values)
                    source = TestUtilities.SharedFixture.Create<TSource>();
                Span<TDestination> destinations = stackalloc TDestination[count];
                destinations.WithSafeFixed(src_ctx, TransformationTest);

                ReadOnlySpan<TSource> read_sources = sources;
                ReadOnlySpan<TDestination> read_destinations = destinations;

                read_sources.WithSafeFixed(src_ctx, (in IReadOnlyFixedContext<TSource> rsrc_ctx, IFixedContext<TSource> src_ctx) =>
                {
                    Assert.False(src_ctx.Equals(rsrc_ctx));
                    Assert.Equal(src_ctx.Values.AsIntPtr(), rsrc_ctx.Values.AsIntPtr());
                    Assert.Equal(src_ctx.Values.Length, rsrc_ctx.Values.Length);
                    var tmp_ctx = rsrc_ctx as IFixedContext<TSource>;
                    Assert.NotNull(tmp_ctx);
                    Assert.Throws<InvalidOperationException>(() => tmp_ctx.Values[0]);
                });
            });
        }
        private static unsafe void TransformationTest<TSource, TDestination>(in IFixedContext<TDestination> des_ctx, IFixedContext<TSource> src_ctx)
            where TSource : unmanaged where TDestination : unmanaged
        {
            Assert.False(src_ctx.Equals(des_ctx));

            ITransformationContext<TSource, TDestination> tsrc_ctx = src_ctx.Transformation<TDestination>();
            Span<TDestination> tsrc_values = tsrc_ctx.Values;
            Span<Byte> tsrc_bytes = tsrc_ctx.ResidualBytes;
            Span<TDestination> des_bytes = des_ctx.Values;
            IReadOnlyTransformationContext<TDestination, TSource> tdes_ctx = des_ctx.Transformation<TSource>().AsReadOnly();

            Span<TSource> src_bytes = src_ctx.Values;
            ReadOnlySpan<TSource> tdes_values = tdes_ctx.Values;
            ReadOnlySpan<Byte> tdes_bytes = tdes_ctx.ResidualBytes;

            Assert.Equal(tsrc_values.Length * sizeof(TDestination) + tsrc_bytes.Length, src_ctx.BinaryValues.Length);
            Assert.Equal(tdes_values.Length * sizeof(TSource) + tdes_bytes.Length, des_ctx.BinaryValues.Length);
            Assert.Equal(tsrc_ctx.Context, src_ctx);
            Assert.True(Object.ReferenceEquals(tsrc_ctx.Context, src_ctx));
            Assert.True(tdes_ctx.Context.Equals(des_ctx));
            Assert.True(Object.ReferenceEquals(tdes_ctx.Context, des_ctx));
            Assert.Equal(tdes_ctx, des_ctx.AsReadOnly().Transformation<TSource>());

            if (tsrc_values.Length == 0)
                Assert.Equal(src_ctx.BinaryValues.AsIntPtr(), tsrc_bytes.AsIntPtr());
            else if (tsrc_bytes.Length > 0)
                Assert.Equal(tsrc_values.AsIntPtr() + tsrc_values.Length * sizeof(TDestination), tsrc_bytes.AsIntPtr());

            if (tdes_values.Length == 0)
                Assert.Equal(des_ctx.BinaryValues.AsIntPtr(), tdes_bytes.AsIntPtr());
            else if (tdes_bytes.Length > 0)
                Assert.Equal(tdes_values.AsIntPtr() + tdes_values.Length * sizeof(TSource), tdes_bytes.AsIntPtr());

            Int32 min_des = Math.Min(tsrc_values.Length, des_bytes.Length);
            Int32 min_src_transform = Math.Min(src_bytes.Length, tdes_values.Length);
            Int32 min_src = Math.Min(min_src_transform, min_des * sizeof(TDestination) / sizeof(TSource));

            if (min_des != 0)
                for (Int32 i = 0; i < min_des; i++)
                    des_bytes[i] = tsrc_values[i];
            else
            {
                Span<Byte> s_bytes = src_ctx.BinaryValues;
                Span<Byte> bytes = des_ctx.BinaryValues;
                Int32 min_byt = Math.Min(s_bytes.Length, bytes.Length);

                for (Int32 i = 0; i < min_byt; i++)
                    bytes[i] = s_bytes[i];
            }

            for (Int32 i = 0; i < min_src; i++)
            {
                TSource o_value = src_bytes[i];
                TSource d_value = tdes_values[i];
                Assert.Equal(o_value, d_value);
            }
        }
        private static void FixedTest<T>(in IFixedContext<T> ctx) where T : unmanaged
        {
            ctx.Values.WithSafeFixed(ctx, FixedTestAction);
            IFixedContext<T> ctxCopy = ctx;
            IFixedContext<T> ctx2 = ctx.Values.WithSafeFixed((in IFixedContext<T> ctx2) => FixedTestFunc(ctx2, ctxCopy));
            IFixedContext<T> ctx3 = ctx.Values.WithSafeFixed(ctx, FixedTestFunc);
            Assert.Equal(ctx, ctx2);
            Assert.Equal(ctx, ctx3);
            Assert.False(Object.ReferenceEquals(ctx, ctx2));
            Assert.False(Object.ReferenceEquals(ctx, ctx3));
            Assert.False(Object.ReferenceEquals(ctx2, ctx3));
            Assert.Throws<InvalidOperationException>(() => ctx2.Values[0]);
            Assert.Throws<InvalidOperationException>(() => ctx2.BinaryValues[0]);
            Assert.Throws<InvalidOperationException>(() => ctx3.Values[0]);
            Assert.Throws<InvalidOperationException>(() => ctx3.BinaryValues[0]);

            ctx.Values[..^1].WithSafeFixed(ctx, FixedReferenceTest1);
        }
        private static void FixedTestAction<T>(in IFixedContext<T> ctx, IFixedContext<T> state) where T : unmanaged
        {
            Assert.Equal(state, ctx);
            Assert.False(Object.ReferenceEquals(state, ctx));
            Assert.True(Unsafe.AreSame(ref state.Values[0], ref ctx.Values[0]));
            Assert.Equal(state.BinaryValues.AsIntPtr(), ctx.BinaryValues.AsIntPtr());
            Assert.Equal(state.GetHashCode(), ctx.GetHashCode());

            state.Values[0] = TestUtilities.SharedFixture.Create<T>();
            ctx.Values[^1] = TestUtilities.SharedFixture.Create<T>();

            IReadOnlyFixedContext<T> roctx = state.AsReadOnly();
            IReadOnlyFixedContext<T> roctx2 = ctx.AsReadOnly();

            Assert.Equal(roctx.Values[0], roctx2.Values[0]);
            Assert.Equal(roctx.Values[^1], roctx2.Values[^1]);
            Assert.Equal(roctx.BinaryValues[0], roctx2.BinaryValues[0]);
            Assert.Equal(roctx.BinaryValues[^1], roctx2.BinaryValues[^1]);

            Assert.Equal(state.Values.AsIntPtr(), roctx2.BinaryValues.AsIntPtr());
            Assert.False(roctx2.Equals(state.Values[0]));
        }
        private static IFixedContext<T> FixedTestFunc<T>(in IFixedContext<T> ctx2, IFixedContext<T> ctx) where T : unmanaged
        {
            FixedTestAction(ctx2, ctx);
            return ctx2;
        }
        private static void FixedReferenceTest1<T>(in IFixedContext<T> ctx1, IFixedContext<T> ctx) where T : unmanaged
        {
            Span<T> values2 = ctx.Values[1..];
            Assert.Equal(ctx.Values.AsIntPtr(), ctx1.Values.AsIntPtr());
            Assert.NotEqual(ctx, ctx1);
            values2.WithSafeFixed(ctx, FixedReferenceTest2);
        }
        private static void FixedReferenceTest2<T>(in IFixedContext<T> ctx2, IFixedContext<T> ctx) where T : unmanaged
        {
            Assert.Equal(ctx.Values[^1].AsIntPtr(), ctx2.Values[^1].AsIntPtr());
            Assert.NotEqual(ctx, ctx2);
        }
    }
}
