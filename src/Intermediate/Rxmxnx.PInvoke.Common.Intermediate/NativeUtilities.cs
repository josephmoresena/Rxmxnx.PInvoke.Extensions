namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of utilities for exchange data within the P/Invoke context.
/// </summary>
[SuppressMessage("csharpsquid", "S6640")]
public static unsafe partial class NativeUtilities
{
	/// <summary>
	/// Size in bytes of a memory pointer.
	/// </summary>
	public static readonly unsafe Int32 PointerSize = sizeof(IntPtr);

	/// <summary>
	/// Retrieves the size of <typeparamref name="T"/> structure.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
	/// <returns>Size of <typeparamref name="T"/> structure.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 SizeOf<T>() where T : unmanaged => sizeof(T);
	/// <summary>
	/// Provides a high-level API for loading a native library.
	/// </summary>
	/// <param name="libraryName">The name of the native library to be loaded.</param>
	/// <param name="searchPath">The search path.</param>
	/// <returns>The OS handle for the loaded native library.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr? LoadNativeLib(String? libraryName, DllImportSearchPath? searchPath = default)
	{
		if (NativeLibrary.TryLoad(libraryName ?? String.Empty, Assembly.GetExecutingAssembly(), searchPath,
		                          out IntPtr handle))
			return handle;
		return default;
	}
	/// <summary>
	/// Provides a high-level API for loading a native library.
	/// </summary>
	/// <param name="libraryName">The name of the native library to be loaded.</param>
	/// <param name="unloadEvent">
	/// An optional event handler that is called when the library is unloaded. The handler's invocation includes a call
	/// to <see cref="NativeLibrary.Free(IntPtr)"/>.
	/// </param>
	/// <param name="searchPath">The search path.</param>
	/// <returns>The OS handle for the loaded native library.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr? LoadNativeLib(String? libraryName, ref EventHandler? unloadEvent,
		DllImportSearchPath? searchPath = default)
	{
		IntPtr? handle = NativeUtilities.LoadNativeLib(libraryName, searchPath);
		if (handle.HasValue)
			unloadEvent += (sender, e) => NativeLibrary.Free(handle.Value);
		return handle;
	}
	/// <summary>
	/// Gets the <typeparamref name="TDelegate"/> delegate of an exported symbol.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the delegate corresponding to the exported symbol.</typeparam>
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
	/// Retrieves an <see langword="unsafe"/> <see cref="IntPtr"/> pointer from a read-only reference to a
	/// <typeparamref name="T"/> <see langword="unmanaged"/> value.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value">A read-only reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
	/// <returns><see cref="IntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr GetUnsafeIntPtr<T>(in T value) where T : unmanaged
	{
		ref T refValue = ref Unsafe.AsRef(value);
		return (IntPtr)Unsafe.AsPointer(ref refValue);
	}
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="UIntPtr"/> pointer from a read-only reference to a
	/// <typeparamref name="T"/> <see langword="unmanaged"/> value.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value">Read-only reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
	/// <returns><see cref="UIntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by the garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIntPtr GetUnsafeUIntPtr<T>(in T value) where T : unmanaged
	{
		ref T refValue = ref Unsafe.AsRef(value);
		return (UIntPtr)Unsafe.AsPointer(ref refValue);
	}
	/// <summary>
	/// Transforms a read-only reference of an <see langword="unmanaged"/> value of type <typeparamref name="TSource"/> into a
	/// read-only reference of an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the source value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <typeparam name="TDestination">
	/// The type of the destination value to which to create a reference. This must be an <see langword="unmanaged"/> value
	/// type.
	/// </typeparam>
	/// <param name="value">
	/// The read-only reference to the source value from which to create the destination reference.
	/// </param>
	/// <returns>A read-only reference to an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when <typeparamref name="TSource"/> and <typeparamref name="TDestination"/> do not have the same memory size.
	/// </exception>
	/// <remarks>
	/// The transformation occurs at the memory level, without copying or moving data.
	/// This transformation can be performed between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>
	/// types
	/// that have the same size in memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref readonly TDestination Transform<TSource, TDestination>(in TSource value)
		where TSource : unmanaged where TDestination : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidCastType<TSource, TDestination>();
		ref TSource refValue = ref Unsafe.AsRef(value);
		return ref Unsafe.As<TSource, TDestination>(ref refValue);
	}
	/// <summary>
	/// Transforms a reference of an <see langword="unmanaged"/> value of type <typeparamref name="TSource"/> into a
	/// reference of an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the source value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <typeparam name="TDestination">
	/// The type of the destination value to which to create a reference. This must be an <see langword="unmanaged"/> value
	/// type.
	/// </typeparam>
	/// <param name="refValue">
	/// The reference to the source value from which to create the destination reference.
	/// </param>
	/// <returns>A reference to an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when <typeparamref name="TSource"/> and <typeparamref name="TDestination"/> do not have the same memory size.
	/// </exception>
	/// <remarks>
	/// The transformation occurs at the memory level, without copying or moving data.
	/// This transformation can be performed between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>
	/// types
	/// that have the same size in memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref TDestination TransformReference<TSource, TDestination>(ref TSource refValue)
		where TSource : unmanaged where TDestination : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidCastType<TSource, TDestination>();
		return ref Unsafe.As<TSource, TDestination>(ref refValue);
	}
	/// <summary>
	/// Retrieves a <see cref="Byte"/> array from a read-only reference to a <typeparamref name="T"/> value.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value">A read-only reference to <typeparamref name="T"/> value.</param>
	/// <returns><see cref="Byte"/> array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Byte[] ToBytes<T>(in T value) where T : unmanaged
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
		ReadOnlySpan<TSource> span = MemoryMarshal.CreateSpan(ref refValue, 1);
		return MemoryMarshal.AsBytes(span);
	}
	/// <summary>
	/// Creates a <see cref="Span{Byte}"/> from an exising reference to a
	/// <typeparamref name="TSource"/> <see langword="unmanaged"/> value.
	/// </summary>
	/// <typeparam name="TSource"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> source value.</typeparam>
	/// <param name="refValue">A read-only reference to a <typeparamref name="TSource"/> <see langword="unmanaged"/> value.</param>
	/// <returns>
	/// A <see cref="ReadOnlySpan{Byte}"/> from an exising memory reference to a <typeparamref name="TSource"/>
	/// <see langword="unmanaged"/> value.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<Byte> AsBinarySpan<TSource>(ref TSource refValue) where TSource : unmanaged
	{
		Span<TSource> span = MemoryMarshal.CreateSpan(ref refValue, 1);
		return MemoryMarshal.AsBytes(span);
	}
	/// <summary>
	/// Creates a new <typeparamref name="T"/> array with a specific length and initializes it after
	/// creation by using the specified callback.
	/// </summary>
	/// <typeparam name="T">An <see langword="unmanaged"/> type of elements in the array.</typeparam>
	/// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
	/// <param name="length">The length of the array to create.</param>
	/// <param name="state">The element to pass to <paramref name="action"/>.</param>
	/// <param name="action">A callback to initialize the array.</param>
	/// <returns>The created array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T[] CreateArray<T, TState>(Int32 length, TState state, SpanAction<T, TState> action)
		where T : unmanaged
	{
		T[] result = new T[length];
		Span<T> span = result;
		NativeUtilities.WriteSpan(span, state, action);
		return result;
	}
	/// <summary>
	/// Preforms a binary copy of <paramref name="value"/> to <paramref name="destination"/> span.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value"><typeparamref name="T"/> value.</param>
	/// <param name="destination">Destination <see cref="Span{T}"/> instance.</param>
	/// <param name="offset">
	/// The offset in <paramref name="destination"/> at which <paramref name="value"/> will be copied.
	/// </param>
	/// <exception cref="ArgumentException">
	/// Throws an exception when the length of <paramref name="destination"/> span minus the offset is less
	/// than the size of <typeparamref name="T"/>.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void CopyBytes<T>(in T value, Span<Byte> destination, Int32 offset = 0) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidCopyType(value, destination, offset, out ReadOnlySpan<Byte> bytes);
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
	private static void WriteSpan<T, TArg>(Span<T> span, TArg arg, SpanAction<T, TArg> action)
		where T : unmanaged
	{
		fixed (T* ptr = &MemoryMarshal.GetReference(span))
			action(new(ptr, span.Length), arg);
	}
}