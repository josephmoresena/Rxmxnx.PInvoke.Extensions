namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Concatenates a <cref name="String"/> instance with a <cref name="CString"/> instance and returns a new
	/// <see cref="CString"/>.
	/// </summary>
	/// <param name="left">The left-hand <cref name="String"/> instance.</param>
	/// <param name="right">The right-hand <cref name="CString"/> instance.</param>
	/// <returns>
	/// A new <see cref="CString"/> instance containing the concatenation of  <paramref name="left"/> and
	/// <paramref name="right"/> encoded as UTF-8.
	/// </returns>
	public static CString operator +(String? left, CString? right)
	{
		if (!String.IsNullOrEmpty(left) && CString.IsNullOrEmpty(right))
			return new(left);
		ReadOnlySpan<Char> leftSpan = left;
		return leftSpan + right;
	}
	/// <summary>
	/// Concatenates two <cref name="CString"/> instance instances and returns a new <see cref="CString"/>.
	/// </summary>
	/// <param name="left">The first <cref name="CString"/> instance.</param>
	/// <param name="right">The second <cref name="CString"/> instance.</param>
	/// <returns>
	/// A new <see cref="CString"/> instance containing the concatenation of <paramref name="left"/> and
	/// <paramref name="right"/>.
	/// </returns>
	public static CString operator +(CString? left, CString? right)
	{
		if (!CString.IsNullOrEmpty(left) && CString.IsNullOrEmpty(right))
			return new(left);
		ReadOnlySpan<Byte> leftSpan = left;
		return leftSpan + right;
	}
	/// <summary>
	/// Concatenates a <cref name="CString"/> instance with a <cref name="String"/> instance and returns a new
	/// <see cref="CString"/>.
	/// </summary>
	/// <param name="left">The left-hand <cref name="CString"/> instance.</param>
	/// <param name="right">The right-hand <cref name="String"/> instance.</param>
	/// <returns>
	/// A new <see cref="CString"/> instance containing the concatenation of <paramref name="left"/> and
	/// <paramref name="right"/> encoded as UTF-8.
	/// </returns>
	public static CString operator +(CString? left, String? right)
	{
		if (CString.IsNullOrEmpty(left) && !String.IsNullOrEmpty(right))
			return new(right);
		ReadOnlySpan<Char> rightSpan = right;
		return left + rightSpan;
	}
	/// <summary>
	/// Concatenates a UTF-16 character span with a <cref name="CString"/> instance and returns a new <see cref="CString"/>.
	/// </summary>
	/// <param name="leftSpan">The left-hand UTF-16 character span.</param>
	/// <param name="right">The right-hand <cref name="CString"/> instance.</param>
	/// <returns>
	/// A new <see cref="CString"/> instance containing the concatenation of <paramref name="leftSpan"/> and
	/// <paramref name="right"/> encoded as UTF-8.
	/// </returns>
	public static CString operator +(ReadOnlySpan<Char> leftSpan, CString? right)
	{
		if (leftSpan.IsEmpty)
			return CString.IsNullOrEmpty(right) ? CString.Empty : right;
		if (CString.IsNullOrEmpty(right))
			return new(leftSpan);

		Int32 leftUtf8Length = Encoding.UTF8.GetByteCount(leftSpan);
		Byte[] result = new Byte[leftUtf8Length + right.Length + 1];
		Encoding.UTF8.GetBytes(leftSpan, result.AsSpan()[..leftUtf8Length]);
		right.AsSpan().CopyTo(result.AsSpan()[..leftUtf8Length]);
		result[^1] = default;
		return new(result, true);
	}
	/// <summary>
	/// Concatenates a UTF-8 byte span with a <see cref="CString"/> and returns a new <see cref="CString"/>.
	/// </summary>
	/// <param name="leftSpan">The left-hand UTF-8 byte span.</param>
	/// <param name="right">The right-hand <cref name="CString"/> instance.</param>
	/// <returns>
	/// A new <see cref="CString"/> instance containing the concatenation of <paramref name="leftSpan"/> and
	/// <paramref name="right"/>.
	/// </returns>
	public static CString operator +(ReadOnlySpan<Byte> leftSpan, CString? right)
	{
		if (leftSpan.IsEmpty)
			return CString.IsNullOrEmpty(right) ? CString.Empty : right;
		if (CString.IsNullOrEmpty(right))
			return new(leftSpan);

		Byte[] result = new Byte[leftSpan.Length + right.Length + 1];
		leftSpan.CopyTo(result.AsSpan()[..leftSpan.Length]);
		right.AsSpan().CopyTo(result.AsSpan()[..leftSpan.Length]);
		result[^1] = default;
		return new(result, true);
	}
	/// <summary>
	/// Concatenates a <cref name="CString"/> instance with a UTF-16 character span and returns a new
	/// <see cref="CString"/>.
	/// </summary>
	/// <param name="left">The left-hand <cref name="CString"/> instance.</param>
	/// <param name="rightSpan">The right-hand UTF-16 character span.</param>
	/// <returns>
	/// A new <see cref="CString"/> instance containing the concatenation of <paramref name="left"/> and
	/// <paramref name="rightSpan"/> encoded as UTF-8.
	/// </returns>
	public static CString operator +(CString? left, ReadOnlySpan<Char> rightSpan)
	{
		if (rightSpan.IsEmpty)
			return CString.IsNullOrEmpty(left) ? CString.Empty : left;
		if (CString.IsNullOrEmpty(left))
			return new(rightSpan);

		Int32 rightUtf8Length = Encoding.UTF8.GetByteCount(rightSpan);
		Byte[] result = new Byte[left.Length + rightUtf8Length + 1];
		left.AsSpan().CopyTo(result.AsSpan()[..left.Length]);
		Encoding.UTF8.GetBytes(rightSpan, result.AsSpan()[left.Length..]);
		result[^1] = default;
		return new(result, true);
	}
	/// <summary>
	/// Concatenates a <cref name="CString"/> instance with a UTF-8 byte span and returns a new <see cref="CString"/>.
	/// </summary>
	/// <param name="left">The left-hand <cref name="CString"/> instance.</param>
	/// <param name="rightSpan">The right-hand UTF-8 byte span.</param>
	/// <returns>
	/// A new <see cref="CString"/> instance containing the concatenation of <paramref name="left"/> and
	/// <paramref name="rightSpan"/>.
	/// </returns>
	public static CString operator +(CString? left, ReadOnlySpan<Byte> rightSpan)
	{
		if (rightSpan.IsEmpty)
			return CString.IsNullOrEmpty(left) ? CString.Empty : left;
		if (CString.IsNullOrEmpty(left))
			return new(rightSpan);

		Byte[] result = new Byte[left.Length + rightSpan.Length + 1];
		left.AsSpan().CopyTo(result.AsSpan()[..left.Length]);
		rightSpan.CopyTo(result.AsSpan()[left.Length..]);
		result[^1] = default;
		return new(result, true);
	}
}