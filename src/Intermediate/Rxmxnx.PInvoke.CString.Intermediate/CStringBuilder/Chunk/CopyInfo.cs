namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private sealed partial class Chunk
	{
		/// <summary>
		/// Structure containing metadata required to copy chunks.
		/// </summary>
		[Preserve(AllMembers = true, Conditional = true)]
		public readonly struct CopyInfo
		{
			public Chunk Chunk
			{
				get;
#if NETFRAMEWORK || NETSTANDARD2_0
				[SecurityCritical]
#endif
				init;
			}
			public Int32 Start
			{
				get;
#if NETFRAMEWORK || NETSTANDARD2_0
				[SecurityCritical]
#endif
				init;
			}
			public Int32 Count
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