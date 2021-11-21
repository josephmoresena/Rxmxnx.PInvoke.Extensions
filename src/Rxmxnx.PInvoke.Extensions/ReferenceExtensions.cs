using System;
using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with memory references to <see cref="ValueType"/> 
    /// <see langword="unmanaged"/> values.
    /// </summary>
    public static class ReferenceExtensions
    {
        /// <summary>
        /// Creates a <see cref="IntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> 
        /// <see langword="unmanaged"/> value.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
        /// <param name="refValue">Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
        /// <returns><see cref="IntPtr"/> pointer.</returns>
        public static IntPtr AsIntPtr<T>(ref this T refValue)
            where T : unmanaged
        {
            unsafe
            {
                return new IntPtr(GetPointerFromRef(ref refValue));
            }
        }

        /// <summary>
        /// Creates a <see cref="UIntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> 
        /// <see langword="unmanaged"/> value.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
        /// <param name="refValue">Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
        /// <returns><see cref="UIntPtr"/> pointer.</returns>
        public static UIntPtr AsUIntPtr<T>(ref this T refValue)
            where T : unmanaged
        {
            unsafe
            {
                return new UIntPtr(GetPointerFromRef(ref refValue));
            }
        }

        /// <summary>
        /// Retrieves the native pointer from a memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
        /// <param name="refValue">Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
        /// <returns>Native pointer.</returns>
        internal static unsafe void* GetPointerFromRef<T>(ref T refValue)
            where T : unmanaged
            => Unsafe.AsPointer<T>(ref refValue);
    }
}
