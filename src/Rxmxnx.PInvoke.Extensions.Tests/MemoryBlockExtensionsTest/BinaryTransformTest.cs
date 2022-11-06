using System;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.MemoryBlockExtensionsTest
{
    public sealed class BinaryTransformTest
    {
        [Fact]
        internal void NormalTest()
        {
            //NormalBinarySourceTest<Byte>();
            //NormalBinarySourceTest<Int16>();
            //NormalBinarySourceTest<Int32>();
            //NormalBinarySourceTest<Int64>();
            //NormalBinarySourceTest<Guid>();

            NormalBinaryDestination<Int16>();
            NormalBinaryDestination<Int32>();
            NormalBinaryDestination<Int64>();
            NormalBinaryDestination<Guid>();
        }

        private static void NormalBinarySourceTest<TDestination>() where TDestination : unmanaged
        {
            Span<Byte> span = stackalloc Byte[Random.Shared.Next(0, 129)];
            ReadOnlySpan<Byte> readonlySpan = span;
            TransformState<TDestination> stateAction = new();
            TransformState<TDestination> stateFunction = new();

            span.BinaryTransform<TDestination, TransformState<TDestination>>(
                stateAction, TransformState<TDestination>.TransformAction<Byte>);
            readonlySpan.BinaryTransform<TDestination, TransformState<TDestination>>(
                stateAction, TransformState<TDestination>.TransformAction<Byte>);

            Assert.Equal(span.Length,
                span.BinaryTransform<TDestination, TransformState<TDestination>, Int32>(
                stateFunction, TransformState<TDestination>.TransformFunction<Byte>));
            Assert.Equal(readonlySpan.Length,
                readonlySpan.BinaryTransform<TDestination, TransformState<TDestination>, Int32>(
                stateFunction, TransformState<TDestination>.TransformFunction<Byte>));
        }
        private static void NormalBinaryDestination<TSource>() where TSource : unmanaged
        {
            Span<TSource> span = stackalloc TSource[Random.Shared.Next(0, 129)];
            ReadOnlySpan<TSource> readonlySpan = span;
            BinaryTransformState stateAction = new();
            BinaryTransformState stateFunction = new();

            span.BinaryTransform(stateAction, BinaryTransformState.TransformAction);
            readonlySpan.BinaryTransform(stateAction, BinaryTransformState.TransformAction);

            Assert.Equal(span.Length, span.BinaryTransform(stateFunction, BinaryTransformState.TransformFunction<TSource>));
            Assert.Equal(readonlySpan.Length, readonlySpan.BinaryTransform(stateFunction, BinaryTransformState.TransformFunction<TSource>));
        }
    }
}