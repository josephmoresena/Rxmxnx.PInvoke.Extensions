using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace PInvoke.Extensions.Tests.ReferenceExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public class AsPointerTest
    {
        [Fact]
        internal void IntPtrTest()
        {
            Byte value = TestUtilities.SharedFixture.Create<Byte>();
            ref Byte refValue = ref value;
            IntPtr result = refValue.AsIntPtr();
            unsafe
            {
                ref Byte unsafeRefValue = ref AsReference<Byte>(result.ToPointer());
                Assert.Equal(unsafeRefValue, value);
                unsafeRefValue = TestUtilities.SharedFixture.Create<Byte>();
                Assert.Equal(unsafeRefValue, value);
            }
        }

        [Fact]
        internal void UIntPtrTest()
        {
            Byte value = TestUtilities.SharedFixture.Create<Byte>();
            ref Byte refValue = ref value;
            UIntPtr result = refValue.AsUIntPtr();
            unsafe
            {
                ref Byte unsafeRefValue = ref AsReference<Byte>(result.ToPointer());
                Assert.Equal(unsafeRefValue, value);
                unsafeRefValue = TestUtilities.SharedFixture.Create<Byte>();
                Assert.Equal(unsafeRefValue, value);
            }
        }

        private unsafe static ref T AsReference<T>(void* ptr)
            where T : unmanaged
        {
            unsafe
            {
                return ref Unsafe.AsRef<T>(ptr);
            }
        }
    }
}
