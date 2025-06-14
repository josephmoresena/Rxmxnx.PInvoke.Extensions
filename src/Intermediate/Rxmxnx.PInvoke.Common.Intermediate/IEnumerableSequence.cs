namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines methods to support a simple iteration over a sequence of a specified type.
/// </summary>
public interface IEnumerableSequence<out T> : IEnumerable<T>
{
	IEnumerator<T> IEnumerable<T>.GetEnumerator()
#if NETCOREAPP || !PACKAGE
		=> IEnumerableSequence<T>.CreateEnumerator(this, i => i.DisposeEnumeration());
#else
		=> IEnumerableSequence<T>.CreateEnumerator(this);
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IEnumerator IEnumerable.GetEnumerator()
#if NETCOREAPP || !PACKAGE
		=> IEnumerableSequence<T>.CreateEnumerator(this, i => i.DisposeEnumeration());
#else
		=> IEnumerableSequence<T>.CreateEnumerator(this);
#endif
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

#if NETCOREAPP || !PACKAGE
	/// <summary>
	/// Method to call when <see cref="IEnumerator{T}"/> is disposing.
	/// </summary>
	protected void DisposeEnumeration()
	{
		// By default, no resources to dispose.
		// Unable to call implementations of this method in Mono Runtime.
	}
#endif

#if NETCOREAPP || !PACKAGE
	/// <summary>
	/// Creates an enumerator that iterates through <paramref name="instance"/> instance.
	/// </summary>
	/// <param name="instance">A <see cref="IEnumerableSequence{T}"/> instance.</param>
	/// <param name="disposeEnumeration">Delegate to dispose enumeration.</param>
	/// <returns>
	/// An <see cref="IEnumerator{T}"/> that can be used to iterate through the sequence.
	/// </returns>
	/// <remarks>
	/// This method ignores the private implementation of <see cref="IEnumerableSequence{T}.DisposeEnumeration()"/> and
	/// uses only the <paramref name="disposeEnumeration"/> delegate.
	/// </remarks>
#else
	/// <summary>
	/// Creates an enumerator that iterates through <paramref name="instance"/> instance.
	/// </summary>
	/// <param name="instance">A <see cref="IEnumerableSequence{T}"/> instance.</param>
	/// <param name="disposeEnumeration">Delegate to dispose enumeration.</param>
	/// <returns>
	/// An <see cref="IEnumerator{T}"/> that can be used to iterate through the sequence.
	/// </returns>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static IEnumerator<T> CreateEnumerator(IEnumerableSequence<T> instance,
		Action<IEnumerableSequence<T>>? disposeEnumeration = default)
		=> new SequenceEnumerator<T>(instance, disposeEnumeration);
}