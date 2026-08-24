namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Provides a mechanism for iterating through a sequence of elements of type
/// <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of elements in the sequence.</typeparam>
/// <typeparam name="TEnumerable">The type of current enumerable.</typeparam>
[Preserve(AllMembers = true)]
internal sealed class SequenceEnumerator<T, TEnumerable> : IEnumerator<T> 
	where TEnumerable : IEnumerableSequence<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	private readonly Action<TEnumerable>? _dispose;
	/// <summary>
	/// The sequence of elements to iterate through.
	/// </summary>
	private readonly TEnumerable _instance;

	/// <summary>
	/// The current position in the sequence.
	/// </summary>
	private Int32 _index = -1;

	/// <summary>
	/// Initializes a new instance of the <see cref="SequenceEnumerator{TEnumerator, T}"/> class.
	/// </summary>
	/// <param name="instance">The sequence of elements for iteration.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public SequenceEnumerator(TEnumerable instance)
	{
		this._instance = instance;
		this._dispose = default;
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="SequenceEnumerator{TEnumerator, T}"/> class.
	/// </summary>
	/// <param name="instance">The sequence of elements for iteration.</param>
	/// <param name="dispose">Dispose delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public SequenceEnumerator(TEnumerable instance, Action<TEnumerable>? dispose)
	{
		this._instance = instance;
		this._dispose = dispose;
	}

	/// <inheritdoc/>
	public T Current
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ValidationUtilities.ThrowIfInvalidIndexEnumerator(this._index, this._instance.GetSize());
			return this._instance.GetItem(this._index);
		}
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Object? IEnumerator.Current
#if !NET9_0_OR_GREATER
		=> this.Current;
#else
	{
		get
		{
			ValidationUtilities.ThrowIfNotObject(typeof(T));
			T value = this.Current;
			return Unsafe.As<T, Object?>(ref value);
		}
	}
#endif

	void IDisposable.Dispose() => this._dispose?.Invoke(this._instance);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean MoveNext()
	{
		this._index++;
		return this._index < this._instance.GetSize();
	}
	/// <inheritdoc/>
	public void Reset() => this._index = -1;
}