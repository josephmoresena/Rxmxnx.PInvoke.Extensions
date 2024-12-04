namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Constants for <see cref="SuppressMessageAttribute"/>.
/// </summary>
internal static class SuppressMessageConstants
{
	public const String CSharpSquid = "csharpsquid";
	public const String CheckIdS107 = "S107";
	public const String CheckIdS6640 = "S6640";
	public const String CheckIdS3881 = "S3881";
	public const String CheckIdS2436 = "S2436";
	public const String CheckIdS4144 = "S4144";
	public const String CheckIdS2743 = "S2743";
	public const String CheckIdS3011 = "S3011";
	public const String CheckIdS6670 = "S6670";

	public const String AvoidableReflectionUseJustification =
		"There are alternatives that avoid the use of reflection.";
	public const String StaticAbstractPropertyUseJustification =
		"There is no static field, but abstract/virtual property.";
	public const String ReflectionPrivateUseJustification =
		"Reflection use is limited privately and is used only to avoid infinity recursive initialization type.";
}