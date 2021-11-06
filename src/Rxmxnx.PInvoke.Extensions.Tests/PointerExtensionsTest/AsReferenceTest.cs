using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.PointerExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsReferenceTest
    {
        [Fact]
        internal void IntPtrTest()
        {
            Byte value = TestUtilities.SharedFixture.Create<Byte>();
            IntPtr refIntPtr;
            unsafe
            {
                refIntPtr = new IntPtr(GetPointerFromRef(ref value));
            }
            ref Byte refValue = ref refIntPtr.AsReference<Byte>();
            Assert.Equal(value, refValue);
            refValue = TestUtilities.SharedFixture.Create<Byte>();
            Assert.Equal(value, refValue);
        }

        [Fact]
        internal void UIntPtrTest()
        {
            Byte value = TestUtilities.SharedFixture.Create<Byte>();
            UIntPtr refUIntPtr;
            unsafe
            {
                refUIntPtr = new UIntPtr(GetPointerFromRef(ref value));
            }
            ref Byte refValue = ref refUIntPtr.AsReference<Byte>();
            Assert.Equal(value, refValue);
            refValue = TestUtilities.SharedFixture.Create<Byte>();
            Assert.Equal(value, refValue);
        }

        internal static unsafe void* GetPointerFromRef<T>(ref T refValue)
            => Unsafe.AsPointer<T>(ref refValue);
    }
}
