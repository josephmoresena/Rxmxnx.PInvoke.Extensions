#if !NETSTANDARD2_1 && !NETCOREAPP
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

internal static partial class MemoryMarshalCompat
{
	/// <summary>
	/// Represents a runtime field-layout probe composed of a structure memory
	/// image and the byte marker of the field being located.
	/// </summary>
	private readonly ref struct FieldProbe
	{
		/// <summary>
		/// Byte representation of the examined structure instance.
		/// </summary>
		private readonly ReadOnlySpan<Byte> _image;

		/// <summary>
		/// Byte representation of the field value being located.
		/// </summary>
		private readonly ReadOnlySpan<Byte> _marker;

		/// <summary>
		/// Gets the last byte offset at which the complete field marker can begin.
		/// </summary>
		public Int32 MaximumOffset => this._image.Length - this._marker.Length;

		/// <summary>
		/// Initializes a new field-layout probe.
		/// </summary>
		/// <param name="image">
		/// The byte representation of the examined structure instance.
		/// </param>
		/// <param name="marker">
		/// The byte representation of the field value being located.
		/// </param>
		public FieldProbe(ReadOnlySpan<Byte> image, ReadOnlySpan<Byte> marker)
		{
			this._image = image;
			this._marker = marker;
		}

		/// <summary>
		/// Determines whether the current probe can be compared with another probe
		/// when searching for a common field offset.
		/// </summary>
		/// <param name="other">
		/// The other field-layout probe.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if both probes contain equally sized memory
		/// images and equally sized, nonempty field markers that fit within their
		/// respective images; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean IsCompatibleWith(in FieldProbe other)
			=> this._image.Length == other._image.Length && this._marker.Length == other._marker.Length &&
				!this._marker.IsEmpty && this._marker.Length <= this._image.Length &&
				other._marker.Length <= other._image.Length;

		/// <summary>
		/// Determines whether the field marker matches the memory image at the
		/// specified byte offset.
		/// </summary>
		/// <param name="offset">
		/// The zero-based byte offset within the memory image at which the
		/// comparison begins.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the complete marker matches the corresponding
		/// bytes in the memory image; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean EqualsAt(Int32 offset)
		{
			if (offset < 0 || offset > this.MaximumOffset)
				return false;

			for (Int32 index = 0; index < this._marker.Length; index++)
			{
				if (this._image[offset + index] != this._marker[index])
					return false;
			}

			return true;
		}
	}
}
#endif