#if !NETSTANDARD2_1 && !NETCOREAPP
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal class to retrieve data from managed objects.
/// </summary>
/// <typeparam name="T">Type of the data.</typeparam>
[StructLayout(LayoutKind.Sequential)]
internal sealed class Pinnable<T>
{
	/// <summary>
	/// Internal reference.
	/// </summary>
#pragma warning disable CS8618
	public T Data;
#pragma warning restore CS8618
}
#endif