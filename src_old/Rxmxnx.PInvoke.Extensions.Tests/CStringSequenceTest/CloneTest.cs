using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringSequenceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class CloneTest
    {
        [Fact]
        internal void NormalTest()
        {
            CStringSequence sequence1 = new(TestUtilities.SharedFixture.Create<String>(), TestUtilities.SharedFixture.Create<String>());
            CStringSequence sequence2 = (CStringSequence)sequence1.Clone();
            ReadOnlySpan<Char> span1 = sequence1.AsSpan(out CString[] _);
            ReadOnlySpan<Char> span2 = sequence2.AsSpan(out CString[] _);

            Assert.Equal(sequence1, sequence2);
            Assert.False(Unsafe.AreSame(ref Unsafe.AsRef(span1[0]), ref Unsafe.AsRef(span2[0])));
        }
    }
}
