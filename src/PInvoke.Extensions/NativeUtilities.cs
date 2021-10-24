using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of utilities for exchange data within the P/Invoke context.
    /// </summary>
    public static class NativeUtilities
    {
        /// <summary>
        /// Retrieves the size of <typeparamref name="T"/> structure.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <returns>Size of <typeparamref name="T"/> structure.</returns>
        public static Int32 SizeOf<T>()
            where T : unmanaged
            => Unsafe.SizeOf<T>();

        /// <summary>
        /// Provides a high-level API for loading a native library.
        /// </summary>
        /// <param name="libraryName">The name of the native library to be loaded.</param>
        /// <param name="searchPath">The search path.</param>
        /// <returns>The OS handle for the loaded native library.</returns>
        public static IntPtr? LoadNativeLib(String libraryName, DllImportSearchPath? searchPath = default)
        {
            if (NativeLibrary.TryLoad(libraryName ?? String.Empty, Assembly.GetExecutingAssembly(), searchPath, out IntPtr handle))
                return handle;
            return default;
        }

        /// <summary>
        /// Provides a high-level API for loading a native library.
        /// </summary>
        /// <param name="libraryName">The name of the native library to be loaded.</param>
        /// <param name="unloadEvent">
        /// <see cref="EventHandler"/> delegate to append <see cref="NativeLibrary.Free(IntPtr)"/> invocation.
        /// </param>
        /// <param name="searchPath">The search path.</param>
        /// <returns>The OS handle for the loaded native library.</returns>
        public static IntPtr? LoadNativeLib(String libraryName, ref EventHandler unloadEvent, DllImportSearchPath? searchPath = default)
        {
            IntPtr? handle = LoadNativeLib(libraryName, searchPath);
            if (handle.HasValue)
                unloadEvent += (sender, e) => NativeLibrary.Free(handle.Value);
            return handle;
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> delegate of an exported symbol.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle">The native library OS handle.</param>
        /// <param name="name">The name of the exported symbol.</param>
        /// <returns><typeparamref name="T"/> delegate.</returns>
        public static T GetNativeMethod<T>(IntPtr handle, String name)
            where T : Delegate
        {
            if (!handle.IsZero() && NativeLibrary.TryGetExport(handle, name ?? String.Empty, out IntPtr address))
                return address.AsDelegate<T>();
            return default;
        }

        /// <summary>
        /// Retrieves a <see cref="Byte"/> array from the given <typeparamref name="T"/> value.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="value"><typeparamref name="T"/> value.</param>
        /// <returns><see cref="Byte"/> array.</returns>
        public static Byte[] AsBytes<T>(in T value)
            where T : unmanaged
            => Unsafe.AsRef(value).AsIntPtr().AsReadOnlySpan<Byte>(NativeUtilities.SizeOf<T>()).ToArray();
    }
}
