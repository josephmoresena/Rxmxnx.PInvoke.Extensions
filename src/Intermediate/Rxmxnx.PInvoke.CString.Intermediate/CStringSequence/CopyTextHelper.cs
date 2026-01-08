namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public partial class CStringSequence
{
	/// <summary>
	/// Represents the state object used during the creation of a <see cref="CStringSequence"/>.
	/// </summary>
#if NET9_0_OR_GREATER
	private readonly unsafe ref struct CopyTextHelper
#else
	private readonly unsafe struct CopyTextHelper
#endif
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