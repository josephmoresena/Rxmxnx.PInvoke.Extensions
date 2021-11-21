using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rxmxnx.PInvoke.Extensions.Tests
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="IntPtr"/> and <see cref="UIntPtr"/> instances.
    /// </summary>
    public static class PointerExtensions
    {
        /// <summary>
        /// Indicates whether the <see cref="IntPtr"/> pointer is a <see langword="null"/> memory reference.
        /// </summary>
        /// <param name="ptr"><see cref="IntPtr"/> pointer.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="IntPtr"/> instance is a <see langword="null"/> memory reference; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static Boolean IsZero(this IntPtr ptr)
            => ptr == IntPtr.Zero;

        /// <summary>
        /// Indicates whether the <see cref="UIntPtr"/> pointer is a <see langword="null"/> memory reference.
        /// </summary>
        /// <param name="uptr"><see cref="UIntPtr"/> pointer.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="UIntPtr"/> instance is a <see langword="null"/> memory reference; 
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static Boolean IsZero(this UIntPtr uptr)
            => uptr == UIntPtr.Zero;

        /// <summary>
        /// Creates a <see cref="UIntPtr"/> value from given <see cref="IntPtr"/> value.
        /// </summary>
        /// <param name="ptr"><see cref="IntPtr"/> value.</param>
        /// <returns><see cref="UIntPtr"/> value.</returns>
        public static UIntPtr AsUIntPtr(this IntPtr ptr)
        {
            unsafe
            {
                return new UIntPtr(ptr.ToPointer());
            }
        }

        /// <summary>
        /// Creates a <see cref="IntPtr"/> value from given <see cref="UIntPtr"/> value.
        /// </summary>
        /// <param name="uptr"><see cref="UIntPtr"/> value.</param>
        /// <returns><see cref="IntPtr"/> value.</returns>
        public static IntPtr AsIntPtr(this UIntPtr uptr)
        {
            unsafe
            {
                return new IntPtr(uptr.ToPointer());
            }
        }

        /// <summary>
        /// Creates a <see cref="String"/> instance taking the memory reference of <see cref="IntPtr"/> 
        /// value as the UTF-16 text starting point.
        /// </summary>
        /// <param name="ptr"><see cref="IntPtr"/> pointer to starting point of UTF-16 text.</param>
        /// <param name="length">Optional. Number of <see cref="Char"/> values contained into the UTF-16 text.</param>
        /// <returns>
        /// <see cref="String"/> representation of UTF-16 text.
        /// If the <paramref name="length"/> value is great than zero the lenght of the resulting <see cref="String"/> instance 
        /// will equal to this value; otherwise, will be equal to the distance between the starting point of UTF-16 text and the
        /// first null character (\0) position.
        /// </returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static String? AsString(this IntPtr ptr, Int32 length = 0)
        {
            ValidateLengthParameter(length, nameof(length));
            if (ptr.IsZero())
                return default;
            else
                unsafe
                {
                    return GetStringFromCharPointer((Char*)ptr.ToPointer(), length);
                }
        }

        /// <summary>
        /// Creates a <see cref="String"/> instance taking the memory reference of <see cref="UIntPtr"/> 
        /// value as the UTF-16 text starting point.
        /// </summary>
        /// <param name="uptr"><see cref="UIntPtr"/> pointer to starting point of UTF-16 text.</param>
        /// <param name="length">Optional. Number of <see cref="Char"/> values contained into the UTF-16 text.</param>
        /// <returns>
        /// <see cref="String"/> representation of UTF-16 text.
        /// If the <paramref name="length"/> value is great than zero the lenght of the resulting <see cref="String"/> instance 
        /// will equal to this value; otherwise, will be equal to the distance between the starting point of UTF-16 text and the
        /// first null character (\0) position.
        /// </returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static String? AsString(this UIntPtr uptr, Int32 length = 0)
        {
            ValidateLengthParameter(length, nameof(length));
            if (uptr.IsZero())
                return default;
            else
                unsafe
                {
                    return GetStringFromCharPointer((Char*)uptr.ToPointer(), length);
                }
        }

        /// <summary>
        /// Creates a <see cref="ReadOnlySpan{T}"/> instance from <see cref="IntPtr"/> pointer.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="ptr"><see cref="IntPtr"/> pointer.</param>
        /// <param name="length">
        /// Number of <typeparamref name="T"/> <see langword="unmanaged"/> values to retrive form the contiguous region of memory.
        /// </param>
        /// <returns><see cref="ReadOnlySpan{T}"/> instance.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this IntPtr ptr, Int32 length)
            where T : unmanaged
        {
            ValidateLengthParameter(length, nameof(length));
            if (ptr.IsZero())
                return default;
            else
                unsafe
                {
                    return GetReadOnlySpanFromPointer<T>(ptr.ToPointer(), length);
                }
        }

        /// <summary>
        /// Creates a <see cref="ReadOnlySpan{T}"/> instance from <see cref="UIntPtr"/> pointer.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="uptr"><see cref="UIntPtr"/> pointer.</param>
        /// <param name="length">
        /// Number of <typeparamref name="T"/> <see langword="unmanaged"/> values to retrive form the contiguous region of memory.
        /// </param>
        /// <returns><see cref="ReadOnlySpan{T}"/> instance.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this UIntPtr uptr, Int32 length)
            where T : unmanaged
        {
            ValidateLengthParameter(length, nameof(length));
            if (uptr.IsZero())
                return default;
            else
                unsafe
                {
                    return GetReadOnlySpanFromPointer<T>(uptr.ToPointer(), length);
                }
        }

        /// <summary>
        /// Creates an <typeparamref name="T"/> delegate from from <see cref="IntPtr"/> pointer.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
        /// <param name="ptr"><see cref="IntPtr"/> pointer.</param>
        /// <returns><typeparamref name="T"/> delegate.</returns>
        public static T? AsDelegate<T>(this IntPtr ptr)
            where T : Delegate
            => !ptr.IsZero() ? Marshal.GetDelegateForFunctionPointer<T>(ptr) : default;

        /// <summary>
        /// Creates an <typeparamref name="T"/> delegate from from <see cref="UIntPtr"/> pointer.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Delegate"/> referenced into the pointer.</typeparam>
        /// <param name="uptr"><see cref="UIntPtr"/> pointer.</param>
        /// <returns><typeparamref name="T"/> delegate.</returns>
        public static T? AsDelegate<T>(this UIntPtr uptr)
            where T : Delegate
            => uptr.AsIntPtr().AsDelegate<T>();

        /// <summary>
        /// Creates a memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value from 
        /// a <see cref="IntPtr"/> pointer.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of the <see langword="unmanaged"/> referenced value.</typeparam>
        /// <param name="ptr"><see cref="IntPtr"/> pointer.</param>
        /// <returns>Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</returns>
        public static ref T AsReference<T>(this IntPtr ptr)
            where T : unmanaged
        {
            unsafe
            {
                return ref GetPointerFromReference<T>(ptr.ToPointer());
            }
        }

        /// <summary>
        /// Creates a memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value from 
        /// a <see cref="UIntPtr"/> pointer.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of the <see langword="unmanaged"/> referenced value.</typeparam>
        /// <param name="uptr"><see cref="UIntPtr"/> pointer.</param>
        /// <returns>Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</returns>
        public static ref T AsReference<T>(this UIntPtr uptr)
            where T : unmanaged
        {
            unsafe
            {
                return ref GetPointerFromReference<T>(uptr.ToPointer());
            }
        }

        /// <summary>
        /// Validates an memory length parameter.
        /// </summary>
        /// <param name="length">Memory length value.</param>
        /// <param name="lengthParameterName">Name of the memory length parameter.</param>
        /// <exception cref="ArgumentException"/>
        private static void ValidateLengthParameter(Int32 length, String lengthParameterName)
        {
            if (length < 0)
                throw new ArgumentException($"The parameter {lengthParameterName} must be zero or positive integer.");
        }

        /// <summary>
        /// Creates a <see cref="String"/> instance taking a <see cref="Char"/> pointer as the UTF-16 text starting point.
        /// </summary>
        /// <param name="chrPtr"><see cref="Char"/> pointer.</param>
        /// <param name="length">Number of <see cref="Char"/> values contained into the UTF-16 text.</param>
        /// <see cref="String"/> representation of UTF-16 text.
        /// <returns>
        /// If the <paramref name="length"/> value is great than zero the lenght of the resulting <see cref="String"/> instance 
        /// will equal to this value; otherwise, will be equal to the distance between the starting point of UTF-16 text and the
        /// first null character (\0) position.
        /// </returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        private static unsafe String GetStringFromCharPointer(Char* chrPtr, Int32 length)
            => length == default ? new String(chrPtr) : new ReadOnlySpan<Char>(chrPtr, length).ToString();

        /// <summary>
        /// Creates a <see cref="ReadOnlySpan{T}"/> instance from a native pointer.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="ptr">Native pointer.</param>
        /// <param name="length">
        /// Number of <typeparamref name="T"/> <see langword="unmanaged"/> values to retrive form the contiguous region of memory.
        /// </param>
        /// <returns><see cref="ReadOnlySpan{T}"/> instance.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        private unsafe static ReadOnlySpan<T> GetReadOnlySpanFromPointer<T>(void* ptr, Int32 length)
            => new(ptr, length);

        /// <summary>
        /// Creates a memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value from 
        /// a native pointer.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of the <see langword="unmanaged"/> referenced value.</typeparam>
        /// <param name="ptr">Native pointer.</param>
        /// <returns>Memory reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</returns>
        private static unsafe ref T GetPointerFromReference<T>(void* ptr)
            where T : unmanaged
            => ref Unsafe.AsRef<T>(ptr);
    }
}
