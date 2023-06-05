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
    public static explicit operator CString?(String? str) => str is not null ? new(str) : default;

    /// <summary>
    /// Defines an implicit conversion of a given <see cref="CString"/> to a read-only span of bytes.
    /// </summary>
    /// <param name="value">A <see cref="CString"/> to implicitly convert.</param>
    public static implicit operator ReadOnlySpan<Byte>(CString? value) => value is not null ? value.AsSpan() : default;

    /// <summary>
    /// Determines whether two specified <see cref="CString"/> have the same value.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> is the same as the value 
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator ==(CString? a, CString? b) => a?.Equals(b) ?? b is null;

    /// <summary>
    /// Determines whether two specified <see cref="CString"/> have different values.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> is different from the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator !=(CString? a, CString? b) => !(a == b);

    /// <summary>
    /// Determines whether two specified <see cref="String"/> and <see cref="CString"/>
    /// have the same value.
    /// </summary>
    /// <param name="a">The first <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> is the same as the value 
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator ==(String? a, CString? b) => b?.Equals(a) ?? a is null;

    /// <summary>
    /// Determines whether two specified <see cref="String"/> and <see cref="CString"/>
    /// have different values.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> is different from the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator !=(String? a, CString? b) => !(a == b);

    /// <summary>
    /// Determines whether two specified <see cref="CString"/> and <see cref="String"/>
    /// have the same value.
    /// </summary>
    /// <param name="a">The first <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> is the same as the value 
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator ==(CString? a, String? b) => a?.Equals(b) ?? b is null;

    /// <summary>
    /// Determines whether two specified <see cref="CString"/> and <see cref="String"/>
    /// have different values.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> is different from the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator !=(CString? a, String? b) => !(a == b);

    /// <summary>
    /// Determines whether <paramref name="a"/> follows to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> follows to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator >(CString? a, CString? b) => CString.Compare(a, b) > 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> follows or is equal to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> follows or is equal to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator >=(CString? a, CString? b) => CString.Compare(a, b) >= 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> precedes to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> precedes to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator <=(CString? a, CString? b) => CString.Compare(a, b) <= 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> precedes or is equal to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> precedes or is equal to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator <(CString? a, CString? b) => CString.Compare(a, b) < 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> follows to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> follows to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator >(CString? a, String? b) => CString.Compare(a, b) > 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> follows or is equal to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> follows or is equal to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator >=(CString? a, String? b) => CString.Compare(a, b) >= 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> precedes to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> precedes to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator <=(CString? a, String? b) => CString.Compare(a, b) <= 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> precedes or is equal to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> precedes or is equal to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator <(CString? a, String? b) => CString.Compare(a, b) < 0;



    /// <summary>
    /// Determines whether <paramref name="a"/> follows to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> follows to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator >(String? a, CString? b) => CString.Compare(b, a) < 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> follows or is equal to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> follows or is equal to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator >=(String? a, CString? b) => CString.Compare(b, a) <= 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> precedes to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> precedes to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator <=(String? a, CString? b) => CString.Compare(b, a) >= 0;

    /// <summary>
    /// Determines whether <paramref name="a"/> precedes or is equal to <paramref name="b"/>.
    /// </summary>
    /// <param name="a">The first <see cref="String"/> to compare, or <see langword="null"/>.</param>
    /// <param name="b">The second <see cref="CString"/> to compare, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value of <paramref name="a"/> precedes or is equal to the value  
    /// of <paramref name="b"/>; otherwise, <see langword="false"/>.
    /// </returns>
    public static Boolean operator <(String? a, CString? b) => CString.Compare(b, a) > 0;
}