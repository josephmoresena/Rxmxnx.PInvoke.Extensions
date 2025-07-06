namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="Enum"/> compatibility utilities for internal use.
/// </summary>
internal static class EnumCompat
{
	/// <inheritdoc cref="Enum.GetName(Type, Object)"/>
	public static String? GetName<TEnum>(TEnum value) where TEnum : struct, Enum
#if !PACKAGE || !NET5_0_OR_GREATER
		=> Enum.GetName(typeof(TEnum), value);
#else
		=> Enum.GetName<TEnum>(value);
#endif
}