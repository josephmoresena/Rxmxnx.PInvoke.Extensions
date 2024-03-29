﻿namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a list of <see cref="IFixedMemory"/> instances.
/// </summary>
/// <remarks>
/// This list can be used for safe operations with fixed blocks of memory using pointers.
/// </remarks>
public readonly ref struct FixedMemoryList
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
	/// Gets the <see cref="IFixedMemory"/> at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The <see cref="IReadOnlyFixedMemory"/> at the specified index.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when the <paramref name="index"/> is out of the range of the list elements.
	/// </exception>
	public IFixedMemory this[Int32 index] => (IFixedMemory)this._values[index];

	/// <summary>
	/// Initializes a new instance of the <see cref="FixedMemoryList"/> structure.
	/// </summary>
	/// <param name="memories">An array of <see cref="FixedMemory"/> instances to be stored in the list.</param>
	/// <remarks>This constructor initializes the list with the provided fixed memory blocks.</remarks>
	internal FixedMemoryList(params FixedMemory[] memories) => this._values = new(memories.Cast<ReadOnlyFixedMemory>());

	/// <summary>
	/// Creates an array from the current <see cref="FixedMemoryList"/> object.
	/// </summary>
	/// <returns>
	/// An array that contains all elements of the current <see cref="FixedMemoryList"/> object.
	/// </returns>
	public IFixedMemory[] ToArray() => this._values.Cast<IFixedMemory>().ToArray();
	/// <summary>
	/// Returns an enumerator that iterates through the <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <returns>An enumerator for the current <see cref="ReadOnlyFixedMemoryList"/> object.</returns>
	public Enumerator GetEnumerator() => new(this._values);

	/// <summary>
	/// Releases all resources used by the <see cref="ReadOnlyFixedMemoryList"/> object.
	/// </summary>
	internal void Unload() => this._values.Unload();

	/// <summary>
	/// Converts a <see cref="FixedMemoryList"/> to a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <param name="memoryList">The <see cref="FixedMemoryList"/> to convert.</param>
	public static implicit operator ReadOnlyFixedMemoryList(FixedMemoryList memoryList) => new(memoryList._values);

	/// <summary>
	/// Enumerates the elements of a <see cref="FixedMemoryList"/>.
	/// </summary>
	public readonly ref struct Enumerator
	{
		/// <summary>
		/// Internal enumerator.
		/// </summary>
		private readonly IEnumerator<FixedMemory> _enumerator;

		/// <summary>
		/// Gets the element at the current position of the enumerator.
		/// </summary>
		/// <value>The element in the list at the current position of the enumerator.</value>
		public IFixedMemory Current
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => this._enumerator.Current;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Enumerator"/> structure.
		/// </summary>
		/// <param name="values">A <see cref="FixedMemory"/> enumerable instance.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal Enumerator(IEnumerable<ReadOnlyFixedMemory> values)
			=> this._enumerator = values.Cast<FixedMemory>().GetEnumerator();

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