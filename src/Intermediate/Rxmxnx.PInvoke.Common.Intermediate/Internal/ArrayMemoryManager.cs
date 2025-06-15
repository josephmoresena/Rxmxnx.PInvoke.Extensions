#if !NET6_0_OR_GREATER
using MemoryMarshalCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.MemoryMarshalCompat;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// <see cref="MemoryManager{T}"/> Implementation for abstract array.
/// </summary>
/// <typeparam name="T">The type of the array.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#if NET6_0_OR_GREATER
[ExcludeFromCodeCoverage]
#endif
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
			ref T managedRef = ref Unsafe.AsRef<T>(this._handle.AddrOfPinnedObject().ToPointer());
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
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(Array? array)
		=> array is not null ? new ArrayMemoryManager<T>(array).Memory : Memory<T>.Empty;

#if NET6_0_OR_GREATER
	/// <inheritdoc cref="MemoryMarshal.GetArrayDataReference(Array)"/>
#else
	/// <inheritdoc cref="MemoryMarshalCompat.GetArrayDataReference{T}(Array)"/>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference(Array array)
	{
#if NET6_0_OR_GREATER
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
#else
		return ref MemoryMarshalCompat.GetArrayDataReference<T>(array);
#endif
	}
}