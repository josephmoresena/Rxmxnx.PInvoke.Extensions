namespace Rxmxnx.PInvoke.Internal.DebugView;

/// <summary>
/// Debug View class for <see cref="CString"/>
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed record CStringDebugView
{
    /// <summary>
    /// Internal value.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly String _value;

    /// <summary>
    /// Value to display.
    /// </summary>
    public String Value => this._value;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="cstr">A <see cref="CString"/> instance.</param>
    public CStringDebugView(CString cstr) => this._value = cstr.ToString();
}