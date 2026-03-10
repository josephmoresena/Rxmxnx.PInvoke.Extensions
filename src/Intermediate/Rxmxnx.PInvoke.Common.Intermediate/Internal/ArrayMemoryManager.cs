namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A <see cref="ManagedMemoryManager{T}"/> implementation for abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
internal sealed partial class ArrayMemoryManager<T> : ManagedMemoryManager<T>
{
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Returns a reference to the 0th element of array. If the array is empty, returns a null reference.
	/// </summary>
	private delegate ref T GetArrayDataReferenceDelegate(Array arr);

	/// <summary>
	/// Delegates for each array rank.
	/// </summary>
	private static readonly GetArrayDataReferenceDelegate?[] ranks = new GetArrayDataReferenceDelegate[31];
#endif

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
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected override ref T GetMemoryReference() => ref ArrayMemoryManager<T>.GetArrayDataReference(this._array);

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

	/// <summary>
	/// Returns a reference to the 0th element of <paramref name="array"/>.
	/// </summary>
	/// <returns>A reference to the 0th element of array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference(Array array)
	{
#if NET6_0_OR_GREATER
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
#else
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2]!;
		return ref getArrayDataReference(array);
#endif
	}
}