#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

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
	/// delimited by the separator UTF-8 text. -or- <see cref="Empty"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	public static Task<CString> JoinAsync(Byte separator, params CString?[] value)
		=> CString.JoinAsync(separator, CancellationToken.None, value);
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-8 text array, using the specified separator
	/// between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 character to use as a separator.
	/// The <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The task's value is a UTF-8 text that consists of the elements
	/// in
	/// <paramref name="value"/> delimited by the separator UTF-8 text. If <paramref name="value"/> has zero elements, the
	/// task's value is
	/// <see cref="Empty"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	// ReSharper disable once MemberCanBePrivate.Global
	public static Task<CString> JoinAsync(Byte separator, CancellationToken cancellationToken, params CString?[] value)
		=> CString.JoinAsync(CString.Create(separator), cancellationToken, value);
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-8 text collection, using the
	/// specified separator between each element.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 character to use as a separator.
	/// <paramref name="separator"/> is included in the returned <see cref="CString"/>
	/// only if <paramref name="values"/> has more than one element.
	/// </param>
	/// <param name="values">
	/// The collection of UTF-8 texts to concatenate.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The value of the task is a UTF-8 text that consists of the
	/// elements in <paramref name="values"/> delimited by the <paramref name="separator"/> character. If
	/// <paramref name="values"/> has no elements, the task's value is <see cref="Empty"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static Task<CString> JoinAsync(Byte separator, IEnumerable<CString?> values,
		CancellationToken cancellationToken = default)
		=> CString.JoinAsync(CString.Create(separator), values, cancellationToken);
	/// <summary>
	/// Asynchronously concatenates a specified number of elements from a UTF-8 text array, using the specified separator
	/// between each element, starting from the element at the given index.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 character to use as a separator. <paramref name="separator"/> is included in the returned
	/// <see cref="CString"/>
	/// only if <paramref name="value"/> has more than one element.
	/// </param>
	/// <param name="value">An array of UTF-8 texts to concatenate.</param>
	/// <param name="startIndex">
	/// The index of the first element in <paramref name="value"/> to use in the concatenation.
	/// </param>
	/// <param name="count">
	/// The number of elements from <paramref name="value"/> to include in the concatenation.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The value of the task is a UTF-8 text that consists of
	/// <paramref name="count"/> elements from <paramref name="value"/>, starting at <paramref name="startIndex"/>,
	/// delimited by the <paramref name="separator"/> UTF-8 character. If <paramref name="count"/> is zero, the task's value
	/// is <see cref="Empty"/>.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static Task<CString> JoinAsync(Byte separator, CString?[] value, Int32 startIndex, Int32 count,
		CancellationToken cancellationToken = default)
		=> CString.JoinAsync(CString.Create(separator), value, startIndex, count, cancellationToken);
	/// <summary>
	/// Asynchronously concatenates all the elements of a UTF-8 text array, using the specified separator
	/// between each element. This method is an overload of the JoinAsync method that uses a default cancellation token.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 text to use as a separator. <paramref name="separator"/> is included in the returned <see cref="CString"/>
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
	public static Task<CString> JoinAsync(CString? separator, params CString?[] value)
		=> CString.JoinAsync(separator, CancellationToken.None, value);
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
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests.
	/// The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous join operation. The result of this task
	/// contains a UTF-8 text that consists of the elements in <paramref name="value"/>
	/// delimited by the separator UTF-8 text, or an empty <see cref="CString"/> if
	/// <paramref name="value"/> has zero elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	// ReSharper disable once MemberCanBePrivate.Global
	public static async Task<CString> JoinAsync(CString? separator, CancellationToken cancellationToken,
		params CString?[] value)
	{
		ArgumentNullException.ThrowIfNull(value);
		await using CStringConcatenator helper = new(separator, cancellationToken);
		// ReSharper disable once ForCanBeConvertedToForeach
		for (Int32 index = 0; index < value.Length; index++)
		{
			CString? utf8Text = value[index];
			await helper.WriteAsync(utf8Text);
		}
		return helper.ToCString();
	}
	/// <summary>
	/// Asynchronously concatenates the members of a constructed
	/// <see cref="IEnumerable{T}"/> collection of type <see cref="CString"/>, using the
	/// specified separator between each member.
	/// </summary>
	/// <param name="separator">
	/// The text to use as a separator between the concatenated elements.
	/// </param>
	/// <param name="values">
	/// A collection that contains the elements to concatenate.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous concatenation operation. The result of this task
	/// contains a UTF-8 text that consists of the elements of <paramref name="values"/>
	/// delimited by the <paramref name="separator"/> text, or an empty string if <paramref name="values"/> has no elements.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
	public static async Task<CString> JoinAsync(CString? separator, IEnumerable<CString?> values,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(values);
		await using CStringConcatenator helper = new(separator, cancellationToken);
		foreach (CString? utf8Text in values)
			await helper.WriteAsync(utf8Text);
		return helper.ToCString();
	}
	/// <summary>
	/// Asynchronously concatenates the specified elements of a UTF-8 text array, using
	/// the specified separator between each element.
	/// </summary>
	/// <param name="separator">
	/// The text to use as a separator between the concatenated elements.
	/// </param>
	/// <param name="value">An array that contains the elements to concatenate.</param>
	/// <param name="startIndex">
	/// The index of the first element in <paramref name="value"/> to use.
	/// </param>
	/// <param name="count">
	/// The number of elements from <paramref name="value"/> to use.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous concatenation operation. The result of this task
	/// contains a UTF-8 text that consists of <paramref name="count"/> elements of
	/// <paramref name="value"/> starting at <paramref name="startIndex"/>, delimited by
	/// the <paramref name="separator"/> text, or an empty string if <paramref name="count"/> is zero.
	/// </returns>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if <paramref name="startIndex"/> or <paramref name="count"/> is out of range.
	/// </exception>
	public static async Task<CString> JoinAsync(CString? separator, CString?[] value, Int32 startIndex, Int32 count,
		CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(value);
		await using CStringConcatenator helper = new(separator, cancellationToken);
		Int32 limit = count + startIndex;
		for (Int32 index = startIndex; index < limit; index++)
		{
			CString? utf8Text = value[index];
			await helper.WriteAsync(utf8Text);
		}
		return helper.ToCString();
	}
}