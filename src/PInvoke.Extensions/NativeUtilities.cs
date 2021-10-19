using System;
using System.Reflection;
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
        {
            unsafe
            {
                return sizeof(T);
            }
        }

        /// <summary>
        /// Provides a high-level API for loading a native library.
        /// </summary>
        /// <param name="libraryName">The name of the native library to be loaded.</param>
        /// <param name="autoUnload">Indicates whether the native library should auto-unloaded.</param>
        /// <param name="searchPath">The search path.</param>
        /// <returns>The OS handle for the loaded native library.</returns>
        public static IntPtr? LoadNativeLib(
            String libraryName, Boolean autoUnload = false, DllImportSearchPath? searchPath = default)
        {
            if (NativeLibrary.TryLoad(libraryName, Assembly.GetExecutingAssembly(), searchPath, out IntPtr handle))
            {
                if (autoUnload)
                    AppDomain.CurrentDomain.DomainUnload += (sender, e) => NativeLibrary.Free(handle);
                return handle;
            }
            return default;
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
            if (!handle.IsZero() && !String.IsNullOrEmpty(name) && NativeLibrary.TryGetExport(handle, name, out IntPtr address))
                return address.AsDelegate<T>();
            return default;
        }
    }
}
