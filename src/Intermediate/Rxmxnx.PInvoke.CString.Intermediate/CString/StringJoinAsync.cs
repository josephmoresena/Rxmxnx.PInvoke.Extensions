#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-16 text array, using the
	/// specified separator between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
	/// delimited by the separator UTF-8 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public static Task<CString> JoinAsync(Char separator, params String?[] value)
		=> CString.JoinAsync(separator, CancellationToken.None, value);
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-16 text array, using the
	/// specified separator between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
	/// delimited by the separator UTF-8 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	// ReSharper disable once MemberCanBePrivate.Global
	public static Task<CString> JoinAsync(Char separator, CancellationToken cancellationToken, params String?[] value)
		=> CString.JoinAsync(CString.CreateSeparator(separator), cancellationToken, value);
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-16 text array, using the
	/// specified separator between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="values"/> has more than one element.
	/// </param>
	/// <param name="values">
	/// A collection that contains the UTF-16 texts to concatenate.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of the members of <paramref name="values"/>
	/// delimited by the <paramref name="separator"/> character. -or-
	/// <see cref="Empty"/> if <paramref name="values"/> has no elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static Task<CString> JoinAsync(Char separator, IEnumerable<String?> values,
		CancellationToken cancellationToken = default)
		=> CString.JoinAsync(CString.CreateSeparator(separator), values, cancellationToken);
	/// <summary>
	/// Asynchronously concatenates an array of UTF-16 texts, using the specified separator
	/// between each member, starting with the element in value located at the
	/// <paramref name="startIndex"/> position, and concatenating up to <paramref name="count"/>
	/// elements.
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
	/// The number of elements of <paramref name="value"/> to use.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of <paramref name="count"/> elements of
	/// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
	/// the <paramref name="separator"/> UTF-8 character. -or-
	/// <see cref="Empty"/> if <paramref name="count"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static Task<CString> JoinAsync(Char separator, String?[] value, Int32 startIndex, Int32 count,
		CancellationToken cancellationToken = default)
		=> CString.JoinAsync(CString.CreateSeparator(separator), value, startIndex, count, cancellationToken);
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-16 text array, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
	/// delimited by the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public static Task<CString> JoinAsync(String? separator, params String?[] value)
		=> CString.JoinAsync(separator, CancellationToken.None, value);
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-16 text array, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
	/// delimited by the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	// ReSharper disable once MemberCanBePrivate.Global
	public static async Task<CString> JoinAsync(String? separator, CancellationToken cancellationToken,
		params String?[] value)
	{
		ArgumentNullException.ThrowIfNull(value);
		await using StringConcatenator helper = new(separator, cancellationToken);
		// ReSharper disable once ForCanBeConvertedToForeach
		for (Int32 index = 0; index < value.Length; index++)
		{
			String? utf8Text = value[index];
			await helper.WriteAsync(utf8Text);
		}
		return helper.ToCString();
	}
	/// <summary>
	/// Asynchronously concatenates the members of a constructed
	/// <see cref="IEnumerable{T}"/> collection of type <see cref="String"/>, using the
	/// specified separator between each member.
	/// </summary>
	/// <param name="separator">
	/// The UTF-16 text to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="values"/> has more than one element.
	/// </param>
	/// <param name="values">
	/// A collection that contains the UTF-16 texts to concatenate.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of the elements of <paramref name="values"/>
	/// delimited by the <paramref name="separator"/> UTF-8 text. -or-
	/// <see cref="Empty"/> if <paramref name="values"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static async Task<CString> JoinAsync(String? separator, IEnumerable<String?> values,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(values);
		await using StringConcatenator helper = new(separator, cancellationToken);
		foreach (String? utf8Text in values)
			await helper.WriteAsync(utf8Text);
		return helper.ToCString();
	}
	/// <summary>
	/// Asynchronously concatenates the specified elements of a UTF-16 texts array, using
	/// the specified separator between each element.
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
	/// The number of element of <paramref name="value"/> to use.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of <paramref name="count"/> elements of
	/// <paramref name="value"/> starting at <paramref name="startIndex"/> delimited by
	/// the <paramref name="separator"/> UTF-8 text. -or- <see cref="Empty"/>
	/// if <paramref name="count"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static async Task<CString> JoinAsync(String? separator, String?[] value, Int32 startIndex, Int32 count,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(value);
		await using StringConcatenator helper = new(separator, cancellationToken);
		Int32 limit = count + startIndex;
		for (Int32 index = startIndex; index < limit; index++)
		{
			String? utf16Text = value[index];
			await helper.WriteAsync(utf16Text);
		}
		return helper.ToCString();
	}
}