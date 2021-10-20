using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AutoFixture;

using Xunit;

namespace PInvoke.Extensions.Tests.NativeUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class GetNativeMethodTest
    {
        [SkippableTheory]
        [InlineData(true, false)]
        [InlineData(false, true)] //Not works on Linux
        [InlineData(true, true)]
        [InlineData(false, false)]
        internal void EmptyTest(Boolean zeroPtr, Boolean generic)
        {
            String prefix = TestUtilities.SharedFixture.Create<String>();
            String sufix = TestUtilities.SharedFixture.Create<String>();
            IntPtr handle = !zeroPtr ? TestUtilities.SharedFixture.Create<IntPtr>() : IntPtr.Zero;
            String name = prefix + TestUtilities.MethodName + sufix;
            Delegate result;
            Skip.If(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !zeroPtr, "Linux aborts the process with fake lib handle.");
            if (!generic)
                result = NativeUtilities.GetNativeMethod<GetInt32>(handle, name);
            else
                result = NativeUtilities.GetNativeMethod<GetT<Int32>>(handle, name);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalTest(Boolean generic)
        {
            IntPtr handle = NativeLibrary.Load(TestUtilities.LibraryName);

            if (!generic)
            {
                GetInt32 result = NativeUtilities.GetNativeMethod<GetInt32>(handle, TestUtilities.MethodName);
                Assert.NotNull(result);
                Assert.Equal(Environment.ProcessId, result());
            }
            else
            {
                Assert.Throws<ArgumentException>(() => NativeUtilities.GetNativeMethod<GetT<Int32>>(handle, TestUtilities.MethodName));
            }
            NativeLibrary.Free(handle);
        }
    }
}
