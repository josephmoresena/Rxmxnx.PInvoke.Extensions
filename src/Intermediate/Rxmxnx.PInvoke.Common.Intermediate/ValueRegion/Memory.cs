namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// Represents a memory region that is contained within a <see cref="ReadOnlyMemory{T}"/> instance.
	/// </summary>
	public abstract class Memory : ValueRegion<T>
	{
		/// <summary>
		/// Memory instance.
		/// </summary>
		protected abstract ReadOnlyMemory<T> Value { get; }

		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public sealed override T this[Int32 index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => base[index];
		}
		/// <inheritdoc/>
		public sealed override Boolean IsMemorySlice
		{
			get
			{
				if (this.AsArray() is not null) return false;
				if (!this.TryGetMemoryManager(out MemoryManager<T>? manager, out Int32 offset, out Int32 length))
					return false;
				return offset > 0 || length < manager.Memory.Span.Length;
			}
		}

		/// <inheritdoc/>
		public sealed override Boolean TryAlloc(GCHandleType type, out GCHandle handle)
			=> MemoryMarshal.TryGetArray(this.Value, out ArraySegment<T> segment) && segment.Array is not null ?
				ManagedRegion.TryAlloc(segment.Array, type, out handle) :
				base.TryAlloc(type, out handle);
		/// <inheritdoc/>
		public sealed override IPinnable? GetPinnable(out Int32 offset)
		{
			Boolean hasArray = MemoryMarshal.TryGetArray(this.Value, out _);
			if (!hasArray && this.TryGetMemoryManager(out MemoryManager<T>? manager, out offset, out _))
				return manager;
			Unsafe.SkipInit(out offset);
			return default;
		}
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override ValueRegion<T> Slice(Int32 startIndex) => this.Slice(this.Value[startIndex..]);
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public sealed override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
			=> this.Slice(this.Value.Slice(startIndex, length));
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public sealed override T[] ToArray() => this.Value.ToArray();

		/// <summary>
		/// Retrieves a subregion from this instance.
		/// The subregion starts at a specified item position and has a specified length.
		/// </summary>
		/// <param name="memory">Slice memory.</param>
		/// <returns>
		/// A <see cref="ValueRegion{T}"/> that is equivalent to the <paramref name="memory"/>.
		/// </returns>
		protected abstract ValueRegion<T> Slice(ReadOnlyMemory<T> memory);

		/// <inheritdoc/>
		private protected sealed override T[]? AsArray()
		{
			if (!MemoryMarshal.TryGetArray(this.Value, out ArraySegment<T> segment)) return base.AsArray();
			if (segment.Array is null || segment.Offset > 0 || segment.Count != segment.Array.Length) return default;
			return segment.Array;
		}

		/// <inheritdoc/>
		internal sealed override Boolean TryGetMemory(out ReadOnlyMemory<T> memory)
		{
			memory = this.Value;
			return true;
		}
		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal sealed override ReadOnlySpan<T> AsSpan() => this.Value.Span;
		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal sealed override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> this.Slice(this.Value.Slice(startIndex, length));

		/// <summary>
		/// Tries to retrieve a <see cref="MemoryManager{T}"/>, start index, and length from the underlying read-only
		/// memory buffer.
		/// </summary>
		/// <param name="manager">When the method returns, the manager of <see cref="Value"/>.</param>
		/// <param name="start">
		/// When the method returns, the offset from the start of the <paramref name="manager"/> that <see cref="Value"/>
		/// represents.
		/// </param>
		/// <param name="length">
		/// When the method returns, the length of the <paramref name="manager"/> that <see cref="Value"/> represents.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the method succeeded; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private Boolean TryGetMemoryManager([NotNullWhen(true)] out MemoryManager<T>? manager, out Int32 start,
			out Int32 length)
			=> MemoryMarshal.TryGetMemoryManager(this.Value, out manager, out start, out length);
	}
}