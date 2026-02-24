namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A <see cref="MemoryManager{T}"/> managed class.
/// </summary>
/// <typeparam name="T">The type of the memory.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
[Preserve(AllMembers = true)]
internal abstract unsafe class ManagedMemoryManager<T> : MemoryManager<T>
{
	/// <summary>
	/// Internal lock object.
	/// </summary>
#if NET9_0_OR_GREATER
	private readonly Lock _lock = new();
#else
	private readonly Object _lock = new();
#endif
	/// <summary>
	/// Number of elements in the current memory.
	/// </summary>
	private readonly Int32? _count;

	/// <summary>
	/// <see cref="GCHandle"/> handle.
	/// </summary>
	private GCHandle _handle;
	/// <summary>
	/// Counter of Pin() calls.
	/// </summary>
	private Int32 _pinCount;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="count">Number of elements in the current memory.</param>
	protected ManagedMemoryManager(Int32? count) => this._count = count;

	/// <inheritdoc/>
	public override Span<T> GetSpan()
		=> this._count.HasValue ? MemoryMarshal.CreateSpan(ref this.GetMemoryReference(), this._count.Value) : default;
	/// <inheritdoc/>
	public override MemoryHandle Pin(Int32 elementIndex = 0)
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
		{
			if (!this._handle.IsAllocated)
				this._handle = this.AllocPinned();

			this._pinCount++;
			ref T managedRef = ref Unsafe.AsRef<T>(this._handle.AddrOfPinnedObject().ToPointer());
			ref T handleRef = ref Unsafe.Add(ref managedRef, elementIndex);
			return new(Unsafe.AsPointer(ref handleRef), default, this);
		}
	}
	/// <inheritdoc/>
	public sealed override void Unpin()
	{
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
		{
			if (this._pinCount > 0) this._pinCount--;
			if (this._pinCount > 0 || !this._handle.IsAllocated) return;

			this._handle.Free();
			this._handle = default;
		}
	}

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected sealed override void Dispose(Boolean disposing) { }

	/// <summary>
	/// Allocates a pinned handle for the current memory.
	/// </summary>
	/// <returns>
	/// A new pinned <see cref="GCHandle"/>.
	/// This <see cref="GCHandle"/> must be released with <see cref="GCHandle.Free()"/> when it is no longer needed.
	/// </returns>
	protected abstract GCHandle AllocPinned();
	/// <summary>
	/// Returns a reference to the 0th element of the current memory.
	/// </summary>
	/// <returns>The managed reference to the 0th element of the current memory.</returns>
	protected abstract ref T GetMemoryReference();
}