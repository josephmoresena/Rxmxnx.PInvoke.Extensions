namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
public class MutableInstance<T> : IMutableWrapper<T>
{
	private T _value = default!;

	T IMutableWrapper<T>.Value
	{
		get => this._value;
		set => this._value = value;
	}
#if (!NETSTANDARD2_1 || LEGACY) && !NETCOREAPP3_0_OR_GREATER
	T IWrapper<T>.Value => this._value;
	Object IStrongBox.Value
	{
		get => this._value!;
		set => this._value = (T)value;
	}
#endif
}