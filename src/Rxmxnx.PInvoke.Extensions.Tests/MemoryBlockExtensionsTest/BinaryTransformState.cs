using System;
using System.Collections.Generic;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.MemoryBlockExtensionsTest
{
    internal sealed record BinaryTransformState
    {
        private readonly List<Byte> _values = new();

        public IList<Byte> Values => this._values;

        public static void TransformAction(Span<Byte> span, BinaryTransformState arg)
        {
            foreach (ref Byte destination in span)
            {
                destination = TestUtilities.SharedFixture.Create<Byte>();
                arg.Values.Add(destination);
            }
        }
        public static void TransformAction(ReadOnlySpan<Byte> span, BinaryTransformState arg)
        {
            Assert.Equal(arg.Values.Count, span.Length);
            for (Int32 i = 0; i < arg.Values.Count; i++)
                Assert.Equal(arg.Values[i], span[i]);
        }

        public static Int32 TransformFunction<TSource>(Span<Byte> span, BinaryTransformState arg) where TSource : unmanaged
        {
            TransformAction(span, arg);
            unsafe
            {
                return arg.Values.Count / sizeof(TSource);
            }
        }
        public static Int32 TransformFunction<TSource>(ReadOnlySpan<Byte> span, BinaryTransformState arg) where TSource : unmanaged
        {
            TransformAction(span, arg);
            unsafe
            {
                return arg.Values.Count / sizeof(TSource);
            }
        }
    }
}