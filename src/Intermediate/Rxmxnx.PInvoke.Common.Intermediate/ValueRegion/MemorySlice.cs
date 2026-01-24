namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// This class represents a memory slice that contains a sequence of values.
	/// </summary>
	private abstract class MemorySlice : ValueRegion<T>, IWrapper.IBase<Range>
	{
		/// <summary>
		/// Indicates whether the current instance represents a memory slice extracted from a larger memory region.
		/// </summary>
		private readonly Boolean _slice;

		/// <inheritdoc/>
		public override Boolean IsMemorySlice => this._slice;

		/// <summary>
		/// Internal length.
		/// </summary>
		protected Int32 End { get; }
		/// <summary>
		/// Internal offset.
		/// </summary>
		protected Int32 Offset { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.MemorySlice"/> class.
		/// </summary>
		/// <param name="initialLength">The initial length of the memory slice.</param>
		/// <param name="offset">The offset for the memory slice.</param>
		/// <param name="length">The length of the memory slice.</param>
		/// <param name="initialOffset">The initial offset of the memory slice.</param>
		protected MemorySlice(Int32 initialLength, Int32 offset, Int32 length, Int32 initialOffset = 0)
		{
			this.Offset = offset + initialOffset;
			this.End = this.Offset + length;
			this._slice = this.Offset != 0 || this.End != initialLength;
		}

#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		Range IWrapper.IBase<Range>.Value => new(this.Offset, this.End);

		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex)
		{
			Int32 regionLength = this.End - this.Offset;
			Int32 length = regionLength - startIndex;
			ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}
		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
		{
			Int32 regionLength = this.End - this.Offset;
			ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}
	}
}