namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This struct stores state for <c>/proc/self/maps</c> file reading.
/// </summary>
[Preserve(AllMembers = true, Conditional = true)]
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2292)]
#endif
internal ref struct FileState
{
	/// <summary>
	/// Read buffer.
	/// </summary>
	public Span<Byte> Buffer { get; set; }
	/// <summary>
	/// The number of bytes read.
	/// </summary>
	public Int32 ReadBytes { get; set; }
	/// <summary>
	/// The current search index.
	/// </summary>
	public Int32 Index { get; set; }
	/// <summary>
	/// Buffer offset.
	/// </summary>
	public Int32 Offset { get; set; }
	/// <summary>
	/// Auxiliary value.
	/// </summary>
	public Int32 Auxiliar { get; set; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="buffer">Read buffer.</param>
	public FileState(Span<Byte> buffer) => this.Buffer = buffer;
}