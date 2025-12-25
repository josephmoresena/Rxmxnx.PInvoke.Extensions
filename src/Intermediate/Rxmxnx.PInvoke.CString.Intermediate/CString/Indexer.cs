#if PACKAGE && !NETCOREAPP
using IEnumerator = System.Collections.IEnumerator;
using IEnumerable = System.Collections.IEnumerable;
#endif

namespace Rxmxnx.PInvoke;

public partial class CString : IEnumerableSequence<Byte>
{
	/// <summary>
	/// Gets the <see cref="Byte"/> value at a specified position in the current
	/// <see cref="CString"/> object.
	/// </summary>
	/// <param name="index">The zero-based index of the byte to get.</param>
	/// <returns>The byte at the specified position.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// <paramref name="index"/> is less than zero or greater than or equal to the length of this
	/// <see cref="CString"/>.
	/// </exception>
	[IndexerName("Unit")]
	public Byte this[Int32 index] => this._data[index];
	/// <summary>
	/// Gets the number of bytes in the current <see cref="CString"/> object.
	/// </summary>
	/// <value>The number of bytes in the current <see cref="CString"/>.</value>
	// ReSharper disable once ConvertToAutoPropertyWhenPossible
	public Int32 Length => this._length;

	Int32 IEnumerableSequence<Byte>.GetSize() => this._length;
	Byte IEnumerableSequence<Byte>.GetItem(Int32 index) => this[index];
#if PACKAGE && !NETCOREAPP
	IEnumerator<Byte> IEnumerable<Byte>.GetEnumerator() => IEnumerableSequence.CreateEnumerator(this);
	IEnumerator IEnumerable.GetEnumerator() => IEnumerableSequence.CreateEnumerator(this);
#endif

	/// <summary>
	/// Retrieves a substring from this instance. The substring starts at a specified character
	/// position and continues to the end of the <see cref="CString"/>.
	/// </summary>
	/// <param name="startIndex">
	/// The zero-based starting byte position of a substring in this instance.
	/// </param>
	/// <returns>
	/// A <see cref="CString"/> that is equivalent to the substring that begins at
	/// <paramref name="startIndex"/> in this instance, or <see cref="Empty"/> if
	/// <paramref name="startIndex"/> is equal to the length of this instance.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="startIndex"/> is less than zero or greater than the length of this instance.
	/// </exception>
	public CString Slice(Int32 startIndex) => this[startIndex..this._length];
	/// <summary>
	/// Retrieves a substring from this instance. The substring starts at a specified character
	/// position and has a specified length.
	/// </summary>
	/// <param name="startIndex">
	/// The zero-based starting byte position of a substring in this instance.
	/// </param>
	/// <param name="length">The number of bytes in the substring.</param>
	/// <returns>
	/// A <see cref="CString"/> that is equivalent to the substring of length
	/// <paramref name="length"/> that begins at <paramref name="startIndex"/> in this
	/// instance, or <see cref="Empty"/> if <paramref name="startIndex"/> is
	/// equal to the length of this instance and <paramref name="length"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// <paramref name="startIndex"/> or <paramref name="length"/> is less than zero, or
	/// <paramref name="startIndex"/> + <paramref name="length"/> is greater than the length of
	/// this instance.
	/// </exception>
	public CString Slice(Int32 startIndex, Int32 length)
	{
		ValidationUtilities.ThrowIfInvalidSubstring(this._length, startIndex, length);
		if (length == 0)
			return CString.Empty;

		if (startIndex == 0 && length == this._length)
			return this;

		return new(this, startIndex, length);
	}
}