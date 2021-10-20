using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using AutoFixture;

using Xunit;

namespace PInvoke.Extensions.Tests.BinaryExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsHexStringTest
    {
        [Fact]
        internal void NormalTest()
        {
            Byte[] input = TestUtilities.SharedFixture.Create<Byte[]>();
            StringBuilder strBuild = new();
            foreach (Byte value in input)
            {
                Assert.Equal(value.ToString("X2").ToLower(), value.AsHexString());
                strBuild.Append(value.ToString("X2"));
            }
            Assert.Equal(strBuild.ToString().ToLower(), input.AsHexString());
        }
    }
}
