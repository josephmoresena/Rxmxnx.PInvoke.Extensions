using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsHexStringTest : CStringBaseTest
    {
        [Fact]
        internal void NormalTest()
        {
            String local = TestUtilities.SharedFixture.Create<String>();
            CreateUtf8StringNulTerminated().WithSafeFixed((in IReadOnlyFixedContext<Byte> ctx) =>
            {
                ReadOnlySpan<Byte> external = ctx.Values;

                CString clocal = local;
                CString cexternal = new(external.AsIntPtr(), external.Length);

                Assert.Equal(Encoding.UTF8.GetBytes(local).AsHexString(), clocal.AsHexString());
                Assert.Equal(external[0..cexternal.Length].ToArray().AsHexString(), cexternal.AsHexString());
            });
        }
    }
}
