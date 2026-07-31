namespace Rxmxnx.PInvoke;

public partial class BufferTypeMetadata
{
	/// <summary>
	/// Represents a buffer type composition.
	/// </summary>
	protected readonly record struct Composition
	{
		/// <summary>The type of items in the buffer.</summary>
		public Type TypeofT { get; init; }
		/// <summary>Buffer capacity.</summary>
		public UInt16 Size { get; init; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="typeofT">The type of items in the buffer.</param>
		/// <param name="size">Buffer capacity.</param>
		public Composition(Type typeofT, UInt16 size)
		{
			this.TypeofT = typeofT;
			this.Size = size;
		}
	}
}