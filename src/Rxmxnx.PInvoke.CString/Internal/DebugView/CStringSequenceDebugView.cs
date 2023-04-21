namespace Rxmxnx.PInvoke.Internal.DebugView;

/// <summary>
/// Debug View class for <see cref="CStringSequence"/>
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed record CStringSequenceDebugView
{
    /// <summary>
    /// Internal value.
    /// </summary>
    private readonly String[] _value;

    /// <summary>
    /// Value to display.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public String[] Display => this._value;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="seq">A <see cref="CStringSequence"/> instance.</param>
    public CStringSequenceDebugView(CStringSequence seq)
        => this._value = seq.Select(c => c.ToString()).ToArray();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="fseq">A <see cref="FixedCStringSequence"/> instance.</param>
    public CStringSequenceDebugView(FixedCStringSequence fseq)
        => this._value = fseq.Values.Select(c => c.ToString()).ToArray();
}