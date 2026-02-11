namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Provides information about the Mono runtime.
/// </summary>
internal static class MonoInfo
{
	/// <summary>
	/// Indicates whether internal UTF-8 empty text is loaded in a read-only memory section.
	/// </summary>
	public static readonly Boolean IsEmptyNonLiteral;
	/// <summary>
	/// The <c>Mono.Runtime</c> CLR type.
	/// </summary>
	public static readonly Type? MonoRuntimeType;
	/// <summary>
	/// The <c>Mono.MonoAssemblyName</c> CLR type.
	/// </summary>
	public static readonly Type? MonoAssemblyNameType;

	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static unsafe MonoInfo()
	{
#if !PACKAGE
		fixed (Byte* ptr = &MemoryMarshal.GetReference(TrimInfo.EmptyUt8Text()))
#else
		fixed (Byte* ptr = CString.Empty)
#endif
			MonoInfo.IsEmptyNonLiteral = MemoryInspector.MayBeNonLiteral(ptr);
		MonoInfo.MonoRuntimeType = TrimInfo.SafeGetType(typeof(String), "Mono.Runtime");
		MonoInfo.MonoAssemblyNameType = TrimInfo.SafeGetType(typeof(String), "Mono.MonoAssemblyName");
	}
}