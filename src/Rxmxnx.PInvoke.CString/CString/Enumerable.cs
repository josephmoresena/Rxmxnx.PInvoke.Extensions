using System.Collections;

namespace Rxmxnx.PInvoke;

public partial class CString : IEnumerable, IEnumerable<Byte>
{
    IEnumerator<Byte> IEnumerable<Byte>.GetEnumerator() => this.CreateEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.CreateEnumerator();

    /// <summary>
    /// Creates a <see cref="IEnumerator{Byte}"/> for <see cref="IEnumerable{Byte}"/>
    /// implementation.
    /// </summary>
    /// <returns></returns>
    private IEnumerator<Byte> CreateEnumerator() => new CStringEnumerator(this); 

    /// <summary>
    /// <see cref="IEnumerable{Byte}"/> implementation for <see cref="CString"/>.
    /// </summary>
    private sealed class CStringEnumerator : IEnumerator, IEnumerator<Byte>
    {
        /// <summary>
        /// <see cref="CString"/> instance.
        /// </summary>
        private readonly CString _instance;

        /// <summary>
        /// Current enumerator value;
        /// </summary>
        private Byte _current = default;
        /// <summary>
        /// Iteration current index.
        /// </summary>
        private Int32 _index = default;

        /// <inheritdoc/>
        public Byte Current {
            get
            {
                ThowIfDisposed();
                return this._current;
            }
        }

        Object IEnumerator.Current => this.Current;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instance"><see cref="CString"/> instance.</param>
        public CStringEnumerator(CString instance)
        {
            this._instance = instance;
            this._current = instance[this._current];
        }

        void IDisposable.Dispose() => this._index = -1;

        /// <inheritdoc/>
        public Boolean MoveNext()
        {
            this.ThowIfDisposed();
            if (this._index + 1 < this._instance.Length)
            {
                this._index++;
                this._current = this._instance[this._index];
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
}

