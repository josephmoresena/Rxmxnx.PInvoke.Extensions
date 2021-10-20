using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AutoFixture;

using Xunit;

namespace PInvoke.Extensions.Tests.DelegateExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsPointerTest
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void IntPtrEmptyTest(Boolean useGeneric)
        {
            if (useGeneric)
            {
                GetValue<Boolean> getValueGeneric = default;
                Assert.Equal(IntPtr.Zero, getValueGeneric.AsIntPtr());
            }
            else
            {
                GetByteValue getByteValue = default;
                Assert.Equal(IntPtr.Zero, getByteValue.AsIntPtr());
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void UIntPtrEmptyTest(Boolean useGeneric)
        {
            if (useGeneric)
            {
                GetValue<Boolean> getValueGeneric = default;
                Assert.Equal(UIntPtr.Zero, getValueGeneric.AsUIntPtr());
            }
            else
            {
                GetByteValue getByteValue = default;
                Assert.Equal(UIntPtr.Zero, getByteValue.AsUIntPtr());
            }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void IntPtrNormalTest(Boolean useGeneric)
        {
            GetByteValue getByteValue = TestUtilities.GetValueMethod<Byte>;
            GetValue<Byte> getValue = TestUtilities.GetByteValueMethod;
            IntPtr intPtr = Marshal.GetFunctionPointerForDelegate<GetByteValue>(getByteValue);
            Byte input = TestUtilities.SharedFixture.Create<Byte>();
            if (!useGeneric)
            {
                IntPtr result = getByteValue.AsIntPtr();
                Assert.Equal(intPtr, result);
                Assert.Equal(getValue(input), Marshal.GetDelegateForFunctionPointer<GetByteValue>(result)(input));
            }
            else
                Assert.Throws<ArgumentException>(() => getValue.AsIntPtr());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void UIntPtrNormalTest(Boolean useGeneric)
        {
            GetByteValue getByteValue = TestUtilities.GetValueMethod<Byte>;
            GetValue<Byte> getValue = TestUtilities.GetByteValueMethod;
            IntPtr intPtr = Marshal.GetFunctionPointerForDelegate<GetByteValue>(getByteValue);
            UIntPtr uIntPtr;
            unsafe
            {
                uIntPtr = new UIntPtr(intPtr.ToPointer());
            }
            Byte input = TestUtilities.SharedFixture.Create<Byte>();
            if (!useGeneric)
            {
                UIntPtr result = getByteValue.AsUIntPtr();
                Assert.Equal(uIntPtr, result);
                unsafe
                {
                    Assert.Equal(getValue(input),
                        Marshal.GetDelegateForFunctionPointer<GetByteValue>(new IntPtr(result.ToPointer()))(input));
                }
            }
            else
                Assert.Throws<ArgumentException>(() => getValue.AsIntPtr());
        }
    }
}
