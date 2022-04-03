using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.InputValueTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ReferenceableWrapperTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void BooleanTest(Boolean? nullableInput) => NormalTest<Boolean>(nullableInput);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void ByteTest(Boolean? nullableInput) => NormalTest<Boolean>(nullableInput);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void Int16Test(Boolean? nullableInput) => NormalTest<Boolean>(nullableInput);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void Int32Test(Boolean? nullableInput) => NormalTest<Boolean>(nullableInput);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void DateTimeTest(Boolean? nullableInput) => NormalTest<DateTime>(nullableInput);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void Int64Test(Boolean? nullableInput) => NormalTest<Int64>(nullableInput);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void UInt64Test(Boolean? nullableInput) => NormalTest<UInt64>(nullableInput);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        internal void GuidTest(Boolean? nullableInput) => NormalTest<Guid>(nullableInput);

        private static void NormalTest<T>(Boolean? nullableInput)
            where T : struct
        {
            T? initialValue = !nullableInput.HasValue || nullableInput.Value ?
                TestUtilities.SharedFixture.Create<T>() :
                default(T?);

            if (!nullableInput.HasValue)
                NormalValueTest(InputValue.CreateInput(initialValue.Value), initialValue.Value);
            else
                NormalNullableTest(InputValue.CreateInput(initialValue), initialValue);
        }

        private static void NormalValueTest<T>(IReferenceableWrapper<T> valueInput, T initialValue)
            where T : struct
        {
            GeneralTest(valueInput, ref initialValue);
            Assert.False(valueInput.Equals(InputValue.CreateInput(initialValue)));
            Assert.False(valueInput.Equals(default(IReferenceable<T>)));
            Assert.False(valueInput.Equals(default(IWrapper<T>)));
        }

        private static void NormalNullableTest<T>(IReferenceableWrapper<T?> valueInput, T? initialValue)
            where T : struct
        {
            GeneralTest(valueInput, ref initialValue);
            Assert.False(valueInput.Equals(InputValue.CreateInput(initialValue)));
            Assert.False(valueInput.Equals(default(IReferenceable<T>)));
            Assert.False(valueInput.Equals(default(IWrapper<T>)));
        }

        private static void GeneralTest<T>(IReferenceableWrapper<T> valueInput, ref T initialValue)
        {
            Assert.Equal(initialValue, valueInput.Value);
            Assert.True(valueInput.Equals(initialValue));
            Assert.False(Unsafe.AreSame(ref initialValue, ref Unsafe.AsRef(valueInput.Reference)));
            Assert.True(valueInput.Equals(valueInput));
        }
    }
}
