namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines methods to support a simple iteration over a sequence of a specified type.
/// </summary>
public interface IEnumerableSequence<out T> : IEnumerable<T>
{
	IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.CreateEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => this.CreateEnumerator();
	/// <summary>
	/// Retrieves the element at the specified index.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The element at the specified index.</returns>
	T GetItem(Int32 index);
	/// <summary>
	/// Retrieves the total number of elements in the sequence.
	/// </summary>
	/// <returns>The total number of elements in the sequence.</returns>
	Int32 GetSize();

	/// <summary>
	/// Creates an enumerator that iterates through the sequence.
	/// </summary>
	/// <returns>
	/// An <see cref="IEnumerator{T}"/> that can be used to iterate through the sequence.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerator<T> CreateEnumerator() => new SequenceEnumerator<T>(this);
}