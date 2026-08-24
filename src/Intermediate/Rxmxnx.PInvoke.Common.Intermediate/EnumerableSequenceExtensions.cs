namespace Rxmxnx.PInvoke;

/// <summary>
/// Extension class for enumerable sequence instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
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
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerator<T> CreateDefaultEnumerator<TEnumerable, T>(this TEnumerable instance,
		Action<TEnumerable>? disposeEnumeration = default) where TEnumerable : struct, IEnumerableSequence<T>
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
		=> new SequenceEnumerator<T, TEnumerable>(instance, disposeEnumeration);
}