namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Concatenates the elements of a specified <see cref="String"/> array.
	/// </summary>
	/// <param name="values">An array of <see cref="String"/> instances.</param>
	/// <returns>The concatenated elements of <paramref name="values"/>, as a UTF-8 text.</returns>
	/// <remarks>
	/// This method takes an array of <see cref="String"/> instances represented by <paramref name="values"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored
	/// during concatenation.
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static CString Concat(
#if !NET9_0_OR_GREATER
		params
#endif
			String?[] values)
	{
		ArgumentNullException.ThrowIfNull(values);
		return CString.Concat(values.AsSpan());
	}
	/// <summary>
	/// Concatenates the elements of a specified read-only <see cref="String"/>.
	/// </summary>
	/// <param name="values">An array of <see cref="String"/> instances.</param>
	/// <returns>The concatenated elements of <paramref name="values"/>, as a UTF-8 text.</returns>
	/// <remarks>
	/// This method takes an array of <see cref="String"/> instances represented by <paramref name="values"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored
	/// during concatenation.
	/// </remarks>
	public static CString Concat(
#if NET9_0_OR_GREATER
		params
#endif
		ReadOnlySpan<String?> values)
	{
		using StringConcatenator helper = new();
		foreach (String? value in values)
			helper.Write(value);
		return helper.ToCString();
	}
	/// <summary>
	/// Asynchronously concatenates the elements of a specified <see cref="String"/>
	/// array.
	/// </summary>
	/// <param name="values">An array of <see cref="String"/> instances.</param>
	/// <returns>
	/// A task that represents the asynchronous concatenation operation. The result of this task
	/// contains the concatenated elements of <paramref name="values"/>, as a UTF-8 text.
	/// </returns>
	/// <remarks>
	/// This asynchronous method takes an array of <see cref="String"/> instances represented by <paramref name="values"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored during concatenation.
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static Task<CString> ConcatAsync(params String?[] values)
		=> CString.ConcatAsync(CancellationToken.None, values);
	/// <summary>
	/// Asynchronously concatenates the elements of a specified <see cref="String"/>
	/// array.
	/// </summary>
	/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
	/// <param name="values">An array of <see cref="String"/> instances.</param>
	/// <returns>
	/// A task that represents the asynchronous concatenation operation. The result of this task
	/// contains the concatenated elements of <paramref name="values"/>, as a UTF-8 text.
	/// </returns>
	/// <remarks>
	/// This asynchronous method takes an array of <see cref="String"/> instances represented by
	/// <paramref name="values"/> and a <see cref="CancellationToken"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored during concatenation.
	/// The operation can be cancelled at any time by triggering the <paramref name="cancellationToken"/>.
	/// </remarks>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static async Task<CString> ConcatAsync(CancellationToken cancellationToken, params String?[] values)
	{
		ArgumentNullException.ThrowIfNull(values);
		await using StringConcatenator helper = new(cancellationToken);
		for (Int32 index = 0; index < values.Length; index++)
		{
			String? value = values[index];
			await helper.WriteAsync(value);
		}
		return helper.ToCString();
	}
}