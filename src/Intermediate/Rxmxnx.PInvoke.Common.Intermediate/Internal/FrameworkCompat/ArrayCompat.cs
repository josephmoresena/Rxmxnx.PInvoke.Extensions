namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Internal class for <see cref="Array"/> compatibility.
/// </summary>
#if !PACKAGE
public static class ArrayCompat
#else
internal static class ArrayCompat
#endif
{
	/// <summary>
	/// Returns an empty array.
	/// </summary>
	/// <typeparam name="T">The type of the elements of the array.</typeparam>
	/// <returns>Returns an empty <see cref="Array" />.</returns>
#if NETSTANDARD1_3_OR_GREATER || NETCOREAPP || NET46_OR_GREATER || UAP10_0
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static T[] Empty<T>() => [];
#else
	public static T[] Empty<T>() => Generic<T>.Empty;

	/// <summary>
	/// Generic class.
	/// </summary>
	/// <typeparam name="T">The type of the elements of the array.</typeparam>
	private static class Generic<T>
	{
		/// <summary>
		/// Internal empty array.
		/// </summary>
		public static readonly T[] Empty = [];
	}
#endif
}