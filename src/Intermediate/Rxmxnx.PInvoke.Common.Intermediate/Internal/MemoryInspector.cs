namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This class allows to retrieve information about memory directions.
/// </summary>
internal abstract partial class MemoryInspector
{
	/// <summary>
	/// Current platform <see cref="MemoryInspector"/> instance.
	/// </summary>
	private static readonly MemoryInspector? instance;

	/// <summary>
	/// A <see cref="MemoryInspector"/> instance.
	/// </summary>
	public static MemoryInspector Instance => ValidationUtilities.ThrowIfNotSupportedPlatform(MemoryInspector.instance);

	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static MemoryInspector()
	{
		if (MemoryInspector.IsWindowsPlatform())
			MemoryInspector.instance = new Windows();
		else if (MemoryInspector.IsLinuxPlatform())
			MemoryInspector.instance = new Linux();
		else if (MemoryInspector.IsMacPlatform())
			MemoryInspector.instance = new Mac();
		else if (MemoryInspector.IsFreeBsd())
			MemoryInspector.instance = new FreeBsd();
	}

	/// <summary>
	/// Indicates whether given span represents a literal or hardcoded memory region.
	/// </summary>
	/// <typeparam name="T">Type of items in <paramref name="span"/>.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="T"/> items.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents a literal o hardcoded memory region;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public abstract Boolean IsLiteral<T>(ReadOnlySpan<T> span);

	/// <summary>
	/// Indicates whether the given span represents memory that is not part of a hardcoded literal.
	/// </summary>
	/// <typeparam name="T">Type of items in <paramref name="span"/>.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="T"/> items.</param>
	/// <returns>
	/// <see langword="true"/> if the given span represents memory that is not part of a hardcoded literal;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean MayBeNonLiteral<T>(ReadOnlySpan<T> span)
	{
		try
		{
			if (MemoryInspector.instance is not null)
				return !MemoryInspector.instance.IsLiteral(span);
		}
		catch (Exception)
		{
			// Ignore
		}
		return true;
	}

	/// <summary>
	/// Indicates whether the current execution is occurring on a Windows-compatible platform.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the current execution is occurring on a Windows-compatible platform; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean IsWindowsPlatform()
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsWindows();
#else
		=> RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on a Linux-compatible platform.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the current execution is occurring on a Linux-compatible platform; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean IsLinuxPlatform()
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsLinux() || OperatingSystem.IsAndroid();
#else
		=> RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"));
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on a macOS-compatible platform.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the current execution is occurring on a macOS-compatible platform; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean IsMacPlatform()
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsMacOS() || OperatingSystem.IsIOS() || OperatingSystem.IsTvOS() ||
			MemoryInspector.IsMacCatalyst();
#else
		=> RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("IOS")) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("TVOS")) || MemoryInspector.IsMacCatalyst();
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on Mac Catalyst platform.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the current execution is occurring on Mac Catalyst platform; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean IsMacCatalyst()
#if NET6_0_OR_GREATER
		=> OperatingSystem.IsMacCatalyst();
#else
		=> RuntimeInformation.IsOSPlatform(OSPlatform.Create("MACCATALYST"));
#endif
	/// <summary>
	/// Indicates whether the current execution is occurring on FreeBSD platform.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the current execution is occurring on FreeBSD platform; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean IsFreeBsd()
#if NET6_0_OR_GREATER
		=> OperatingSystem.IsFreeBSD();
#elif NETCOREAPP
		=> RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
#else
		=> RuntimeInformation.IsOSPlatform(OSPlatform.Create("FREEBSD"));
#endif
}