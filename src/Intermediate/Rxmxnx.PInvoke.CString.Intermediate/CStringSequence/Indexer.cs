namespace Rxmxnx.PInvoke;

public partial class CStringSequence : IReadOnlyList<CString>, IEnumerableSequence<CString>
{
	Int32 IEnumerableSequence<CString>.GetSize() => this._lengths.Length;
	CString IEnumerableSequence<CString>.GetItem(Int32 index) => this[index];
	void IEnumerableSequence<CString>.DisposeEnumeration()
	{
		if (!this._cache.IsReadOnly) this._cache.Clear();
	}

	/// <summary>
	/// Gets the <see cref="CString"/> at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The <see cref="CString"/> at the specified index.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// <paramref name="index"/> is outside the bounds of the <see cref="CStringSequence"/>.
	/// </exception>
	[IndexerName("Item")]
	public CString this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this._lengths.Length);

			if (!this._lengths[index].HasValue)
				return CString.Zero;

			return this._lengths[index].GetValueOrDefault() == 0 ? CString.Empty : this.GetCString(index);
		}
	}

	/// <summary>
	/// Gets the number of <see cref="CString"/> instances contained in this <see cref="CStringSequence"/>.
	/// </summary>
	public Int32 Count => this._lengths.Length;

	/// <summary>
	/// Retrieves a subsequence from this instance, starting from the specified index and extending to the end
	/// of the sequence.
	/// </summary>
	/// <param name="startIndex">
	/// The zero-based index at which the subsequence starts.
	/// </param>
	/// <returns>A <see cref="CStringSequence"/> that is a subsequence of this instance.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="startIndex"/> is out of range.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public CStringSequence Slice(Int32 startIndex) => this[startIndex..this._lengths.Length];
	/// <summary>
	/// Retrieves a subsequence from this instance, starting from a specified index and having a specified length.
	/// </summary>
	/// <param name="startIndex">
	/// The zero-based index at which the subsequence starts.
	/// </param>
	/// <param name="length">The number of elements in the subsequence.</param>
	/// <returns>A <see cref="CStringSequence"/> that is a subsequence of this instance.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="length"/> are out of range.
	/// </exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public CStringSequence Slice(Int32 startIndex, Int32 length)
	{
		ValidationUtilities.ThrowIfInvalidSubsequence(this._lengths.Length, startIndex, length);

		if (length == 0)
			return CStringSequence.empty;

		if (startIndex == 0 && length == this._lengths.Length)
			return this;

		return new SubsequenceHelper(this, startIndex, length).CreateSequence();
	}

	/// <summary>
	/// Retrieves the binary span for the given index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <param name="count">
	/// The number of UTF-8 strings included in the resulting span.
	/// </param>
	/// <returns>The binary span for the specified index.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private ReadOnlySpan<Byte> GetBinarySpan(Int32 index, Int32 count = 1)
	{
		ReadOnlySpan<Byte> span = MemoryMarshal.AsBytes<Char>(this._value);
		this.CalculateSubRange(index, count, out Int32 binaryOffset, out Int32 binaryLength);
		return span.Slice(binaryOffset, binaryLength);
	}
	/// <summary>
	/// Retrieves <see cref="CString"/> instance at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The <see cref="CString"/> at the specified index.</returns>
	private CString GetCString(Int32 index) => this._cache[index] ??= new(this, index);

	/// <summary>
	/// Retrieves the binary span for the given index in <paramref name="sequence"/>.
	/// </summary>
	/// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The binary span for the specified index.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static ReadOnlySpan<Byte> GetItemSpan(CStringSequence sequence, Int32 index)
		=> sequence.GetBinarySpan(index);

	/// <summary>
	/// A helper class used to create subsequences from a parent <see cref="CStringSequence"/>.
	/// </summary>
	private sealed class SubsequenceHelper
	{
		/// <summary>
		/// A function that returns the binary representation of the subsequence.
		/// </summary>
		private readonly ReadOnlySpanFunc<Byte> _function;
		/// <summary>
		/// The lengths of the strings in the subsequence.
		/// </summary>
		private readonly Int32?[] _lengths;

		/// <summary>
		/// Initializes a new instance of the <see cref="SubsequenceHelper"/> class.
		/// </summary>
		/// <param name="sequence">The parent sequence from which to create the subsequence.</param>
		/// <param name="startIndex">
		/// The zero-based index in the parent sequence at which the subsequence begins.
		/// </param>
		/// <param name="length">The number of strings in the subsequence.</param>
		public SubsequenceHelper(CStringSequence sequence, Int32 startIndex, Int32 length)
		{
			this._lengths = sequence._lengths.Skip(startIndex).Take(length).ToArray();
			this._function = () => sequence.GetBinarySpan(startIndex, length);
		}
		/// <summary>
		/// Creates a new <see cref="CStringSequence"/> instance using the data stored in this helper.
		/// </summary>
		/// <returns>A new <see cref="CStringSequence"/> that contains the subsequence.</returns>
		public CStringSequence CreateSequence()
		{
			Int32 binaryLength = this._lengths.Sum(CStringSequence.GetSpanLength);
			Int32 charLength = binaryLength / CStringSequence.sizeOfChar + binaryLength % CStringSequence.sizeOfChar;
			String value = String.Create(charLength, this, SubsequenceHelper.CopyBytes);
			return new(value, this._lengths);
		}

		/// <summary>
		/// Copies the binary data of the subsequence from <paramref name="helper"/> to
		/// <paramref name="destination"/>.
		/// </summary>
		/// <param name="destination">The destination buffer where the data should be copied.</param>
		/// <param name="helper">
		/// The helper instance that contains the binary data of the subsequence.
		/// </param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void CopyBytes(Span<Char> destination, SubsequenceHelper helper)
		{
			Span<Byte> destinationBytes = MemoryMarshal.AsBytes(destination);
			ReadOnlySpan<Byte> sourceBytes = helper._function();
			sourceBytes.CopyTo(destinationBytes);
		}
	}
}