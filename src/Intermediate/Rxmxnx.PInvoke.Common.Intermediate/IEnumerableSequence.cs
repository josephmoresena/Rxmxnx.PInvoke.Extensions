namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines methods to support a simple iteration over a sequence of a specified type.
/// </summary>
/// <remarks>
/// This interface should not be implemented directly; it should only be implemented through the generic interface
/// <see cref="IEnumerableSequence{T}"/>.
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public interface IEnumerableSequence
{
	/// <summary>
	/// This method is intentionally declared to prevent external consumers from implementing this interface.
	/// It should not be implemented or overridden outside the defining assembly.
	/// </summary>
	private protected void DoNotImplement();

#if !PACKAGE || NETCOREAPP
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
	protected static IEnumerator<T> CreateEnumerator<T>(IEnumerableSequence<T> instance,
		Action<IEnumerableSequence<T>>? disposeEnumeration = default)
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
		=> new SequenceEnumerator<T>(instance, disposeEnumeration);
}

/// <summary>
/// Defines methods to support a simple iteration over a sequence of a specified type.
/// </summary>
/// <typeparam name="T">The type of objects to enumerate.</typeparam>
public interface IEnumerableSequence<out T> : IEnumerableSequence, IEnumerable<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	void IEnumerableSequence.DoNotImplement() { }
	IEnumerator<T> IEnumerable<T>.GetEnumerator()
#if !PACKAGE || NETCOREAPP
		=> IEnumerableSequence.CreateEnumerator(this, i => i.DisposeEnumeration());
#else
		=> IEnumerableSequence.CreateEnumerator(this);
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IEnumerator IEnumerable.GetEnumerator()
#if !PACKAGE || NETCOREAPP
		=> IEnumerableSequence.CreateEnumerator(this, i => i.DisposeEnumeration());
#else
		=> IEnumerableSequence.CreateEnumerator(this);
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

#if !PACKAGE || NETCOREAPP
	/// <summary>
	/// Method to call when <see cref="IEnumerator{T}"/> is disposing.
	/// </summary>
	protected void DisposeEnumeration()
	{
		// By default, no resources to dispose.
		// Unable to call implementations of this method in Mono Runtime.
	}
#endif
}