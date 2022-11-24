using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class LiteralTest : CStringBaseTest
    {
        private readonly CString inline = new(() => "tHiS @ uTf-B 27R1N6"u8);
        private readonly CString function = new(GetLiteral);
        private readonly CString created = CString.Create(GetLiteral);
        private readonly CStringSequence inlineSequence = new(new CString(() => "tHiS @ uTf-B 27R1N6"u8));
        private readonly CStringSequence functionSequence = new(new CString(GetLiteral));
        private readonly CStringSequence createdSequence = new(CString.Create(GetLiteral));

        [Fact]
        internal void NullTest()
        {
            ReadOnlySpanFunc<Byte> del = default!;
            Assert.Throws<NullReferenceException>(() => new CString(del));
            Assert.Null(CString.Create(del));
        }

        [Fact]
        internal void NormalTest()
        {
            ref Byte inlineChar = ref Unsafe.AsRef(inline.AsSpan()[0]);
            ref Byte functionChar = ref Unsafe.AsRef(function.AsSpan()[0]);
            ref Byte createdChar = ref Unsafe.AsRef(created.AsSpan()[0]);

            Assert.True(Unsafe.AreSame(ref functionChar, ref createdChar));
            Assert.True(Unsafe.AreSame(ref inlineChar, ref createdChar));

            Assert.True(inline.IsFunction);
            Assert.True(function.IsFunction);
            Assert.True(created.IsFunction);

            Assert.True(inline.IsNullTerminated);
            Assert.True(function.IsNullTerminated);
            Assert.False(created.IsNullTerminated);

            Assert.Equal(inline.Length, created.Length);
            Assert.Equal(function.Length, created.Length);

            Assert.Equal(inlineChar, createdChar);
            Assert.Equal(functionChar, createdChar);
        }

        [Fact]
        internal void SequenceTest()
        {
            unsafe
            {
                fixed (Char* ptr1 = &MemoryMarshal.GetReference(inlineSequence.AsSpan(out CString[] c1)))
                fixed (Char* ptr2 = &MemoryMarshal.GetReference(functionSequence.AsSpan(out CString[] c2)))
                fixed (Char* ptr3 = &MemoryMarshal.GetReference(createdSequence.AsSpan(out CString[] c3)))
                {
                    Assert.Equal(inlineSequence.ToString(), createdSequence.ToString());
                    Assert.Equal(functionSequence.ToString(), createdSequence.ToString());

                    Assert.False(Unsafe.AreSame(ref Unsafe.AsRef<Char>(ptr1), ref Unsafe.AsRef<Char>(ptr3)));
                    Assert.False(Unsafe.AreSame(ref Unsafe.AsRef<Char>(ptr2), ref Unsafe.AsRef<Char>(ptr3)));

                    Assert.Equal(c1.Length, c3.Length);
                    Assert.Equal(c2.Length, c3.Length);

                    Assert.Equal(c1[0], inline);
                    Assert.Equal(c2[0], function);
                    Assert.Equal(c3[0], created);

                    Assert.True(c1[0].IsReference);
                    Assert.True(c1[0].IsNullTerminated);

                    Assert.True(c2[0].IsReference);
                    Assert.True(c2[0].IsNullTerminated);

                    Assert.True(c3[0].IsReference);
                    Assert.True(c3[0].IsNullTerminated);
                }
            }
        }

        [Fact]
        internal void NonLiteralTest()
        {
            CString fromArrayInvalid = new(GetArray); //This instance is dangerous.
            CString fromArrayCreated = CString.Create(GetArray);
            CString fromClone = new(GetClone);
            CString fromCloneCreated = CString.Create(GetClone);


            Assert.True(fromArrayInvalid.IsNullTerminated);
            Assert.False(fromArrayCreated.IsNullTerminated);
            Assert.True(fromClone.IsNullTerminated);
            Assert.True(fromCloneCreated.IsNullTerminated);

            Assert.Equal(inline, fromArrayInvalid.Clone());
            Assert.Equal(inline, fromArrayCreated.Clone());
            Assert.Equal(inline, fromClone.Clone());
            Assert.Equal(inline, fromCloneCreated.Clone());
        }

        private static ReadOnlySpan<Byte> GetLiteral() => "tHiS @ uTf-B 27R1N6"u8;
        private static ReadOnlySpan<Byte> GetArray() => GetLiteral().ToArray();
        private static ReadOnlySpan<Byte> GetClone() => ((new CString(GetLiteral)).Clone() as CString);
    }
}