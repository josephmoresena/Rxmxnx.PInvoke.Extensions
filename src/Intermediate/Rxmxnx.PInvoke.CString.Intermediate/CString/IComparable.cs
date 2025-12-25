namespace Rxmxnx.PInvoke;

public partial class CString : IComparable, IComparable<String>, IComparable<CString>
{
	/// <summary>
	/// Compares the current instance with another object of the same type or a <see cref="String"/>
	/// and returns an integer that indicates whether the current instance precedes, follows,
	/// or occurs in the same position in the sort order as the other object.
	/// </summary>
	/// <param name="obj">An object to compare with this instance.</param>
	/// <returns>
	/// A value that indicates the relative order of the objects being compared.
	/// The return value has these meanings:
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description>This instance precedes <paramref name="obj"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         This instance occurs in the same position in the sort order as <paramref name="obj"/>.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description>This instance follows <paramref name="obj"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	/// <exception cref="ArgumentException">
	/// <paramref name="obj"/> is neither a <see cref="String"/> nor a <see cref="CString"/> instance.
	/// </exception>
	public Int32 CompareTo(Object? obj)
	{
		switch (obj)
		{
			case null:
				return this.IsZero ? 0 : 1;
			case String str:
				return this.CompareTo(str);
			default:
				ValidationUtilities.ThrowIfInvalidCastType(obj, nameof(CString), out CString cstr);
				return this.CompareTo(cstr);
		}
	}
	/// <summary>
	/// Compares the current instance with another <see cref="CString"/> instance and returns an
	/// integer that indicates whether the current instance precedes, follows, or occurs in the
	/// same position in the sort order as the other <see cref="CString"/>.
	/// </summary>
	/// <param name="other">A <see cref="CString"/> to compare with this instance.</param>
	/// <returns>
	/// A value that indicates the relative order of the objects being compared.
	/// The return value has these meanings:
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description>This instance precedes <paramref name="other"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         This instance occurs in the same position in the sort order as <paramref name="other"/>.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description>This instance follows <paramref name="other"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public Int32 CompareTo(CString? other)
		=> CString.NullCompare(this, other) ?? CStringUtf8Comparator.Create().Compare(this, other);
	/// <summary>
	/// Compares the current instance with another <see cref="String"/> instance and returns an
	/// integer that indicates whether the current instance precedes, follows, or occurs in the
	/// same position in the sort order as the other <see cref="String"/>.
	/// </summary>
	/// <param name="other">A string to compare with this instance.</param>
	/// <returns>
	/// A value that indicates the relative order of the objects being compared.
	/// The return value has these meanings:
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description>This instance precedes <paramref name="other"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         This instance occurs in the same position in the sort order as <paramref name="other"/>.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description>This instance follows <paramref name="other"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public Int32 CompareTo(String? other)
		=> CString.NullCompare(this, other) ?? StringUtf8Comparator.Create().Compare(this, other, other);

