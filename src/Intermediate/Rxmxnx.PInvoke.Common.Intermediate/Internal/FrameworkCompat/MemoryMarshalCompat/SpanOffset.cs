#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

internal static partial class MemoryMarshalCompat
{
	/// <summary>
	/// Internal struct with span offsets.
	/// </summary>
	private readonly struct SpanOffset
	{
		/// <summary>
		/// Size of <see cref="Span{T}"/> value.
		/// </summary>
		public Int32 SpanSize { get; init; }
		/// <summary>
		/// Offset to <c>Pinnable&lt;T&gt; _pinnable</c> field.
		/// </summary>
		public Int32 PinnableOffset { get; init; }
		/// <summary>
		/// Offset to <c>IntPtr _byteOffset</c> field.
		/// </summary>
		public Int32 PointerOffset { get; init; }
		/// <summary>
		/// Offset to <c>_length</c> field.
		/// </summary>
		public Int32 LengthOffset { get; init; }

		/// <summary>
		/// Validates the detected field offsets for a three-field <see cref="Span{T}"/> representation.
		/// </summary>
		/// <returns>The current value.</returns>
		/// <exception cref="PlatformNotSupportedException">
		/// One or more fields do not fit within the structure, or the detected field ranges overlap.
		/// </exception>
		public SpanOffset ValidateLayout()
		{
			if (this.SpanSize <= 2 * IntPtr.Size)
				return new()
				{
					SpanSize = this.SpanSize, PinnableOffset = -1, PointerOffset = -1, LengthOffset = -1,
				};
			//TODO: Exceptions to ValidationUtilities
			if (!SpanOffset.Fits(this.PinnableOffset, IntPtr.Size, this.SpanSize))
				throw new PlatformNotSupportedException("Unable to identify the three-field Span<T> layout.1");
			if (!SpanOffset.Fits(this.PointerOffset, IntPtr.Size, this.SpanSize))
				throw new PlatformNotSupportedException("Unable to identify the three-field Span<T> layout.2");
			if (!SpanOffset.Fits(this.LengthOffset, sizeof(Int32), this.SpanSize))
				throw new PlatformNotSupportedException("Unable to identify the three-field Span<T> layout. 3");
			if (SpanOffset.Overlaps(this.PointerOffset, IntPtr.Size, this.LengthOffset, sizeof(Int32)))
				throw new PlatformNotSupportedException("The detected Span<T> fields overlap.");
			if (this.SpanSize <= 2 * IntPtr.Size) return this;
			if (SpanOffset.Overlaps(this.PinnableOffset, IntPtr.Size, this.PointerOffset, IntPtr.Size))
				throw new PlatformNotSupportedException("The detected Span<T> fields overlap.2");
			if (SpanOffset.Overlaps(this.PinnableOffset, IntPtr.Size, this.LengthOffset, sizeof(Int32)))
				throw new PlatformNotSupportedException("The detected Span<T> fields overlap.3");
			return this;
		}

		/// <summary>
		/// Determines whether a field range fits completely within a containing memory range.
		/// </summary>
		/// <param name="offset">The zero-based byte offset at which the field begins.</param>
		/// <param name="fieldSize">The size, in bytes, of the field. </param>
		/// <param name="containerSize">The total size, in bytes, of the containing memory range.</param>
		/// <returns>
		/// <see langword="true"/> if the field range is nonnegative and fits entirely within the containing range;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Boolean Fits(Int32 offset, Int32 fieldSize, Int32 containerSize)
			=> offset >= 0 && fieldSize >= 0 && offset <= containerSize - fieldSize;
		/// <summary>
		/// Determines whether two half-open byte ranges overlap.
		/// </summary>
		/// <param name="firstOffset">The zero-based byte offset at which the first range begins.</param>
		/// <param name="firstSize">The size, in bytes, of the first range.</param>
		/// <param name="secondOffset">The zero-based byte offset at which the second range begins.</param>
		/// <param name="secondSize"> The size, in bytes, of the second range.</param>
		/// <returns>
		/// <see langword="true"/> if the ranges share at least one byte; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Boolean Overlaps(Int32 firstOffset, Int32 firstSize, Int32 secondOffset, Int32 secondSize)
			=> firstOffset < secondOffset + secondSize && secondOffset < firstOffset + firstSize;
	}
}
#endif