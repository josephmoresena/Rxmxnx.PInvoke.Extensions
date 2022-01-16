using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.BinaryExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ConcatUtf8Test
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void EmptyTest(Boolean emptyData)
        {
            IEnumerable<Byte[]> values = !emptyData ? Enumerable.Repeat(new Byte[] { default }, 3).ToArray() : default;
            Assert.Null(values.ConcatUtf8());
        }

        [Fact]
        internal void NormalTest()
        {
            String[] valuesString = TestUtilities.SharedFixture.Create<String[]>().ToArray();
            Byte[][] values = valuesString.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();

            String expectedString = String.Concat(valuesString);

            Byte[] resultBytes = values.ConcatUtf8();

            Assert.Equal(expectedString, Encoding.UTF8.GetString(resultBytes));
            Assert.Equal(Encoding.UTF8.GetBytes(expectedString), resultBytes);
        }
    }
}
