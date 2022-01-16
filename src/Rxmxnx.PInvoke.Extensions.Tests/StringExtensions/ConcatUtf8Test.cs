using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.StringExtensions
{
    [ExcludeFromCodeCoverage]
    public sealed class ConcatUtf8Test
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void EmptyTest(Boolean emptyData)
        {
            IEnumerable<String> values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : default;
            Byte[] resultString = values.ConcatUtf8();
            Assert.Null(resultString);
        }

        [Fact]
        internal void NormalTest()
        {
            String[] values = TestUtilities.SharedFixture.Create<String[]>();
            String expectedString = String.Concat(values);
            Byte[] resultString = values.ConcatUtf8();

            Assert.Equal(expectedString, Encoding.UTF8.GetString(resultString));
            Assert.Equal(Encoding.UTF8.GetBytes(expectedString), resultString);
        }
    }
}
