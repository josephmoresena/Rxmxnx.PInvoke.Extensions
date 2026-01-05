namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public partial class CStringSequence
{
	/// <summary>
	/// Information for span creation.
	/// </summary>
#if NET9_0_OR_GREATER
	private readonly unsafe ref struct SpanCreationInfo
#else
	private readonly unsafe struct SpanCreationInfo
#endif
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