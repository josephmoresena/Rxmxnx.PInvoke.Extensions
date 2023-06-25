namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// Represents a managed memory slice that is contained within an array of
	/// <typeparamref name="T"/> values.
	/// </summary>
	private sealed class ManagedMemorySlice : MemorySlice
	{
		/// <summary>
		/// The internal array of <typeparamref name="T"/>.
		/// </summary>
		private readonly T[] _array;

		/// <summary>
		/// Initializes a new instance of the <see cref="ManagedMemorySlice"/> class from a subrange of an existing
		/// <see cref="ManagedRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="ManagedRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		public ManagedMemorySlice(ManagedRegion region, Int32 offset, Int32 length) : base(
			region.AsArray()!.Length, offset, length)
			=> this._array = region.AsArray()!;
		/// <summary>
		/// Initializes a new instance of the <see cref="ManagedMemorySlice"/> class from a subrange of an existing
		/// <see cref="ManagedMemorySlice"/>.
		/// </summary>
		/// <param name="region">A <see cref="ManagedMemorySlice"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		public ManagedMemorySlice(ManagedMemorySlice region, Int32 offset, Int32 length) : base(
			region._array.Length, offset, length, region._offset)
			=> this._array = region._array;

		/// <inheritdoc/>
		public override T this[Int32 index] => this._array[this._offset + index];

		/// <inheritdoc/>
		protected override T[]? AsArray() => !this.IsMemorySlice ? this._array : default;

		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan() => this._array.AsSpan()[this._offset..this._end];
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new ManagedMemorySlice(this, startIndex, length);
	}
}