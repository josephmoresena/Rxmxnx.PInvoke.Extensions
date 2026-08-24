namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// Represents a native memory region in which a sequence of values is stored.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private sealed unsafe class NativeRegion : ValueRegion<T>, IFixedPointer
	{
#pragma warning disable CS8500
		/// <summary>
		/// An empty instance of <see cref="ValueRegion{T}.NativeRegion"/>.
		/// </summary>
		public static readonly NativeRegion Empty = new(IntPtr.Zero, 0);
		/// <summary>
		/// The length of the sequence.
		/// </summary>
		private readonly Int32 _length;

		/// <inheritdoc/>
		public IntPtr Pointer { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.NativeRegion"/> class.
		/// </summary>
		/// <param name="ptr">The pointer to the native memory region.</param>
		/// <param name="length">The length of the sequence.</param>
		public NativeRegion(IntPtr ptr, Int32 length)
		{
			this.Pointer = ptr;
			this._length = this.Pointer != IntPtr.Zero ? length : 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.NativeRegion"/> class from a sub-range
		/// of an existing <see cref="ValueRegion{T}.NativeRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.NativeRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		private NativeRegion(NativeRegion region, Int32 offset, Int32 length)
		{
			T* tPtr = region.GetElementPointer(offset);
			this.Pointer = new(tPtr);
			this._length = length;
		}

		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex) => this.Slice(startIndex, this._length - startIndex);
		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
		{
			ValidationUtilities.ThrowIfInvalidSubregion(this._length, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}

		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan()
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			ref T refValue = ref Unsafe.AsRef<T>(this.Pointer.ToPointer());
			return MemoryMarshal.CreateReadOnlySpan(ref refValue, this._length);
#else
			return typeof(T).IsPrimitive ?
				new(this.Pointer.ToPointer(), this._length) :
				MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(this.Pointer.ToPointer(), this._length);
#endif
		}
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> length != 0 ? new(this, startIndex, length) : NativeRegion.Empty;

		/// <summary>
		/// Retrieves the pointer of the element at the given index.
		/// </summary>
		/// <param name="index">The element index.</param>
		/// <returns>The pointer of the element at the given index.</returns>
		private T* GetElementPointer(Int32 index)
		{
			T* tPtr = (T*)this.Pointer.ToPointer();
			tPtr += index;
			return tPtr;
		}
#pragma warning restore CS8500
	}
}