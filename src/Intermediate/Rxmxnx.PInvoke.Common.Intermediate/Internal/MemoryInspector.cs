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
		if (OperatingSystem.IsWindows())
			MemoryInspector.instance = new Windows();
		else if (MemoryInspector.IsLinuxPlatform())
			MemoryInspector.instance = new Linux();
		else if (MemoryInspector.IsMacPlatform())
			MemoryInspector.instance = new Mac();
	}

	/// <summary>
	/// Indicates whether current span represents a literal or hardcoded memory region.
	/// </summary>
	/// <typeparam name="T">Type of items in <paramref name="span"/>.</typeparam>
	/// <param name="span">A read-only span of <typeparamref name="T"/> items.</param>
	/// <returns>
	/// <see langword="true"/> if current span represents a constant, literal o hardcoded memory region;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public abstract Boolean IsLiteral<T>(ReadOnlySpan<T> span);

	/// <summary>
	/// Indicates whether the current execution is occurring on a Linux-compatible platform.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if the current execution is occurring on a Linux-compatible platform; otherwise,
	/// <see langword="false"/>.
	/// </returns>
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
	private static Boolean IsMacPlatform()
#if NET5_0_OR_GREATER
		=> OperatingSystem.IsMacOS() || OperatingSystem.IsIOS() || OperatingSystem.IsTvOS() ||
			OperatingSystem.IsMacCatalyst();
#else
		=> RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("IOS")) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("TVOS")) ||
			RuntimeInformation.IsOSPlatform(OSPlatform.Create("MACCATALYST"));
#endif
}