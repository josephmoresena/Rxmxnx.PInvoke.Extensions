#if !NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Enum utilities for internal use.
/// </summary>
internal static class EnumUtilities
{
	/// <inheritdoc cref="Enum.GetName(Type, Object)"/>
	public static String? GetName<TEnum>(TEnum value) where TEnum : Enum => Enum.GetName(typeof(TEnum), value);
}
#endif