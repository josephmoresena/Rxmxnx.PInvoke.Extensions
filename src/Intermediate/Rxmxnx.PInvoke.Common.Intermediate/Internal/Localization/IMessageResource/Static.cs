namespace Rxmxnx.PInvoke.Internal.Localization;

#pragma warning disable CA2252
internal partial interface IMessageResource
{
	/// <summary>
	/// Retrieves internal resource objects.
	/// </summary>
	/// <returns>Resource resource object.</returns>
	public static IMessageResource GetInstance()
		=> IMessageResource.GetLangCode() switch
		{
			Iso639P1.Es => IMessageResource.GetInstance<SpanishMessageResource>(),
			Iso639P1.Fr => IMessageResource.GetInstance<FrenchMessageResource>(),
			Iso639P1.De => IMessageResource.GetInstance<GermanMessageResource>(),
			Iso639P1.Zh => IMessageResource.GetInstance<ChineseMessageResource>(),
			Iso639P1.Ja => IMessageResource.GetInstance<JapaneseMessageResource>(),
			Iso639P1.Ru => IMessageResource.GetInstance<RussianMessageResource>(),
			Iso639P1.Ar => IMessageResource.GetInstance<ArabicMessageResource>(),
			Iso639P1.Pt => IMessageResource.GetInstance<PortugueseMessageResource>(),
			Iso639P1.It => IMessageResource.GetInstance<ItalianMessageResource>(),
			_ => IMessageResource.GetInstance<DefaultMessageResource>(),
		};

	/// <summary>
	/// Retrieves internal resource objects.
	/// </summary>
	/// <typeparam name="TResource">A <see cref="IMessageResource"/> type.</typeparam>
	/// <returns>Resource resource object.</returns>
	private static IMessageResource GetInstance<TResource>() where TResource : IMessageResource => TResource.Instance;
	/// <summary>
	/// Retrieves the ISO 639-1 code for the language of the current UI.
	/// </summary>
	/// <returns>The ISO 639-1 code for the language of the current UI.</returns>
	private static Iso639P1 GetLangCode()
	{
		try
		{
			Iso639P1 result = NativeUtilities.GetIso639P1(CultureInfo.CurrentUICulture);
			if (result == default) result = Iso639P1.En;
			return result;
		}
		catch (Exception)
		{
			return Iso639P1.En;
		}
	}
}
#pragma warning restore CA2252