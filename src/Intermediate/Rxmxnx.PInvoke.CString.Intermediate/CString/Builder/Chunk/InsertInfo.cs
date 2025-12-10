namespace Rxmxnx.PInvoke;

public partial class CString
{
	public sealed partial class Builder
	{
		private sealed partial class Chunk
		{
			/// <summary>
			/// Structure containing metadata required to allocate chunks for an insertion.
			/// </summary>
			private struct InsertInfo
			{
				/// <summary>
				/// Total number of newly allocated chunks.
				/// </summary>
				public Byte Chunks { get; init; }
				/// <summary>
				/// Number of bytes written into the last chunk.
				/// </summary>
				public Int32 LastCount { get; init; }
			}
		}
	}
}