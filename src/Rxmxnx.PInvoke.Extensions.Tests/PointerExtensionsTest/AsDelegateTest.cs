using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.PointerExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsDelegateTest
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void IntPtrEmptyTest(Boolean useGeneric)
        {
            if (useGeneric)
                Assert.Null(IntPtr.Zero.AsDelegate<GetValue<Byte>>());
            else
                Assert.Null(IntPtr.Zero.AsDelegate<GetByteValue>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void UIntPtrEmptyTest(Boolean useGeneric)
        {
            if (useGeneric)
                Assert.Null(UIntPtr.Zero.AsDelegate<GetValue<Byte>>());
            else
                Assert.Null(UIntPtr.Zero.AsDelegate<GetByteValue>());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void IntPtrNormalTest(Boolean useGeneric)
        {
            IntPtr intPtr = Marshal.GetFunctionPointerForDelegate<GetByteValue>(TestUtilities.GetByteValueMethod);
            Byte input = TestUtilities.SharedFixture.Create<Byte>();
            if (!useGeneric)
                Assert.Equal(TestUtilities.GetValueMethod(input), intPtr.AsDelegate<GetByteValue>()(input));
            else
                Assert.Throws<ArgumentException>(() => intPtr.AsDelegate<GetValue<Byte>>()(input));
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        internal void UIntPtrNormalTest(Boolean useGeneric)
        {
            IntPtr intPtr = Marshal.GetFunctionPointerForDelegate<GetByteValue>(TestUtilities.GetByteValueMethod);
            UIntPtr uIntPtr;
            unsafe
            {
                uIntPtr = new UIntPtr(intPtr.ToPointer());
            }
            Byte input = TestUtilities.SharedFixture.Create<Byte>();
            if (!useGeneric)
                Assert.Equal(TestUtilities.GetValueMethod(input), uIntPtr.AsDelegate<GetByteValue>()(input));
            else
                Assert.Throws<ArgumentException>(() => uIntPtr.AsDelegate<GetValue<Byte>>()(input));
        }
    }
}
