using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.StringExtensions
{
    [ExcludeFromCodeCoverage]
    public sealed class AsUtf8Test
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void EmptyTest(Boolean emptyString)
        {
            String str = emptyString ? String.Empty : default;
            Byte[] result = str.AsUtf8();
            Assert.Null(result);
        }

        [Fact]
        internal void NormalTest()
        {
            String str = TestUtilities.SharedFixture.Create<String>();
            Byte[] utfEncode = Encoding.UTF8.GetBytes(str);
            Byte[] result = str.AsUtf8();
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(utfEncode.Length, result.Length);
            for (Int32 i = 0; i < result.Length; i++)
                Assert.Equal(utfEncode[i], result[i]);
        }
    }
}
