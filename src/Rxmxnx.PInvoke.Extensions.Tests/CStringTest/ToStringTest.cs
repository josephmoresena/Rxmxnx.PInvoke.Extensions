using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ToStringTest : CStringBaseTest
    {
        [Fact]
        internal void NormalTest()
        {
            String local = TestUtilities.SharedFixture.Create<String>();
            ReadOnlySpan<Byte> external = CreateUtf8StringNulTerminated();

            CString clocal = local;
            CString cexternal = new(external.AsIntPtr(), external.Length);
            ReadOnlySpan<Byte> sExternal = cexternal;

            Assert.Equal(local, clocal.ToString());
            Assert.Equal(Encoding.UTF8.GetString(sExternal[0..cexternal.Length]), cexternal.ToString());
        }
    }
}
