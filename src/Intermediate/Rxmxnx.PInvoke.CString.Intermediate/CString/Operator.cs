namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="Byte"/> array to <see cref="CString"/>.
	/// </summary>
	/// <param name="bytes">A <see cref="Byte"/> array to implicitly convert.</param>
	[return: NotNullIfNotNull(nameof(bytes))]
	public static implicit operator CString?(Byte[]? bytes) => bytes is not null ? new(bytes) : default;
	/// <summary>
	/// Defines an explicit conversion of a given <see cref="String"/> to <see cref="CString"/>.
	/// </summary>
	/// <param name="str">A <see cref="String"/> to implicitly convert.</param>
	[return: NotNullIfNotNull(nameof(str))]
	public static explicit operator CString?(String? str)
		=> str switch
		{
			null => default,
			"" => CString.Empty,
			_ => new(str),
		};
	/// <summary>
	/// Defines an implicit conversion of a given <see cref="CString"/> to a read-only span of bytes.
	/// </summary>
	/// <param name="value">A <see cref="CString"/> to implicitly convert.</param>
	public static implicit operator ReadOnlySpan<Byte>(CString? value) => value is not null ? value.AsSpan() : default;

	/// <summary>
	/// Determines whether two specified <see cref="CString"/> instances have the same value.
	/// </summary>
	/// <param name="left">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(CString? left, CString? right) => left?.Equals(right) ?? right is null;
	/// <summary>
	/// Determines whether two specified <see cref="CString"/> instances have different values.
	/// </summary>
	/// <param name="left">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator !=(CString? left, CString? right) => !(left == right);

	/// <summary>
	/// Determines whether a specified <see cref="String"/> and a <see cref="CString"/> instance
	/// have the same value.
	/// </summary>
	/// <param name="left">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(String? left, CString? right) => right?.Equals(left) ?? left is null;
	/// <summary>
	/// Determines whether a specified <see cref="String"/> and a <see cref="CString"/> instance
	/// have different values.
	/// </summary>
	/// <param name="left">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator !=(String? left, CString? right) => !(left == right);

	/// <summary>
	/// Determines whether a specified <see cref="CString"/> instance and a <see cref="String"/>
	/// have the same value.
	/// </summary>
	/// <param name="left">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator ==(CString? left, String? right) => left?.Equals(right) ?? right is null;
	/// <summary>
	/// Determines whether a specified <see cref="CString"/> instance and a <see cref="String"/>
	/// have different values.
	/// </summary>
	/// <param name="left">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator !=(CString? left, String? right) => !(left == right);

	/// <summary>
	/// Determines whether <paramref name="left"/> is greater than <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator >(CString? left, CString? right) => CString.Compare(left, right) > 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator >=(CString? left, CString? right) => CString.Compare(left, right) >= 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is less than or equal to <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator <=(CString? left, CString? right) => CString.Compare(left, right) <= 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is less than <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is less than the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator <(CString? left, CString? right) => CString.Compare(left, right) < 0;

	/// <summary>
	/// Determines whether <paramref name="left"/> is greater than <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator >(CString? left, String? right) => CString.Compare(left, right) > 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator >=(CString? left, String? right) => CString.Compare(left, right) >= 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is less than or equal to <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator <=(CString? left, String? right) => CString.Compare(left, right) <= 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is less than <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is less than the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator <(CString? left, String? right) => CString.Compare(left, right) < 0;

	/// <summary>
	/// Determines whether <paramref name="left"/> is greater than <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator >(String? left, CString? right) => CString.Compare(right, left) < 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator >=(String? left, CString? right) => CString.Compare(right, left) <= 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is less than or equal to <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator <=(String? left, CString? right) => CString.Compare(right, left) >= 0;
	/// <summary>
	/// Determines whether <paramref name="left"/> is less than <paramref name="right"/>.
	/// </summary>
	/// <param name="left">The <see cref="String"/> to compare, or <see langword="null"/>.</param>
	/// <param name="right">The <see cref="CString"/> to compare, or <see langword="null"/>.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="left"/> is less than the value
	/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean operator <(String? left, CString? right) => CString.Compare(right, left) > 0;
}