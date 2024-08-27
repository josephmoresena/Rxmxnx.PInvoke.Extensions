namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// <see cref="MemoryManager{T}"/> Implementation for abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal sealed class ArrayMemoryManager<T> : MemoryManager<T>
{
	/// <summary>
	/// Internal array.
	/// </summary>
	private readonly Array _array;

	/// <summary>
	/// <see cref="GCHandle"/> handle.
	/// </summary>
	private GCHandle _handle;

	/// <summary>
	/// <see cref="MemoryManager{T}"/> Implementation for abstract array.
	/// </summary>
	private ArrayMemoryManager(Array array) => this._array = array;

	/// <inheritdoc/>
	public override Span<T> GetSpan() => ArrayMemoryManager<T>.GetSpan(this._array);
	/// <inheritdoc/>
	public override unsafe MemoryHandle Pin(Int32 elementIndex = 0)
	{
		if (this._handle == default)
			this._handle = GCHandle.Alloc(this._array, GCHandleType.Pinned);
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(this._array);
		ref T handleRef = ref Unsafe.Add(ref managedRef, elementIndex);
		return new(Unsafe.AsPointer(ref handleRef), this._handle, this);
	}
	/// <inheritdoc/>
	public override void Unpin()
	{
		if (this._handle == default) return;
		this._handle.Free();
		this._handle = default;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected override void Dispose(Boolean disposing) => this.Unpin();

	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(Array? array)
	{
		if (array is null) return default;
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(Array? array)
		=> array is not null ? new ArrayMemoryManager<T>(array).Memory : Memory<T>.Empty;

	/// <inheritdoc cref="MemoryMarshal.GetArrayDataReference(Array)"/>
	private static ref T GetArrayDataReference(Array array)
	{
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
	}
}