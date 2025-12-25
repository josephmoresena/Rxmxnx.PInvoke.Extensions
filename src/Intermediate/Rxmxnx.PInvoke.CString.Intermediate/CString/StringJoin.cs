#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Concatenates all the elements of a UTF-16 text array, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 character to use as a separator.
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
	public static CString Join(Char separator,
#if !NET9_0_OR_GREATER
		params String?[] value
#else
		String?[] value
#endif
	)
		=> CString.Join(separator, value.AsSpan());
	/// <summary>
	/// Concatenates all the elements of a read-only span UTF-16 text, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <returns>
	/// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
	/// by the <paramref name="separator"/> character. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	public static CString Join(Char separator,
#if NET9_0_OR_GREATER
		params ReadOnlySpan<String?> value
#else
		ReadOnlySpan<String?> value
#endif
	)
		=> CString.Join(CString.CreateSeparator(separator), value);
	/// <summary>
	/// Concatenates all the elements of a UTF-16 text enumeration, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="values"/> has more than one element.
	/// </param>
	/// <param name="values">
	/// A collection that contains the UTF-16 texts to concatenate.
	/// </param>
	/// <returns>
	/// A UTF-8 text that consists of the members of <paramref name="values"/> delimited
	/// by the <paramref name="separator"/> character. -or- <see cref="Empty"/>
	/// if <paramref name="values"/> has no elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static CString Join(Char separator, IEnumerable<String?> values)
		=> CString.Join(CString.CreateSeparator(separator), values);
	/// <summary>
	/// Concatenates an array of UTF-16 texts, using the specified separator between each
	/// member, starting with the element in value located at the
	/// <paramref name="startIndex"/> position, and concatenating up to
	/// <paramref name="count"/> elements.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">An array of UTF-16 texts to concatenate.</param>
	/// <param name="startIndex">
	/// The first element in <paramref name="value"/> to use.
	/// </param>
	/// <param name="count">
	/// The number of element of <paramref name="value"/> to use.
	/// </param>
	/// <returns>
	/// A UTF-8 text that consists of <paramref name="count"/> elements of
	/// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
	/// the <paramref name="separator"/> UTF-16 character. -or-
	/// <see cref="Empty"/> if <paramref name="count"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static CString Join(Char separator, String?[] value, Int32 startIndex, Int32 count)
		=> CString.Join(CString.CreateSeparator(separator), value, startIndex, count);
	/// <summary>
	/// Concatenates all the elements of a UTF-16 text array, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <returns>
	/// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
	/// by the separator UTF-16 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public static CString Join(String? separator,
#if !NET9_0_OR_GREATER
		params String?[] value
#else
		String?[] value
#endif
	)
	{
		ArgumentNullException.ThrowIfNull(value);
		return CString.Join(separator, value.AsSpan());
	}
	/// <summary>
	/// Concatenates all the elements of a read-only span UTF-16 text, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">A read-only span that contains the elements to concatenate.</param>
	/// <returns>
	/// A UTF-8 text that consists of the elements in <paramref name="value"/> delimited
	/// by the separator UTF-16 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	// ReSharper disable once MemberCanBePrivate.Global
	public static CString Join(String? separator,
#if NET9_0_OR_GREATER
		params ReadOnlySpan<String?> value
#else
		ReadOnlySpan<String?> value
#endif
	)
	{
		using StringConcatenator helper = new(separator);
		foreach (String? utf8Text in value)
			helper.Write(utf8Text);
		return helper.ToCString();
	}
	/// <summary>
	/// Concatenates the members of a constructed <see cref="IEnumerable{T}"/>
	/// collection of type <see cref="String"/>, using the specified separator between
	/// each member.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="values"/> has more than one element.
	/// </param>
	/// <param name="values">
	/// A collection that contains the UTF-16 texts to concatenate.
	/// </param>
	/// <returns>
	/// A UTF-8 text that consists of the members of <paramref name="values"/> delimited
	/// by the <paramref name="separator"/> UTF-16 text. -or- <see cref="Empty"/>
	/// if <paramref name="values"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
#if !PACKAGE
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
#endif
	public static CString Join(String? separator, IEnumerable<String?> values)
	{
		ArgumentNullException.ThrowIfNull(values);
		using StringConcatenator helper = new(separator);
		foreach (String? utf8Text in values)
			helper.Write(utf8Text);
		return helper.ToCString();
	}
	/// <summary>
	/// Concatenates the specified elements of a UTF-16 text array, using the specified
	/// separator between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 text to use as a separator.
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
	/// the <paramref name="separator"/> UTF-16 text. -or- <see cref="Empty"/>
	/// if <paramref name="count"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static CString Join(String? separator, String?[] value, Int32 startIndex, Int32 count)
	{
		ArgumentNullException.ThrowIfNull(value);
		ReadOnlySpan<String?> span = value.AsSpan();
		return CString.Join(separator, span[startIndex..(startIndex + count)]);
	}
}