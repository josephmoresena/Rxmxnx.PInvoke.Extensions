using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.ReferenceExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsPointerTest
    {
        [Fact]
        internal void IntPtrTest()
        {
            Byte value = TestUtilities.SharedFixture.Create<Byte>();
            IntPtr result = Unsafe.AsRef(value).AsIntPtr();
            unsafe
            {
                ref Byte unsafeRefValue = ref Unsafe.AsRef<Byte>(result.ToPointer());
                Assert.Equal(unsafeRefValue, value);
                unsafeRefValue = TestUtilities.SharedFixture.Create<Byte>();
                Assert.Equal(unsafeRefValue, value);
            }
        }

        [Fact]
        internal void UIntPtrTest()
        {
            Byte value = TestUtilities.SharedFixture.Create<Byte>();
            UIntPtr result = Unsafe.AsRef(value).AsUIntPtr();
            unsafe
            {
                ref Byte unsafeRefValue = ref Unsafe.AsRef<Byte>(result.ToPointer());
                Assert.Equal(unsafeRefValue, value);
                unsafeRefValue = TestUtilities.SharedFixture.Create<Byte>();
                Assert.Equal(unsafeRefValue, value);
            }
        }
    }
}
