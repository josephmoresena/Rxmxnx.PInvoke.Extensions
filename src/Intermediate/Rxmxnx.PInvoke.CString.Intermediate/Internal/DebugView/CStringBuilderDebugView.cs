namespace Rxmxnx.PInvoke.Internal.DebugView;

/// <summary>
/// Provides a debug view for the <see cref="CStringBuilder"/> class.
/// </summary>
/// <remarks>
/// This class automatically decodes the UTF-8 bytes to provide a readable <see cref="String"/> representation.
/// </remarks>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed record CStringBuilderDebugView
{
	/// <summary>
	/// Provides the builder chunks information.
	/// </summary>
	private readonly ChunkInfo[] _chunks;
	/// <summary>
	/// Provides the count of UTF-8 units for debugging.
	/// </summary>
	private readonly Int32 _length;

	/// <summary>
	/// Provides the builder chunks information.
	/// </summary>
	public ChunkInfo[] Chunks => this._chunks;
	/// <summary>
	/// Provides the count of UTF-8 units for debugging.
	/// </summary>
	public Int32 Length => this._length;
	/// <summary>
	/// Provides a readable string representation of the <see cref="CString"/> value for debugging.
	/// </summary>
	public String Value { get; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CStringBuilderDebugView"/> class with the specified
	/// <see cref="CStringBuilder"/> instance.
	/// </summary>
	/// <param name="value">The <see cref="CStringBuilder"/> instance to provide a debug view for.</param>
	public CStringBuilderDebugView(CStringBuilder value)
		=> this.Value = value.GetDebugInfo(out this._length, out this._chunks);

	/// <summary>
	/// Chunk information.
	/// </summary>
	/// <param name="Size">Number of used bytes in the chunk.</param>
	/// <param name="Used">Number of the total bytes in the chunk.</param>
	public readonly record struct ChunkInfo(Int32 Size, Int32 Used);
}