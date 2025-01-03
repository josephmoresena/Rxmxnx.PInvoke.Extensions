﻿namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Represents the state object used during the creation of a <see cref="CStringSequence"/>.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the creation callback.</typeparam>
	private readonly
#if NET9_0_OR_GREATER
		ref
#endif
		struct SequenceCreationHelper<TState>
#if NET9_0_OR_GREATER
	where TState : allows ref struct
#endif
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