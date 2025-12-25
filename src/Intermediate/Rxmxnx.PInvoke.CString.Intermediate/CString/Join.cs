#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

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
	/// by the <paramref name="separator"/> character. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public static CString Join(Byte separator,
#if !NET9_0_OR_GREATER
		params CString?[] value
#else
		CString?[] value
#endif
	)
	{
		ArgumentNullException.ThrowIfNull(value);
		return CString.Join(separator, value.AsSpan());
	}
	/// <summary>
	/// Concatenates all the elements of a read-only span UTF-8 text, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">A read-only that contains the elements to concatenate.</param>
	/// <returns>
	/// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
	/// by the <paramref name="separator"/> character. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	public static CString Join(Byte separator,
#if NET9_0_OR_GREATER
		params ReadOnlySpan<CString?> value
#else
		ReadOnlySpan<CString?> value
#endif
	)
	{
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
#if !PACKAGE
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
#endif
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
		ReadOnlySpan<CString?> span = value.AsSpan();
		return CString.Join(separator, span[startIndex..(startIndex + count)]);
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
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public static CString Join(ReadOnlySpan<Byte> separator,
#if !NET9_0_OR_GREATER
		params CString?[] value
#else
		CString?[] value
#endif
	)
	{
		ArgumentNullException.ThrowIfNull(value);
		return CString.Join(separator, value.AsSpan());
	}
	/// <summary>
	/// Concatenates all the elements of a UTF-8 text read-only span, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">A read-only span that contains the elements to concatenate.</param>
	/// <returns>
	/// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
	/// by the separator UTF-8 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	// ReSharper disable once MemberCanBePrivate.Global
	public static unsafe CString Join(ReadOnlySpan<Byte> separator,
#if NET9_0_OR_GREATER
		params ReadOnlySpan<CString?> value
#else
		ReadOnlySpan<CString?> value
#endif
	)
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(separator))
		{
			CString separatorCstr = new(new IntPtr(ptr), separator.Length);
			return CString.Join(separatorCstr, value);
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
#if !PACKAGE
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
#endif
	public static unsafe CString Join(ReadOnlySpan<Byte> separator, IEnumerable<CString?> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		fixed (void* ptr = &MemoryMarshal.GetReference(separator))
		{
			CString separatorCstr = new(new IntPtr(ptr), separator.Length);
			return CString.Join(separatorCstr, values);
		}
	}
	/// <summary>
	/// Concatenates the specified elements of a UTF-8 text array, using the specified
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
	/// The number of elements from <paramref name="value"/> to use.
	/// </param>
	/// <returns>
	/// A UTF-8 text that consists of <paramref name="count"/> elements of
	/// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
	/// the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/>
	/// if <paramref name="count"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static CString Join(ReadOnlySpan<Byte> separator, CString?[] value, Int32 startIndex, Int32 count)
	{
		ArgumentNullException.ThrowIfNull(value);
		ReadOnlySpan<CString?> span = value.AsSpan();
		return CString.Join(separator, span[startIndex..(startIndex + count)]);
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
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public static CString Join(CString? separator,
#if !NET9_0_OR_GREATER
		params CString?[] value
#else
		CString?[] value
#endif
	)
	{
		ArgumentNullException.ThrowIfNull(value);
		return CString.Join(separator, value.AsSpan());
	}
	/// <summary>
	/// Concatenates all the elements of a UTF-8 text read-only span, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">A read-only span that contains the elements to concatenate.</param>
	/// <returns>
	/// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
	/// by the separator UTF-8 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	// ReSharper disable once MemberCanBePrivate.Global
	public static CString Join(CString? separator,
#if NET9_0_OR_GREATER
		params ReadOnlySpan<CString?> value
#else
		ReadOnlySpan<CString?> value
#endif
	)
	{
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
#if !PACKAGE
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
#endif
	public static CString Join(CString? separator, IEnumerable<CString?> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		using CStringConcatenator helper = new(separator);
		foreach (CString? utf8Text in values)
			helper.Write(utf8Text);
		return helper.ToCString();
	}
	/// <summary>
	/// Concatenates the specified elements of a UTF-8 text array, using the specified
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
	/// The number of elements from <paramref name="value"/> to use.
	/// </param>
	/// <returns>
	/// A UTF-8 text that consists of <paramref name="count"/> elements of
	/// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
	/// the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/>
	/// if <paramref name="count"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static CString Join(CString? separator, CString?[] value, Int32 startIndex, Int32 count)
	{
		ArgumentNullException.ThrowIfNull(value);
		ReadOnlySpan<CString?> span = value.AsSpan();
		return CString.Join(separator, span[startIndex..(startIndex + count)]);
	}
}