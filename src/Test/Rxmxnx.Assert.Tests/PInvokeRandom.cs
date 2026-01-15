namespace Rxmxnx.PInvoke.Tests;

/// <inheritdoc cref="Random"/>
[ExcludeFromCodeCoverage]
public class PInvokeRandom
{
#if !NET6_0_OR_GREATER
	/// <summary>
	/// Provides a thread-safe <see cref="Random" /> instance that may be used concurrently from any thread.
	/// </summary>
	/// <returns>A <see cref="Random" /> instance.</returns>
	public static readonly Random Shared = new();
#else
	/// <inheritdoc cref="Random.Shared"/>
	public static Random Shared => Random.Shared;
#endif
}