namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="Enum"/> compatibility utilities for internal use.
/// </summary>
internal static class EnumCompat
{
	/// <inheritdoc cref="Enum.GetName(Type, Object)"/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String? GetName<TEnum>(TEnum value) where TEnum : struct, Enum
#if !PACKAGE || !NET5_0_OR_GREATER
#pragma warning disable CA2263
	// ReSharper disable once HeapView.BoxingAllocation
		=> Enum.GetName(typeof(TEnum), value);
#pragma warning restore CA2263
#else
		=> Enum.GetName(value);
#endif
}