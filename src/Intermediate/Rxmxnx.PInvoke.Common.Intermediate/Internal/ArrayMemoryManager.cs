namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE || NET6_0_OR_GREATER
/// <summary>
/// <see cref="MemoryManager{T}"/> Implementation for abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed class ArrayMemoryManager<T> : MemoryManager<T>
{
	/// <summary>
	/// Internal array.
	/// </summary>
	private readonly Array _array;
	/// <summary>
	/// Internal lock object.
	/// </summary>
	private readonly Object _lock = new();

	/// <summary>
	/// <see cref="GCHandle"/> handle.
	/// </summary>
	private GCHandle _handle;
	/// <summary>
	/// Counter of Pin() calls.
	/// </summary>
	private Int32 _pinCount;

	/// <summary>
	/// <see cref="MemoryManager{T}"/> Implementation for abstract array.
	/// </summary>
	private ArrayMemoryManager(Array array) => this._array = array;

	/// <inheritdoc/>
	public override Span<T> GetSpan() => ArrayMemoryManager<T>.GetSpan(this._array);
	/// <inheritdoc/>
	public override unsafe MemoryHandle Pin(Int32 elementIndex = 0)
	{
		lock (this._lock)
		{
			if (!this._handle.IsAllocated)
				this._handle = GCHandle.Alloc(this._array, GCHandleType.Pinned);

			this._pinCount++;
			ref T managedRef =
#if NET6_0_OR_GREATER
				ref ArrayMemoryManager<T>.GetArrayDataReference(this._array);
#elif NETCOREAPP3_1_OR_GREATER
				ref Unsafe.NullRef<T>();
#else
				ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(Span<Byte>.Empty));
#endif
			ref T handleRef = ref Unsafe.Add(ref managedRef, elementIndex);
			return new(Unsafe.AsPointer(ref handleRef), default, this);
		}
	}
	/// <inheritdoc/>
	public override void Unpin()
	{
		lock (this._lock)
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
	protected override void Dispose(Boolean disposing) { }

	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(Array? array)
	{
		if (array is null) return default;
		ref T managedRef =
#if NET6_0_OR_GREATER
			ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#elif NETCOREAPP3_1_OR_GREATER
			ref Unsafe.NullRef<T>();
#else
			ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetReference(Span<Byte>.Empty));
#endif
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(Array? array)
		=> array is not null ? new ArrayMemoryManager<T>(array).Memory : Memory<T>.Empty;

#if NET6_0_OR_GREATER
	/// <inheritdoc cref="MemoryMarshal.GetArrayDataReference(Array)"/>
	private static ref T GetArrayDataReference(Array array)
	{
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
	}
#endif
}
#endif