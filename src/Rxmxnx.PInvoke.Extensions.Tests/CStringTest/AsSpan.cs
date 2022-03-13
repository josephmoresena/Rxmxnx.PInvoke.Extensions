using System;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsSpanText : CStringBaseTest
    {
        [Fact]
        internal void NormalTest()
        {
            String local = TestUtilities.SharedFixture.Create<String>();
            ReadOnlySpan<Byte> external = CreateUtf8String();
            ReadOnlySpan<Byte> externalNull = CreateUtf8StringNulTerminated();

            CString clocal = local;
            CString cexternal = new(external.AsIntPtr(), external.Length);
            CString cexternalNull = new(externalNull.AsIntPtr(), externalNull.Length);

            ReadOnlySpan<Byte> sLocal = clocal;
            ReadOnlySpan<Byte> sExternal = cexternal;
            ReadOnlySpan<Byte> sExternalNull = cexternalNull;

            ReadOnlySpan<Byte> localSpan = clocal.AsSpan();
            ReadOnlySpan<Byte> externalSpan = cexternal.AsSpan();
            ReadOnlySpan<Byte> externalNullSpan = cexternalNull.AsSpan();

            Assert.Equal(sLocal.AsIntPtr(), localSpan.AsIntPtr());
            Assert.Equal(sLocal.Length, localSpan.Length);
            Assert.Equal(clocal.Length + 1, localSpan.Length);

            Assert.Equal(sExternal.AsIntPtr(), externalSpan.AsIntPtr());
            Assert.Equal(sExternal.Length, externalSpan.Length);
            Assert.Equal(cexternal.Length, externalSpan.Length);

            Assert.Equal(sExternalNull.AsIntPtr(), externalNullSpan.AsIntPtr());
            Assert.Equal(sExternalNull.Length, externalNullSpan.Length);
            Assert.Equal(cexternalNull.Length + 1, externalNullSpan.Length);
        }
    }
}
