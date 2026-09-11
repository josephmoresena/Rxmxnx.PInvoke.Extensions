namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private sealed partial class Chunk
	{
		/// <summary>
		/// Structure containing metadata required to allocate chunks for an insertion.
		/// </summary>
		[Preserve(AllMembers = true, Conditional = true)]
		private struct InsertInfo
		{
			/// <summary>
			/// Total number of newly allocated chunks.
			/// </summary>
			public Byte Chunks
			{
				get;
#if NETFRAMEWORK || NETSTANDARD2_0
				[SecurityCritical]
#endif
				init;
			}
			/// <summary>
			/// Number of bytes written into the last chunk.
			/// </summary>
			public Int32 LastCount
			{
				get;
#if NETFRAMEWORK || NETSTANDARD2_0
				[SecurityCritical]
#endif
				init;
			}
		}
	}
}