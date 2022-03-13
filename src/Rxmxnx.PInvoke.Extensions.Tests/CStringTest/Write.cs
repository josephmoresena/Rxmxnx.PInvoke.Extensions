using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class WriteTest : CStringBaseTest
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void LocalNormalTest(Boolean nullEnd)
        {
            CString value = TestUtilities.SharedFixture.Create<CString>();
            Byte[] bytes = CString.GetBytes(value);
            NormalTest(value, bytes, nullEnd);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void ReferenceNormalTest(Boolean nullEndInput, Boolean nullEnd)
        {
            Byte[] text = nullEndInput ?
                Encoding.UTF8.GetBytes(TestUtilities.SharedFixture.Create<String>()) :
                CreateUtf8StringNulTerminated();
            ReadOnlySpan<Byte> span = text;
            CString value = new(span.AsIntPtr(), text.Length);
            Byte[] bytes = value.ToArray();
            NormalTest(value, bytes, nullEnd);
        }

        private static void NormalTest(CString value, Byte[] bytes, Boolean nullEnd)
        {
            Byte[] result = TestUtilities.GetWriting(value, nullEnd);

            Assert.Equal(value.Length + (nullEnd ? 1 : 0), result.Length);
            for (Int32 i = 0; i < value.Length; i++)
                Assert.Equal(result[i], bytes[i]);
        }
    }
}
