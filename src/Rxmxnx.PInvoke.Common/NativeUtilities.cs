namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of utilities for exchange data within the P/Invoke context.
/// </summary>
public static partial class NativeUtilities
{
    /// <summary>
    /// Retrieves the size of <typeparamref name="T"/> structure.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <returns>Size of <typeparamref name="T"/> structure.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Int32 SizeOf<T>() where T : unmanaged => sizeof(T);

    /// <summary>
    /// Provides a high-level API for loading a native library.
    /// </summary>
    /// <param name="libraryName">The name of the native library to be loaded.</param>
    /// <param name="searchPath">The search path.</param>
    /// <returns>The OS handle for the loaded native library.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr? LoadNativeLib(String? libraryName, DllImportSearchPath? searchPath = default)
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr? LoadNativeLib(String? libraryName, ref EventHandler? unloadEvent, DllImportSearchPath? searchPath = default)
    {
        IntPtr? handle = LoadNativeLib(libraryName, searchPath);
        if (handle.HasValue)
            unloadEvent += (sender, e) => NativeLibrary.Free(handle.Value);
        return handle;
    }

    /// <summary>
    /// Gets the <typeparamref name="TDelegate"/> delegate of an exported symbol.
    /// </summary>
    /// <typeparam name="TDelegate"></typeparam>
    /// <param name="handle">The native library OS handle.</param>
    /// <param name="name">The name of the exported symbol.</param>
    /// <returns><typeparamref name="TDelegate"/> delegate.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TDelegate? GetNativeMethod<TDelegate>(IntPtr handle, String? name) where TDelegate : Delegate
    {
        if (handle != IntPtr.Zero && NativeLibrary.TryGetExport(handle, name ?? String.Empty, out IntPtr address))
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(address);
        return default;
    }

    /// <summary>
    /// Retrieves an unsafe <see cref="IntPtr"/> pointer from a read-only reference to a <typeparamref name="T"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
    /// <param name="value">A read-only reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
    /// <returns><see cref="IntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe IntPtr GetUnsafeIntPtr<T>(in T value) where T : unmanaged
    {
        ref T refValue = ref Unsafe.AsRef(value);
        return (IntPtr)Unsafe.AsPointer(ref refValue);
    }

    /// <summary>
    /// Retrieves an unsafe <see cref="UIntPtr"/> pointer from a read-only reference to a <typeparamref name="T"/> 
    /// <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
    /// <param name="value">Read-only reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
    /// <returns><see cref="UIntPtr"/> pointer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe UIntPtr GetUnsafeUIntPtr<T>(in T value) where T : unmanaged
    {
        ref T refValue = ref Unsafe.AsRef(value);
        return (UIntPtr)Unsafe.AsPointer(ref refValue);
    }

    /// <summary>
    /// Creates a read-only reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value from 
    /// an exising read-only reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
    /// <typeparam name="TDestination"><see cref="ValueType"/> of the destination reference.</typeparam>
    /// <param name="value">A read-only reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
    /// <returns>A read-only reference to a <typeparamref name="TDestination"/> <see langword="unmanaged"/> value.</returns>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref readonly TDestination AsReferenceOf<TSource, TDestination>(in TSource value)
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        ValidationUtilities.ThrowIfInvalidCastType<TSource, TDestination>();
        ref TSource refValue = ref Unsafe.AsRef(value);
        return ref Unsafe.As<TSource, TDestination>(ref refValue);
    }

    /// <summary>
    /// Retrieves a <see cref="Byte"/> array from a read-only reference to a <typeparamref name="T"/> value.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <param name="value">A read-only reference to <typeparamref name="T"/> value.</param>
    /// <returns><see cref="Byte"/> array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Byte[] ToBytes<T>(in T value) where T : unmanaged
    {
        ref T refValue = ref Unsafe.AsRef(value);
        ReadOnlySpan<T> intermediateSpan = MemoryMarshal.CreateReadOnlySpan(ref refValue, 1);
        ReadOnlySpan<Byte> bytes = MemoryMarshal.AsBytes(intermediateSpan);
        return bytes.ToArray();
    }

    /// <summary>
    /// Creates a <see cref="ReadOnlySpan{Byte}"/> from an exising read-only reference to a 
    /// <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
    /// </summary>
    /// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
    /// <param name="value">A read-only reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
    /// <returns>
    /// A <see cref="ReadOnlySpan{Byte}"/> from an exising memory reference to a <typeparamref name="TSource"/> 
    /// <see langword="unmanaged"/> value.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Byte> AsBytes<TSource>(in TSource value) where TSource : unmanaged
    {
        ref TSource refValue = ref Unsafe.AsRef(value);
        Span<TSource> span = MemoryMarshal.CreateSpan(ref refValue, 1);
        return MemoryMarshal.AsBytes(span);
    }

    /// <summary>
    /// Creates a new <typeparamref name="T"/> array with a specific length and initializes it after 
    /// creation by using the specified callback.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
    /// <param name="length">The length of the array to create.</param>
    /// <param name="state">The element to pass to <paramref name="action"/>.</param>
    /// <param name="action">A callback to initialize the array.</param>
    /// <returns>The created array.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] CreateArray<T, TState>(Int32 length, TState state, SpanAction<T, TState> action) where T : unmanaged
    {
        T[] result = new T[length];
        Span<T> span = result;
        WriteSpan(span, state, action);
        return result;
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void CopyBytes<T>(in T value, Span<Byte> destination, Int32 offset = 0) where T : unmanaged
    {
        ValidationUtilities.ThrowIfInvalidCopyType<T>(value, destination, offset, out ReadOnlySpan<Byte> bytes);
        bytes.CopyTo(destination[offset..]);
    }

    /// <summary>
    /// Writes <paramref name="span"/> using <paramref name="arg"/> and <paramref name="action"/>.
    /// </summary>
    /// <typeparam name="T">Unmanaged type of elements in <paramref name="span"/>.</typeparam>
    /// <typeparam name="TArg">Type of state object.</typeparam>
    /// <param name="span">A <typeparamref name="T"/> writable memory block.</param>
    /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
    /// <param name="action">A <see cref="SpanAction{T, TState}"/> delegate.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void WriteSpan<T, TArg>(Span<T> span, TArg arg, SpanAction<T, TArg> action) where T : unmanaged
    {
        fixed (T* ptr = &MemoryMarshal.GetReference(span))
            action(new(ptr, span.Length), arg);
    }
}
