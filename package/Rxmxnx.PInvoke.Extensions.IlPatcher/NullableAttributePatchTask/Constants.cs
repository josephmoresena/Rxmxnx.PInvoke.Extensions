namespace Rxmxnx.PInvoke.Extensions.IlPatcher;

// ReSharper disable MemberCanBePrivate.Global
public partial class NullableAttributePatchTask
{
	/// <summary>
	/// <c>NullablePublicOnlyAttribute</c> type name.
	/// </summary>
	public const String NullablePublicOnlyAttributeName = "NullablePublicOnlyAttribute";
	/// <summary>
	/// <c>NullableAttribute</c> type name.
	/// </summary>
	public const String NullableAttributeName = "NullableAttribute";
	/// <summary>
	/// <c>NullableContextAttribute</c> type name.
	/// </summary>
	public const String NullableContextAttributeName = "NullableContextAttribute";
	/// <summary>
	/// Nullable flag for an annotated reference, the <c>?</c> annotation.
	/// </summary>
	public const Byte AnnotatedFlag = 2;
}