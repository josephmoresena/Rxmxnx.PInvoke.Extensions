namespace Rxmxnx.PInvoke.Internal.DebugView;

/// <summary>
/// Provides a debug view for the <see cref="CStringSequence"/> classes.
/// </summary>
/// <remarks>
/// This class helps to visualize the content of a <see cref="CStringSequence"/> instance, displaying each
/// <see cref="CString"/> as a part of a sequence.
/// </remarks>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
#if NETSTANDARD1_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
internal sealed record CStringSequenceDebugView
#else
internal sealed class CStringSequenceDebugView
#endif
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
	public CStringSequenceDebugView(CStringSequence seq) => this._values = [.. seq,];
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequenceDebugView"/> class with the
	/// specified <see cref="FixedCStringSequence"/> instance.
	/// </summary>
	/// <param name="fseq">The <see cref="FixedCStringSequence"/> instance to provide a debug view for.</param>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[Obsolete]
#endif
	public CStringSequenceDebugView(FixedCStringSequence fseq) => this._values = fseq.Values.ToArray();
#endif
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequenceDebugView"/> class with the
	/// specified <see cref="CStringSequence.Utf8View"/> instance.
	/// </summary>
	/// <param name="value">
	/// The <see cref="CStringSequence.Utf8View"/> instance to provide a debug view for.
	/// </param>
	public CStringSequenceDebugView(CStringSequence.Utf8View value) => this._values = value.ToArray();
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequenceDebugView"/> class with the
	/// specified <see cref="CStringSequence.Utf8View"/> instance.
	/// </summary>
	/// <param name="value">
	/// The <see cref="CStringSequence.Utf8View"/> instance to provide a debug view for.
	/// </param>
	public CStringSequenceDebugView(CStringSequence.Builder value) => this._values = value.ToArray();
}