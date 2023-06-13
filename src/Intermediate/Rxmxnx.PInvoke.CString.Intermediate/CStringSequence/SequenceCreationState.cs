namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Represents the state object used during the creation of a <see cref="CStringSequence"/>.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to the creation callback.</typeparam>
    private sealed record SequenceCreationState<TState>
    {
        /// <summary>
        /// The state element that is passed to creation method.
        /// </summary>
        private readonly TState _state;
        /// <summary>
        /// The state element that is passed to creation method.
        /// </summary>
        private readonly CStringSequenceCreationAction<TState> _action;
        /// <summary>
        /// An array containing the lengths of each UTF-8 text in the sequence to be created.
        /// </summary>
        private readonly Int32?[] _lengths;

        /// <summary>
        /// Gets an array containing the lengths of each UTF-8 text in the sequence to be created.
        /// </summary>
        public Int32?[] Lengths => this._lengths;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceCreationState{TState}"/> class.
        /// </summary>
        /// <param name="state">The state object to pass to <paramref name="action"/>.</param>
        /// <param name="action">A callback used to initialize each <see cref="CString"/>.</param>
        /// <param name="lengths">An array containing the lengths of each UTF-8 text in the sequence to be created.</param>
        public SequenceCreationState(TState state, CStringSequenceCreationAction<TState> action, Int32?[] lengths)
        {
            this._state = state;
            this._action = action;
            this._lengths = lengths;
        }

        /// <summary>
        /// Invokes the specified action for UTF-8 text creation.
        /// </summary>
        /// <param name="span">The buffer used for the UTF-8 text.</param>
        /// <param name="index">The index of the current text in the sequence.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvokeAction(Span<Byte> span, Int32 index) => this._action(span, index, this._state);
    }
}

