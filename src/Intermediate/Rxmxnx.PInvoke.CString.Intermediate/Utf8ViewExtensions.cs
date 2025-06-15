namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for viewing <see cref="CStringSequence"/> operations.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static class Utf8ViewExtensions
{
	/// <summary>
	/// Creates a new instance of <see cref="CStringSequence.Utf8View"/> from the current sequence.
	/// </summary>
	/// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="includeEmptyItems">Specifies whether empty items should be included in the enumeration.</param>
	/// <returns>A <see cref="CStringSequence.Utf8View"/> instance.</returns>
	public static CStringSequence.Utf8View CreateView(this CStringSequence? sequence, Boolean includeEmptyItems = true)
		=> new(sequence, includeEmptyItems);
}