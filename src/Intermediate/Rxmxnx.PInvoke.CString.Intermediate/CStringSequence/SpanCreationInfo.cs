namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public partial class CStringSequence
{
	/// <summary>
	/// Information for span creation.
	/// </summary>
	private readonly unsafe
#if NET9_0_OR_GREATER
		ref
#endif
		struct SpanCreationInfo
	{
		/// <summary>
		/// Pointer to pointers' span.
		/// </summary>
		public void* Pointers { get; init; }
		/// <summary>
		/// UTF-8 lengths.
		/// </summary>
		public Int32?[] Lengths { get; init; }
	}
}