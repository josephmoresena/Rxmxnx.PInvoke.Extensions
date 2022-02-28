using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringSequenceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ToStringTest
    {
        [Fact]
        internal void NormalTest()
        {
            CStringSequence sequence = new(
                TestUtilities.SharedFixture.Create<String>(), TestUtilities.SharedFixture.Create<String>(),
                TestUtilities.SharedFixture.Create<String>(), TestUtilities.SharedFixture.Create<String>());

            ReadOnlySpan<Char> span = sequence.AsSpan(out CString[] cstrs);
            String toString = sequence.ToString();
            String strBuild = GetExpectedString(cstrs);
            Int32 expectedLength = span.Length * sizeof(Char);
            CString spanAsCString = new CString(span.AsIntPtr(), expectedLength);

            Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(span[0]), ref Unsafe.AsRef(toString.AsSpan()[0])));
            Assert.NotEqual(strBuild, toString);
            Assert.Equal(strBuild, spanAsCString.ToString());
        }

        private static String GetExpectedString(CString[] cstrs)
        {
            StringBuilder strBuild = new();
            foreach (CString cstr in cstrs)
            {
                strBuild.Append(cstr);
                strBuild.Append(default(Char));
            }
            strBuild.Remove(strBuild.Length - 1, 1);
            return strBuild.ToString();
        }
    }
}
