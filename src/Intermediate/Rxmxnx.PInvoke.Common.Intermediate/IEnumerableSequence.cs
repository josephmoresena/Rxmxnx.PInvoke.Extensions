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
[Preserve(AllMembers = true)]
public interface IEnumerableSequence
{
	/// <summary>
	/// This method is intentionally declared to prevent external consumers from implementing this interface.
	/// It should not be implemented or overridden outside the defining assembly.
	/// </summary>
#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER
	internal void DoNotImplement();
#else
	private protected void DoNotImplement();

#if !PACKAGE || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Creates an enumerator that iterates through <paramref name="instance"/> instance.
	/// </summary>
	/// <typeparam name="T">The type of elements in the sequence.</typeparam>
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
	/// <typeparam name="T">The type of elements in the sequence.</typeparam>
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
		=> instance.CreateDefaultEnumerator(disposeEnumeration);
#endif
}

/// <summary>
/// Defines methods to support a simple iteration over a sequence of a specified type.
/// </summary>
/// <typeparam name="T">The type of objects to enumerate.</typeparam>
public interface IEnumerableSequence<out T> : IEnumerable<T>, IEnumerableSequence
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
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

#if !PACKAGE && NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
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

/// <summary>
/// Extension class for enumerable sequence instances.
/// </summary>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
#endif
public static class EnumerableSequenceExtensions
{
#if !PACKAGE && NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
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
	/// <typeparam name="T">The type of elements in the sequence.</typeparam>
	/// <param name="instance">A <see cref="IEnumerableSequence{T}"/> instance.</param>
	/// <param name="disposeEnumeration">Delegate to dispose enumeration.</param>
	/// <returns>
	/// An <see cref="IEnumerator{T}"/> that can be used to iterate through the sequence.
	/// </returns>
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerator<T> CreateDefaultEnumerator<T>(this IEnumerableSequence<T> instance,
		Action<IEnumerableSequence<T>>? disposeEnumeration = default)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> new SequenceEnumerator<T, IEnumerableSequence<T>>(instance, disposeEnumeration);
	/// <summary>
	/// Creates an enumerator that iterates through <paramref name="instance"/> instance.
	/// </summary>
	/// <typeparam name="T">The type of elements in the sequence.</typeparam>
	/// <typeparam name="TEnumerable">The type of current enumerable.</typeparam>
	/// <param name="instance">A <see cref="IEnumerableSequence{T}"/> instance.</param>
	/// <param name="disposeEnumeration">Delegate to dispose enumeration.</param>
	/// <returns>
	/// An <see cref="IEnumerator{T}"/> that can be used to iterate through the sequence.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerator<T> CreateDefaultEnumerator<TEnumerable, T>(this TEnumerable instance,
		Action<TEnumerable>? disposeEnumeration = default) where TEnumerable : struct, IEnumerableSequence<T>
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> new SequenceEnumerator<T, TEnumerable>(instance, disposeEnumeration);
}