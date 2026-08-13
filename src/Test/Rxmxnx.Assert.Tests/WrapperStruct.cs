namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
[StructLayout(LayoutKind.Sequential)]
public struct WrapperStruct<T>
{
	public T Value { get; set; }
}