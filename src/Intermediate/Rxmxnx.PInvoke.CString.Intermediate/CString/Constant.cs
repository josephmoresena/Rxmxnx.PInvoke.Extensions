namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Threshold for stackalloc usage in bytes.
	/// </summary>
	private const Int32 stackallocByteThreshold = 256;

	/// <summary>
	/// Represents an empty UTF-8 byte array. This field is read-only.
	/// </summary>
	private static readonly Byte[] empty = [default,];
	/// <summary>
	/// An instance of <see cref="EqualsDelegate"/> for native comparison.
	/// Optimized for either 32 or 64-bit processes.
	/// </summary>
	/// <remarks>
	/// The purpose of initializing the <c>equals</c> delegate is to allow an optimized comparison
	/// between UTF-8 strings in both 32 and 64-bit architectures by comparing 4 or 8 bytes
	/// respectively.
	/// </remarks>
	private static readonly EqualsDelegate equals = CString.GetEquals();

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