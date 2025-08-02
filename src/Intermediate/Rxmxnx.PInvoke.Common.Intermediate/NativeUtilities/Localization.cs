namespace Rxmxnx.PInvoke;

public partial class NativeUtilities
{
	/// <summary>
	/// Cache for <see cref="GlobalizationInvariantModeEnabled"/>
	/// </summary>
	private static Boolean? globalizationInvariantMode;

	/// <summary>
	/// Indicates whether globalization-invariant mode is enabled.
	/// </summary>
	/// <remarks>This property allows trim propagation at compile time using <c>InvariantGlobalization</c>.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean GlobalizationInvariantModeEnabled
		=> NativeUtilities.globalizationInvariantMode ??= NativeUtilities.IsGlobalizationInvariantMode();
	/// <summary>
	/// Retrieves the <see cref="Iso639P1"/> enum value corresponding to the current user interface culture.
	/// </summary>
	/// <remarks>This property allows trim propagation at compile time using <c>InvariantGlobalization</c>.</remarks>
	public static Iso639P1 UserInterfaceIso639P1 => (Iso639P1)NativeUtilities.GetUserInterfaceTwoLetterLangCode();

	/// <summary>
	/// Retrieves the <see cref="Iso639P1"/> enum value corresponding to the specified <paramref name="culture"/>.
	/// </summary>
	/// <param name="culture">A <see cref="CultureInfo"/> instance.</param>
	/// <returns>A <see cref="Iso639P1"/> enum value.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Iso639P1 GetIso639P1(CultureInfo culture)
	{
		try
		{
			String languageCode = culture.TwoLetterISOLanguageName;
			return languageCode.ToLowerInvariant() switch
			{
				"ab" => Iso639P1.Ab,
				"aa" => Iso639P1.Aa,
				"af" => Iso639P1.Af,
				"ak" => Iso639P1.Ak,
				"sq" => Iso639P1.Sq,
				"am" => Iso639P1.Am,
				"ar" => Iso639P1.Ar,
				"an" => Iso639P1.An,
				"hy" => Iso639P1.Hy,
				"as" => Iso639P1.As,
				"av" => Iso639P1.Av,
				"ae" => Iso639P1.Ae,
				"ay" => Iso639P1.Ay,
				"az" => Iso639P1.Az,
				"bm" => Iso639P1.Bm,
				"ba" => Iso639P1.Ba,
				"eu" => Iso639P1.Eu,
				"be" => Iso639P1.Be,
				"bn" => Iso639P1.Bn,
				"bi" => Iso639P1.Bi,
				"bs" => Iso639P1.Bs,
				"br" => Iso639P1.Br,
				"bg" => Iso639P1.Bg,
				"my" => Iso639P1.My,
				"ca" => Iso639P1.Ca,
				"ch" => Iso639P1.Ch,
				"ce" => Iso639P1.Ce,
				"ny" => Iso639P1.Ny,
				"zh" => Iso639P1.Zh,
				"cv" => Iso639P1.Cv,
				"kw" => Iso639P1.Kw,
				"co" => Iso639P1.Co,
				"cr" => Iso639P1.Cr,
				"cs" => Iso639P1.Cs,
				"da" => Iso639P1.Da,
				"dv" => Iso639P1.Dv,
				"nl" => Iso639P1.Nl,
				"dz" => Iso639P1.Dz,
				"en" => Iso639P1.En,
				"eo" => Iso639P1.Eo,
				"et" => Iso639P1.Et,
				"ee" => Iso639P1.Ee,
				"fo" => Iso639P1.Fo,
				"fj" => Iso639P1.Fj,
				"fi" => Iso639P1.Fi,
				"fr" => Iso639P1.Fr,
				"ff" => Iso639P1.Ff,
				"gl" => Iso639P1.Gl,
				"ka" => Iso639P1.Ka,
				"de" => Iso639P1.De,
				"el" => Iso639P1.El,
				"gn" => Iso639P1.Gn,
				"gu" => Iso639P1.Gu,
				"ht" => Iso639P1.Ht,
				"ha" => Iso639P1.Ha,
				"he" => Iso639P1.He,
				"hz" => Iso639P1.Hz,
				"hi" => Iso639P1.Hi,
				"ho" => Iso639P1.Ho,
				"hu" => Iso639P1.Hu,
				"is" => Iso639P1.Is,
				"io" => Iso639P1.Io,
				"ig" => Iso639P1.Ig,
				"id" => Iso639P1.Id,
				"ia" => Iso639P1.Ia,
				"ie" => Iso639P1.Ie,
				"iu" => Iso639P1.Iu,
				"ik" => Iso639P1.Ik,
				"ga" => Iso639P1.Ga,
				"it" => Iso639P1.It,
				"ja" => Iso639P1.Ja,
				"jv" => Iso639P1.Jv,
				"kl" => Iso639P1.Kl,
				"kn" => Iso639P1.Kn,
				"kr" => Iso639P1.Kr,
				"ks" => Iso639P1.Ks,
				"kk" => Iso639P1.Kk,
				"ki" => Iso639P1.Ki,
				"rw" => Iso639P1.Rw,
				"ky" => Iso639P1.Ky,
				"kv" => Iso639P1.Kv,
				"kg" => Iso639P1.Kg,
				"ko" => Iso639P1.Ko,
				"kj" => Iso639P1.Kj,
				"ku" => Iso639P1.Ku,
				"lo" => Iso639P1.Lo,
				"la" => Iso639P1.La,
				"lv" => Iso639P1.Lv,
				"li" => Iso639P1.Li,
				"ln" => Iso639P1.Ln,
				"lt" => Iso639P1.Lt,
				"lu" => Iso639P1.Lu,
				"lb" => Iso639P1.Lb,
				"mk" => Iso639P1.Mk,
				"mg" => Iso639P1.Mg,
				"ms" => Iso639P1.Ms,
				"ml" => Iso639P1.Ml,
				"mt" => Iso639P1.Mt,
				"gv" => Iso639P1.Gv,
				"mi" => Iso639P1.Mi,
				"mr" => Iso639P1.Mr,
				"mh" => Iso639P1.Mh,
				"mn" => Iso639P1.Mn,
				"na" => Iso639P1.Na,
				"nv" => Iso639P1.Nv,
				"ng" => Iso639P1.Ng,
				"ne" => Iso639P1.Ne,
				"nd" => Iso639P1.Nd,
				"se" => Iso639P1.Se,
				"no" => Iso639P1.No,
				"nb" => Iso639P1.Nb,
				"nn" => Iso639P1.Nn,
				"ii" => Iso639P1.Ii,
				"oc" => Iso639P1.Oc,
				"oj" => Iso639P1.Oj,
				"or" => Iso639P1.Or,
				"om" => Iso639P1.Om,
				"os" => Iso639P1.Os,
				"pa" => Iso639P1.Pa,
				"pi" => Iso639P1.Pi,
				"fa" => Iso639P1.Fa,
				"pl" => Iso639P1.Pl,
				"ps" => Iso639P1.Ps,
				"pt" => Iso639P1.Pt,
				"qu" => Iso639P1.Qu,
				"ro" => Iso639P1.Ro,
				"rm" => Iso639P1.Rm,
				"rn" => Iso639P1.Rn,
				"ru" => Iso639P1.Ru,
				"sm" => Iso639P1.Sm,
				"sg" => Iso639P1.Sg,
				"sa" => Iso639P1.Sa,
				"sc" => Iso639P1.Sc,
				"gd" => Iso639P1.Gd,
				"sr" => Iso639P1.Sr,
				"sn" => Iso639P1.Sn,
				"sd" => Iso639P1.Sd,
				"si" => Iso639P1.Si,
				"sk" => Iso639P1.Sk,
				"sl" => Iso639P1.Sl,
				"so" => Iso639P1.So,
				"st" => Iso639P1.St,
				"nr" => Iso639P1.Nr,
				"es" => Iso639P1.Es,
				"su" => Iso639P1.Su,
				"sw" => Iso639P1.Sw,
				"ss" => Iso639P1.Ss,
				"sv" => Iso639P1.Sv,
				"tl" => Iso639P1.Tl,
				"ty" => Iso639P1.Ty,
				"tg" => Iso639P1.Tg,
				"ta" => Iso639P1.Ta,
				"tt" => Iso639P1.Tt,
				"te" => Iso639P1.Te,
				"th" => Iso639P1.Th,
				"bo" => Iso639P1.Bo,
				"ti" => Iso639P1.Ti,
				"to" => Iso639P1.To,
				"ts" => Iso639P1.Ts,
				"tn" => Iso639P1.Tn,
				"tr" => Iso639P1.Tr,
				"tk" => Iso639P1.Tk,
				"tw" => Iso639P1.Tw,
				"ug" => Iso639P1.Ug,
				"uk" => Iso639P1.Uk,
				"ur" => Iso639P1.Ur,
				"uz" => Iso639P1.Uz,
				"ve" => Iso639P1.Ve,
				"vi" => Iso639P1.Vi,
				"vo" => Iso639P1.Vo,
				"wa" => Iso639P1.Wa,
				"cy" => Iso639P1.Cy,
				"fy" => Iso639P1.Fy,
				"wo" => Iso639P1.Wo,
				"xh" => Iso639P1.Xh,
				"yi" => Iso639P1.Yi,
				"yo" => Iso639P1.Yo,
				"za" => Iso639P1.Za,
				"zu" => Iso639P1.Zu,
				_ => Iso639P1.Iv,
			};
		}
		catch (Exception)
		{
			return Iso639P1.Iv;
		}
	}

	/// <summary>
	/// Checks if globalization-invariant mode is enabled.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if globalization-invariant mode is enabled; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean IsGlobalizationInvariantMode()
	{
		try
		{
			return CultureInfo.GetCultureInfo(0x409).LCID == 0x1000;
		}
		catch (CultureNotFoundException)
		{
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
			return cultures.Length <= 1;
		}
	}
	/// <summary>
	/// Retrieves the Iso639-1 language code enum value corresponding to the current user interface culture.
	/// </summary>
	/// <returns><see cref="Iso639P1"/> code integer value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 GetUserInterfaceTwoLetterLangCode()
		=> (Int32)NativeUtilities.GetIso639P1(CultureInfo.CurrentUICulture);
}