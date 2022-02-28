using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringSequenceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsSpanTest
    {
        [Fact]
        internal void NormalTest()
        {
            CString[] localValues = TestUtilities.SharedFixture.Create<String[]>().Select(x => (CString)x).ToArray();
            String[] strValue = TestUtilities.SharedFixture.Create<String[]>();
            Byte[][] bytes = strValue.Select(x => Encoding.UTF8.GetBytes(x)).ToArray();
            GCHandle[] handles = TestUtilities.Alloc(bytes);
            try
            {
                ExecuteTest(localValues, TestUtilities.CreateCStrings(bytes, handles));
            }
            finally
            {
                TestUtilities.Free(handles);
            }
        }

        private static void ExecuteTest(CString[] localValues, CString[] externalValues)
        {
            CString[] allValues = MixValues(localValues, externalValues);
            Int32 expectedTextLength = GetExpectedTextLength(allValues);
            Int32 expectedStringLength = GetExpectedStringLength(expectedTextLength);
            String[] expectedValue = allValues.Select(x => x?.ToString()).ToArray();

            CStringSequence sequence = new(allValues);
            ReadOnlySpan<Char> buffer = sequence.AsSpan(out CString[] output);

            Assert.Equal(expectedStringLength, buffer.Length);
            Assert.Equal(allValues.Length, output.Length);
            for (Int32 i = 0; i < allValues.Length; i++)
            {
                Boolean isNullOrEmpty = CString.IsNullOrEmpty(allValues[i]);

                Assert.Equal(isNullOrEmpty, CString.IsNullOrEmpty(output[i]));
                Assert.NotNull(output[i]);
                Assert.True(output[i].IsNullTerminated);
                if (!isNullOrEmpty)
                {
                    Assert.True(output[i].IsReference);
                    Assert.Equal(expectedValue[i], output[i].ToString());
                }
                else
                    Assert.False(output[i].IsReference);
            }
        }

        private static CString[] MixValues(CString[] localValues, CString[] externalValues)
            => localValues.Concat(externalValues)
            .Concat(Enumerable.Repeat<CString>(default, sizeof(Char)))
            .Concat(Enumerable.Repeat<CString>("", sizeof(Char)))
            .OrderBy(x => Guid.NewGuid())
            .ToArray();

        private static Int32 GetExpectedTextLength(CString[] allValues)
            => allValues.Where(x => !CString.IsNullOrEmpty(x)).Select(x => x.Length + 1).Sum() - 1;

        private static Int32 GetExpectedStringLength(int expectedTextLength)
            => expectedTextLength / sizeof(Char) + expectedTextLength % sizeof(Char);
    }
}
