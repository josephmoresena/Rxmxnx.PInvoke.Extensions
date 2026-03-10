namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Linux
	{
		/// <summary>
		/// This struct storages state for <c>/proc/self/maps</c> file reading.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2292)]
#endif
		[Preserve(AllMembers = true, Conditional = true)]
		private ref struct FileState
		{
			/// <inheritdoc cref="FileState.Buffer"/>
			private Span<Byte> _buffer;

			/// <summary>
			/// Read buffer.
			/// </summary>
			public Span<Byte> Buffer
			{
				// Required backing field for Mono AOT.
				get => this._buffer;
				set => this._buffer = value;
			}
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
			public FileState(Span<Byte> buffer) => this._buffer = buffer;
		}
	}
}