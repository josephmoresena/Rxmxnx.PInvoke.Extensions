namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Asynchronously concatenates the elements of a specified <see cref="CString"/> array.
	/// </summary>
	/// <param name="values">An array of <see cref="CString"/> instances.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains the concatenated elements of <paramref name="values"/>.
	/// </returns>
	/// <remarks>
	/// This asynchronous method takes an array of <see cref="CString"/> instances represented by <paramref name="values"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored during concatenation.
	/// </remarks>
	public static Task<CString> ConcatAsync(params CString?[] values)
		=> CString.ConcatAsync(CancellationToken.None, values);
	/// <summary>
	/// Asynchronously concatenates the elements of a specified <see cref="CString"/> array.
	/// </summary>
	/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
	/// <param name="values">An array of <see cref="CString"/> instances.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains the concatenated elements of <paramref name="values"/>.
	/// </returns>
	/// <remarks>
	/// This asynchronous method takes an array of <see cref="CString"/> instances represented by
	/// <paramref name="values"/> and a <see cref="CancellationToken"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored during concatenation.
	/// The operation can be cancelled at any time by triggering the <paramref name="cancellationToken"/>.
	/// </remarks>
	public static async Task<CString> ConcatAsync(CancellationToken cancellationToken, params CString?[] values)
	{
		ArgumentNullException.ThrowIfNull(values);
		await using CStringConcatenator helper = new(cancellationToken);
		foreach (CString? value in values)
			await helper.WriteAsync(value);
		return helper.ToCString();
	}
	/// <summary>
	/// Asynchronously concatenates the elements of a specified UTF-8 bytes array.
	/// </summary>
	/// <param name="values">An array of UTF-8 bytes instances.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains the concatenated elements of <paramref name="values"/>.
	/// </returns>
	/// <remarks>
	/// This asynchronous method takes an array of UTF-8 byte arrays represented by <paramref name="values"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored during concatenation.
	/// </remarks>
	public static Task<CString> ConcatAsync(params Byte[]?[] values)
		=> CString.ConcatAsync(CancellationToken.None, values);
	/// <summary>
	/// Asynchronously concatenates the elements of a specified UTF-8 bytes array.
	/// </summary>
	/// <param name="values">An array of UTF-8 bytes instances.</param>
	/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains the concatenated elements of <paramref name="values"/>.
	/// </returns>
	/// <remarks>
	/// This asynchronous method takes an array of UTF-8 byte arrays represented by <paramref name="values"/> and a
	/// <see cref="CancellationToken"/>,
	/// and concatenates each element into a single <see cref="CString"/> instance.
	/// If any element within <paramref name="values"/> is <see langword="null"/>, it is ignored during concatenation.
	/// The operation can be cancelled at any time by triggering the <paramref name="cancellationToken"/>.
	/// </remarks>
	public static async Task<CString> ConcatAsync(CancellationToken cancellationToken, params Byte[]?[] values)
	{
		ArgumentNullException.ThrowIfNull(values);
		await using CStringConcatenator helper = new(cancellationToken);
		foreach (Byte[]? value in values)
			await helper.WriteAsync(CString.Create(value));
		return helper.ToCString();
	}
}