using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.TextUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ConcatUtf8AsyncTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task EmptyTest(Boolean emptyData)
        {
            String initialValue = !emptyData ? String.Empty : default;
            String[] values = !emptyData ? Enumerable.Repeat(String.Empty, 3).ToArray() : default;

            Byte[] resultString = await TextUtilities.ConcatUtf8Async(initialValue, values);

            Assert.Null(resultString);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task NormalTest(Boolean withInitial)
        {
            String initial = withInitial ? TestUtilities.SharedFixture.Create<String>() : default;
            String[] values = TestUtilities.SharedFixture.Create<String[]>();

            String expectedString = String.Concat(Enumerable.Repeat(initial, 1).Concat(values));
            Byte[] expectedResultString = Encoding.UTF8.GetBytes(expectedString);

            Byte[] resultString = await TextUtilities.ConcatUtf8Async(initial, values);
            String resultStringString = Encoding.UTF8.GetString(resultString);

            Assert.Equal(expectedString, resultStringString);
            Assert.Equal(expectedResultString, resultString);
        }
    }
}
