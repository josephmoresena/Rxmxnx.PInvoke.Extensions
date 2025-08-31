namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// Represents a memory region that is contained within an array of array of values.
	/// </summary>
	private sealed class ManagedRegion : ValueRegion<T>
	{
		/// <summary>
		/// The internal array.
		/// </summary>
		private readonly T[] _array;

		/// <inheritdoc/>
		public override T this[Int32 index] => this._array[index];

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.ManagedRegion"/> class.
		/// </summary>
		/// <param name="array">The array of values.</param>
		public ManagedRegion(T[] array) => this._array = array;

		/// <inheritdoc/>
		public override Boolean TryAlloc(GCHandleType type, out GCHandle handle)
			=> ManagedRegion.TryAlloc(this._array, type, out handle);
		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex)
			=> this.Slice(startIndex, this._array.Length - startIndex);
		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
		{
			ValidationUtilities.ThrowIfInvalidSubregion(this._array.Length, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}
		/// <inheritdoc/>
		private protected override T[] AsArray() => this._array;

		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan()
			=> MemoryMarshal.CreateReadOnlySpan(ref NativeUtilities.GetArrayDataReference(this._array),
			                                    this._array.Length);
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new ManagedMemorySlice(this, startIndex, length);

		/// <summary>
		/// Tries to create a new <see cref="GCHandle"/> from <paramref name="array"/> instance.
		/// </summary>
		/// <param name="array">The array that uses the <see cref="GCHandle"/>.</param>
		/// <param name="type">The type of <see cref="GCHandle"/> to create.</param>
		/// <param name="handle">Output. Created <see cref="GCHandle"/> that protects the value region.</param>
		/// <returns>
		/// <see langword="true"/> if a <paramref name="handle"/> was successfully created; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean TryAlloc(T[] array, GCHandleType type, out GCHandle handle)
		{
			try
			{
				handle = GCHandle.Alloc(array, type);
				return handle.IsAllocated;
			}
			catch (Exception)
			{
				handle = default;
				return false;
			}
		}
	}
}