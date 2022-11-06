using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.UnmanagedValueExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsValuesTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalBooleanBooleanTest(Boolean? input)
            => SimpleTest<Boolean, Boolean>(input);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalBooleanByteTest(Boolean? input)
            => SimpleTest<Boolean, Byte>(input);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalByteInt16Test(Boolean? input)
            => SimpleTest<Byte, Int16>(input);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalInt16IntByteTest(Boolean? input)
            => SimpleTest<Int16, Byte>(input);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalGuidInt32Test(Boolean? input)
            => SimpleTest<Guid, Int32>(input);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalByteGuidTest(Boolean? input)
            => SimpleTest<Byte, Guid>(input);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalGuidCharTest(Boolean? input)
            => SimpleTest<Guid, Char>(input);

        private static void SimpleTest<TFrom, TTo>(Boolean? input)
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            TFrom[] inputArray = input.HasValue ? input.Value ?
                TestUtilities.SharedFixture.CreateMany<TFrom>(120).ToArray() : Array.Empty<TFrom>() : default;
            TTo[] result = inputArray.AsValues<TFrom, TTo>();
            if (!input.HasValue)
                Assert.Null(result);
            else if (!input.Value)
                Assert.Empty(result);
            else
            {
                Assert.NotNull(result);
                if (typeof(TFrom) == typeof(TTo))
                {
                    Assert.Equal(inputArray.Length, result.Length);
                    Assert.Equal(inputArray, result.Select(v => v is TFrom from ? from : default).ToArray());
                }
                else
                    unsafe
                    {
                        Int32 length = inputArray.Length * sizeof(TFrom);
                        Int32 step = sizeof(TTo);
                        Int32 count = length / step;
                        Int32 module = length % step;
                        Assert.Equal(count + (module != 0 ? 1 : 0), result.Length);
                    }
            }
        }
    }
}
