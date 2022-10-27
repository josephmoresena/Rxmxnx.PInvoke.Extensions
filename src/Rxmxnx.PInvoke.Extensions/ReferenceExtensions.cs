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
        public static IntPtr AsIntPtr<T>(ref this T refValue) where T : unmanaged
        {
            unsafe
            {
                return new IntPtr(Unsafe.AsPointer(ref refValue));
            }
        }

        /// <summary>
        /// Creates a <see cref="UIntPtr"/> pointer from a memory reference to a <typeparamref name="T"/> 
        /// <see langword="unmanaged"/> value.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
        /// <param name="refValue">Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
        /// <returns><see cref="UIntPtr"/> pointer.</returns>
        public static UIntPtr AsUIntPtr<T>(ref this T refValue) where T : unmanaged
        {
            unsafe
            {
                return new UIntPtr(Unsafe.AsPointer(ref refValue));
            }
        }

        /// <summary>
        /// Creates a memory reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value from 
        /// an exising memory reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
        /// </summary>
        /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
        /// <typeparam name="TDestination"><see cref="ValueType"/> of the destination reference.</typeparam>
        /// <param name="refValue">Memory reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
        /// <returns>A memory reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value.</returns>
        /// <exception cref="InvalidOperationException"/>
        public static ref TDestination AsReferenceOf<TSource, TDestination>(this ref TSource refValue)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                if (sizeof(TDestination) != sizeof(TSource))
                    throw new InvalidOperationException("The sizes of both source and destination unmanaged types must be equal.");

                return ref Unsafe.AsRef<TDestination>(Unsafe.AsPointer(ref refValue));
            }
        }
        /// <summary>
        /// Creates a <see cref="Span{T}"/> from an exising memory reference to a <typeparamref name="TSource"/> 
        /// <see langword="unmanaged"/> value.
        /// </summary>
        /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
        /// <param name="refValue">>Memory reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
        /// <returns>
        /// A <see cref="Span{T}"/> from an exising memory reference to a <typeparamref name="TSource"/> 
        /// <see langword="unmanaged"/> value.
        /// </returns>
        public static Span<Byte> AsBinarySpan<TSource>(this ref TSource refValue) where TSource : unmanaged
        {
            unsafe
            {
                return new(Unsafe.AsPointer(ref refValue), sizeof(TSource));
            }
        }
    }
}