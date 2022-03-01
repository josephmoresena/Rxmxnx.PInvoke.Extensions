using System;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringSequenceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class EqualityTest
    {
        [Fact]
        internal void NormalTest()
        {
            String value = TestUtilities.SharedFixture.Create<String>();
            CStringSequence sequence1 = new(value);
            CStringSequence sequence2 = new(value, default);
            CStringSequence sequence3 = new(default, value);
            CStringSequence sequence4 = new(value);

            Assert.Equal(sequence1.ToString(), sequence2.ToString());
            Assert.Equal(sequence1.GetHashCode(), sequence2.GetHashCode());
            Assert.False(sequence1.Equals(sequence2));
            Assert.False(sequence1.Equals((Object)sequence2));

            Assert.Equal(sequence1.ToString(), sequence3.ToString());
            Assert.Equal(sequence1.GetHashCode(), sequence3.GetHashCode());
            Assert.False(sequence1.Equals(sequence3));
            Assert.False(sequence1.Equals((Object)sequence3));

            Assert.Equal(sequence1.ToString(), sequence4.ToString());
            Assert.Equal(sequence1.GetHashCode(), sequence4.GetHashCode());
            Assert.True(sequence1.Equals(sequence4));
            Assert.True(sequence1.Equals((Object)sequence4));
        }
    }
}
