using System;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    internal partial class ValueRegion<T>
    {
        /// <summary>
        /// This class represents a memory region in which an array of <typeparamref name="T"/> 
        /// values is found.
        /// </summary>
        private sealed class FuncRegion : ValueRegion<T>
        {
            /// <summary>
            /// Internal <see cref="ReadOnlySpanFunc{T}"/> instance. 
            /// </summary>
            private readonly ReadOnlySpanFunc<T> _func;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="func">Internal <see cref="ReadOnlySpanFunc{T}"/> instance. </param>
            public FuncRegion(ReadOnlySpanFunc<T> func) => this._func = func;

            /// <inheritdoc/>
            protected override ReadOnlySpan<T> AsSpan() => this._func();

            /// <summary>
            /// Gets a function from this memory region.
            /// </summary>
            /// <returns>A function from this memory region.</returns>
            public ReadOnlySpanFunc<T>? AsReadOnlySpanFunc() => this._func;
        }
    }
}
