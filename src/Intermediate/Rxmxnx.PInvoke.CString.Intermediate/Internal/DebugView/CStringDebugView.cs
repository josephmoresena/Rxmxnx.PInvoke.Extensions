namespace Rxmxnx.PInvoke.Internal.DebugView;

/// <summary>
/// Provides a debug view for the <see cref="CString"/> class.
/// </summary>
/// <remarks>
/// This class automatically decodes the UTF-8 bytes to provide a readable <see cref="String"/> representation.
/// </remarks>
[ExcludeFromCodeCoverage]
internal sealed record CStringDebugView
{
    /// <summary>
    /// Enumeration of the flags used in the <see cref="CString"/> class for debugging purposes.
    /// </summary>
    [Flags]
    public enum CStringFeatures : Byte
    {
        None = 0,
        NullTerminated = 1,
        Managed = 2,
        Unmanaged = 4,
        Function = 8,
        Slice = 16,
    }

    /// <summary>
    /// Internal string representation of the value for debugging.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly String _value;
    /// <summary>
    /// Length of the UTF-8 string for debugging.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Int32 _utf8Length;
    /// <summary>
    /// Flags that represent the properties of the current <see cref="CString"/> instance for debugging.
    /// </summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly CStringFeatures _flags;

    /// <summary>
    /// Provides a readable string representation of the <see cref="CString"/> value for debugging.
    /// </summary>
    public String Value => this._value;
    /// <summary>
    /// Provides the count of UTF-8 units for debugging.
    /// </summary>
    public Int32 Utf8Length => this._utf8Length;
    /// <summary>
    /// Provides the debug flags associated with the <see cref="CString"/> instance.
    /// </summary>
    public CStringFeatures Flags => _flags;

    /// <summary>
    /// Initializes a new instance of the <see cref="CStringDebugView"/> class with the specified
    /// <see cref="CString"/> instance.
    /// </summary>
    /// <param name="cstr">The <see cref="CString"/> instance to provide a debug view for.</param>
    public CStringDebugView(CString cstr)
    {
        this._value = cstr.ToString();
        this._utf8Length = cstr.Length;
        this._flags = cstr.IsNullTerminated ? CStringFeatures.NullTerminated : CStringFeatures.None;

        if (cstr.IsReference)
            this._flags |= CStringFeatures.Unmanaged;
        else if (cstr.IsFunction)
            this._flags |= CStringFeatures.Function;
        else
            this._flags |= CStringFeatures.Managed;

        if (cstr.IsSegmented)
            this._flags |= CStringFeatures.Slice;
    }
}