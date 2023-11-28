namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
public static class UnsafeLegacy
{
	public static ref T AsRef<T>(in T value) => ref Unsafe.AsRef(value);
}