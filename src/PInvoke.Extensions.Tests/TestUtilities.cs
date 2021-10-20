using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AutoFixture;

namespace PInvoke.Extensions.Tests
{
    internal delegate T GetValue<T>(T value);
    internal delegate Byte GetByteValue(Byte value);
    internal delegate Int32 GetInt32();
    internal delegate T GetT<T>();

    [ExcludeFromCodeCoverage]
    internal static class TestUtilities
    {
        private const String LIBRARYNAME_WINDOWS = "kernel32.dll";
        private const String LIBRARYNAME_OSX = "libSystem.B.dylib";
        private const String LIBRARYNAME_LINUX = "libc.so.6";

        private const String METHODNAME_WINDOWS = "GetCurrentProcessId";
        private const String METHODNAME_UNIX = "getpid";

        public static readonly Fixture SharedFixture = GetFixture();
        public static readonly String LibraryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? LIBRARYNAME_WINDOWS :
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? LIBRARYNAME_OSX : LIBRARYNAME_LINUX;
        public static readonly String MethodName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? METHODNAME_WINDOWS : METHODNAME_UNIX;

        private static Fixture GetFixture()
        {
            Fixture result = new();
            result.Register<IntPtr>(() => Environment.Is64BitProcess ? new IntPtr(result.Create<Int64>()) : new IntPtr(result.Create<Int32>()));
            result.Register<UIntPtr>(() => Environment.Is64BitProcess ? new UIntPtr(result.Create<UInt64>()) : new UIntPtr(result.Create<UInt32>()));
            return result;
        }

        public static T GetValueMethod<T>(T value) => value;
        public static Byte GetByteValueMethod(Byte value) => GetValueMethod(value);
    }
}
