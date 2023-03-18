namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implmentation of <see cref="IEnumerator{T}"/>.
/// </summary>
/// <typeparam name="T">Type of <see cref="IEnumerator"/> item.</typeparam>
internal sealed class SequenceEnumerator<T> : IEnumerator, IEnumerator<T>
{
    /// <summary>
    /// <see cref="IEnumerableSequence{T}"/> instance.
    /// </summary>
    private readonly IEnumerableSequence<T> _instance;

    /// <summary>
    /// Current enumerator value;
    /// </summary>
    private T _current = default!;
    /// <summary>
    /// Iteration current index.
    /// </summary>
    private Int32 _index = -1;

    /// <inheritdoc/>
    public T Current
    {
        get
        {
            if (this._index < 0)
                throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
            if (this._index >= this._instance.GetSize())
                throw new InvalidOperationException("Enumeration already finished.");
            return this._current;
        }
    }

    Object IEnumerator.Current => this.Current!;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance"><see cref="IEnumerableSequence{T}"/> instance.</param>
    public SequenceEnumerator(IEnumerableSequence<T> instance)
    {
        this._instance = instance;
    }

    void IDisposable.Dispose() { }

    /// <inheritdoc/>
    public Boolean MoveNext()
    {
        this._index++;
        if (this._index < this._instance.GetSize())
        {
            this._current = this._instance.GetItem(this._index);
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public void Reset() => this._index = -1;
}

