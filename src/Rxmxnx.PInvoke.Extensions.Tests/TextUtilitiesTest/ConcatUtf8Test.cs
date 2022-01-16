using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.TextUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ConcatUtf8Test
    {
        private static readonly Random random = new();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void EmptyTest(Boolean emptyData)
        {
            String initialString = !emptyData ? String.Empty : default;
            Byte[] initialBytes = !emptyData ? Encoding.UTF8.GetBytes(String.Empty) : default;
            String[] valuesString = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : default;
            Byte[][] valuesUtf8 = !emptyData ? valuesString.Select(x => Encoding.UTF8.GetBytes(x)).ToArray() : default;

            Byte[] resultString = TextUtilities.ConcatUtf8(initialString, valuesString);
            Byte[] resultBytes = TextUtilities.ConcatUtf8(initialBytes, valuesUtf8);

            Assert.Null(resultBytes);
            Assert.Null(resultString);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalTest(Boolean withInitial)
        {
            String initialString = withInitial ? TestUtilities.SharedFixture.Create<String>() : default;
            Byte[] intialBytes = withInitial ? Encoding.UTF8.GetBytes(TestUtilities.SharedFixture.Create<String>()) : default;

            String[] valuesString = TestUtilities.SharedFixture.Create<String[]>();
            Byte[][] valuesUtf8 = valuesString.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();

            String expectedBytes = String.Concat(Enumerable.Repeat(intialBytes != default ? Encoding.UTF8.GetString(intialBytes) : String.Empty, 1).Concat(valuesString));
            String expectedString = String.Concat(Enumerable.Repeat(initialString, 1).Concat(valuesString));

            Byte[] expectedResultString = Encoding.UTF8.GetBytes(expectedString);
            Byte[] expectedResultBytes = Encoding.UTF8.GetBytes(expectedBytes);

            Byte[] resultString = TextUtilities.ConcatUtf8(initialString, valuesString);
            Byte[] resultBytes = TextUtilities.ConcatUtf8(intialBytes, valuesUtf8);

            String resultStringString = Encoding.UTF8.GetString(resultString);
            String resultStringBytes = Encoding.UTF8.GetString(resultBytes);

            Assert.Equal(expectedString, resultStringString);
            Assert.Equal(expectedBytes, resultStringBytes);

            Assert.Equal(resultString, resultString);
            Assert.Equal(resultBytes, resultBytes);
        }
    }
}
