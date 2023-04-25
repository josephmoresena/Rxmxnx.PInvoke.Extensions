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
    /// by the separator UTF-8 text. -or- <see cref="Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static CString Join(Byte separator, params CString?[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        using BinaryConcatenator helper = new(separator);
        foreach (CString? utf8Text in value)
            helper.Write(utf8Text);
        return helper.ToArray(true) ?? Empty;
    }

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
    /// by the <paramref name="separator"/> character. -or- <see cref="Empty"/>
    /// if <paramref name="values"/> has no elements.
    /// </returns>
    public static CString Join(Byte separator, IEnumerable<CString?> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using BinaryConcatenator helper = new(separator);
        foreach (CString? utf8Text in values)
            helper.Write(utf8Text);
        return helper.ToArray(true) ?? Empty;
    }

    /// <summary>
    /// Concatenates an array of UTF-8 texts, using the specified separator between each
    /// member, starting with the element in value located at the
    /// <paramref name="startIndex"/> position, and concatenating up to
    /// <paramref name="count"/> elements.
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
    /// <returns>
    /// A UTF-8 text that consists of <paramref name="count"/> elements of
    /// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
    /// the <paramref name="separator"/> UTF-8 character. -or-
    /// <see cref="Empty"/> if <paramref name="count"/> is zero.
    /// </returns>
    public static CString Join(Byte separator, CString?[] value, Int32 startIndex, Int32 count)
    {
        ArgumentNullException.ThrowIfNull(value);
        using BinaryConcatenator helper = new(separator);
        foreach (CString? utf8Text in value.Skip(startIndex).Take(count))
            helper.Write(utf8Text);
        return helper.ToArray(true) ?? Empty;
    }

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
    /// by the separator UTF-8 text. -or- <see cref="Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static unsafe CString Join(ReadOnlySpan<Byte> separator, params CString?[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        fixed (void* ptr = &MemoryMarshal.GetReference(separator))
        {
            CString separatorCstr = new(new IntPtr(ptr), separator.Length);
            return Join(separatorCstr, value);
        }
    }

    /// <summary>
    /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/>
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
    /// by the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/>
    /// if <paramref name="values"/> has zero elements.
    /// </returns>
    public static unsafe CString Join(ReadOnlySpan<Byte> separator, IEnumerable<CString?> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        fixed (void* ptr = &MemoryMarshal.GetReference(separator))
        {
            CString separatorCstr = new(new IntPtr(ptr), separator.Length);
            return Join(separatorCstr, values);
        }
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
    /// the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/>
    /// if <paramref name="count"/> is zero.
    /// </returns>
    public static unsafe CString Join(ReadOnlySpan<Byte> separator, CString?[] value, Int32 startIndex, Int32 count)
    {
        ArgumentNullException.ThrowIfNull(value);
        fixed (void* ptr = &MemoryMarshal.GetReference(separator))
        {
            CString separatorCstr = new(new IntPtr(ptr), separator.Length);
            return Join(separatorCstr, value, startIndex, count);
        }
    }

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
    /// by the separator UTF-8 text. -or- <see cref="Empty"/> if
    /// <paramref name="value"/> has zero elements.
    /// </returns>
    public static CString Join(CString? separator, params CString?[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        using CStringConcatenator helper = new(separator);
        foreach (CString? utf8Text in value)
            helper.Write(utf8Text);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/>
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
    /// by the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/>
    /// if <paramref name="values"/> has zero elements.
    /// </returns>
    public static CString Join(CString? separator, IEnumerable<CString?> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using CStringConcatenator helper = new(separator);
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
    /// the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/>
    /// if <paramref name="count"/> is zero.
    /// </returns>
    public static CString Join(CString? separator, CString?[] value, Int32 startIndex, Int32 count)
    {
        ArgumentNullException.ThrowIfNull(value);
        using CStringConcatenator helper = new(separator);
        foreach (CString? utf8Text in value.Skip(startIndex).Take(count))
            helper.Write(utf8Text);
        return helper.ToCString();
    }
}
