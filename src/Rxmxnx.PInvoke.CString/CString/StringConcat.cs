namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Concatenates the elements of a specified <see cref="String"/> array.
    /// </summary>
    /// <param name="values">An array of <see cref="String"/> instances.</param>
    /// <returns>The concatenated elements of <paramref name="values"/>.</returns>
    public static CString Concat(params String?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
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
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains the concatenated elements of <paramref name="values"/>.
    /// </returns>
    public static Task<CString> ConcatAsync(params String?[] values) => ConcatAsync(CancellationToken.None, values);

    /// <summary>
    /// Asynchronously concatenates the elements of a specified <see cref="String"/>
    /// array.
    /// </summary>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="values">An array of <see cref="String"/> instances.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains the concatenated elements of <paramref name="values"/>.
    /// </returns>
    public static async Task<CString> ConcatAsync(CancellationToken cancellationToken, params String?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using StringConcatenator helper = new(cancellationToken);
        foreach (String? value in values)
            await helper.WriteAsync(value);
        return helper.ToCString();
    }
}
