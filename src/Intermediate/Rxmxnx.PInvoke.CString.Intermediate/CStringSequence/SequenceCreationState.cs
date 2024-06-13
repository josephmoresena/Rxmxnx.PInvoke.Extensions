namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public partial class CStringSequence
{
	/// <summary>
	/// Represents the state object used during the creation of a <see cref="CStringSequence"/>.
	/// </summary>
	private readonly unsafe struct SequenceCreationState
	{
		/// <summary>
		/// Pointer to byte buffer.
		/// </summary>
		public Byte* Pointer { get; init; }
		/// <summary>
		/// Byte buffer length.
		/// </summary>
		public Int32 Length { get; init; }
		/// <summary>
		/// List to hold the lengths of each UTF-8 text in the sequence.
		/// </summary>
		public List<Int32> NullChars { get; init; }
	}

	/// <summary>
	/// Represents the state object used during the creation of a <see cref="CStringSequence"/>.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the creation callback.</typeparam>
	private readonly struct SequenceCreationState<TState>
	{
		/// <summary>
		/// Creation method.
		/// </summary>
		public CStringSequenceCreationAction<TState> Action { get; init; }
		/// <summary>
		/// State element that is passed to creation method.
		/// </summary>
		public TState State { get; init; }
		/// <summary>
		/// Array containing the lengths of each UTF-8 text in the sequence to be created.
		/// </summary>
		public Int32?[] Lengths { get; init; }

		/// <summary>
		/// Invokes the specified action for UTF-8 text creation.
		/// </summary>
		/// <param name="span">The buffer used for the UTF-8 text.</param>
		/// <param name="index">The index of the current text in the sequence.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void InvokeAction(Span<Byte> span, Int32 index) => this.Action(span, index, this.State);
	}
}