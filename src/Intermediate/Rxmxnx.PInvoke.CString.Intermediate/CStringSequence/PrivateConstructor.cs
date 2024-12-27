namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Private constructor.
	/// </summary>
	private CStringSequence()
	{
		this._lengths = [];
		this._value = String.Empty;
		this._cache = Array.Empty<CString?>();
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class by making a deep copy of
	/// the values in the existing <see cref="CStringSequence"/>.
	/// </summary>
	/// <param name="sequence">The <see cref="CStringSequence"/> instance to copy.</param>
	private CStringSequence(CStringSequence sequence)
	{
		this._lengths = sequence._lengths.ToArray();
		this._value = String.Create(sequence._value.Length, sequence, CStringSequence.CopySequence);
		this._cache = CStringSequence.CreateCache(this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class with a predefined
	/// internal buffer and lengths for buffer interpretation.
	/// </summary>
	/// <param name="value">The string that represents the internal buffer for the sequence.</param>
	/// <param name="lengths">
	/// The collection of lengths for each text in the buffer. Used for interpreting the buffer content.
	/// </param>
	private CStringSequence(String value, Int32?[] lengths)
	{
		this._value = value;
		this._lengths = lengths;
		this._cache = CStringSequence.CreateCache(this._lengths);
	}
}