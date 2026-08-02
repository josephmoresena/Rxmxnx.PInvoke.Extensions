namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A <see cref="ManagedMemoryManager{T}"/> implementation for abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
internal sealed partial class ArrayMemoryManager<T> : ManagedMemoryManager<T>
{
	/// <summary>
	/// Internal array.
	/// </summary>
	private readonly Array _array;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	private ArrayMemoryManager(Array? array) : base(array?.Length) => this._array = array ?? Array.Empty<T>();

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected override GCHandle AllocPinned() => GCHandle.Alloc(this._array, GCHandleType.Pinned);
	/// <param name="pinnable"></param>
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected override ref T GetMemoryReference(out Pinnable<T>? pinnable)
	{
#if NET6_0_OR_GREATER
		pinnable = default;
		return ref ArrayMemoryManager<T>.GetArrayDataReference(this._array);
#else
		pinnable = Unsafe.As<Array, Pinnable<T>>(ref Unsafe.AsRef(this._array));
		return ref Unsafe.AddByteOffset(ref pinnable.Data, ArrayMemoryManager<T>.GetArrayOffset(this._array)!.Value);
#endif
	}

#if NET6_0_OR_GREATER
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(Array? array)
		=> array is not null ? new ArrayMemoryManager<T>(array).Memory : Memory<T>.Empty;
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(Array? array)
	{
		if (array is null) return default;
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
#endif

#if NET6_0_OR_GREATER
	/// <summary>
	/// Returns a reference to the 0th element of <paramref name="array"/>.
	/// </summary>
	/// <returns>A reference to the 0th element of array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference(Array array)
	{
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
	}
#endif
}