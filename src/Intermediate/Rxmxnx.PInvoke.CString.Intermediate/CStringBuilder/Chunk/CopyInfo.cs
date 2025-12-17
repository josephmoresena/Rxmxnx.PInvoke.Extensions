namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	internal sealed partial class Chunk
	{
		/// <summary>
		/// Structure containing metadata required to copy chunks.
		/// </summary>
		public readonly struct CopyInfo
		{
			public Chunk Chunk { get; init; }
			public Int32 Start { get; init; }
			public Int32 Count { get; init; }
		}
	}
}