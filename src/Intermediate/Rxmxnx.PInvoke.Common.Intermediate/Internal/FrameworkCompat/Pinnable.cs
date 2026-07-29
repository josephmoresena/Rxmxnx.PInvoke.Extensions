namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Internal class to retrieve data from managed objects.
/// </summary>
/// <typeparam name="T">Type of the data.</typeparam>
#if !PACKAGE && (NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER)
[ExcludeFromCodeCoverage]
#endif
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