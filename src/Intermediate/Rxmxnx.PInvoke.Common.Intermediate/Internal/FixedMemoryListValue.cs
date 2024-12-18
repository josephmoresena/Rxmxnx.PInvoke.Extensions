namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a list of <see cref="FixedMemory"/> instances.
/// </summary>
internal readonly ref struct FixedMemoryListValue
{
	/// <summary>
	/// Read-only span of <see cref="ReadOnlyFixedMemory"/> instances.
	/// </summary>
	private readonly ReadOnlySpan<ReadOnlyFixedMemory> _memories;

	/// <summary>
	/// Gets the total number of elements in the list.
	/// </summary>
	public Int32 Count => this._memories.Length;
	/// <summary>
	/// Gets the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The element at the specified index.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when <paramref name="index"/> is not a valid index in the list.
	/// </exception>
	public ReadOnlyFixedMemory this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ValidationUtilities.ThrowIfInvalidListIndex(index, this.Count);
			return this._memories[index];
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FixedMemory"/> struct.
	/// </summary>
	/// <param name="memories">A read-only span of <see cref="FixedMemory"/> instances.</param>
	private FixedMemoryListValue(ReadOnlySpan<ReadOnlyFixedMemory> memories) => this._memories = memories;

	/// <summary>
	/// Retrieves read-only span from current instance.
	/// </summary>
	/// <returns>A read-only span from current instance.</returns>
	public ReadOnlySpan<ReadOnlyFixedMemory> AsSpan() => this._memories;

	/// <summary>
	/// Releases all resources used by the <see cref="ReadOnlyFixedMemory"/> instances in the list.
	/// </summary>
	public void Unload()
	{
		foreach (ReadOnlyFixedMemory mem in this._memories)
			mem.Unload();
	}

	/// <summary>
	/// Creates a new <see cref="FixedMemoryListValue"/> instance from <paramref name="memories"/>.
	/// </summary>
	/// <typeparam name="TMemory">Type of <see cref="ReadOnlyFixedMemory"/>.</typeparam>
	/// <param name="memories">Read-only of <see cref="ReadOnlyFixedMemory"/> instances.</param>
	/// <returns>A new <see cref="FixedMemoryListValue"/> instance.</returns>
	public static FixedMemoryListValue Create<TMemory>(
#if NET9_0_OR_GREATER
		params
#endif
		ReadOnlySpan<TMemory> memories) where TMemory : ReadOnlyFixedMemory
	{
		ref TMemory refMem = ref MemoryMarshal.GetReference(memories);
		ref ReadOnlyFixedMemory refRoMem = ref Unsafe.As<TMemory, ReadOnlyFixedMemory>(ref refMem);
		ReadOnlySpan<ReadOnlyFixedMemory> roSpan = MemoryMarshal.CreateReadOnlySpan(ref refRoMem, memories.Length);
		return new(roSpan);
	}
}