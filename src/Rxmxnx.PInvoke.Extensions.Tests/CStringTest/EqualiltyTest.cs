using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class EqualiltyTest : CStringBaseTest
    {
        [Fact]
        internal void NormalTest()
        {
            Byte[] bytes = CreateUtf8StringNulTerminated();
            ReadOnlySpan<Byte> span = bytes;
            CString a = bytes;
            CString b = new(span.AsIntPtr(), span.Length);
            CString c = TextUtilities.Concat(a, b);
            CString d = bytes[0..a.Length].Concat(bytes).ToArray();

            Assert.False(a.Equals(null));
            Assert.False(b.Equals((Object)b.ToString()));
            Assert.False(c.Equals(new Object()));
            Assert.False(d.Equals(CString.Empty));

            Assert.Equal(a, b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
            Assert.True(a.Equals(b));
            Assert.True(a.Equals((Object)b));
            Assert.NotEqual(a, c);
            Assert.NotEqual(a.GetHashCode(), c.GetHashCode());
            Assert.False(a.Equals(c));
            Assert.False(a.Equals((Object)c));
            Assert.NotEqual(b, c);
            Assert.NotEqual(b.GetHashCode(), c.GetHashCode());
            Assert.False(b.Equals(c));
            Assert.False(b.Equals((Object)c));
            Assert.Equal(d, c);
            Assert.Equal(d.GetHashCode(), c.GetHashCode());
            Assert.True(d.Equals(c));
            Assert.True(d.Equals((Object)c));
        }

        [Fact]
        internal void SingleTest()
        {
            Byte[] bytes = GetPrintableBytes(4).ToArray();
            CString a = new(bytes[0], 1);
            CString b = new(bytes[1], 1);
            CString c = new(bytes[2], 1);
            CString d = new(bytes[3], 1);

            Assert.Equal(bytes[0] == bytes[1], a.Equals(b));
            Assert.Equal(bytes[0] == bytes[2], a.Equals(c));
            Assert.Equal(bytes[0] == bytes[3], a.Equals(d));
            Assert.Equal(bytes[1] == bytes[2], b.Equals(c));
            Assert.Equal(bytes[1] == bytes[3], b.Equals(d));
            Assert.Equal(bytes[2] == bytes[3], c.Equals(d));
        }

        [Fact]
        internal void PropialTest()
        {
            Byte[] bytes1 = GetPrintableBytes(sizeof(Int32)).ToArray();
            Byte[] bytes2 = GetPrintableBytes(sizeof(Int32)).ToArray();

            Byte[] com1 = bytes1.Concat(bytes1).Concat(Enumerable.Repeat<Byte>(default, 1)).ToArray();
            Byte[] com2 = bytes1.Concat(bytes2).Concat(Enumerable.Repeat<Byte>(default, 1)).ToArray();
            Byte[] com3 = bytes2.Concat(bytes1).Concat(Enumerable.Repeat<Byte>(default, 1)).ToArray();
            Byte[] com4 = bytes2.Concat(bytes2).Concat(Enumerable.Repeat<Byte>(default, 1)).ToArray();

            CString cstr1 = com1;
            CString cstr2 = com2;
            CString cstr3 = com3;
            CString cstr4 = com4;

            Assert.Equal(com1.SequenceEqual(com2), cstr1.Equals(cstr2));
            Assert.Equal(com1.SequenceEqual(com3), cstr1.Equals(cstr3));
            Assert.Equal(com1.SequenceEqual(com4), cstr1.Equals(cstr4));
            Assert.Equal(com2.SequenceEqual(com3), cstr2.Equals(cstr3));
            Assert.Equal(com2.SequenceEqual(com4), cstr2.Equals(cstr4));
            Assert.Equal(com3.SequenceEqual(com4), cstr3.Equals(cstr4));
        }

        private static IEnumerable<Byte> GetPrintableBytes(Int32 count)
        {
            for (Int32 i = 0; i < count; i++)
                yield return (Byte)TestUtilities.GetPrintableByte();
        }
    }
}
