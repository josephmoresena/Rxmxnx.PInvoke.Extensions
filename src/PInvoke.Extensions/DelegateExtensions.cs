using System;
using System.Runtime.InteropServices;

namespace PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="Delegate"/> instances.
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// Creates a <see cref="IntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> delegate.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
        /// <param name="delegateInstance"><typeparamref name="T"/> delegate.</param>
        /// <returns><see cref="IntPtr"/> pointer.</returns>
        public static IntPtr AsIntPtr<T>(this T delegateInstance)
            where T : Delegate
            => delegateInstance != default ? Marshal.GetFunctionPointerForDelegate<T>(delegateInstance) : default;

        /// <summary>
        /// Creates a <see cref="UIntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> delegate.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
        /// <param name="delegateInstance"><typeparamref name="T"/> delegate.</param>
        /// <returns><see cref="UIntPtr"/> pointer.</returns>
        public static UIntPtr AsUIntPtr<T>(this T delegateInstance)
            where T : Delegate
            => delegateInstance.AsIntPtr().AsUIntPtr();
    }
}
