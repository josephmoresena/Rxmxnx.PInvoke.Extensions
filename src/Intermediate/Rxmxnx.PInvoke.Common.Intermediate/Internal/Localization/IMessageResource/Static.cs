namespace Rxmxnx.PInvoke.Internal.Localization;

#pragma warning disable CA2252
internal partial interface IMessageResource
{
	/// <summary>
	/// Retrieves internal resource objects.
	/// </summary>
	/// <returns>Resource resource object.</returns>
	[ExcludeFromCodeCoverage]
	public static IMessageResource GetInstance()
	{
		IMessageResource result = IMessageResource.GetInstance<DefaultMessageResource>();
		if (NativeUtilities.GlobalizationInvariantModeEnabled) return result;

		return NativeUtilities.UserInterfaceIso639P1 switch
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
			_ => result,
		};
	}

	/// <summary>
	/// Retrieves internal resource objects.
	/// </summary>
	/// <typeparam name="TResource">A <see cref="IMessageResource"/> type.</typeparam>
	/// <returns>Resource resource object.</returns>
	private static IMessageResource GetInstance<TResource>() where TResource : IMessageResource => TResource.Instance;
}
#pragma warning restore CA2252