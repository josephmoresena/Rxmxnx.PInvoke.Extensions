#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Extension class for <see cref="SortedSet{T}"/> and <see cref="SortedList{TKey,TValue}"/>.
/// </summary>
internal static class SortedCollectionsExtensions
{
#if NETFRAMEWORK && !NET472_OR_GREATER || NETSTANDARD2_0
	/// <summary>
	/// Searches the set for a given value and returns the equal value it finds, if any.
	/// </summary>
	/// <param name="set">Current <see cref="SortedSet{T}"/> instance.</param>
	/// <param name="equalValue">The value to search for.</param>
	/// <param name="actualValue">
	/// The value from the set that the search found, or the default value of <typeparamref name="T"/>
	/// when the search yielded no match.
	/// </param>
	/// <returns>A value indicating whether the search was successful.</returns>
	public static Boolean TryGetValue<T>(this SortedSet<T> set, T equalValue, out T actualValue) where T : struct
	{
		if (set == null)
			throw new ArgumentNullException(nameof(set));

		SortedSet<T> view = set.GetViewBetween(equalValue, equalValue);

		using (SortedSet<T>.Enumerator enumerator = view.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				actualValue = enumerator.Current;
				return true;
			}
		}

		actualValue = default;
		return false;
	}
#endif
	/// <summary>
	/// Tries to add the specified <paramref name="key"/> and <paramref name="value"/> to the <paramref name="dictionary"/>.
	/// </summary>
	/// <param name="dictionary">
	/// A sorted dictionary with keys of type <typeparamref name="TKey"/> and values of type
	/// <typeparamref name="TValue"/>.
	/// </param>
	/// <param name="key">The key of the value to add.</param>
	/// <param name="value">The value to add.</param>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	/// <returns>
	/// <see langword="true"/> when the <paramref name="key"/> and <paramref name="value"/> are successfully added
	/// to the <paramref name="dictionary"/>; <see langword="false"/> when the <paramref name="dictionary"/> already
	/// contains the specified <paramref name="key"/>, in which case nothing gets added.
	/// </returns>
	public static Boolean TryAdd<TKey, TValue>(this SortedList<TKey, TValue> dictionary, TKey key, TValue value)
	{
		if (dictionary.ContainsKey(key)) return false;
		try
		{
			dictionary.Add(key, value);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
#endif