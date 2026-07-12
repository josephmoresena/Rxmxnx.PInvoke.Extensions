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
/// Initial storage.
/// </summary>
/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal abstract class BootstrapMainBinaryStore<TBuffer, T> : IMainBinaryStore<T>
	where TBuffer : struct, IManagedBinaryBuffer<O>
{
	/// <summary>
	/// Initial buffer.
	/// </summary>
	private TBuffer _buffer;

	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	protected BootstrapMainBinaryStore()
	{
		if (typeof(T).IsValueType) return;
		ref BufferTypeMetadata<O>? r0 = ref Unsafe.As<TBuffer, BufferTypeMetadata<O>?>(ref this._buffer);
		Span<BufferTypeMetadata<O>?> span = MemoryMarshal.CreateSpan(ref r0, this.Length);
		MetadataStorage.Initialize(span, this._buffer.Metadata);
	}

	/// <inheritdoc/>
	public UInt16 Length
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this._buffer.Metadata.Size;
	}
	/// <inheritdoc/>
	public ref BufferTypeMetadata<T>? this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref Unsafe.As<TBuffer, BufferTypeMetadata<T>?>(ref this._buffer);
			return ref Unsafe.Add(ref r0, index);
		}
	}
	/// <inheritdoc/>
	public Span<BufferTypeMetadata<T>?> Span
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ref BufferTypeMetadata<T>? r0 = ref Unsafe.As<TBuffer, BufferTypeMetadata<T>?>(ref this._buffer);
			return MemoryMarshal.CreateSpan(ref r0, this.Length);
		}
	}
	/// <inheritdoc/>
	public virtual Int32 SlotCount => 0;
}

/// <summary>
/// Initial storage.
/// </summary>
/// <typeparam name="TSpace">Type of binary space.</typeparam>
/// <typeparam name="TBuffer">Type of object buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal abstract class BootstrapMainBinaryStore<TSpace, TBuffer, T> : BootstrapMainBinaryStore<TBuffer, T>
	where TSpace : IBinarySpace where TBuffer : struct, IManagedBinaryBuffer<O>
{
	/// <inheritdoc/>
	public sealed override Int32 SlotCount => Math.Max(base.SlotCount, TSpace.Dimension);
}

/// <summary>
/// Internal store with 2^5-1 binary space.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class G31<T> : BootstrapMainBinaryStore<Composite<
	Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, T>;

/// <summary>
/// Internal store with 2^7-1 binary space.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class G127<T> : BootstrapMainBinaryStore<Composite<
	Composite<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64, O>, T>;

/// <summary>
/// Internal store with 2^8-1 binary space.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class G255<TSpace, T> : BootstrapMainBinaryStore<TSpace, Composite<
	Composite<Composite<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64, O>,
	Composite<Composite<Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64, O>,
	O>, T> where TSpace : IBinarySpace;

/// <summary>
/// Internal store with 2^11-1 binary space.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class G2047<TSpace, T> : BootstrapMainBinaryStore<TSpace, Composite<
	Composite<Composite<
			Composite<Composite<Composite<
					Composite<Composite<Composite<Composite<O1, O2, O>, O4, O>, O8, O>, O16, O>, O32, O>, O64, O>,
				Composite<O64, O64, O>, O>, Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>,
		Composite<Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
			Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, O>, Composite<
		Composite<Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
			Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, Composite<
			Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>,
			Composite<Composite<O64, O64, O>, Composite<O64, O64, O>, O>, O>, O>, O>, T> where TSpace : IBinarySpace;
#endif