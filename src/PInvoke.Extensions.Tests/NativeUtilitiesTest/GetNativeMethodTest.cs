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
        [InlineData(true, false, true)]
        [InlineData(false, true, true)] //Not works on Linux
        [InlineData(true, true, true)]
        [InlineData(false, false, true)]
        [InlineData(false, true, true, true)]
        [InlineData(false, false, true, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, false, true)]
        internal void EmptyTest(Boolean zeroPtr, Boolean generic, Boolean emptyName = false, Boolean useRealHandle = false)
        {
            String prefix = TestUtilities.SharedFixture.Create<String>();
            String sufix = TestUtilities.SharedFixture.Create<String>();
            IntPtr handle = !zeroPtr ?
                !useRealHandle ? TestUtilities.SharedFixture.Create<IntPtr>() : NativeLibrary.Load(TestUtilities.LibraryName)
                : IntPtr.Zero;
            String name = !emptyName? prefix + TestUtilities.MethodName + sufix : default;
            Delegate result;
            Skip.If(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && !zeroPtr && !useRealHandle,
                "Linux aborts the process with fake lib handle.");
            if (!generic)
                result = NativeUtilities.GetNativeMethod<GetInt32>(handle, name);
            else
                result = NativeUtilities.GetNativeMethod<GetT<Int32>>(handle, name);
            Assert.Null(result);
            if (!zeroPtr && useRealHandle)
                NativeLibrary.Free(handle);
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
