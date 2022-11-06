using System;
using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.MemoryBlockExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class TransformTest
    {
        [Fact]
        internal void NormalTest()
        {
            NormalTestFullTransform<Byte, Byte>();
            NormalTestFullTransform<Byte, Int16>();
            NormalTestFullTransform<Byte, Int32>();
            NormalTestFullTransform<Byte, Int64>();
            NormalTestFullTransform<Byte, Guid>();

            NormalTestFullTransform<Int16, Byte>();
            NormalTestFullTransform<Int16, Int16>();
            NormalTestFullTransform<Int16, Int32>();
            NormalTestFullTransform<Int16, Int64>();
            NormalTestFullTransform<Int16, Guid>();

            NormalTestFullTransform<Int32, Byte>();
            NormalTestFullTransform<Int32, Int16>();
            NormalTestFullTransform<Int32, Int32>();
            NormalTestFullTransform<Int32, Int64>();
            NormalTestFullTransform<Int32, Guid>();

            NormalTestFullTransform<Int64, Byte>();
            NormalTestFullTransform<Int64, Int16>();
            NormalTestFullTransform<Int64, Int32>();
            NormalTestFullTransform<Int64, Int64>();
            NormalTestFullTransform<Int64, Guid>();

            NormalTestFullTransform<Guid, Byte>();
            NormalTestFullTransform<Guid, Int16>();
            NormalTestFullTransform<Guid, Int32>();
            NormalTestFullTransform<Guid, Int64>();
            NormalTestFullTransform<Guid, Guid>();
        }

        private static void NormalTestFullTransform<TSource, TDestination>()
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            Span<TSource> span = stackalloc TSource[Random.Shared.Next(0, 129)];
            ReadOnlySpan<TSource> readonlySpan = span;
            TransformState<TDestination> stateAction = new();
            TransformState<TDestination> stateFunction = new();

            span.Transform<TSource, TDestination, TransformState<TDestination>>(
                stateAction, TransformState<TDestination>.TransformAction<TSource>);
            readonlySpan.Transform<TSource, TDestination, TransformState<TDestination>>(
                stateAction, TransformState<TDestination>.TransformAction<TSource>);

            Assert.Equal(span.Length,
                span.Transform<TSource, TDestination, TransformState<TDestination>, Int32>(
                stateFunction, TransformState<TDestination>.TransformFunction<TSource>));
            Assert.Equal(readonlySpan.Length,
                readonlySpan.Transform<TSource, TDestination, TransformState<TDestination>, Int32>(
                stateFunction, TransformState<TDestination>.TransformFunction<TSource>));
        }
    }
}