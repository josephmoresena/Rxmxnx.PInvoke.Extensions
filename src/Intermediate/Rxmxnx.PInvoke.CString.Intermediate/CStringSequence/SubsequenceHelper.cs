namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// A helper class used to create subsequences from a parent <see cref="CStringSequence"/>.
	/// </summary>
	private readonly
#if NET9_0_OR_GREATER
		ref
#endif
		struct SubsequenceHelper
	{
		/// <summary>
		/// The lengths of the strings in the subsequence.
		/// </summary>
		private readonly Int32?[] _lengths;
		/// <summary>
		/// Start index of subsequence.
		/// </summary>
		private readonly Int32 _startIndex;
		/// <summary>
		/// Source sequence.
		/// </summary>
		private readonly CStringSequence _sequence;

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
			this._startIndex = startIndex;
			this._sequence = sequence;
			this._lengths = sequence._lengths.AsSpan().Slice(startIndex, length).ToArray();
		}

		/// <summary>
		/// Creates a new <see cref="CStringSequence"/> instance using the data stored in this helper.
		/// </summary>
		/// <returns>A new <see cref="CStringSequence"/> that contains the subsequence.</returns>
		public CStringSequence CreateSequence()
		{
			Int32 length = CStringSequence.GetBufferLength(this._lengths.AsSpan());
			String value = String.Create(length, this, SubsequenceHelper.CopyBytes);
			return new(value, this._lengths);
		}

		/// <summary>
		/// Retrieves the binary span for the current subsequence.
		/// </summary>
		/// <returns>The binary span for the current subsequence.</returns>
		private ReadOnlySpan<Byte> GetBinarySpan()
			=> this._sequence.GetBinarySpan(this._startIndex, this._lengths.Length);

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
			ReadOnlySpan<Byte> sourceBytes = helper.GetBinarySpan();
			sourceBytes.CopyTo(destinationBytes);
		}
	}
}