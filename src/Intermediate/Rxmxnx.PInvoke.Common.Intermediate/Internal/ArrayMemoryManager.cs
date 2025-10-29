namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// <see cref="MemoryManager{T}"/> Implementation for abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed partial class ArrayMemoryManager<T> : MemoryManager<T>
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
	/// Internal lock object.
	/// </summary>
#if NET9_0_OR_GREATER
	private readonly Lock _lock = new();
#else
	private readonly Object _lock = new();
#endif

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
#if NET9_0_OR_GREATER
		using (this._lock.EnterScope())
#else
		lock (this._lock)
#endif
		{
			if (!this._handle.IsAllocated)
				this._handle = GCHandle.Alloc(this._array, GCHandleType.Pinned);

			this._pinCount++;
			ref T managedRef = ref Unsafe.AsRef<T>(this._handle.AddrOfPinnedObject().ToPointer());
			ref T handleRef = ref Unsafe.Add(ref managedRef, elementIndex);
			return new(Unsafe.AsPointer(ref handleRef), default, this);
		}
	}
	/// <inheritdoc/>
	public override void Unpin()
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
	protected override void Dispose(Boolean disposing) { }

	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(Array? array)
	{
		if (array is null) return default;
#if NET6_0_OR_GREATER
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
#else
		GetArrayDataReferenceDelegate getArrayDataReference = ArrayMemoryManager<T>.ranks[array.Rank - 2]!;
		ref T managedRef = ref getArrayDataReference(array);
#endif
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
#if NET6_0_OR_GREATER
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(Array? array)
		=> array is not null ? new ArrayMemoryManager<T>(array).Memory : Memory<T>.Empty;

	/// <inheritdoc cref="MemoryMarshal.GetArrayDataReference(Array)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference(Array array)
	{
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
	}
#endif
}