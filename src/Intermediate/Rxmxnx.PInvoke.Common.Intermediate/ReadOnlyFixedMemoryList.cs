namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a list of <see cref="IReadOnlyFixedMemory"/> instances.
/// </summary>
/// <remarks>
/// This list can be used for safe operations with fixed blocks of memory using pointers.
/// </remarks>
public readonly ref struct ReadOnlyFixedMemoryList
{
	/// <summary>
	/// <see cref="FixedMemoryListValue"/> value.
	/// </summary>
	private readonly FixedMemoryListValue _values;

	/// <summary>
	/// Gets the total number of elements in the list.
	/// </summary>
	/// <value>The total number of elements in the list.</value>
	public Int32 Count => this._values.Count;
	/// <summary>
	/// Indicates whether the current list is empty.
	/// </summary>
	/// <value><see langword="true"/> if the list is empty; otherwise, <see langword="false"/>.</value>
	public Boolean IsEmpty => this.Count == 0;
	/// <summary>
	/// Gets the <see cref="IReadOnlyFixedMemory"/> at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The <see cref="IReadOnlyFixedMemory"/> at the specified index.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when the <paramref name="index"/> is out of the range of the list elements.
	/// </exception>
	[IndexerName("Item")]
	public IReadOnlyFixedMemory this[Int32 index] => this._values[index];

#if !NET9_0_OR_GREATER
	/// <summary>
	/// Initializes a new instance of the <see cref="ReadOnlyFixedMemoryList"/> structure.
	/// </summary>
	/// <param name="memories">An array of <see cref="FixedMemory"/> instances to be stored in the list.</param>
	/// <remarks>This constructor initializes the list with the provided fixed memory blocks.</remarks>
	internal ReadOnlyFixedMemoryList(params FixedMemory[] memories) : this(memories.AsSpan()) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="ReadOnlyFixedMemoryList"/> structure.
	/// </summary>
	/// <param name="memories">An array of <see cref="ReadOnlyFixedMemory"/> instances to be stored in the list.</param>
	/// <remarks>This constructor initializes the list with the provided fixed memory blocks.</remarks>
	internal ReadOnlyFixedMemoryList(params ReadOnlyFixedMemory[] memories) : this(memories.AsSpan()) { }
#endif

	/// <summary>
	/// Initializes a new instance of the <see cref="ReadOnlyFixedMemoryList"/> structure.
	/// </summary>
	/// <param name="memories">A read-only span of <see cref="FixedMemory"/> instances to be stored in the list.</param>
	/// <remarks>This constructor initializes the list with the provided fixed memory blocks.</remarks>
	internal ReadOnlyFixedMemoryList(
#if NET9_0_OR_GREATER
		params
#endif
		ReadOnlySpan<FixedMemory> memories)
		=> this._values = FixedMemoryListValue.Create(memories);
	/// <summary>
	/// Initializes a new instance of the <see cref="ReadOnlyFixedMemoryList"/> structure.
	/// </summary>
	/// <param name="memories">A read-only span of <see cref="ReadOnlyFixedMemory"/> instances to be stored in the list.</param>
	/// <remarks>This constructor initializes the list with the provided fixed memory blocks.</remarks>
	internal ReadOnlyFixedMemoryList(
#if NET9_0_OR_GREATER
		params
#endif
		ReadOnlySpan<ReadOnlyFixedMemory> memories)
		=> this._values = FixedMemoryListValue.Create(memories);
	/// <summary>
	/// Initializes a new instance of the <see cref="ReadOnlyFixedMemoryList"/> structure.
	/// </summary>
	/// <param name="values">An instance of the <see cref="FixedMemoryListValue"/> to be stored in the list.</param>
	/// <remarks>This constructor initializes the list with the provided internal fixed memory list value.</remarks>
	internal ReadOnlyFixedMemoryList(FixedMemoryListValue values) => this._values = values;

	/// <summary>
	/// Creates an array from the current <see cref="ReadOnlyFixedMemoryList"/> instance.
	/// </summary>
	/// <returns>
	/// An array that contains all elements of the current <see cref="ReadOnlyFixedMemoryList"/> instance.
	/// </returns>
	public IReadOnlyFixedMemory[] ToArray()
	{
		if (this._values.Count <= 0) return [];

		IReadOnlyFixedMemory[] result = new IReadOnlyFixedMemory[this._values.Count];
		ref ReadOnlyFixedMemory refI = ref Unsafe.As<IReadOnlyFixedMemory, ReadOnlyFixedMemory>(ref result[0]);
		Span<ReadOnlyFixedMemory> span = MemoryMarshal.CreateSpan(ref refI, result.Length);
		this._values.AsSpan().CopyTo(span);
		return result;
	}
	/// <summary>
	/// Returns an enumerator that iterates through the <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <returns>An enumerator for the current <see cref="ReadOnlyFixedMemoryList"/> instance.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public Enumerator GetEnumerator() => new(this._values.AsSpan());

	/// <summary>
	/// Releases all resources used by the <see cref="ReadOnlyFixedMemoryList"/> instance.
	/// </summary>
	internal void Unload() => this._values.Unload();

	/// <summary>
	/// Enumerates the elements of a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	public ref struct Enumerator
	{
		/// <summary>
		/// Internal enumerator.
		/// </summary>
		private ReadOnlySpan<ReadOnlyFixedMemory>.Enumerator _enumerator;

		/// <summary>
		/// Gets the element at the current position of the enumerator.
		/// </summary>
		/// <value>The element in the list at the current position of the enumerator.</value>
		public IReadOnlyFixedMemory Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this._enumerator.Current;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Enumerator"/> structure.
		/// </summary>
		/// <param name="values">A <see cref="FixedMemory"/> enumerable instance.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Enumerator(ReadOnlySpan<ReadOnlyFixedMemory> values) => this._enumerator = values.GetEnumerator();

		/// <summary>
		/// Advances the enumerator to the next element of the <see cref="ReadOnlyFixedMemoryList"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the enumerator was successfully advanced to the next element;
		/// <see langword="false"/> if the enumerator has passed the end of the list.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Boolean MoveNext() => this._enumerator.MoveNext();
	}
}