namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Represents an empty UTF-8 byte array. This field is read-only.
	/// </summary>
	private static readonly Byte[] empty = [default,];

	/// <summary>
	/// Delegate for comparing two <see cref="ReadOnlySpan{Byte}"/> instances for equality.
	/// </summary>
	/// <param name="current">
	/// The first <see cref="ReadOnlySpan{Byte}"/> instance to compare.
	/// </param>
	/// <param name="other">
	/// The second <see cref="ReadOnlySpan{Byte}"/> instance to compare with the <paramref name="current"/>.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the <paramref name="current"/> instance equals the
	/// <paramref name="other"/> instance; otherwise, <see langword="false"/>.
	/// </returns>
	private delegate Boolean EqualsDelegate(ReadOnlySpan<Byte> current, ReadOnlySpan<Byte> other);
}