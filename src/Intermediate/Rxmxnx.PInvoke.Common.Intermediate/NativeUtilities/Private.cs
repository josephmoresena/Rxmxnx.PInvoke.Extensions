namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of utilities for exchange data within the P/Invoke context.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class NativeUtilities
{
	/// <summary>
	/// Cache for <see cref="GlobalizationInvariantModeEnabled"/>
	/// </summary>
	private static Boolean? globalizationInvariantMode;

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
	/// <summary>
	/// Writes <paramref name="span"/> using <paramref name="arg"/> and <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">Unmanaged type of elements in <paramref name="span"/>.</typeparam>
	/// <typeparam name="TArg">Type of state object.</typeparam>
	/// <param name="span">A <typeparamref name="T"/> writable memory block.</param>
	/// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
	/// <param name="action">A <see cref="SpanAction{T, TState}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WriteSpan<T, TArg>(Span<T> span, TArg arg, SpanAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
#pragma warning disable CS8500
		fixed (void* _ = &MemoryMarshal.GetReference(span))
#pragma warning restore CS8500
			action(span, arg);
	}
}