	/// <summary>
	/// Compares two specified <see cref="CString"/> instances, and returns an integer that indicates their
	/// relative position in the sort order.
	/// </summary>
	/// <param name="cstrA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="cstrB">The second <see cref="CString"/> instance to compare.</param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="cstrA"/> precedes <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         <paramref name="cstrA"/> is in the same position as <paramref name="cstrB"/> in the sort order.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="cstrA"/> follows <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? cstrA, CString? cstrB)
		=> CString.NullCompare(cstrA, cstrB) ?? CStringUtf8Comparator.Create().Compare(cstrA, cstrB);
	/// <summary>
	/// Compares two specified <see cref="CString"/> instances, ignoring or honoring their case, and returns
	/// an integer that indicates their relative position in the sort order.
	/// </summary>
	/// <param name="cstrA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="cstrB">The second <see cref="CString"/> instance to compare.</param>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="cstrA"/> precedes <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         <paramref name="cstrA"/> is in the same position as <paramref name="cstrB"/> in the sort order.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="cstrA"/> follows <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? cstrA, CString? cstrB, Boolean ignoreCase)
		=> CString.NullCompare(cstrA, cstrB) ?? CStringUtf8Comparator.Create(ignoreCase).Compare(cstrA, cstrB);
	/// <summary>
	/// Compares two specified <see cref="CString"/> objects, ignoring or honoring their case, and using
	/// culture-specific information to influence the comparison, and returns an integer that indicates
	/// their relative position in the sort order.
	/// </summary>
	/// <param name="cstrA">The first <see cref="CString"/> to compare.</param>
	/// <param name="cstrB">The second <see cref="CString"/> to compare.</param>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="culture">
	/// An object that provides culture-specific comparison information.
	/// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
	/// </param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two strings being compared.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="cstrA"/> precedes <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         <paramref name="cstrA"/> is in the same position as <paramref name="cstrB"/> in the sort order.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="cstrA"/> follows <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? cstrA, CString? cstrB, Boolean ignoreCase, CultureInfo? culture)
		=> CString.NullCompare(cstrA, cstrB) ?? CStringUtf8Comparator.Create(ignoreCase, culture).Compare(cstrA, cstrB);
	/// <summary>
	/// Compares two specified <see cref="CString"/> instances using the specified rules, and returns an integer that indicates
	/// their
	/// relative position in the sort order.
	/// </summary>
	/// <param name="cstrA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="cstrB">The second <see cref="CString"/> instance to compare.</param>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies the rules to use in the comparison.
	/// </param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="cstrA"/> precedes <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description><paramref name="cstrA"/> is in the same position as <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="cstrA"/> follows <paramref name="cstrB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? cstrA, CString? cstrB, StringComparison comparisonType)
		=> CString.NullCompare(cstrA, cstrB) ?? CStringUtf8Comparator.Create(comparisonType).Compare(cstrA, cstrB);
	/// <summary>
	/// Compares two specified text instances using default rules, and returns an integer that indicates their
	/// relative position in the sort order.
	/// </summary>
	/// <param name="textA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="textB">The second <see cref="String"/> instance to compare.</param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         <paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? textA, String? textB)
		=> CString.NullCompare(textA, textB) ?? StringUtf8Comparator.Create().Compare(textA, textB, textB);
	/// <summary>
	/// Compares two specified text instances, ignoring or honoring their case, and returns an integer that indicates their
	/// relative position in the sort order.
	/// </summary>
	/// <param name="textA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="textB">The second <see cref="String"/> instance to compare.</param>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? textA, String? textB, Boolean ignoreCase)
		=> CString.NullCompare(textA, textB) ?? StringUtf8Comparator.Create(ignoreCase).Compare(textA, textB, textB);
	/// <summary>
	/// Compares two specified text instances, ignoring or honoring their case, and using culture-specific
	/// information to influence the comparison, and returns an integer that indicates their relative position
	/// in the sort order.
	/// </summary>
	/// <param name="textA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="textB">The second <see cref="String"/> instance to compare.</param>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="culture">
	/// An object that supplies culture-specific comparison information.
	/// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
	/// </param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         <paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? textA, String? textB, Boolean ignoreCase, CultureInfo? culture)
		=> CString.NullCompare(textA, textB) ??
			StringUtf8Comparator.Create(ignoreCase, culture).Compare(textA, textB, textB);
	/// <summary>
	/// Compares two specified text instances using the specified rules, and returns an integer that indicates their
	/// relative position in the sort order.
	/// </summary>
	/// <param name="textA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="textB">The second <see cref="String"/> instance to compare.</param>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies the rules to use in the comparison.
	/// </param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description>
	///         <paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public static Int32 Compare(CString? textA, String? textB, StringComparison comparisonType)
		=> CString.NullCompare(textA, textB) ??
			StringUtf8Comparator.Create(comparisonType).Compare(textA, textB, textB);
	/// <summary>
	/// Compares two text instances for <see langword="null"/> values.
	/// </summary>
	/// <typeparam name="TString">Type of the second text instance.</typeparam>
	/// <param name="cstrA">The first <see cref="CString"/> instance to compare.</param>
	/// <param name="tstrB">The second text instance to compare.</param>
	/// <returns>
	/// A nullable 32-bit signed integer that indicates the relationship between the two instances.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>
	///             <see langword="null"/>
	///         </term>
	///         <description>
	///         Both <paramref name="cstrA"/> and <paramref name="tstrB"/> are not <see langword="null"/>.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Less than zero (-1)</term>
	///         <description>
	///         <paramref name="cstrA"/> is <see langword="null"/> and <paramref name="tstrB"/> is not <see langword="null"/>.
	///         </description>
	///     </item>
	///     <item>
	///         <term>Zero (0)</term>
	///         <description>Both <paramref name="cstrA"/> and <paramref name="tstrB"/> are <see langword="null"/>.</description>
	///     </item>
	///     <item>
	///         <term>Greater than zero (1)</term>
	///         <description>
	///         <paramref name="cstrA"/> is not <see langword="null"/> and <paramref name="tstrB"/> is <see langword="null"/>.
	///         </description>
	///     </item>
	/// </list>
	/// </returns>
	private static Int32? NullCompare<TString>(CString? cstrA, TString? tstrB) where TString : class
		=> cstrA switch
		{
			null when tstrB is null => 0,
			null when tstrB is CString { IsZero: true, } => 0,
			null => -1,
			_ => tstrB is null ? 1 : default(Int32?),
		};
}