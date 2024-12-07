namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[StructLayout(LayoutKind.Sequential)]
public readonly struct WrapperStruct<T>
{
	public T Value { get; init; }
}