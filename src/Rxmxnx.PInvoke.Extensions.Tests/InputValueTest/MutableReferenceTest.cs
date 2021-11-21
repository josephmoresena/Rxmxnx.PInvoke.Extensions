using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.InputValueTest
{
    [ExcludeFromCodeCoverage]
    public sealed class MutableReferenceTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void BooleanTest(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<Boolean>(nullableInput, nullableNew);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void ByteTest(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<Boolean>(nullableInput, nullableNew);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void Int16Test(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<Boolean>(nullableInput, nullableNew);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void Int32Test(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<Boolean>(nullableInput, nullableNew);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void DateTimeTest(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<DateTime>(nullableInput, nullableNew);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void Int64Test(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<Int64>(nullableInput, nullableNew);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void UInt64Test(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<UInt64>(nullableInput, nullableNew);

        [Theory]
        [InlineData(null)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void GuidTest(Boolean? nullableInput, Boolean nullableNew = default) => NormalTest<Guid>(nullableInput, nullableNew);

        private static void NormalTest<T>(Boolean? nullableInput, Boolean nullableNew)
            where T : struct
        {
            T? initialValue = !nullableInput.HasValue || nullableInput.Value ?
                TestUtilities.SharedFixture.Create<T>() :
                default(T?);
            T? newValue = !nullableNew ? TestUtilities.SharedFixture.Create<T>() : default(T?);

            if (!nullableInput.HasValue)
                NormalTest(InputValue.CreateReference(initialValue.Value), initialValue.Value, newValue.Value);
            else
                NormalNullableTest(InputValue.CreateReference(initialValue), initialValue, newValue);
        }

        private static void NormalTest<T>(IMutableReference<T> reference, T initialValue, T newValue)
            where T : struct
        {
            Assert.Equal(initialValue, reference.GetInstance());
            Assert.True(reference.Equals(initialValue));
            Assert.False(Unsafe.AreSame(ref initialValue, ref Unsafe.AsRef(reference.Reference)));
            reference.SetInstance(newValue);
            Assert.Equal(newValue, reference.Reference);
        }

        private static void NormalNullableTest<T>(IMutableReference<T?> reference, T? initialValue, T? newValue)
            where T : struct
        {
            Assert.Equal(initialValue, reference.GetInstance());
            Assert.True(reference.Equals(initialValue));
            Assert.False(Unsafe.AreSame(ref initialValue, ref Unsafe.AsRef(reference.Reference)));
            reference.SetInstance(newValue);
            Assert.Equal(newValue, reference.Reference);
        }
    }
}
