namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Name <see cref="Enum"/> helper.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2743)]
#endif
internal static class EnumNameHelper<TEnum> where TEnum : struct, Enum
{
	/// <summary>
	/// Internal array.
	/// </summary>
#pragma disable warning S2743
	public static readonly String[] Values;
#pragma warning restore S2743

	/// <summary>
	/// Static constructor.
	/// </summary>
	static EnumNameHelper()
	{
#if NET5_0_OR_GREATER
		EnumNameHelper<TEnum>.Values = Enum.GetNames<TEnum>();
#else
		EnumNameHelper<TEnum>.Values = Enum.GetNames(typeof(TEnum));
#endif
	}
}