using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.StringExtensions
{
    [ExcludeFromCodeCoverage]
    public sealed class ConcatUtf8AsyncTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task EmptyTest(Boolean emptyData)
        {
            IEnumerable<String> values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : default;
            Byte[] resultString = await values.ConcatUtf8Async();
            Assert.Null(resultString);
        }

        [Fact]
        internal async Task NormalTest()
        {
            String[] values = TestUtilities.SharedFixture.Create<String[]>();
            String expectedString = String.Concat(values);
            Byte[] resultString = await values.ConcatUtf8Async();

            Assert.Equal(expectedString, Encoding.UTF8.GetString(resultString));
            Assert.Equal(Encoding.UTF8.GetBytes(expectedString), resultString);
        }
    }
}
