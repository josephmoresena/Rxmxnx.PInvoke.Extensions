namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
internal sealed class EnumerableSequence<T>(GetSpanDelegate<T> getSpan) : IEnumerableSequence<T>
{
	T IEnumerableSequence<T>.GetItem(Int32 index) => getSpan()[index];
	Int32 IEnumerableSequence<T>.GetSize() => getSpan().Length;
}