namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Linux
	{
		/// <summary>
		/// This struct storages state for <c>/proc/self/maps</c> file reading.
		/// </summary>
		private ref struct FileState
		{
			/// <summary>
			/// Read buffer.
			/// </summary>
			public Span<Byte> Buffer { get; set; }
			/// <summary>
			/// Count of read bytes.
			/// </summary>
			public Int32 ReadBytes { get; set; }
			/// <summary>
			/// Index of search.
			/// </summary>
			public Int32 Index { get; set; }
			/// <summary>
			/// Buffer offset.
			/// </summary>
			public Int32 Offset { get; set; }
			/// <summary>
			/// Auxiliar value.
			/// </summary>
			public Int32 Auxiliar { get; set; }

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="buffer">Read buffer.</param>
			public FileState(Span<Byte> buffer) => this.Buffer = buffer;
		}
	}
}