namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Sequence null state.
/// </summary>
internal ref struct NullState
{
	/// <summary>
	/// Current source offset.
	/// </summary>
	public Int32 Offset { get; set; }
	/// <summary>
	/// Current UTF-8 source.
	/// </summary>
	public ReadOnlySpan<Byte> Buffer { get; set; }
	/// <summary>
	/// Current null span.
	/// </summary>
	public Span<Int32> Initial { get; set; }
}