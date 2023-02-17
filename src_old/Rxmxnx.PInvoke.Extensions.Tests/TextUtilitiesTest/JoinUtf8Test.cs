using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.TextUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class JoinUtf8Test
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void EmptyTest(Boolean withSeperator, Boolean emptyData)
        {
            Char separatorChar = withSeperator ? TestUtilities.SharedFixture.Create<Char>() : default;
            String separatorString = withSeperator ? TestUtilities.SharedFixture.Create<String>() : default;
            Byte separatorByte = withSeperator ? TestUtilities.GetPrintableByte() : default;
            Byte[] separatorBytes = withSeperator ? Encoding.UTF8.GetBytes(separatorString) : default;

            IEnumerable<String> valuesString = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : default;
            IEnumerable<Byte[]> valuesUtf8 = !emptyData ? valuesString.Select(x => Encoding.UTF8.GetBytes(x)).ToArray() : default;

            Byte[] resultByte = TextUtilities.JoinUtf8(separatorByte, valuesUtf8);
            Byte[] resultBytes = TextUtilities.JoinUtf8(separatorBytes, valuesUtf8);
            Byte[] resultChar = TextUtilities.JoinUtf8(separatorChar, valuesString);
            Byte[] resultString = TextUtilities.JoinUtf8(separatorString, valuesString);

            Assert.Null(resultByte);
            Assert.Null(resultBytes);
            Assert.Null(resultChar);
            Assert.Null(resultString);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalTest(Boolean withSeperator)
        {
            Byte separatorByte = withSeperator ? TestUtilities.GetPrintableByte() : default;
            Byte[] separatorBytes = withSeperator ? Encoding.UTF8.GetBytes(TestUtilities.SharedFixture.Create<String>()) : default;
            Char separatorChar = withSeperator ? TestUtilities.SharedFixture.Create<Char>() : default;
            String separatorString = withSeperator ? TestUtilities.SharedFixture.Create<String>() : default;

            String[] valuesString = TestUtilities.SharedFixture.Create<String[]>();
            Byte[][] valuesUtf8 = valuesString.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();

            String expectedByte = String.Join(Encoding.UTF8.GetString(new[] { separatorByte }), valuesString);
            String expectedBytes = String.Join(Encoding.UTF8.GetString(separatorBytes ?? Array.Empty<Byte>()), valuesString);
            String expectedChar = String.Join(separatorChar, valuesString);
            String expectedString = String.Join(separatorString, valuesString);

            Byte[] expectedResultByte = Encoding.UTF8.GetBytes(expectedByte);
            Byte[] expectedResultBytes = Encoding.UTF8.GetBytes(expectedBytes);
            Byte[] expectedResultChar = Encoding.UTF8.GetBytes(expectedChar);
            Byte[] expectedResultString = Encoding.UTF8.GetBytes(expectedString);

            Byte[] resultByte = TextUtilities.JoinUtf8(separatorByte, valuesUtf8);
            Byte[] resultBytes = TextUtilities.JoinUtf8(separatorBytes, valuesUtf8);
            Byte[] resultChar = TextUtilities.JoinUtf8(separatorChar, valuesString);
            Byte[] resultString = TextUtilities.JoinUtf8(separatorString, valuesString);

            String resultStringByte = Encoding.UTF8.GetString(resultByte);
            String resultStringBytes = Encoding.UTF8.GetString(resultBytes);
            String resultStringChar = Encoding.UTF8.GetString(resultChar);
            String resultStringString = Encoding.UTF8.GetString(resultString);

            Assert.Equal(expectedByte, resultStringByte);
            Assert.Equal(expectedBytes, resultStringBytes);
            Assert.Equal(expectedChar, resultStringChar);
            Assert.Equal(expectedString, resultStringString);

            Assert.Equal(resultByte, resultByte);
            Assert.Equal(resultBytes, resultBytes);
            Assert.Equal(resultChar, resultChar);
            Assert.Equal(resultString, resultString);
        }
    }
}
