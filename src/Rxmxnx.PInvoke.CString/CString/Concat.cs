namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of two specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="cstr0">The first read-only binary span.</param>
    /// <param name="cstr1">The second read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="cstr0"/> and <paramref name="cstr1"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> cstr0, ReadOnlySpan<Byte> cstr1)
    {
        using BinaryConcatenator helper = new();
        helper.Write(cstr0);
        helper.Write(cstr1);
        return helper.ToArray(true) ?? CString.Empty;
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of three specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="cstr0">The first read-only binary span.</param>
    /// <param name="cstr1">The second read-only binary span.</param>
    /// <param name="cstr2">The third read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="cstr0"/>, <paramref name="cstr1"/> and <paramref name="cstr2"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> cstr0, ReadOnlySpan<Byte> cstr1, ReadOnlySpan<Byte> cstr2)
    {
        using BinaryConcatenator helper = new();
        helper.Write(cstr0);
        helper.Write(cstr1);
        helper.Write(cstr2);
        return helper.ToArray(true) ?? CString.Empty;
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of four specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="cstr0">The first read-only binary span.</param>
    /// <param name="cstr1">The second read-only binary span.</param>
    /// <param name="cstr2">The third read-only binary span.</param>
    /// <param name="cstr3">The fourth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="cstr0"/>, <paramref name="cstr1"/>, <paramref name="cstr2"/>,
    /// and <paramref name="cstr3"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> cstr0, ReadOnlySpan<Byte> cstr1, ReadOnlySpan<Byte> cstr2, ReadOnlySpan<Byte> cstr3)
    {
        using BinaryConcatenator helper = new();
        helper.Write(cstr0);
        helper.Write(cstr1);
        helper.Write(cstr2);
        helper.Write(cstr3);
        return helper.ToArray(true) ?? CString.Empty;
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of five specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="cstr0">The first read-only binary span.</param>
    /// <param name="cstr1">The second read-only binary span.</param>
    /// <param name="cstr2">The third read-only binary span.</param>
    /// <param name="cstr3">The fourth read-only binary span.</param>
    /// <param name="cstr4">The fifth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="cstr0"/>, <paramref name="cstr1"/>, <paramref name="cstr2"/>,
    /// <paramref name="cstr3"/> and <paramref name="cstr4"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> cstr0, ReadOnlySpan<Byte> cstr1, ReadOnlySpan<Byte> cstr2, ReadOnlySpan<Byte> cstr3, ReadOnlySpan<Byte> cstr4)
    {
        using BinaryConcatenator helper = new();
        helper.Write(cstr0);
        helper.Write(cstr1);
        helper.Write(cstr2);
        helper.Write(cstr3);
        helper.Write(cstr4);
        return helper.ToArray(true) ?? CString.Empty;
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of six specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="cstr0">The first read-only binary span.</param>
    /// <param name="cstr1">The second read-only binary span.</param>
    /// <param name="cstr2">The third read-only binary span.</param>
    /// <param name="cstr3">The fourth read-only binary span.</param>
    /// <param name="cstr4">The fifth read-only binary span.</param>
    /// <param name="cstr5">The sixth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="cstr0"/>, <paramref name="cstr1"/>, <paramref name="cstr2"/>,
    /// <paramref name="cstr3"/>, <paramref name="cstr4"/> and <paramref name="cstr5"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> cstr0, ReadOnlySpan<Byte> cstr1, ReadOnlySpan<Byte> cstr2, ReadOnlySpan<Byte> cstr3, ReadOnlySpan<Byte> cstr4, ReadOnlySpan<Byte> cstr5)
    {
        using BinaryConcatenator helper = new();
        helper.Write(cstr0);
        helper.Write(cstr1);
        helper.Write(cstr2);
        helper.Write(cstr3);
        helper.Write(cstr4);
        helper.Write(cstr5);
        return helper.ToArray(true) ?? CString.Empty;
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of seven specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="cstr0">The first read-only binary span.</param>
    /// <param name="cstr1">The second read-only binary span.</param>
    /// <param name="cstr2">The third read-only binary span.</param>
    /// <param name="cstr3">The fourth read-only binary span.</param>
    /// <param name="cstr4">The fifth read-only binary span.</param>
    /// <param name="cstr5">The sixth read-only binary span.</param>
    /// <param name="cstr6">The seventh read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="cstr0"/>, <paramref name="cstr1"/>, <paramref name="cstr2"/>,
    /// <paramref name="cstr3"/>, <paramref name="cstr4"/>, <paramref name="cstr5"/> 
    /// and <paramref name="cstr6"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> cstr0, ReadOnlySpan<Byte> cstr1, ReadOnlySpan<Byte> cstr2, ReadOnlySpan<Byte> cstr3, ReadOnlySpan<Byte> cstr4, ReadOnlySpan<Byte> cstr5, ReadOnlySpan<Byte> cstr6)
    {
        using BinaryConcatenator helper = new();
        helper.Write(cstr0);
        helper.Write(cstr1);
        helper.Write(cstr2);
        helper.Write(cstr3);
        helper.Write(cstr4);
        helper.Write(cstr5);
        helper.Write(cstr6);
        return helper.ToArray(true) ?? CString.Empty;
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of eight specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="cstr0">The first read-only binary span.</param>
    /// <param name="cstr1">The second read-only binary span.</param>
    /// <param name="cstr2">The third read-only binary span.</param>
    /// <param name="cstr3">The fourth read-only binary span.</param>
    /// <param name="cstr4">The fifth read-only binary span.</param>
    /// <param name="cstr5">The sixth read-only binary span.</param>
    /// <param name="cstr6">The seventh read-only binary span.</param>
    /// <param name="cstr7">The eighth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="cstr0"/>, <paramref name="cstr1"/>, <paramref name="cstr2"/>,
    /// <paramref name="cstr3"/>, <paramref name="cstr4"/>, <paramref name="cstr5"/>,
    /// <paramref name="cstr6"/> and <paramref name="cstr7"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> cstr0, ReadOnlySpan<Byte> cstr1, ReadOnlySpan<Byte> cstr2, ReadOnlySpan<Byte> cstr3, ReadOnlySpan<Byte> cstr4, ReadOnlySpan<Byte> cstr5, ReadOnlySpan<Byte> cstr6, ReadOnlySpan<Byte> cstr7)
    {
        using BinaryConcatenator helper = new();
        helper.Write(cstr0);
        helper.Write(cstr1);
        helper.Write(cstr2);
        helper.Write(cstr3);
        helper.Write(cstr4);
        helper.Write(cstr5);
        helper.Write(cstr6);
        helper.Write(cstr7);
        return helper.ToArray(true) ?? CString.Empty;
    }

    /// <summary>
    /// Concatenates the elements of a specified <see cref="CString"/> array.
    /// </summary>
    /// <param name="values">An array of <see cref="CString"/> instances.</param>
    /// <returns>The concatenated elements of <paramref name="values"/>.</returns>
    public static CString Concat(params CString?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using CStringConcatenator helper = new();
        foreach (CString? value in values)
            helper.Write(value);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the elements of a specified UTF-8 bytes array.
    /// </summary>
    /// <param name="values">An array of UTF-8 bytes instances.</param>
    /// <returns>The concatenated elements of <paramref name="values"/>.</returns>
    public static CString Concat(params Byte[]?[] values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using CStringConcatenator helper = new();
        foreach (CString? value in values)
            helper.Write(value);
        return helper.ToCString();
    }
}

