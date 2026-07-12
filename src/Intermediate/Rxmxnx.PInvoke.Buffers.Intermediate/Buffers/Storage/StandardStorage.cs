namespace Rxmxnx.PInvoke.Buffers.Storage;

/// <summary>
/// Standard storage.
/// </summary>
internal sealed class StandardStorage : MetadataStorage
{
	/// <inheritdoc/>
	public override Boolean TryAdd<T>(BufferTypeMetadata<T> component)
		=> BinaryStore<MainBinaryStore<T>, T>.TryAdd(component);
	/// <inheritdoc/>
	protected override Int32 GetCurrentCapacity<T>() => BinaryStore<MainBinaryStore<T>, T>.CurrentCapacity;
	/// <inheritdoc/>
	protected override ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
		=> ref BinaryStore<MainBinaryStore<T>, T>.GetBinaryReference(componentSize);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
		=> BinaryStore<MainBinaryStore<T>, T>.GetBinaryValue(componentSize);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(UInt16 count, Boolean allowMinimal)
		=> BinaryStore<MainBinaryStore<T>, T>.ComputeBinaryMetadata(this, count, allowMinimal);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetFundamental<T>(UInt16 space)
		=> BinaryStore<MainBinaryStore<T>, T>.GetFundamental(this, space);

	/// <summary>
	/// Initial storage struct.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	private readonly struct MainBinaryStore<T> : IMainBinaryStore<T>
	{
		/// <summary>
		/// Internal array.
		/// </summary>
		private readonly BufferTypeMetadata<T>?[] _initial;

		/// <inheritdoc/>
		public UInt16 Length
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => (UInt16)this._initial.Length;
		}
		/// <inheritdoc/>
		public ref BufferTypeMetadata<T>? this[Int32 index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => ref this._initial[index];
		}
		/// <inheritdoc/>
		public Span<BufferTypeMetadata<T>?> Span
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this._initial.AsSpan();
		}

		/// <summary>
		/// Parameterless constructor.
		/// </summary>
		public MainBinaryStore()
		{
			Int32 initialLength = typeof(T).IsValueType ? 255 : 2047;
			this._initial = new BufferTypeMetadata<T>?[initialLength];
		}
	}
#if !PACKAGE
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>()
		=> BinaryStore<MainBinaryStore<T>, T>.Initial;
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>()
		=> BinaryStore<MainBinaryStore<T>, T>.Slots;
#endif
}