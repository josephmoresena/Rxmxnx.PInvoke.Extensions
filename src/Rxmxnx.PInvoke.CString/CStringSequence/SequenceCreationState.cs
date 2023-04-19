namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// The state object for <see cref="CStringSequence"/> creation.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to creation callback.</typeparam>
	private sealed record SequenceCreationState<TState>
	{
        /// <summary>
        /// The element to pass to <see cref="SequenceCreationState{TState}._action"/>
        /// </summary>
        private readonly TState _state;
        /// <summary>
        /// A callback to initialize each <see cref="CString"/>.
        /// </summary>
        private readonly CStringSequenceCreationAction<TState> _action;
        /// <summary>
        /// The lengths of the UTF-8 text sequence to create.
        /// </summary>
        private readonly Int32?[] _lengths;

        /// <summary>
        /// The lengths of the UTF-8 text sequence to create.
        /// </summary>
        public Int32?[] Lengths => this._lengths;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="state">The element to pass to <paramref name="action"/>.</param>
        /// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
        /// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
        public SequenceCreationState(TState state, CStringSequenceCreationAction<TState> action, Int32?[] lengths)
        {
            this._state = state;
            this._action = action;
            this._lengths = lengths;
        }

        /// <summary>
        /// Invokes the action for UTF-8 text creation.
        /// </summary>
        /// <param name="span">UTF-8 buffer.</param>
        /// <param name="index">Text index.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvokeAction(Span<Byte> span, Int32 index) => this._action(span, index, this._state);
    }
}

