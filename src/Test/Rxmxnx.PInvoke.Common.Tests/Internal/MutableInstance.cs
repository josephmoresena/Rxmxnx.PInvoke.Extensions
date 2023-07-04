namespace Rxmxnx.PInvoke.Tests.Internal;

[ExcludeFromCodeCoverage]
public class MutableInstance<T> : IMutableWrapper<T>
{
	protected T _value;

	public MutableInstance() => this._value = default!;

	T IMutableWrapper<T>.Value
	{
		get => this._value;
		set => this._value = value;
	}
}