namespace Rxmxnx.PInvoke.Internal.Debug;

/// <summary>
/// Debug View class for <see cref="CString"/>
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed record CStringDebugView
{
    /// <summary>
    /// Internal value.
    /// </summary>
    private readonly String _value;

    /// <summary>
    /// Value to display.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public String Display => this._value;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="cstr">A <see cref="CString"/> instance.</param>
    public CStringDebugView(CString cstr) => this._value = cstr.ToString();
}