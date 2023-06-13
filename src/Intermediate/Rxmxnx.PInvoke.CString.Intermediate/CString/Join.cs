namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Concatenates UTF-8 text array elements with a specified byte separator.
    /// </summary>
    /// <param name="separator">Byte to use as a separator between concatenated elements.</param>
    /// <param name="value">Array of UTF-8 text elements to concatenate.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if the input array is empty.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    public static CString Join(Byte separator, params CString?[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        using BinaryConcatenator helper = new(separator);
        foreach (CString? utf8Text in value)
            helper.Write(utf8Text);
        return helper.ToCString();
    }
    /// <summary>
    /// Concatenates UTF-8 text elements from a collection with a specified byte separator.
    /// </summary>
    /// <param name="separator">Byte to use as a separator between concatenated elements.</param>
    /// <param name="values">Collection of UTF-8 text elements to concatenate.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if the collection is empty.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
    public static CString Join(Byte separator, IEnumerable<CString?> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using BinaryConcatenator helper = new(separator);
        foreach (CString? utf8Text in values)
            helper.Write(utf8Text);
        return helper.ToCString();
    }
    /// <summary>
    /// Concatenates a specific number of UTF-8 text array elements starting from a given index with a
    /// specified byte separator.
    /// </summary>
    /// <param name="separator">Byte to use as a separator between concatenated elements.</param>
    /// <param name="value">Array of UTF-8 text elements to concatenate.</param>
    /// <param name="startIndex">Index of the first element in the array to be used in concatenation.</param>
    /// <param name="count">Number of elements from the array to use in concatenation.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if count is zero.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
    /// </exception>
    public static CString Join(Byte separator, CString?[] value, Int32 startIndex, Int32 count)
    {
        ArgumentNullException.ThrowIfNull(value);
        using BinaryConcatenator helper = new(separator);
        foreach (CString? utf8Text in value.Skip(startIndex).Take(count))
            helper.Write(utf8Text);
        return helper.ToCString();
    }
    /// <summary>
    /// Concatenates UTF-8 text array elements with a specified text separator.
    /// </summary>
    /// <param name="separator">Text to use as a separator between concatenated elements.</param>
    /// <param name="value">Array of UTF-8 text elements to concatenate.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if the input array is empty.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
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
    /// Concatenates UTF-8 text elements from a collection with a specified text separator.
    /// </summary>
    /// <param name="separator">Text to use as a separator between concatenated elements.</param>
    /// <param name="values">Collection of UTF-8 text elements to concatenate.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if the collection is empty.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
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
    /// Concatenates a specific number of UTF-8 text array elements starting from a given index with a specified text
    /// separator.
    /// </summary>
    /// <param name="separator">Text to use as a separator between concatenated elements.</param>
    /// <param name="value">Array of UTF-8 text elements to concatenate.</param>
    /// <param name="startIndex">Index of the first element in the array to be used in concatenation.</param>
    /// <param name="count">Number of elements from the array to use in concatenation.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if count is zero.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
    /// </exception>
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
    /// Concatenates UTF-8 text array elements with a specified text separator.
    /// </summary>
    /// <param name="separator">Text to use as a separator between concatenated elements.</param>
    /// <param name="value">Array of UTF-8 text elements to concatenate.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if count is zero.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    public static CString Join(CString? separator, params CString?[] value)
    {
        ArgumentNullException.ThrowIfNull(value);
        using CStringConcatenator helper = new(separator);
        foreach (CString? utf8Text in value)
            helper.Write(utf8Text);
        return helper.ToCString();
    }
    /// <summary>
    /// Concatenates UTF-8 text elements from a collection with a specified text separator.
    /// </summary>
    /// <param name="separator">Text to use as a separator between concatenated elements.</param>
    /// <param name="values">Collection of UTF-8 text elements to concatenate.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if the collection is empty.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
    public static CString Join(CString? separator, IEnumerable<CString?> values)
    {
        ArgumentNullException.ThrowIfNull(values);
        using CStringConcatenator helper = new(separator);
        foreach (CString? utf8Text in values)
            helper.Write(utf8Text);
        return helper.ToCString();
    }
    /// <summary>
    /// Concatenates a specific number of UTF-8 text array elements starting from a given index with a specified text
    /// separator.
    /// </summary>
    /// <param name="separator">Text to use as a separator between concatenated elements.</param>
    /// <param name="value">Array of UTF-8 text elements to concatenate.</param>
    /// <param name="startIndex">Index of the first element in the array to be used in concatenation.</param>
    /// <param name="count">Number of elements from the array to use in concatenation.</param>
    /// <returns>
    /// Concatenated UTF-8 text with separators or an empty <see cref="CString"/> if count is zero.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
    /// </exception>
    public static CString Join(CString? separator, CString?[] value, Int32 startIndex, Int32 count)
    {
        ArgumentNullException.ThrowIfNull(value);
        using CStringConcatenator helper = new(separator);
        foreach (CString? utf8Text in value.Skip(startIndex).Take(count))
            helper.Write(utf8Text);
        return helper.ToCString();
    }
}
