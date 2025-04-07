namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// State value for sequence item.
	/// </summary>
	/// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="index">The zero-based index of the element into the sequence.</param>
	private readonly struct SequenceItemState(CStringSequence sequence, Int32 index)
		: IUtf8FunctionState<SequenceItemState>
	{
		/// <summary>
		/// Internal sequence.
		/// </summary>
		private readonly CStringSequence _sequence = sequence;
		/// <summary>
		/// Index of the element.
		/// </summary>
		private readonly Int32 _index = index;

		/// <inheritdoc/>
		public static Func<SequenceItemState, GCHandleType, GCHandle> Alloc
			=> (s, t) => GCHandle.Alloc(s._sequence.ToString(), t);

#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		Boolean IUtf8FunctionState<SequenceItemState>.IsNullTerminated => true;

		/// <summary>
		/// Retrieves the span from <paramref name="state"/>.
		/// </summary>
		/// <param name="state">Current item sequence state.</param>
		/// <returns>The binary span for the specified state.</returns>
		public static ReadOnlySpan<Byte> GetSpan(SequenceItemState state)
			=> CStringSequence.GetItemSpan(state._sequence, state._index);

#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
#if NET6_0
		[RequiresPreviewFeatures]
#endif
		static ReadOnlySpan<Byte> IUtf8FunctionState<SequenceItemState>.GetSpan(SequenceItemState state)
			=> SequenceItemState.GetSpan(state);
	}
}