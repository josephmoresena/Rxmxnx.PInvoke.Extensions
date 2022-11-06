using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.MemoryBlockExtensionsTest
{

    [ExcludeFromCodeCoverage]
    public sealed class TransformTest
    {
        private static Fixture fixture = TestUtilities.SharedFixture;

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

        private sealed record TransformState<TDestination> where TDestination : unmanaged
        {
            private readonly List<TDestination> _values = new();
            private readonly List<Byte> _recidues = new();

            public IList<TDestination> Values => this._values;
            public IList<Byte> Residues => this._recidues;

            public static void TransformAction<TSource>(Span<TDestination> span, TransformState<TDestination> arg, Span<Byte> residue)
                where TSource : unmanaged
            {
                Int32 sizeOfTSource = NativeUtilities.SizeOf<TSource>();
                Int32 sizeOfTDestination = NativeUtilities.SizeOf<TDestination>();

                Assert.Equal(span.Length * sizeOfTDestination / sizeOfTDestination, span.Length);
                foreach (ref TDestination destination in span)
                {
                    destination = fixture.Create<TDestination>();
                    arg.Values.Add(destination);
                }
                foreach (ref Byte recidue in residue)
                {
                    recidue = fixture.Create<Byte>();
                    arg.Residues.Add(recidue);
                }
            }
            public static void TransformAction<TSource>(ReadOnlySpan<TDestination> span, TransformState<TDestination> arg, ReadOnlySpan<Byte> residue)
            {
                Assert.Equal(arg.Values.Count, span.Length);
                Assert.Equal(arg.Residues.Count, residue.Length);

                for (Int32 i = 0; i < arg.Values.Count; i++)
                    Assert.Equal(arg.Values[i], span[i]);

                for (Int32 i = 0; i < arg.Residues.Count; i++)
                    Assert.Equal(arg.Residues[i], residue[i]);
            }

            public static Int32 TransformFunction<TSource>(Span<TDestination> span, TransformState<TDestination> arg, Span<Byte> residue)
                where TSource : unmanaged
            {
                TransformAction<TSource>(span, arg, residue);
                unsafe
                {
                    Int32 byteLength = arg.Values.Count * sizeof(TDestination) + residue.Length;

                    Assert.Equal(0, byteLength % sizeof(TSource));
                    return byteLength / sizeof(TSource);
                }
            }
            public static Int32 TransformFunction<TSource>(ReadOnlySpan<TDestination> span, TransformState<TDestination> arg, ReadOnlySpan<Byte> residue)
                where TSource : unmanaged
            {
                TransformAction<TSource>(span, arg, residue);
                unsafe
                {
                    Int32 byteLength = arg.Values.Count * sizeof(TDestination) + residue.Length;

                    Assert.Equal(0, byteLength % sizeof(TSource));
                    return byteLength / sizeof(TSource);
                }
            }
        }
    }
}