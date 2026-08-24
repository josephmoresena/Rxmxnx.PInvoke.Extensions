namespace Rxmxnx.PInvoke.Internal.Localization;

internal static class MessageResource
{
	/// <summary>
	/// Retrieves internal resource objects.
	/// </summary>
	/// <returns>Resource object.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static IMessageResource GetInstance()
	{
		IMessageResource result = DefaultMessageResource.Instance;
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
		if (NativeUtilities.GlobalizationInvariantModeEnabled) return result;
#endif
		return NativeUtilities.UserInterfaceIso639P1 switch
		{
			Iso639P1.Es => SpanishMessageResource.Instance,
			Iso639P1.Fr => FrenchMessageResource.Instance,
			Iso639P1.De => GermanMessageResource.Instance,
			Iso639P1.Zh => ChineseMessageResource.Instance,
			Iso639P1.Ja => JapaneseMessageResource.Instance,
			Iso639P1.Ru => RussianMessageResource.Instance,
			Iso639P1.Ar => ArabicMessageResource.Instance,
			Iso639P1.Pt => PortugueseMessageResource.Instance,
			Iso639P1.It => ItalianMessageResource.Instance,
			_ => result,
		};
	}
}