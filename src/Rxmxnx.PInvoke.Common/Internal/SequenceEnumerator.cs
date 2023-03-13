using System.Collections;

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
    private Int32 _index = default;

    /// <inheritdoc/>
    public T Current
    {
        get
        {
            ThowIfDisposed();
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
        this._current = instance.Item(this._index);
    }

    void IDisposable.Dispose() => this._index = -1;

    /// <inheritdoc/>
    public Boolean MoveNext()
    {
        this.ThowIfDisposed();
        if (this._index + 1 < this._instance.Size())
        {
            this._index++;
            this._current = this._instance.Item(this._index);
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public void Reset()
    {
        this.ThowIfDisposed();
        this._index = 0;
    }

    /// <summary>
    /// Throws an exception if current instance is disposed.
    /// </summary>
    /// <exception cref="InvalidOperationException"/>
    private void ThowIfDisposed()
    {
        if (this._index < 0)
            throw new InvalidOperationException("Enumerator Ended");
    }
}

