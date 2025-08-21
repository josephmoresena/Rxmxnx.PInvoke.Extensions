#if !PACKAGE || TEMP_PACKAGE
namespace Rxmxnx.PInvoke;

public static partial class SystemInfo
{
	/// <summary>
	/// Target framework for the current build.
	/// </summary>
	public const String CompilationFramework =
#if NET9_0_OR_GREATER
			".NET 9.0"
#elif NET8_0_OR_GREATER
			".NET 8.0"
#elif NET7_0_OR_GREATER
			".NET 7.0"
#elif NET6_0_OR_GREATER
			".NET 6.0"
#elif NET5_0_OR_GREATER
			".NET 5.0"
#elif NETCOREAPP3_1
			".NET Core 3.1"
#elif NETCOREAPP3_0
			".NET Core 3.0"
#else
			".NET Standard 2.1"
#endif
		;
}
#endif