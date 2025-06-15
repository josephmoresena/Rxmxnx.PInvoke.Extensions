namespace Rxmxnx.PInvoke.Internal.DebugView;

/// <summary>
/// Provides a debug view for the <see cref="CStringSequence"/> and <see cref="FixedCStringSequence"/> classes.
/// </summary>
/// <remarks>
/// This class helps to visualize the content of a <see cref="CStringSequence"/> or  <see cref="FixedCStringSequence"/>
/// instance,
/// displaying each CString as a part of a sequence.
/// </remarks>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed record CStringSequenceDebugView
{
	/// <summary>
	/// Internal array representation of the sequence for debugging.
	/// </summary>
	private readonly CString[] _values;

	/// <summary>
	/// Provides a readable representation of the sequence for debugging.
	/// </summary>
	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public IReadOnlyList<CString> Display => this._values;
	/// <summary>
	/// Provides the number of items in the sequence for debugging.
	/// </summary>
	public Int32 Count => this._values.Length;

	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequenceDebugView"/> class with the
	/// specified <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <param name="seq">The <see cref="CStringSequence"/> instance to provide a debug view for.</param>
	public CStringSequenceDebugView(CStringSequence seq) => this._values = seq.ToArray();
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequenceDebugView"/> class with the
	/// specified <see cref="FixedCStringSequence"/> instance.
	/// </summary>
	/// <param name="fseq">The <see cref="FixedCStringSequence"/> instance to provide a debug view for.</param>
	public CStringSequenceDebugView(FixedCStringSequence fseq) => this._values = fseq.Values.ToArray();
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequenceDebugView"/> class with the
	/// specified <see cref="CStringSequence.Utf8View"/> instance.
	/// </summary>
	/// <param name="value">
	/// The <see cref="CStringSequence.Utf8View"/> instance to provide a debug view for.
	/// </param>
	public CStringSequenceDebugView(CStringSequence.Utf8View value) => this._values = value.ToArray();
}