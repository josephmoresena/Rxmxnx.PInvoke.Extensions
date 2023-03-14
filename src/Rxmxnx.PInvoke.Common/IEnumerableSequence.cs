using System.Collections;

namespace Rxmxnx.PInvoke;

/// <summary>
/// Exposes the enumerator, which supports a simple iteration over a sequence of a
/// specified type.
/// </summary>
public interface IEnumerableSequence<out T> : IEnumerable, IEnumerable<T>
{
    /// <summary>
    /// Retrieves the element at given index.
    /// </summary>
    /// <param name="index">A position in the current instance.</param>
    /// <returns>Element at <paramref name="index"/>.</returns>
    T GetItem(Int32 index);

    /// <summary>
    /// Retrieves the number of total elements in current instance.
    /// </summary>
    /// <returns>Number of total elements in current instance.</returns>
    Int32 GetSize();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.CreateEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.CreateEnumerator();

    /// <summary>
    /// Creates a <see cref="IEnumerator{Byte}"/> for <see cref="IEnumerable{Byte}"/>
    /// implementation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator<T> CreateEnumerator() => new SequenceEnumerator<T>(this);
}

