using System;
using System.Buffers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rxmxnx.PInvoke.Extensions
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
        public static Int32 SizeOf<T>() where T : unmanaged
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
        public static T? GetNativeMethod<T>(IntPtr handle, String name) where T : Delegate
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
        public static Byte[] AsBytes<T>(in T value) where T : unmanaged
        {
            unsafe
            {
                Byte[] result = new Byte[sizeof(T)];
                void* pointer = Unsafe.AsPointer(ref Unsafe.AsRef(value));
                new ReadOnlySpan<Byte>(pointer, result.Length).CopyTo(result);
                return result;
            }
        }
        /// <summary>
        /// Preforms a binary copy of <paramref name="value"/> to <paramref name="destination"/> span.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="value"><typeparamref name="T"/> value.</param>
        /// <param name="destination">Destination <see cref="Span{T}"/> instance.</param>
        /// <param name="offset">
        /// The offset in <paramref name="destination"/> at which <paramref name="value"/> copying begins.
        /// </param>
        /// <exception cref="ArgumentException"/>
        public static void BinaryCopyTo<T>(in T value, Span<Byte> destination, Int32 offset = 0) where T : unmanaged
        {
            unsafe
            {
                Int32 typeSize = sizeof(T);
                if (destination.Length - offset < typeSize)
                    throw new ArgumentException($"Insufficient available size on {nameof(destination)} to copy {nameof(value)}.");
                void* pointer = Unsafe.AsPointer(ref Unsafe.AsRef(value));
                new ReadOnlySpan<Byte>(pointer, typeSize).CopyTo(destination[offset..]);
            }
        }

        /// <summary>
        /// Creates a new <typeparamref name="T"/> array with a specific length and initializes it after 
        /// creation by using the specified callback.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
        /// <param name="length">The length of the array to create.</param>
        /// <param name="state">The type of the element to pass to <paramref name="action"/>.</param>
        /// <param name="action">A callback to initialize the array.</param>
        /// <returns>The created array.</returns>
        public static T[] CreateArray<T, TState>(Int32 length, TState state, SpanAction<T, TState> action) where T : unmanaged
        {
            T[] result = new T[length];
            Span<T> span = result;
            WriteSpan(span, state, action);
            return result;
        }

        /// <summary>
        /// Creates a new string with a specific length and initializes it after creation by using 
        /// the specified callback.
        /// </summary>
        /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
        /// <param name="length">The length of the string to create.</param>
        /// <param name="state">The type of the element to pass to <paramref name="action"/>.</param>
        /// <param name="action">A callback to initialize the string.</param>
        /// <returns>The created string.</returns>
        public static String CreateString<TState>(Int32 length, TState state, SpanAction<Char, TState> action)
            => String.Create(length, state, (chars, state) => WriteSpan(chars, state, action));

        /// <summary>
        /// Writes <paramref name="span"/> using <paramref name="state"/> and <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="T">Unmanaged type of elements in <paramref name="span"/>.</typeparam>
        /// <typeparam name="TState">Type of state object.</typeparam>
        /// <param name="span">A <typeparamref name="TState"/> writable memory block.</param>
        /// <param name="state">A <typeparamref name="TState"/> instance.</param>
        /// <param name="action">A <see cref="SpanAction{T, TState}"/> delegate.</param>
        private static void WriteSpan<T, TState>(Span<T> span, TState state, SpanAction<T, TState> action) where T : unmanaged
        {
            unsafe
            {
                fixed (T* prt = &MemoryMarshal.GetReference(span))
                    action(new(prt, span.Length), state);
            }
        }
    }
}
