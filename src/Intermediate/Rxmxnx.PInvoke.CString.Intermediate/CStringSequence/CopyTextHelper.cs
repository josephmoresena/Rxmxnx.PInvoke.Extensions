namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public partial class CStringSequence
{
	/// <summary>
	/// Represents the state object used during the creation of a <see cref="CStringSequence"/>.
	/// </summary>
	private readonly unsafe struct CopyTextHelper
	{
		/// <summary>
		/// Pointer to byte buffer.
		/// </summary>
		public Byte* Pointer { get; init; }
		/// <summary>
		/// Byte buffer length.
		/// </summary>
		public Int32 Length { get; init; }
		/// <summary>
		/// List to hold the lengths of each UTF-8 text in the sequence.
		/// </summary>
		public List<Int32> NullChars { get; init; }
	}
}