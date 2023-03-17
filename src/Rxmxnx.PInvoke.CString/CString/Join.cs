namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Concatenates all the elements of a UTF-8 text array, using the specified separator
    /// between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 character to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <returns>
    /// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
    /// by the separator UTF-8 text. -or- <see cref="CString.Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static CString Join(Byte separator, params CString?[] value)
        => Join(new CString(separator, 1), value);

    /// <summary>
    /// Concatenates all the elements of a UTF-8 text array, using the specified separator
    /// between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 character to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="values"/> has more than one element.
    /// </param>
    /// <param name="values">
    /// A collection that contains the UTF-8 texts to concatenate.
    /// </param>
    /// <returns>
    /// A UTF-8 text that consists of the members of <paramref name="values"/> delimited
    /// by the <paramref name="separator"/> character. -or- <see cref="CString.Empty"/>
    /// if <paramref name="values"/> has no elements.
    /// </returns>
    public static CString Join(Byte separator, IEnumerable<CString?> values)
        => Join(new CString(separator, 1), values);

    /// <summary>
    /// Concatenates an array of UTF-8 texts, using the specified separator between each
    /// member,
    /// starting with the element in value located at the startIndex position, and concatenating up to count elements.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 character to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="value">An array of UTF-8 texts to concatenate.</param>
    /// <returns>
    /// A UTF-8 text that consists of <paramref name="count"/> elements of
    /// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
    /// the <paramref name="separator"/> UTF-8 character. -or-
    /// <see cref="CString.Empty"/> if <paramref name="count"/> is zero.
    /// </returns>
    public static CString Join(Byte separator, CString?[] value, Int32 startIndex, Int32 count)
        => Join(new CString(separator, 1), value);

    /// <summary>
    /// Concatenates all the elements of a UTF-8 text array, using the specified separator
    /// between each element.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 text to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="value"/> has more than one element.
    /// </param>
    /// <param name="value">An array that contains the elements to concatenate.</param>
    /// <returns>
    /// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
    /// by the separator UTF-8 text. -or- <see cref="CString.Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static CString Join(CString? separator, params CString?[] value)
    {
        using CStringConcatenatorHelper helper = new(separator);
        foreach (CString? utf8Text in value)
            helper.Write(utf8Text);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the members of a constructed <see cref="IEnumerable{out T}"/>
    /// collection of type <see cref="CString"/>, using the specified separator between
    /// each member.
    /// </summary>
    /// <param name="separator">
    /// The UTF-8 text to use as a separator.
    /// <paramref name="separator"/> is included in the returned <see cref="CString"/>
    /// only if <paramref name="values"/> has more than one element.
    /// </param>
    /// <param name="values">
    /// A collection that contains the UTF-8 texts to concatenate.
    /// </param>
    /// <returns>
    /// A UTF-8 text that consists of the elements of <paramref name="values"/> delimited
    /// by the <paramref name="separator"/> UTF-8 text. -or- <see cref="CString.Empty"/>
    /// if <paramref name="values"/> has zero elements.
    /// </returns>
    public static CString Join(CString? separator, IEnumerable<CString?> values)
    {
        using CStringConcatenatorHelper helper = new(separator);
        foreach (CString? utf8Text in values)
            helper.Write(utf8Text);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the specified elements of a UTF-8 texts array, using the specified
    /// separator between each element.
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
    /// <returns>
    /// A UTF-8 text that consists of <paramref name="count"/> elements of
    /// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
    /// the <paramref name="separator"/> UTF-8 text. -or- <see cref="CString.Empty"/>
    /// if <paramref name="count"/> is zero.
    /// </returns>
    public static CString Join(CString? separator, CString?[] value, Int32 startIndex, Int32 count)
    {
        using CStringConcatenatorHelper helper = new(separator);
        foreach (CString? utf8Text in value[startIndex..count])
            helper.Write(utf8Text);
        return helper.ToCString();
    }
}

