using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AutoFixture;

namespace Rxmxnx.PInvoke.Extensions.Tests
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

        public static readonly Random Random = new();
        public static readonly Fixture SharedFixture = GetFixture();
        public static readonly String LibraryName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? LIBRARYNAME_WINDOWS :
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? LIBRARYNAME_OSX : LIBRARYNAME_LINUX;
        public static readonly String MethodName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? METHODNAME_WINDOWS : METHODNAME_UNIX;

        private static Fixture GetFixture()
        {
            Fixture result = new();
            result.Register<IntPtr>(() => Environment.Is64BitProcess ? new IntPtr(result.Create<Int64>()) : new IntPtr(result.Create<Int32>()));
            result.Register<UIntPtr>(() => Environment.Is64BitProcess ? new UIntPtr(result.Create<UInt64>()) : new UIntPtr(result.Create<UInt32>()));
            result.Register<CString>(() => result.Create<String>());
            return result;
        }

        public static T GetValueMethod<T>(T value) => value;
        public static Byte GetByteValueMethod(Byte value) => GetValueMethod(value);
        public static Byte GetPrintableByte() => Convert.ToByte(Random.Next(32, 128));
        public static T[] AsArray<T>(params T[] args) => args;
        public static GCHandle[] Alloc<T>(IEnumerable<T[]> arr)
            where T : unmanaged
        {
            List<GCHandle> result = new();
            foreach (T[] s in arr)
                result.Add(GCHandle.Alloc(s, GCHandleType.Pinned));
            return result.ToArray();
        }
        public static void Free(IEnumerable<GCHandle> handles)
        {
            foreach (GCHandle handle in handles)
                handle.Free();
        }
        public static CString[] CreateCStrings(IReadOnlyList<GCHandle> handles)
        {
            CString[] result = new CString[handles.Count];
            for (Int32 i = 0; i < handles.Count; i++)
            {
                Byte[] arr = handles[i].Target as Byte[];
                result[i] = new(Marshal.UnsafeAddrOfPinnedArrayElement(arr, 0), arr.Length);
            }
            return result;
        }
        public static Byte[] GetWriting(CString value, Boolean nullEnd)
        {
            using MemoryStream mem = new();
            value.Write(mem, nullEnd);
            return mem.ToArray();
        }
        public static async Task<Byte[]> GetWritingAsync(CString value, Boolean nullEnd)
        {
            using MemoryStream mem = new();
            await value.WriteAsync(mem, nullEnd);
            return mem.ToArray();
        }
    }
}
