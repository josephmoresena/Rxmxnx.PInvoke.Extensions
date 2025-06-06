namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe partial struct ReadOnlyValPtr<T> : IComparable, IComparable<ReadOnlyValPtr<T>>
{
	/// <inheritdoc/>
	public Int32 CompareTo(Object? obj)
		=> ValidationUtilities.ThrowIfInvalidValuePointer<T>(obj, this.Pointer, nameof(ReadOnlyValPtr<T>));
	/// <inheritdoc/>
	public Int32 CompareTo(ReadOnlyValPtr<T> value)
#if NET5_0_OR_GREATER
		=> this.Pointer.CompareTo(value.Pointer);
#else
		=> ((Int64)this.Pointer).CompareTo((Int64)value.Pointer);
#endif

	/// <summary>Compares two values to determine which is less.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static Boolean operator <(ReadOnlyValPtr<T> left, ReadOnlyValPtr<T> right) => left < right._value;
	/// <summary>Compares two values to determine which is less or equal.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns>
	/// <c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise,
	/// <c>false</c>.
	/// </returns>
	public static Boolean operator <=(ReadOnlyValPtr<T> left, ReadOnlyValPtr<T> right) => left._value <= right._value;
	/// <summary>Compares two values to determine which is greater.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
	public static Boolean operator >(ReadOnlyValPtr<T> left, ReadOnlyValPtr<T> right) => left._value > right._value;
	/// <summary>Compares two values to determine which is greater or equal.</summary>
	/// <param name="left">The value to compare with <paramref name="right"/>.</param>
	/// <param name="right">The value to compare with <paramref name="left"/>.</param>
	/// <returns>
	/// <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise,
	/// <c>false</c>.
	/// </returns>
	public static Boolean operator >=(ReadOnlyValPtr<T> left, ReadOnlyValPtr<T> right) => left._value >= right._value;
}