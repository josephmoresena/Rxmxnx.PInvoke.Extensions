namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Asynchronously concatenates the elements of a specified <see cref="CString"/>
    /// array.
    /// </summary>
    /// <param name="values">An array of <see cref="CString"/> instances.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains the concatenated elements of <paramref name="values"/>.
    /// </returns>
    public static Task<CString> ConcatAsync(params CString?[] values) => ConcatAsync(CancellationToken.None, values);

    /// <summary>
    /// Asynchronously concatenates the elements of a specified <see cref="CString"/>
    /// array.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="values">An array of <see cref="CString"/> instances.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains the concatenated elements of <paramref name="values"/>.
    /// </returns>
    public static async Task<CString> ConcatAsync(CancellationToken cancellationToken, params CString?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using CStringConcatenator helper = new(cancellationToken);
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
    public static Task<CString> ConcatAsync(params Byte[]?[] values) => ConcatAsync(CancellationToken.None, values);

    /// <summary>
    /// Asynchronously concatenates the elements of a specified UTF-8 bytes array.
    /// </summary>
    /// <param name="values">An array of UTF-8 bytes instances.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains the concatenated elements of <paramref name="values"/>.
    /// </returns>
    public static async Task<CString> ConcatAsync(CancellationToken cancellationToken, params Byte[]?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using CStringConcatenator helper = new(cancellationToken);
        foreach (CString? value in values)
            await helper.WriteAsync(value);
        return helper.ToCString();
    }
}
