// ReSharper disable NotAccessedPositionalProperty.Global

namespace Rxmxnx.PInvoke;

public partial class BufferTypeMetadata
{
	/// <summary>
	/// Represents a buffer type composition.
	/// </summary>
	/// <param name="TypeofT">The type of items in the buffer.</param>
	/// <param name="Size">Buffer capacity.</param>
	protected readonly record struct Composition(Type TypeofT, UInt16 Size);
}