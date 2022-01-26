using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class WriteAsyncTest : CStringBaseTest
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task LocalNormalTest(Boolean nullEnd)
        {
            CString value = TestUtilities.SharedFixture.Create<CString>();
            Byte[] bytes = CString.GetBytes(value);
            await NormalTest(value, bytes, nullEnd);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public async Task ReferenceNormalTest(Boolean nullEndInput, Boolean nullEnd)
        {
            Byte[] text = nullEndInput ?
                Encoding.UTF8.GetBytes(TestUtilities.SharedFixture.Create<String>()) :
                CreateUtf8StringNulTerminated();
            GCHandle handle = GCHandle.Alloc(text, GCHandleType.Pinned);
            try
            {
                CString value = new(handle.AddrOfPinnedObject(), text.Length);
                Byte[] bytes = value.ToArray();
                await NormalTest(value, bytes, nullEnd);
            }
            finally
            {
                handle.Free();
            }
        }

        private static async Task NormalTest(CString value, Byte[] bytes, Boolean nullEnd)
        {
            Byte[] result = await TestUtilities.GetWritingAsync(value, nullEnd);

            Assert.Equal(value.Length + (nullEnd ? 1 : 0), result.Length);
            for (Int32 i = 0; i < value.Length; i++)
                Assert.Equal(result[i], bytes[i]);
        }
    }
}
