namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[StructLayout(LayoutKind.Sequential)]
internal readonly struct WrapperStruct<T>
{
	public T Value { get; init; }
}