namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// <see cref="MemoryManager{T}"/> Implementation for abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal sealed class ArrayMemoryManager<T>(Array array) : MemoryManager<T>
{
	/// <summary>
	/// Internal array.
	/// </summary>
	private readonly Array _array = array;

	/// <summary>
	/// <see cref="GCHandle"/> handle.
	/// </summary>
	private GCHandle _handle;

	/// <inheritdoc/>
	public override Span<T> GetSpan()
	{
		ref T managedRef = ref this.GetArrayDataReference();
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, this._array.Length);
		return span;
	}
	/// <inheritdoc/>
	public override unsafe MemoryHandle Pin(Int32 elementIndex = 0)
	{
		if (this._handle == default)
			this._handle = GCHandle.Alloc(this._array, GCHandleType.Pinned);
		ref T managedRef = ref this.GetArrayDataReference();
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
	protected override void Dispose(Boolean _) => this.Unpin();

	/// <inheritdoc cref="MemoryMarshal.GetArrayDataReference(Array)"/>
	private ref T GetArrayDataReference()
	{
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(this._array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
	}
}