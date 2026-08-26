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
	public static T[] Empty<T>() => Generic<T>.EmptyArray;

	/// <summary>
	/// Generic class.
	/// </summary>
	/// <typeparam name="T">The type of the elements of the array.</typeparam>
	private static class Generic<T>
	{
		/// <summary>
		/// Internal empty array.
		/// </summary>
		public static readonly T[] EmptyArray = [];
	}
#endif
	/// <summary>
	/// Retrieves a read-only span of lower bounds for each array dimension.
	/// </summary>
	/// <param name="lowerBounds">Destination lower bound span.</param>
	/// <param name="array">Current array instance.</param>
	/// <returns>A read-only view of <paramref name="lowerBounds"/>.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlySpan<Int32> GetLowerBounds(Span<Int32> lowerBounds, Array array)
	{
		for (Int32 i = 0; i < array.Length; i++)
			lowerBounds[i] = array.GetLowerBound(i);
		return lowerBounds;
	}
}