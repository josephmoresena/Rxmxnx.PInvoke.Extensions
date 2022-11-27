using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.TextUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ConcatAsyncTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task LocalEmptyTest(Boolean emptyData)
        {
            CString initialValue = !emptyData ? CString.Empty : default;
            CString[] values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : default;
            await EmptyTest(initialValue, values);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task ReferenceEmptyTest(Boolean emptyData)
        {
            CString initialValue = !emptyData ? CString.Empty : default;
            CString[] values = !emptyData ? Enumerable.Repeat(new CString(IntPtr.Zero, default), 3).ToArray() : default;
            await EmptyTest(initialValue, values);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task LocalNormalTest(Boolean withInitial)
        {
            String initial = withInitial ? TestUtilities.SharedFixture.Create<String>() : default;
            String[] values = TestUtilities.SharedFixture.Create<String[]>();

            CString initialCString = initial;
            CString[] valuesCString = values.Select(x => (CString)x).ToArray();

            await NormalTest(initial, values, initialCString, valuesCString);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task ReferenceNormalTest(Boolean withInitial)
        {
            String initial = withInitial ? TestUtilities.SharedFixture.Create<String>() : default;
            String[] values = TestUtilities.SharedFixture.Create<String[]>();
            Byte[][] bytes = values.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();
            GCHandle[] handles = TestUtilities.Alloc(bytes);

            try
            {
                CString initialCString = initial;
                CString[] valuesCString = TestUtilities.CreateCStrings(handles);

                await NormalTest(initial, values, initialCString, valuesCString);
            }
            finally
            {
                TestUtilities.Free(handles);
            }
        }

        private static async Task EmptyTest(CString initialValue, CString[] values)
        {
            CString resultCString = await TextUtilities.ConcatAsync(initialValue, values);
            Assert.Null(resultCString);
        }

        private static async Task NormalTest(string initial, string[] values, CString initialCString, CString[] valuesCString)
        {
            String expectedCString = String.Concat(Enumerable.Repeat(initial, 1).Concat(values));
            Byte[] expectedResultCString = Encoding.UTF8.GetBytes(expectedCString);

            CString resultCString = await TextUtilities.ConcatAsync(initialCString, valuesCString);
            String resultCStringCString = Encoding.UTF8.GetString(CString.GetBytes(resultCString)[0..^1]);

            Assert.Equal(expectedCString, resultCStringCString);
            Assert.Equal(expectedResultCString, CString.GetBytes(resultCString)[0..^1]);
        }
    }
}
