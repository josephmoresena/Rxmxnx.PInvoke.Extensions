using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.InputValueTest
{
    [ExcludeFromCodeCoverage]
    public sealed class ReferenceableTest
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

        /// <summary>
        /// Performs the test for implicit operators and <see cref="NullableValueInput{T}.Instance"/> property.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of object.</typeparam>
        private static void NormalTest<T>(Boolean? nullableInput)
            where T : struct
        {
            T? initialValue = !nullableInput.HasValue || nullableInput.Value ?
                TestUtilities.SharedFixture.Create<T>() :
                default(T?);

            if (!nullableInput.HasValue)
                NormalTest(InputValue.CreateInput(initialValue.Value), initialValue.Value);
            else
                NormalNullableTest(InputValue.CreateInput(initialValue), initialValue);
        }

        private static void NormalTest<T>(IReferenceable<T> valueInput, T initialValue)
            where T : struct
        {
            Assert.Equal(valueInput.Reference, initialValue);
            Assert.True(valueInput.Equals(initialValue));
            Assert.False(Unsafe.AreSame(ref initialValue, ref Unsafe.AsRef(valueInput.Reference)));
            Assert.True(valueInput.Equals(valueInput));
            Assert.False(valueInput.Equals(InputValue.CreateInput(initialValue)));
            Assert.False(valueInput.Equals(default(IReferenceable<T>)));
        }

        private static void NormalNullableTest<T>(IReferenceable<T?> valueInput, T? initialValue)
            where T : struct
        {
            Assert.Equal(valueInput.Reference, initialValue);
            Assert.True(valueInput.Equals(initialValue));
            Assert.False(Unsafe.AreSame(ref initialValue, ref Unsafe.AsRef(valueInput.Reference)));
            Assert.True(valueInput.Equals(valueInput));
            Assert.False(valueInput.Equals(InputValue.CreateInput(initialValue)));
            Assert.False(valueInput.Equals(default(IReferenceable<T>)));
        }
    }
}
