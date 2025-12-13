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
#pragma warning disable S2743
	// ReSharper disable once StaticMemberInGenericType
	public static readonly ReadOnlyMemory<String> Values;
#pragma warning restore S2743

	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static EnumNameHelper()
	{
#if NET5_0_OR_GREATER
		EnumNameHelper<TEnum>.Values = new(Enum.GetNames<TEnum>());
#else
		EnumNameHelper<TEnum>.Values = new(Enum.GetNames(typeof(TEnum)));
#endif
	}
}