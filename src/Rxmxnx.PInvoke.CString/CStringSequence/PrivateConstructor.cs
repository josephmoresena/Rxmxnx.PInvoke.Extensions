namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="sequence"><see cref="CStringSequence"/> instance.</param>
    private CStringSequence(CStringSequence sequence)
    {
        this._lengths = sequence._lengths.ToArray();
        this._value = String.Create(sequence._value.Length, sequence, CopySequence);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="value">Internal buffer.</param>
    /// <param name="lengths">Collection of text length for buffer interpretation.</param>
    private CStringSequence(String value, Int32?[] lengths)
    {
        this._value = value;
        this._lengths = lengths;
    }
}

