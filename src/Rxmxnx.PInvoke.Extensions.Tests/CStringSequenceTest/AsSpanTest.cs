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
            Random random = new Random();
            CString[] localValues = TestUtilities.SharedFixture.CreateMany<String>(100).Select(x =>
            {
                Int32 start = random.Next(0, x.Length);
                Int32 end = random.Next(start, x.Length);
                return (CString)x[start..end];
            }).ToArray();
            String[] strValue = TestUtilities.SharedFixture.CreateMany<String>(100).Select(x =>
            {
                Int32 start = random.Next(0, x.Length);
                Int32 end = random.Next(start, x.Length);
                return x[start..end];
            }).ToArray();
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
            TextTest(allValues, expectedValue, output);
            ConcatTest(sequence);
        }

        private static void TextTest(CString[] allValues, String[] expectedValue, CString[] output)
        {
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

        private static void ConcatTest(CStringSequence sequence)
        {
            ReadOnlySpan<Char> buffer = sequence.AsSpan(out CString[] output);
            Int32[] lengths = output.Select(x => x.Length).ToArray();
            CString join = output.Concat();
            ReadOnlySpan<Byte> span = join;
            CString[] output2 = GetCString(span, lengths);
            CStringSequence sequence2 = new(output2);
            ReadOnlySpan<Char> buffer2 = sequence2.AsSpan(out CString[] output3);

            Assert.Equal(sequence, sequence2);
            for (Int32 i = 0; i < lengths.Length; i++)
            {
                Assert.Equal(output[i], output2[i]);
                Assert.Equal(output[i], output3[i]);
                Assert.True(output[i].IsNullTerminated);
                Assert.True(output3[i].IsNullTerminated);
            }
        }

        private static CString[] GetCString(ReadOnlySpan<Byte> span, Int32[] lengths)
        {
            IntPtr ptr = span.AsIntPtr();
            CString[] result = new CString[lengths.Length];
            Int32 offset = 0;
            for (Int32 i = 0; i < lengths.Length; i++)
            {
                result[i] = new(ptr + offset, lengths[i]);
                offset += lengths[i];
            }
            return result;
        }

        private static CString[] MixValues(CString[] localValues, CString[] externalValues)
            => localValues.Concat(externalValues)
            .Concat(Enumerable.Repeat<CString>(default, sizeof(Char)))
            .Concat(Enumerable.Repeat<CString>("", sizeof(Char)))
            .OrderBy(x => Guid.NewGuid())
            .ToArray();

        private static Int32 GetExpectedTextLength(CString[] allValues)
            => allValues.Where(x => !CString.IsNullOrEmpty(x)).Select(x => x.Length + 1).Sum();

        private static Int32 GetExpectedStringLength(Int32 expectedTextLength)
            => (expectedTextLength / sizeof(Char)) + (expectedTextLength % sizeof(Char));
    }
}
