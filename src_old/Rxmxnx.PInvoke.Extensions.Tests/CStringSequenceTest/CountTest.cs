using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringSequenceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class CountTest
    {
        [Fact]
        internal void NormalTest()
        {
            CString[] cstrs = TestUtilities.SharedFixture.Build<String>()
                .CreateMany().Select(x => (CString)x).ToArray();
            CStringSequence sequence = new(cstrs);
            Assert.Equal(cstrs.Length, sequence.Count);
        }
    }
}
