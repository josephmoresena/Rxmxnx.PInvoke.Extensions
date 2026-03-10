namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Provides information about the trimming of the runtime.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static class TrimInfo
{
	/// <summary>
	/// Web Assembly .NET Architecture.
	/// </summary>
#if !NET5_0_OR_GREATER
	public const Architecture WasmArch = (Architecture)4;
#else
	public const Architecture WasmArch = Architecture.Wasm;
#endif

	/// <summary>
	/// Internal UTF-8 empty text.
	/// </summary>
	/// <returns>A read-only byte span of UTF-8 null-characters.</returns>
	public static ReadOnlySpan<Byte> EmptyUt8Text() => "\0\0\0"u8;
	/// <summary>
	/// Internal Windows New line UTF-8 sequence.
	/// </summary>
	/// <returns>A read-only byte span containing UTF-8 new line.</returns>
	public static ReadOnlySpan<Byte> WindowsNewLine() => "\r\n"u8;
	/// <summary>
	/// Internal non-Windows New line UTF-8 sequence.
	/// </summary>
	/// <returns>A read-only byte span containing UTF-8 new line.</returns>
	public static ReadOnlySpan<Byte> NonWindowsNewLine() => "\n"u8;
	/// <summary>
	/// Indicates whether <see cref="String"/> type name contains the <c>String</c> word.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if <see cref="String"/> type name contains the <c>String</c> word;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean StringTypeNameContainsString()
		=> typeof(String).ToString().AsSpan().EndsWith(nameof(String).AsSpan());
	/// <summary>
	/// Indicates whether the current runtime has been trimmed for the platform.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the current runtime has been trimmed; otherwise; <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE && !NET5_0_OR_GREATER
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3400)]
#endif
	public static Boolean IsPlatformTrimmed()
	{
#if NET5_0_OR_GREATER
		return TrimInfo.IsDesktopTrimmedPlatform() || TrimInfo.IsMobileTrimmedPlatform() ||
			TrimInfo.IsWebTrimmedPlatform();
#else
		return false;
#endif
	}
	/// <summary>
	/// Retrieves the CLR type whose name is <paramref name="typeFullName"/> on the <paramref name="assemblyType"/> assembly.
	/// </summary>
	/// <param name="assemblyType">A CLR type on the searching assembly.</param>
	/// <param name="typeFullName">Full name of the searching type.</param>
	/// <returns>The CLR type found.</returns>
	[UnconditionalSuppressMessage("Trimming", "IL2026")]
	public static Type? SafeGetType(Type assemblyType, String typeFullName)
	{
		try
		{
			return assemblyType.Assembly.GetType(typeFullName);
		}
		catch (Exception)
		{
			return default;
		}
	}
#if NET5_0_OR_GREATER
	/// <summary>
	/// Indicates the current platform is desktop and trimmed.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if current platform is desktop and trimmed; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsDesktopTrimmedPlatform()
		=> OperatingSystem.IsWindows() || OperatingSystem.IsLinux() || OperatingSystem.IsMacOS() ||
			OperatingSystem.IsFreeBSD();
	/// <summary>
	/// Indicates the current platform is mobile XNU and trimmed.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if current platform is mobile XNU and trimmed; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean IsMobileTrimmedXnu()
		=> OperatingSystem.IsIOS() || OperatingSystem.IsTvOS() || OperatingSystem.IsWatchOS()
#if NET6_0_OR_GREATER
			|| OperatingSystem.IsMacCatalyst()
#endif
	;
#if NET6_0_OR_GREATER
	/// <summary>
	/// Indicates whether no IL code bytes have been processed at the current runtime.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if no IL code bytes have been processed at the current runtime; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean ZeroIlBytes() => JitInfo.GetCompiledILBytes() == 0L && JitInfo.GetCompiledMethodCount() == 0;
#endif

	/// <summary>
	/// Indicates the current platform is mobile and trimmed.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if current platform is mobile and trimmed; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsMobileTrimmedPlatform() => OperatingSystem.IsAndroid() || TrimInfo.IsMobileTrimmedXnu();
	/// <summary>
	/// Indicates the current platform is web and trimmed.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if current platform is web and trimmed; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsWebTrimmedPlatform()
		=> OperatingSystem.IsBrowser()
#if NET8_0_OR_GREATER
			|| OperatingSystem.IsWasi()
#endif
	;
#endif
}