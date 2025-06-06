namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// State value for sequence item.
	/// </summary>
	/// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="index">The zero-based index of the element into the sequence.</param>
	private readonly struct SequenceItemState(CStringSequence sequence, Int32 index)
#if NET6_0_OR_GREATER
		: IUtf8FunctionState<SequenceItemState>
#endif
	{
		/// <summary>
		/// Internal sequence.
		/// </summary>
		private readonly CStringSequence _sequence = sequence;
		/// <summary>
		/// Index of the element.
		/// </summary>
		private readonly Int32 _index = index;

		/// <summary>
		/// Allocates a <see cref="GCHandle"/> for the specified <paramref name="t"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GCHandle Alloc(SequenceItemState s, GCHandleType t) => GCHandle.Alloc(s._sequence.ToString(), t);

#if NET6_0_OR_GREATER
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		Boolean IUtf8FunctionState<SequenceItemState>.IsNullTerminated => true;
#endif

		/// <summary>
		/// Retrieves the span from <paramref name="state"/>.
		/// </summary>
		/// <param name="state">Current item sequence state.</param>
		/// <returns>The binary span for the specified state.</returns>
		public static ReadOnlySpan<Byte> GetSpan(SequenceItemState state)
			=> CStringSequence.GetItemSpan(state._sequence, state._index);

#if NET6_0_OR_GREATER
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
#if NET6_0
		[RequiresPreviewFeatures]
#endif
		static ReadOnlySpan<Byte> IUtf8FunctionState<SequenceItemState>.GetSpan(SequenceItemState state)
			=> SequenceItemState.GetSpan(state);
#endif
	}
}