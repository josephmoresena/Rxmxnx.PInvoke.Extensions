namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Asynchronously concatenates all the elements of a UTF-8 text array, using the
    /// specified separator between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 character to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
    /// delimited by the separator UTF-8 text. -or- <see cref="CString.Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static Task<CString> JoinAsync(Byte separator, params CString?[] value)
        => JoinAsync(separator, CancellationToken.None, value);

    /// <summary>
    /// Asynchronously concatenates all the elements of a UTF-8 text array, using the
    /// specified separator between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 character to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
    /// delimited by the separator UTF-8 text. -or- <see cref="CString.Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static Task<CString> JoinAsync(Byte separator, CancellationToken cancellationToken, params CString?[] value)
        => JoinAsync(new Byte[] { separator }, cancellationToken, value);

    /// <summary>
    /// Asynchronously concatenates all the elements of a UTF-8 text array, using the
    /// specified separator between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 character to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="values"/> has more than one element.
    /// </param>
    /// <param name="values">
    /// A collection that contains the UTF-8 texts to concatenate.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of the members of <paramref name="values"/>
    /// delimited by the <paramref name="separator"/> character. -or-
    /// <see cref="CString.Empty"/> if <paramref name="values"/> has no elements.
    /// </returns>
    public static Task<CString> JoinAsync(Byte separator, IEnumerable<CString?> values, CancellationToken cancellationToken = default)
        => JoinAsync(new Byte[] { separator }, values, cancellationToken);

    /// <summary>
    /// Asynchronously concatenates an array of UTF-8 texts, using the specified separator
    /// between each member, starting with the element in value located at the
    /// <paramref name="startIndex"/> position, and concatenating up to <paramref name="count"/>
    /// elements.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 character to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="value">An array of UTF-8 texts to concatenate.</param>
    /// <param name="startIndex">
    /// The first element in <paramref name="value"/> to use.
    /// </param>
    /// <param name="count">
    /// The number of element of <paramref name="value"/> to use.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of <paramref name="count"/> elements of
    /// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
    /// the <paramref name="separator"/> UTF-8 character. -or-
    /// <see cref="CString.Empty"/> if <paramref name="count"/> is zero.
    /// </returns>
    public static Task<CString> JoinAsync(Byte separator, CString?[] value, Int32 startIndex, Int32 count, CancellationToken cancellationToken = default)
        => JoinAsync(new Byte[] { separator }, value, startIndex, count, cancellationToken);

    /// <summary>
    /// Asynchronously concatenates all the elements of a UTF-8 text array, using the specified separator
    /// between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 text to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
    /// delimited by the separator UTF-8 text. -or- <see cref="CString.Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static Task<CString> JoinAsync(CString? separator, params CString?[] value)
        => JoinAsync(separator, CancellationToken.None, value);

    /// <summary>
    /// Asynchronously concatenates all the elements of a UTF-8 text array, using the specified separator
    /// between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 text to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
    /// delimited by the separator UTF-8 text. -or- <see cref="CString.Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static async Task<CString> JoinAsync(CString? separator, CancellationToken cancellationToken, params CString?[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        using CStringConcatenator helper = new(separator, cancellationToken);
        foreach (CString? utf8Text in value)
            await helper.WriteAsync(utf8Text);
        return helper.ToCString();
    }

    /// <summary>
    /// Asynchronously concatenates the members of a constructed
    /// <see cref="IEnumerable{T}"/> collection of type <see cref="CString"/>, using the
    /// specified separator between each member.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 text to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="values"/> has more than one element.
    /// </param>
    /// <param name="values">
    /// A collection that contains the UTF-8 texts to concatenate.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of the elements of <paramref name="values"/>
    /// delimited by the <paramref name="separator"/> UTF-8 text. -or-
    /// <see cref="CString.Empty"/> if <paramref name="values"/> has zero elements.
    /// </returns>
    public static async Task<CString> JoinAsync(CString? separator, IEnumerable<CString?> values, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(values);
        using CStringConcatenator helper = new(separator, cancellationToken);
        foreach (CString? utf8Text in values)
            await helper.WriteAsync(utf8Text);
        return helper.ToCString();
    }

    /// <summary>
    /// Asynchronously concatenates the specified elements of a UTF-8 texts array, using
    /// the specified separator between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 text to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <param name="startIndex">
    /// The first element in <paramref name="value"/> to use.
    /// </param>
    /// <param name="count">
    /// The number of element of <paramref name="value"/> to use.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous join operation. The result of this task
    /// contains a UTF-8 text that consists of <paramref name="count"/> elements of
    /// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
    /// the <paramref name="separator"/> UTF-8 text. -or- <see cref="CString.Empty"/>
    /// if <paramref name="count"/> is zero.
    /// </returns>
    public static async Task<CString> JoinAsync(CString? separator, CString?[] value, Int32 startIndex, Int32 count, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(value);
        using CStringConcatenator helper = new(separator, cancellationToken);
        foreach (CString? utf8Text in value.Skip(startIndex).Take(count))
            await helper.WriteAsync(utf8Text);
        return helper.ToCString();
    }
}

