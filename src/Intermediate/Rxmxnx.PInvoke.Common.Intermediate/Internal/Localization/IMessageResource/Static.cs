namespace Rxmxnx.PInvoke.Internal.Localization;

#pragma warning disable CA2252
internal partial interface IMessageResource
{
	/// <summary>
	/// Retrieves internal resource objects.
	/// </summary>
	/// <returns>Resource resource object.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static IMessageResource GetInstance()
	{
		IMessageResource result = DefaultMessageResource.Instance;
		if (NativeUtilities.GlobalizationInvariantModeEnabled) return result;

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
#pragma warning restore CA2252