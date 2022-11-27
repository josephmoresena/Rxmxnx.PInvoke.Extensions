using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.StringExtensions
{
    [ExcludeFromCodeCoverage]
    public sealed class ConcatAsyncTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task LocalEmptyTest(Boolean emptyData)
        {
            IEnumerable<CString> values = !emptyData ? Enumerable.Repeat(CString.Empty, 3).ToArray() : default;
            CString result = await values.ConcatAsync();
            Assert.Null(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal async Task ReferenceEmptyTest(Boolean emptyData)
        {
            IEnumerable<CString> values = !emptyData ? Enumerable.Repeat(new CString(IntPtr.Zero, 0), 3) : default;
            CString result = await values.ConcatAsync();
            Assert.Null(result);
        }

        [Fact]
        internal async Task LocalNormalTest()
        {
            String[] values = TestUtilities.SharedFixture.Create<String[]>();
            CString[] cValues = values.Select(x => (CString)x).ToArray();
            String expectedString = String.Concat(values);

            CString result = await cValues.ConcatAsync();
            String resultString = Encoding.UTF8.GetString(result);

            Assert.Equal(expectedString, resultString[0..^1]);
            Assert.Equal(Encoding.UTF8.GetBytes(expectedString), CString.GetBytes(result)[0..^1]);
        }

        [Fact]
        internal async Task ReferenceNormalTest()
        {
            String[] values = TestUtilities.SharedFixture.Create<String[]>();
            String expectedString = String.Concat(values);
            Byte[][] bytes = values.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();
            GCHandle[] handles = TestUtilities.Alloc(bytes);
            try
            {
                CString[] cValues = TestUtilities.CreateCStrings(handles);
                CString result = await cValues.ConcatAsync();
                String resultString = Encoding.UTF8.GetString(result);

                Assert.Equal(expectedString, resultString[0..^1]);
                Assert.Equal(Encoding.UTF8.GetBytes(expectedString), CString.GetBytes(result)[0..^1]);
            }
            finally
            {
                TestUtilities.Free(handles);
            }
        }
    }
}
