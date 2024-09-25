namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public unsafe partial class CStringSequence
{
	/// <summary>
	/// State for pinned <see cref="CString"/> span.
	/// </summary>
	internal readonly struct CStringSpanState
	{
#pragma warning disable CS8500
		/// <summary>
		/// <see cref="CString"/> pointer.
		/// </summary>
		public CString?* Ptr { get; init; }
		/// <summary>
		/// Span length.
		/// </summary>
		public Int32 Length { get; init; }
#pragma warning restore CS8500
	}
}