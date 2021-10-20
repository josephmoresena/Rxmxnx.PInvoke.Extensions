using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AutoFixture;

using Xunit;

namespace PInvoke.Extensions.Tests.NativeUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class LoadNativeLibTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(true, true)]
        [InlineData(false, true)]
        internal void EmptyTest(Boolean unloadEvent, Boolean emptyName = false)
        {
            String prefix = TestUtilities.SharedFixture.Create<String>();
            String sufix = TestUtilities.SharedFixture.Create<String>();
            IntPtr? result = default;
            EventHandler eventHandler = default;
            String libraryName = !emptyName ? prefix + TestUtilities.LibraryName + sufix : default;
            if (unloadEvent)
            {
                result = NativeUtilities.LoadNativeLib(libraryName, ref eventHandler);
                Assert.Null(eventHandler);
            }
            else
                result = NativeUtilities.LoadNativeLib(libraryName);
            Assert.Null(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        internal void NormalTest(Boolean unloadEvent)
        {
            IntPtr? result = default;
            EventHandler eventHandler = default;
            if (unloadEvent)
                result = NativeUtilities.LoadNativeLib(TestUtilities.LibraryName, ref eventHandler);
            else
                result = NativeUtilities.LoadNativeLib(TestUtilities.LibraryName);
            Assert.NotNull(result);
            if (unloadEvent)
            {
                Assert.NotNull(eventHandler);
                eventHandler(null, null);
            }
            else
                NativeLibrary.Free(result.Value);
        }
    }
}
