using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class OperatorsTest : CStringBaseTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void EmptyTest(Boolean empty)
        {
            CString cstr1 = empty ? CString.Empty : default;
            CString cstr2 = empty ? String.Empty : default;
            CString cstr3 = empty ? Encoding.UTF8.GetBytes(String.Empty) : default;
            CString cstr4 = empty ? Array.Empty<Byte>() : default;
            CString cstr5 = new(empty ? TestUtilities.GetPrintableByte() : default, 0);
            CString cstr6 = new((empty ? Array.Empty<Byte>() : default).AsSpan().AsIntPtr(), 0);

            Assert.True((null as CString) == null);
            Assert.True(cstr1 == cstr2);
            Assert.Equal(empty, null != cstr1);
            Assert.Equal(empty, CString.Empty == cstr1);
            Assert.Equal(empty, CString.Empty == cstr2);
            Assert.Equal(empty, CString.Empty == cstr3);
            Assert.Equal(empty, CString.Empty == cstr4);
            Assert.True(CString.Empty == cstr5);
            Assert.True(CString.Empty == cstr6);

            ReadOnlySpan<Byte> span1 = cstr1;
            ReadOnlySpan<Byte> span2 = cstr2;
            ReadOnlySpan<Byte> span3 = cstr3;
            ReadOnlySpan<Byte> span4 = cstr4;
            ReadOnlySpan<Byte> span5 = cstr5;
            ReadOnlySpan<Byte> span6 = cstr6;

            Assert.Equal(!empty, span1.IsEmpty);
            Assert.Equal(!empty, span2.IsEmpty);
            Assert.True(span3.IsEmpty);
            Assert.True(span4.IsEmpty);
            Assert.False(span5.IsEmpty);
            Assert.True(span6.IsEmpty);

            if (empty)
            {
                Assert.NotNull(cstr1);
                Assert.NotNull(cstr2);
                Assert.NotNull(cstr3);
                Assert.NotNull(cstr4);

                Assert.Single(CString.GetBytes(cstr1));
                Assert.Single(CString.GetBytes(cstr2));
                Assert.Empty(CString.GetBytes(cstr3));
                Assert.Empty(CString.GetBytes(cstr4));

                Assert.Single(cstr1.ToArray());
                Assert.Single(cstr2.ToArray());
                Assert.Empty(cstr3.ToArray());
                Assert.Empty(cstr4.ToArray());

                Assert.False(cstr1.IsReference);
                Assert.False(cstr2.IsReference);
                Assert.False(cstr3.IsReference);
                Assert.False(cstr4.IsReference);

                Assert.Empty(cstr1.ToString());
                Assert.Empty(cstr2.ToString());
                Assert.Empty(cstr3.ToString());
                Assert.Empty(cstr4.ToString());

                Assert.True(cstr1.IsNullTerminated);
                Assert.True(cstr2.IsNullTerminated);
                Assert.False(cstr3.IsNullTerminated);
                Assert.False(cstr4.IsNullTerminated);

                Assert.Equal(0, cstr1.Length);
                Assert.Equal(0, cstr2.Length);
                Assert.Equal(0, cstr3.Length);
                Assert.Equal(0, cstr4.Length);

                Assert.Equal(1, span1.Length);
                Assert.Equal(1, span2.Length);
                Assert.Equal(0, span3.Length);
                Assert.Equal(0, span4.Length);

                Assert.Equal(default, cstr1[0]);
                Assert.Equal(default, cstr2[0]);
                Assert.Throws<IndexOutOfRangeException>(() => cstr3[0]);
                Assert.Throws<IndexOutOfRangeException>(() => cstr4[0]);

                Assert.Throws<ArgumentOutOfRangeException>(() => cstr1[0..]);
                Assert.Throws<ArgumentOutOfRangeException>(() => cstr2[0..]);
                Assert.Throws<ArgumentOutOfRangeException>(() => cstr3[0..]);
                Assert.Throws<ArgumentOutOfRangeException>(() => cstr4[0..]);
                Assert.Throws<ArgumentOutOfRangeException>(() => cstr5[0..]);
                Assert.Throws<ArgumentOutOfRangeException>(() => cstr6[0..]);

            }
            else
            {
                Assert.Null(cstr1);
                Assert.Null(cstr2);
                Assert.Null(cstr3);
                Assert.Null(cstr4);

                Assert.Throws<ArgumentNullException>(() => CString.GetBytes(cstr1));
                Assert.Throws<ArgumentNullException>(() => CString.GetBytes(cstr2));
                Assert.Throws<ArgumentNullException>(() => CString.GetBytes(cstr3));
                Assert.Throws<ArgumentNullException>(() => CString.GetBytes(cstr4));
            }

            Assert.NotNull(cstr5);
            Assert.NotNull(cstr6);

            Assert.Single(CString.GetBytes(cstr5));
            Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr6));

            Assert.Single(cstr5.ToArray());
            Assert.Empty(cstr6.ToArray());

            Assert.False(cstr5.IsReference);
            Assert.True(cstr6.IsReference);

            Assert.Empty(cstr5.ToString());
            Assert.Empty(cstr6.ToString());

            Assert.True(cstr5.IsNullTerminated);
            Assert.False(cstr6.IsNullTerminated);

            Assert.Equal(0, cstr5.Length);
            Assert.Equal(0, cstr6.Length);

            Assert.Equal(1, span5.Length);
            Assert.Equal(0, span6.Length);

            Assert.Equal(default, cstr5[0]);

            if (empty)
                Assert.Throws<IndexOutOfRangeException>(() => cstr4[0]);
            else
                Assert.Throws<NullReferenceException>(() => cstr4[0]);

            Assert.True(CString.IsNullOrEmpty(cstr1));
            Assert.True(CString.IsNullOrEmpty(cstr2));
            Assert.True(CString.IsNullOrEmpty(cstr3));
            Assert.True(CString.IsNullOrEmpty(cstr4));
            Assert.True(CString.IsNullOrEmpty(cstr5));
            Assert.True(CString.IsNullOrEmpty(cstr6));
        }

        [Fact]
        internal void NormalTest()
        {
            String str1 = TestUtilities.SharedFixture.Create<String>();
            Byte[] byt2 = Encoding.UTF8.GetBytes(TestUtilities.SharedFixture.Create<String>());
            Byte[] byt3 = TestUtilities.AsArray(TestUtilities.GetPrintableByte(), TestUtilities.GetPrintableByte());
            Byte[] byt4 = CreateUtf8StringNulTerminated();

            Byte[] byt5 = TestUtilities.AsArray(TestUtilities.GetPrintableByte(), TestUtilities.GetPrintableByte());
            Byte[] byt6 = CreateUtf8StringNulTerminated();

            unsafe
            {
                fixed (void* ptr5 = byt5)
                fixed (void* ptr6 = byt6)
                {
                    ReadOnlySpan<Byte> spa5 = new(ptr5, byt5.Length);
                    ReadOnlySpan<Byte> spa6 = new(ptr6, byt6.Length);
                    Byte ccr7 = TestUtilities.GetPrintableByte();

                    Byte[] byt1 = Encoding.UTF8.GetBytes(str1);
                    String str2 = Encoding.UTF8.GetString(byt2);
                    String str3 = Encoding.UTF8.GetString(byt3);
                    String str4 = Encoding.UTF8.GetString(byt4);
                    String str5 = Encoding.UTF8.GetString(spa5);
                    String str6 = Encoding.UTF8.GetString(spa6);
                    String str7 = Encoding.UTF8.GetString(Enumerable.Repeat(ccr7, 3).ToArray());

                    CString cstr1 = str1;
                    CString cstr2 = byt2;
                    CString cstr3 = byt3;
                    CString cstr4 = byt4;
                    CString cstr5 = new(spa5.AsIntPtr(), spa5.Length);
                    CString cstr6 = new(spa6.AsIntPtr(), spa6.Length);
                    CString cstr7 = new(ccr7, 3);

                    Assert.True(CString.Empty != cstr1);
                    Assert.True(CString.Empty != cstr2);
                    Assert.True(CString.Empty != cstr3);
                    Assert.True(CString.Empty != cstr4);
                    Assert.True(CString.Empty != cstr5);
                    Assert.True(CString.Empty != cstr6);

                    ReadOnlySpan<Byte> cstrSpan1 = cstr1;
                    ReadOnlySpan<Byte> cstrSpan2 = cstr2;
                    ReadOnlySpan<Byte> cstrSpan3 = cstr3;
                    ReadOnlySpan<Byte> cstrSpan4 = cstr4;
                    ReadOnlySpan<Byte> cstrSpan5 = cstr5;
                    ReadOnlySpan<Byte> cstrSpan6 = cstr6;
                    ReadOnlySpan<Byte> cstrSpan7 = cstr7;

                    Assert.NotNull(cstr1);
                    Assert.NotNull(cstr2);
                    Assert.NotNull(cstr3);
                    Assert.NotNull(cstr4);
                    Assert.NotNull(cstr5);
                    Assert.NotNull(cstr6);
                    Assert.NotNull(cstr7);

                    Assert.Equal(str1, cstr1.ToString());
                    Assert.Equal(str1.Length, cstr1.Length);
                    Assert.Equal(cstr1.Length + 1, cstrSpan1.Length);
                    Assert.True(cstr1.IsNullTerminated);
                    AssertIndex(byt1, cstr1);
                    AssertReference(cstr1, false);

                    Assert.Equal(str2, cstr2.ToString());
                    Assert.Equal(str2.Length, cstr2.Length);
                    Assert.Equal(cstr2.Length, cstrSpan2.Length);
                    Assert.False(cstr2.IsNullTerminated);
                    AssertIndex(byt2, cstr2);
                    AssertReferenceEquality(byt2, cstr2, false);
                    AssertReferenceEquality(byt2, (CString)cstr2.Clone(), true);
                    AssertReference(cstr2, false);

                    Assert.Equal(str3, cstr3.ToString());
                    Assert.Equal(str3.Length, cstr3.Length);
                    Assert.Equal(cstr3.Length, cstrSpan3.Length);
                    Assert.False(cstr3.IsNullTerminated);
                    AssertIndex(byt3, cstr3);
                    AssertReferenceEquality(byt3, cstr3, false);
                    AssertReferenceEquality(byt3, (CString)cstr3.Clone(), true);
                    AssertReference(cstr3, false);

                    Assert.Equal(str4[0..^1], cstr4.ToString());
                    Assert.Equal(str4.Length - 1, cstr4.Length);
                    Assert.Equal(cstr4.Length + 1, cstrSpan4.Length);
                    Assert.True(cstr4.IsNullTerminated);
                    AssertIndex(byt4, cstr4);
                    AssertReferenceEquality(byt4, cstr4, false);
                    AssertReferenceEquality(byt4, (CString)cstr4.Clone(), true);
                    AssertReference(cstr4, false);

                    Assert.Equal(str5, cstr5.ToString());
                    Assert.Equal(str5.Length, cstr5.Length);
                    Assert.Equal(cstr5.Length, cstrSpan5.Length);
                    Assert.False(cstr5.IsNullTerminated);
                    AssertIndex(spa5, cstr5);
                    AssertReferenceEquality(spa5, cstr5, false);
                    AssertReferenceEquality(spa5, (CString)cstr5.Clone(), true);
                    AssertReference(cstr5, true);

                    Assert.Equal(str6[0..^1], cstr6.ToString());
                    Assert.Equal(str6.Length - 1, cstr6.Length);
                    Assert.Equal(cstr6.Length + 1, cstrSpan6.Length);
                    Assert.True(cstr6.IsNullTerminated);
                    AssertIndex(spa6, cstr6);
                    AssertReferenceEquality(spa6, cstr6, false);
                    AssertReferenceEquality(spa6, (CString)cstr6.Clone(), true);
                    AssertReference(cstr6, true);

                    Assert.Equal(str7, cstr7.ToString());
                    Assert.Equal(str7.Length, cstr7.Length);
                    Assert.Equal(cstr7.Length + 1, cstrSpan7.Length);
                    Assert.True(cstr7.IsNullTerminated);
                    AssertIndex(cstr7, cstr7);
                    AssertReference(cstr7, false);

                    AssertSegment(cstr1);
                    AssertSegment(cstr2);
                    AssertSegment(cstr3);
                    AssertSegment(cstr4);
                    AssertSegment(cstr5);
                    AssertSegment(cstr6);
                    AssertSegment(cstr7);
                }
            }
        }

        private static void AssertIndex(ReadOnlySpan<Byte> span, CString cstr)
        {
            for (Int32 i = 0; i < span.Length; i++)
                Assert.Equal(span[i], cstr[i]);
            Int32 index = 0;
            foreach (Byte c in cstr)
            {
                c.Equals(span[index]);
                index++;
                if (index == cstr.Length)
                    break;
            }
            if (!cstr.IsNullTerminated)
                Assert.Throws<IndexOutOfRangeException>(() => cstr[index]);
            else
                Assert.Equal(default, cstr[index]);
        }

        private static void AssertReferenceEquality(ReadOnlySpan<Byte> span, CString cstr, Boolean clone)
        {
            ReadOnlySpan<Byte> cstrSpan = cstr;
            Byte[] bytes = cstr.ToArray();

            Assert.Equal(!clone, span.AsIntPtr() == cstrSpan.AsIntPtr());
            Assert.Equal(!clone || span[^1] == default, span.Length == cstrSpan.Length);

            Int32 index = 0;
            foreach (ref readonly Byte c in cstr)
            {
                Assert.Equal(c, span[index]);
                Assert.Equal(c, bytes[index]);
                Assert.Equal(!clone, Unsafe.AreSame(ref Unsafe.AsRef(c), ref Unsafe.AsRef(span[index])));
                Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(c), ref Unsafe.AsRef(bytes[index])));
                index++;
                if (index == cstr.Length)
                    break;
            }
        }

        private static void AssertReference(CString cstr, Boolean isReference)
        {
            Assert.Equal(isReference, cstr.IsReference);
            if (isReference)
            {
                Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
                CString clone = (CString)cstr.Clone();
                if (cstr.IsNullTerminated)
                    Assert.Equal(cstr.ToArray(), CString.GetBytes(clone));
                else
                    Assert.Equal(cstr.Length + 1, CString.GetBytes(clone).Length);
                Assert.False(clone.IsReference);
            }
            else
                Assert.Equal(cstr.ToArray(), CString.GetBytes(cstr));
        }

        private static void AssertSegment(CString cstr)
        {
            for (Int32 i = 0; i < cstr.Length; i++)
            {
                Int32 start = Random.Shared.Next(i, cstr.Length);
                Int32 end = Random.Shared.Next(start, cstr.Length);
                Int32 end2 = Random.Shared.Next(0, end - start);

                CString seg1 = cstr[start..end];

                AssertSegment(cstr, start, end, seg1);
                if (seg1.Length > 0)
                    AssertSegment(seg1, default, end2, seg1[..end2]);
            }
        }

        private static void AssertSegment(CString cstr, Int32 start, Int32 end, CString seg)
        {
            if (seg.Length > 0)
            {
                Assert.Equal(seg.IsReference, cstr.IsReference);
                Assert.Equal(!seg.IsReference && seg.Length != cstr.Length, seg.IsSegmented);
                AssertSequenceSegment(cstr, seg);
            }
            else
            {
                Assert.False(seg.IsReference);
                Assert.False(seg.IsSegmented);
            }
            Assert.Equal(cstr.ToString()[start..end], seg.ToString());

            if ((seg.IsSegmented || seg.IsReference) && seg.Length != 0)
                Assert.Throws<InvalidOperationException>(() => CString.GetBytes(seg));
            else if (cstr.IsSegmented)
                Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
            else if (cstr.Length == seg.Length)
                Assert.Equal(CString.GetBytes(cstr), CString.GetBytes(seg));

            for (Int32 i = start; i < end; i++)
                Assert.Equal(cstr[i], seg[i - start]);
        }

        private static void AssertSequenceSegment(CString cstr, CString seg)
        {
            CString newJoin = new CStringSequence(cstr, seg).ToCString();
            ReadOnlySpan<Byte> newJoinBytes = CString.GetBytes(newJoin);
            CString newJoin2 = new CString(newJoinBytes.AsIntPtr(), newJoinBytes.Length);

            CString newCstr1 = newJoin[..cstr.Length];
            CString newSeg1 = newJoin[(cstr.Length + 1)..];

            CString newCstr2 = newJoin2[..cstr.Length];
            CString newSeg2 = newJoin2[(cstr.Length + 1)..];

            Assert.Equal(cstr, newCstr1);
            Assert.Equal(seg, newSeg1);
            Assert.Equal(newCstr1, newCstr2);
            Assert.Equal(newSeg1, newSeg2);

            Assert.True(newCstr1.IsNullTerminated);
            Assert.True(newSeg1.IsNullTerminated);
            Assert.False(newCstr1.IsReference);
            Assert.False(newSeg1.IsReference);

            Assert.True(newCstr2.IsNullTerminated);
            Assert.True(newSeg2.IsNullTerminated);
            Assert.True(newCstr2.IsReference);
            Assert.True(newSeg2.IsReference);
        }
    }
}
