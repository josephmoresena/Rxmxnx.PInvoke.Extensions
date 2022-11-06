using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.MemoryBlockExtensionsTest
{
    [ExcludeFromCodeCoverage]
    internal sealed record TransformState<TDestination> where TDestination : unmanaged
    {
        private readonly List<TDestination> _values = new();
        private readonly List<Byte> _recidues = new();

        public IList<TDestination> Values => this._values;
        public IList<Byte> Residues => this._recidues;

        public static void TransformAction<TSource>(Span<TDestination> span, TransformState<TDestination> arg, Span<Byte> residue)
            where TSource : unmanaged
        {
            foreach (ref TDestination destination in span)
            {
                destination = TestUtilities.SharedFixture.Create<TDestination>();
                arg.Values.Add(destination);
            }
            foreach (ref Byte recidue in residue)
            {
                recidue = TestUtilities.SharedFixture.Create<Byte>();
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