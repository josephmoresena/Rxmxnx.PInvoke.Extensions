using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.TextUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class JoinUtf8AsyncTest
    {
        [Theory]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal async Task EmptyTest(Boolean withSeperator, Boolean emptyData)
        {
            Char separatorChar = withSeperator ? TestUtilities.SharedFixture.Create<Char>() : default;
            String separatorString = withSeperator ? TestUtilities.SharedFixture.Create<String>() : default;

            IEnumerable<String> values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : default;

            Byte[] resultChar = await TextUtilities.JoinUtf8Async(separatorChar, values);
            Byte[] resultString = await TextUtilities.JoinUtf8Async(separatorString, values);

            Assert.Null(resultChar);
            Assert.Null(resultString);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task NormalTest(Boolean withSeperator)
        {
            Char separatorChar = withSeperator ? TestUtilities.SharedFixture.Create<Char>() : default;
            String separatorString = withSeperator ? TestUtilities.SharedFixture.Create<String>() : default;

            String[] values = TestUtilities.SharedFixture.Create<String[]>();

            String expectedChar = String.Join(separatorChar, values);
            String expectedString = String.Join(separatorString, values);
            Byte[] expectedResultChar = Encoding.UTF8.GetBytes(expectedChar);
            Byte[] expectedResultString = Encoding.UTF8.GetBytes(expectedString);

            Byte[] resultChar = await TextUtilities.JoinUtf8Async(separatorChar, values);
            Byte[] resultString = await TextUtilities.JoinUtf8Async(separatorString, values);
            String resultStringChar = Encoding.UTF8.GetString(resultChar);
            String resultStringString = Encoding.UTF8.GetString(resultString);

            Assert.Equal(expectedChar, resultStringChar);
            Assert.Equal(expectedString, resultStringString);
            Assert.Equal(expectedResultChar, resultChar);
            Assert.Equal(expectedResultString, resultString);
        }
    }
}
