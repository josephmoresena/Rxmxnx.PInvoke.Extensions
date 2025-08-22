namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// Represents a managed memory slice that is contained within an array of values.
	/// </summary>
	private sealed class ManagedMemorySlice : MemorySlice
	{
		/// <summary>
		/// The internal array.
		/// </summary>
		private readonly T[] _array;

		/// <inheritdoc/>
		public override T this[Int32 index] => this._array[this.Offset + index];

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.ManagedMemorySlice"/> class from a sub-range
		/// of an existing <see cref="ValueRegion{T}.ManagedRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.ManagedRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
#nullable disable
		public ManagedMemorySlice(ManagedRegion region, Int32 offset, Int32 length) : base(
			region.AsArray().Length, offset, length)
			=> this._array = region.AsArray();
#nullable restore

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.ManagedMemorySlice"/> class from a sub-range of an
		/// existing <see cref="ValueRegion{T}.ManagedMemorySlice"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.ManagedMemorySlice"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		private ManagedMemorySlice(ManagedMemorySlice region, Int32 offset, Int32 length) : base(
			region._array.Length, offset, length, region.Offset)
			=> this._array = region._array;

		/// <inheritdoc/>
		public override Boolean TryAlloc(GCHandleType type, out GCHandle handle)
			=> ManagedRegion.TryAlloc(this._array, type, out handle);

		/// <inheritdoc/>
		private protected override T[]? AsArray() => !this.IsMemorySlice ? this._array : default;

		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan()
		{
			if (this._array.Length <= 0 || this.Offset - this.End <= 0)
				return default;
			ref T refValue = ref this._array[this.Offset];
			return MemoryMarshal.CreateReadOnlySpan(ref refValue, this.End - this.Offset);
		}
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new ManagedMemorySlice(this, startIndex, length);
	}
}