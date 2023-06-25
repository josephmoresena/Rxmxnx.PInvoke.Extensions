namespace Rxmxnx.PInvoke.Tests.Internal;

internal delegate ReadOnlySpan<T> GetSpanDelegate<T>();

[ExcludeFromCodeCoverage]
internal sealed class EnumerableSequence<T> : IEnumerableSequence<T>
{
	private readonly GetSpanDelegate<T> _getSpan;

	public EnumerableSequence(GetSpanDelegate<T> getSpan) => this._getSpan = getSpan;

	T IEnumerableSequence<T>.GetItem(Int32 index) => this._getSpan()[index];
	Int32 IEnumerableSequence<T>.GetSize() => this._getSpan().Length;
}