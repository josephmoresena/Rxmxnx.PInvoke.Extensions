namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of two specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="span0">The first read-only binary span.</param>
    /// <param name="span1">The second read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="span0"/> and <paramref name="span1"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1)
    {
        using BinaryConcatenator helper = new();
        helper.Write(span0);
        helper.Write(span1);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of three specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="span0">The first read-only binary span.</param>
    /// <param name="span1">The second read-only binary span.</param>
    /// <param name="span2">The third read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="span0"/>, <paramref name="span1"/> and <paramref name="span2"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2)
    {
        using BinaryConcatenator helper = new();
        helper.Write(span0);
        helper.Write(span1);
        helper.Write(span2);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of four specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="span0">The first read-only binary span.</param>
    /// <param name="span1">The second read-only binary span.</param>
    /// <param name="span2">The third read-only binary span.</param>
    /// <param name="span3">The fourth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="span0"/>, <paramref name="span1"/>, <paramref name="span2"/>,
    /// and <paramref name="span3"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2, ReadOnlySpan<Byte> span3)
    {
        using BinaryConcatenator helper = new();
        helper.Write(span0);
        helper.Write(span1);
        helper.Write(span2);
        helper.Write(span3);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of five specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="span0">The first read-only binary span.</param>
    /// <param name="span1">The second read-only binary span.</param>
    /// <param name="span2">The third read-only binary span.</param>
    /// <param name="span3">The fourth read-only binary span.</param>
    /// <param name="span4">The fifth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="span0"/>, <paramref name="span1"/>, <paramref name="span2"/>,
    /// <paramref name="span3"/> and <paramref name="span4"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2, ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4)
    {
        using BinaryConcatenator helper = new();
        helper.Write(span0);
        helper.Write(span1);
        helper.Write(span2);
        helper.Write(span3);
        helper.Write(span4);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of six specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="span0">The first read-only binary span.</param>
    /// <param name="span1">The second read-only binary span.</param>
    /// <param name="span2">The third read-only binary span.</param>
    /// <param name="span3">The fourth read-only binary span.</param>
    /// <param name="span4">The fifth read-only binary span.</param>
    /// <param name="span5">The sixth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="span0"/>, <paramref name="span1"/>, <paramref name="span2"/>,
    /// <paramref name="span3"/>, <paramref name="span4"/> and <paramref name="span5"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2, ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4, ReadOnlySpan<Byte> span5)
    {
        using BinaryConcatenator helper = new();
        helper.Write(span0);
        helper.Write(span1);
        helper.Write(span2);
        helper.Write(span3);
        helper.Write(span4);
        helper.Write(span5);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of seven specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="span0">The first read-only binary span.</param>
    /// <param name="span1">The second read-only binary span.</param>
    /// <param name="span2">The third read-only binary span.</param>
    /// <param name="span3">The fourth read-only binary span.</param>
    /// <param name="span4">The fifth read-only binary span.</param>
    /// <param name="span5">The sixth read-only binary span.</param>
    /// <param name="span6">The seventh read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="span0"/>, <paramref name="span1"/>, <paramref name="span2"/>,
    /// <paramref name="span3"/>, <paramref name="span4"/>, <paramref name="span5"/> 
    /// and <paramref name="span6"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2, ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4, ReadOnlySpan<Byte> span5, ReadOnlySpan<Byte> span6)
    {
        using BinaryConcatenator helper = new();
        helper.Write(span0);
        helper.Write(span1);
        helper.Write(span2);
        helper.Write(span3);
        helper.Write(span4);
        helper.Write(span5);
        helper.Write(span6);
        return helper.ToCString();
    }

    /// <summary>
    /// Concatenates the <see cref="CString"/> representations of eight specified
    /// read-only binary spans.
    /// </summary>
    /// <param name="span0">The first read-only binary span.</param>
    /// <param name="span1">The second read-only binary span.</param>
    /// <param name="span2">The third read-only binary span.</param>
    /// <param name="span3">The fourth read-only binary span.</param>
    /// <param name="span4">The fifth read-only binary span.</param>
    /// <param name="span5">The sixth read-only binary span.</param>
    /// <param name="span6">The seventh read-only binary span.</param>
    /// <param name="span7">The eighth read-only binary span.</param>
    /// <returns>
    /// The concatenated <see cref="CString"/> representations of the values of
    /// <paramref name="span0"/>, <paramref name="span1"/>, <paramref name="span2"/>,
    /// <paramref name="span3"/>, <paramref name="span4"/>, <paramref name="span5"/>,
    /// <paramref name="span6"/> and <paramref name="span7"/>.
    /// </returns>
    public static CString Concat(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2, ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4, ReadOnlySpan<Byte> span5, ReadOnlySpan<Byte> span6, ReadOnlySpan<Byte> span7)
    {
        using BinaryConcatenator helper = new();
        helper.Write(span0);
        helper.Write(span1);
        helper.Write(span2);
        helper.Write(span3);
        helper.Write(span4);
        helper.Write(span5);
        helper.Write(span6);
        helper.Write(span7);
        return helper.ToCString();
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
        using BinaryConcatenator helper = new();
        foreach (Byte[]? value in values)
            helper.Write(value);
        return helper.ToCString();
    }
}

