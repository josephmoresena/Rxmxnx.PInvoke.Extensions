namespace Rxmxnx.PInvoke.Buffers.Storage;

/// <summary>
/// Standard storage.
/// </summary>
internal readonly struct StandardBackend : IMetadataStorageBackend
{
	/// <inheritdoc/>
	public Int32 MaxStorageCapacity
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => UInt16.MaxValue;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean TryAdd<T>(BufferTypeMetadata<T> component) => BinaryStore<MainBinaryStore<T>, T>.TryAdd(component);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Int32 GetCurrentCapacity<T>() => BinaryStore<MainBinaryStore<T>, T>.CurrentCapacity;
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
		=> BinaryStore<MainBinaryStore<T>, T>.GetBinaryValue(componentSize);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T> SetBinaryValue<T>(BufferTypeMetadata<T> component)
		=> BinaryStore<MainBinaryStore<T>, T>.SetBinaryValue(component);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(MetadataStorage storage, UInt16 count,
		Int32 nonBinaryMinimal)
		=> BinaryStore<MainBinaryStore<T>, T>.ComputeBinaryMetadata(storage, count, nonBinaryMinimal);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetFundamental<T>(MetadataStorage storage, UInt16 space)
		=> BinaryStore<MainBinaryStore<T>, T>.GetFundamental(storage, space);

	/// <summary>
	/// Main binary storage struct.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	private readonly struct MainBinaryStore<T> : IMainBinaryStore<T>
	{
		/// <summary>
		/// Internal static array.
		/// </summary>
		private static readonly BufferTypeMetadata<T>?[] initial =
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
			new BufferTypeMetadata<T>?[typeof(T).IsValueType ? 255 : 2047];
#else
			new BufferTypeMetadata<T>?[typeof(T).GetTypeInfo().IsValueType ? 255 : 2047];
#endif

		/// <inheritdoc/>
		public UInt16 Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (UInt16)MainBinaryStore<T>.initial.Length;
		}
		/// <inheritdoc/>
		public BufferTypeMetadata<T>? this[Int32 index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MainBinaryStore<T>.initial[index];
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => MainBinaryStore<T>.initial[index] = value;
		}
		/// <inheritdoc/>
		public Int32 SlotCount
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => BuffersHelper.GetLeadingZeros(this.Length);
		}
#if !PACKAGE
		/// <inheritdoc/>
		public Span<BufferTypeMetadata<T>?> Span => new(MainBinaryStore<T>.initial);
#endif

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BufferTypeMetadata<T>? CompareExchange(BufferTypeMetadata<T> component)
			=> Interlocked.CompareExchange(ref MainBinaryStore<T>.initial[component.Size - 1], component, null);
		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BufferTypeMetadata<T>? Search(Int32 start, Int32 count)
			=> BuffersHelper.Search(ref MainBinaryStore<T>.initial[0], start, count);
	}
#if !PACKAGE
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>() => BinaryStore<MainBinaryStore<T>, T>.Initial;
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>() => BinaryStore<MainBinaryStore<T>, T>.Slots;
#endif
}