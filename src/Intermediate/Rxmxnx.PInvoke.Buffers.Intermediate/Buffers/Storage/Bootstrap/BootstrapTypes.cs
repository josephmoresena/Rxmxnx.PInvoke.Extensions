#if NET8_0_OR_GREATER

#region Buffers
using O = System.Object;
using O1 = Rxmxnx.PInvoke.Buffers.Atomic<System.Object>;
using O2 =
	Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
		Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>;
using O4 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		System.Object>;
using O8 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.
		Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using O16 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>, Rxmxnx
		.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers
			.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
					Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
		System.Object>;
using O32 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.
				Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
						Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>,
			System.Object>, System.Object>;
using O64 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				,
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>,
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, Rxmxnx.PInvoke.Buffers.Composite<
					Rxmxnx.PInvoke.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke
					.Buffers.Composite<
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
						Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
							Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>
				, System.Object>, System.Object>, System.Object>;
#endregion

namespace Rxmxnx.PInvoke.Buffers.Storage.Bootstrap;

/// <summary>
/// Main binary static storage.
/// </summary>
/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static class BootstrapBinaryStore<TBuffer, T> where TBuffer : struct, IManagedBinaryBuffer<O>
{
	/// <summary>
	/// Static default-initialized <typeparamref name="TBuffer"/> instance.
	/// </summary>
	private static TBuffer initial;

	/// <summary>
	/// Store managed reference.
	/// </summary>
	public static ref BufferTypeMetadata<T>? Reference
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref Unsafe.As<TBuffer, BufferTypeMetadata<T>?>(ref BootstrapBinaryStore<TBuffer, T>.initial);
	}

	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
	static unsafe BootstrapBinaryStore()
	{
		if (typeof(T).IsValueType) return;
		BufferTypeMetadata<O> bufferTypeMetadata = BootstrapBinaryStore<TBuffer, T>.initial.Metadata;
		ref BufferTypeMetadata<O>? r0 =
			ref Unsafe.As<TBuffer, BufferTypeMetadata<O>?>(ref BootstrapBinaryStore<TBuffer, T>.initial);
		Span<BufferTypeMetadata<O>?> span = MemoryMarshal.CreateSpan(ref r0, sizeof(TBuffer) / IntPtr.Size);
		MetadataStorage.Initialize(span, bufferTypeMetadata);
	}
#pragma warning restore CS8500
}

/// <summary>
/// Internal store of 2^5-1 elements.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct G31<T> : IMainBinaryStore<T>
{
	/// <inheritdoc/>
	public UInt16 Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 31;
	}
	/// <inheritdoc/>
	public ref BufferTypeMetadata<T>? this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G31<T>.GetR0();
			return ref Unsafe.Add(ref r0, index);
		}
	}
	/// <inheritdoc/>
	public Span<BufferTypeMetadata<T>?> Span
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G31<T>.GetR0();
			return MemoryMarshal.CreateSpan(ref r0, this.Length);
		}
	}
	/// <inheritdoc/>
	public Int32 SlotCount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 0;
	}

	/// <summary>
	/// Retrieves the 0th element of the bootstrap binary store.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref BufferTypeMetadata<T>? GetR0()
		=> ref BootstrapBinaryStore<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, T>
			.Reference;
}

/// <summary>
/// Internal store of 2^7-1 elements.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct G127<T> : IMainBinaryStore<T>
{
	/// <inheritdoc/>
	public UInt16 Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 127;
	}
	/// <inheritdoc/>
	public ref BufferTypeMetadata<T>? this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G127<T>.GetR0();
			return ref Unsafe.Add(ref r0, index);
		}
	}
	/// <inheritdoc/>
	public Span<BufferTypeMetadata<T>?> Span
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G127<T>.GetR0();
			return MemoryMarshal.CreateSpan(ref r0, this.Length);
		}
	}
	/// <inheritdoc/>
	public Int32 SlotCount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 0;
	}

	/// <summary>
	/// Retrieves the 0th element of the bootstrap binary store.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref BufferTypeMetadata<T>? GetR0()
		=> ref BootstrapBinaryStore<Composite<
				Composite<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64, O>,
			T>
			.Reference;
}

/// <summary>
/// Internal store of 2^8-1 elements.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct G255<TSpace, T> : IMainBinaryStore<T> where TSpace : struct, IBinarySpace
{
	/// <inheritdoc/>
	public UInt16 Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 255;
	}
	/// <inheritdoc/>
	public ref BufferTypeMetadata<T>? this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G255<TSpace, T>.GetR0();
			return ref Unsafe.Add(ref r0, index);
		}
	}
	/// <inheritdoc/>
	public Span<BufferTypeMetadata<T>?> Span
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G255<TSpace, T>.GetR0();
			return MemoryMarshal.CreateSpan(ref r0, this.Length);
		}
	}
	/// <inheritdoc/>
	public Int32 SlotCount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => TSpace.Dimension - 8;
	}

	/// <summary>
	/// Retrieves the 0th element of the bootstrap binary store.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref BufferTypeMetadata<T>? GetR0()
		=> ref BootstrapBinaryStore<Composite<
			Composite<Composite<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>,
				O64, O>, Composite<Composite<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>
				, O32, O>, O64, O>, O>, T>.Reference;
}

/// <summary>
/// Internal store of 2^11-1 elements.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct G2047<TSpace, T> : IMainBinaryStore<T> where TSpace : struct, IBinarySpace
{
	/// <inheritdoc/>
	public UInt16 Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 2047;
	}
	/// <inheritdoc/>
	public ref BufferTypeMetadata<T>? this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G2047<TSpace, T>.GetR0();
			return ref Unsafe.Add(ref r0, index);
		}
	}
	/// <inheritdoc/>
	public Span<BufferTypeMetadata<T>?> Span
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref G2047<TSpace, T>.GetR0();
			return MemoryMarshal.CreateSpan(ref r0, this.Length);
		}
	}
	/// <inheritdoc/>
	public Int32 SlotCount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => TSpace.Dimension - 11;
	}

	/// <summary>
	/// Retrieves the 0th element of the bootstrap binary store.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref BufferTypeMetadata<T>? GetR0()
		=> ref BootstrapBinaryStore<Composite<
			Composite<Composite<
					Composite<Composite<Composite<
							Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64,
						O>,
						Composite<O64, O64, O>, O>, Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>,
				Composite<Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, O>, Composite<
				Composite<Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, Composite<
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
					Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, O>, O>, T>.Reference;
}
#endif