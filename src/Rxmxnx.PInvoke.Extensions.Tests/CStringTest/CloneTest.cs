using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public sealed class CloneTest : CStringBaseTest
    {
        [Fact]
        internal void NormalTest()
        {
            String local = TestUtilities.SharedFixture.Create<String>();
            ReadOnlySpan<Byte> external = TestUtilities.AsArray(TestUtilities.GetPrintableByte(), TestUtilities.GetPrintableByte(), TestUtilities.GetPrintableByte(), default);

            CString clocal = local;
            CString cexternal = new(external.AsIntPtr(), external.Length);

            AssertClone(clocal, (CString)clocal.Clone());
            AssertClone(cexternal, (CString)cexternal.Clone());
        }

        private static void AssertClone(CString value, CString valueClone)
        {
            Assert.False(valueClone.IsReference);
            Assert.Equal(value.Length, valueClone.Length);

            ReadOnlySpan<Byte> span = value;
            ReadOnlySpan<Byte> spanClone = valueClone;

            for (Int32 i = 0; i < value.Length; i++)
            {
                Assert.Equal(value[i], valueClone[i]);
                Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(span[i]), ref Unsafe.AsRef(spanClone[i])));
            }
        }
    }
}
