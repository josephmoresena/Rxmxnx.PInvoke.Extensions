namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// A helper class used to create subsequences from a parent <see cref="CStringSequence"/>.
	/// </summary>
	private readonly struct SubsequenceHelper
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