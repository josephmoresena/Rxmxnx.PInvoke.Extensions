namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Value <see cref="Enum"/> helper.
/// </summary>
internal static class EnumValueHelper<TEnum> where TEnum : struct, Enum
{
	/// <summary>
	/// Internal array.
	/// </summary>
	public static readonly ReadOnlyMemory<TEnum> Values;

	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static EnumValueHelper()
	{
#if NET5_0_OR_GREATER
		EnumValueHelper<TEnum>.Values = new(Enum.GetValues<TEnum>());
#else
		EnumValueHelper<TEnum>.Values = new((TEnum[])Enum.GetValues(typeof(TEnum)));
#endif
	}
}