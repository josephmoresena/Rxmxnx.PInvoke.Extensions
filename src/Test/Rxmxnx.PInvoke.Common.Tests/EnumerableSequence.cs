using System.Collections;

namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal sealed class EnumerableSequence<T>(GetSpanDelegate<T> getSpan) : IEnumerableSequence<T>
{
	T IEnumerableSequence<T>.GetItem(Int32 index) => getSpan()[index];
	Int32 IEnumerableSequence<T>.GetSize() => getSpan().Length;
#if (!NETSTANDARD2_1 || LEGACY) && !NETCOREAPP3_0_OR_GREATER
	IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.CreateDefaultEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => this.CreateDefaultEnumerator();
#endif
}