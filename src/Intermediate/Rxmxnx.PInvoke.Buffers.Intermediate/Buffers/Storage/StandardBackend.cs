namespace Rxmxnx.PInvoke.Buffers.Storage;

/// <summary>
/// Standard storage.
/// </summary>
internal readonly struct StandardBackend : IMetadataStorageBackend
{
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean TryAdd<T>(BufferTypeMetadata<T> component) => BinaryStore<MainBinaryStore<T>, T>.TryAdd(component);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Int32 GetCurrentCapacity<T>() => BinaryStore<MainBinaryStore<T>, T>.CurrentCapacity;
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
		=> ref BinaryStore<MainBinaryStore<T>, T>.GetBinaryReference(componentSize);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
		=> BinaryStore<MainBinaryStore<T>, T>.GetBinaryValue(componentSize);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(MetadataStorage storage, UInt16 count, Boolean allowMinimal)
		=> BinaryStore<MainBinaryStore<T>, T>.ComputeBinaryMetadata(storage, count, allowMinimal);
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
			new BufferTypeMetadata<T>?[typeof(T).IsValueType ? 255 : 2047];

		/// <inheritdoc/>
		public UInt16 Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (UInt16)MainBinaryStore<T>.initial.Length;
		}
		/// <inheritdoc/>
		public ref BufferTypeMetadata<T>? this[Int32 index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ref MainBinaryStore<T>.initial[index];
		}
		/// <inheritdoc/>
		public Span<BufferTypeMetadata<T>?> Span
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => MainBinaryStore<T>.initial.AsSpan();
		}
	}
#if !PACKAGE
	/// <inheritdoc/>
	public ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>() => BinaryStore<MainBinaryStore<T>, T>.Initial;
	/// <inheritdoc/>
	public ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>() => BinaryStore<MainBinaryStore<T>, T>.Slots;
#endif
}