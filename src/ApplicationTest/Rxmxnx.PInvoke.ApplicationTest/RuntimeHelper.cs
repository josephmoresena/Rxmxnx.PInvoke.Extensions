using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
#if NET6_0_OR_GREATER
using System.Runtime;
#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if NET5_0_OR_GREATER
	[UnconditionalSuppressMessage("SingleFile", "IL3000")]
#endif
	public static class RuntimeHelper
	{
		private const String runtimeName =
#if NET10_0_OR_GREATER
				".NET 10.0"
#elif NET9_0_OR_GREATER
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
                    "Mono"
#endif
#if REFLECTION_FREE
                    + " Reflection-free"
#endif
#if NATIVE_AOT
                    + " NativeAOT"
#endif
#if R2R_EXECUTABLE
                    + " ReadyToRun (R2R)"
#endif
#if WEB_ASSEMBLY
                    + " WASM"
#endif
			;

#if !CSHARP_90
		public static readonly Random Shared = new Random();
		public static readonly CString Null = new CString(RuntimeHelper.NullBytes);
#else
		public static readonly Random Shared = new();
		public static readonly CString Null = new(static () =>
		{
			Byte[] utf8 = { (Byte)'N', (Byte)'u', (Byte)'l', (Byte)'l', (Byte)'\0', };
			return utf8.AsSpan()[..^1];
		});
#endif

		public static void PrintRuntimeInfo()
		{
			Console.WriteLine("========== Application for " + RuntimeHelper.runtimeName + " ==========");
			RuntimeHelper.PrintDomainInfo();
			Console.WriteLine("========== Runtime information ==========");
			Console.WriteLine($"Number of Cores: {Environment.ProcessorCount}");
			Console.WriteLine($"Is Little-Endian: {BitConverter.IsLittleEndian}");
			Console.WriteLine($"OS: {RuntimeInformation.OSDescription}");
			Console.WriteLine($"OS Arch: {RuntimeInformation.OSArchitecture.GetName()}");
			Console.WriteLine($"OS Version: {Environment.OSVersion}");
			Console.WriteLine($"Computer: {Environment.MachineName}");
			Console.WriteLine($"User: {Environment.UserName}");
			Console.WriteLine($"UI Culture: {CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}");
			Console.WriteLine($"System Path: {Environment.SystemDirectory}");
			Console.WriteLine($"Current Path: {Environment.CurrentDirectory}");
			Console.WriteLine($"Process Arch: {RuntimeInformation.ProcessArchitecture.GetName()}");
			try
			{
				Console.WriteLine($"Framework Version: {Environment.Version}");
				Console.WriteLine($"Runtime Path: {RuntimeEnvironment.GetRuntimeDirectory()}");
				Console.WriteLine($"Runtime Version: {RuntimeEnvironment.GetSystemVersion()}");
			}
			catch (Exception ex)
			{
				Console.WriteLine("**Unable to retrieve runtime info**");
				if (!AotInfo.IsReflectionDisabled)
					Console.WriteLine(ex);
			}
			Console.WriteLine($"Dynamic Code Compiled: {RuntimeFeature.IsDynamicCodeCompiled}");
			Console.WriteLine($"Dynamic Code Supported: {RuntimeFeature.IsDynamicCodeSupported}");
#if NET6_0_OR_GREATER
			Console.WriteLine($"IL compiled bytes: {JitInfo.GetCompiledILBytes()}");
			Console.WriteLine($"IL method count: {JitInfo.GetCompiledMethodCount()}");
			Console.WriteLine($"IL compilation time: {JitInfo.GetCompilationTime()}");
#endif
			Console.WriteLine("========== Rxmxnx.PInvoke Runtime information ==========");
#if !RELEASE_PACKAGE
			Console.WriteLine($"Package: {SystemInfo.CompilationFramework}");
#endif
			Console.WriteLine($"Native AOT: {AotInfo.IsNativeAot}");
			Console.WriteLine($"Reflection Enabled: {!AotInfo.IsReflectionDisabled}");
			Console.WriteLine($"IL Code Generation Supported: {AotInfo.IsCodeGenerationSupported}");
			Console.WriteLine($"Trimmed Runtime: {AotInfo.IsPlatformTrimmed}");
			Console.WriteLine($"Mono Runtime: {SystemInfo.IsMonoRuntime}");
			Console.WriteLine($"Web Runtime: {SystemInfo.IsWebRuntime}");
			Console.WriteLine($"Windows Platform: {SystemInfo.IsWindows}");
			Console.WriteLine($"Linux Platform: {SystemInfo.IsLinux}");
			Console.WriteLine($"macOS Platform: {SystemInfo.IsMac}");
			Console.WriteLine($"FreeBSD Platform: {SystemInfo.IsFreeBsd}");
			Console.WriteLine($"NetBSD Platform: {SystemInfo.IsNetBsd}");
			Console.WriteLine($"Solaris Platform: {SystemInfo.IsSolaris}");
			Console.WriteLine($"Pointer Size: {NativeUtilities.PointerSize}");
			Console.WriteLine($"Globalization-Invariant Mode: {NativeUtilities.GlobalizationInvariantModeEnabled}");
			Console.WriteLine($"UI Iso639-1: {NativeUtilities.UserInterfaceIso639P1}");
			Console.WriteLine($"Buffer AutoComposition Enabled: {BufferManager.BufferAutoCompositionEnabled}");
			Console.WriteLine($"String constant: {!RuntimeHelper.runtimeName.AsSpan().MayBeNonLiteral()}");
			Console.WriteLine($"CString.Empty literal: {CString.IsImagePersistent(CString.Empty)}");
			Console.WriteLine($"Hardcoded Array literal: {!RuntimeHelper.Null.AsSpan().MayBeNonLiteral()}");
		}
		private static void PrintDomainInfo()
		{
			try
			{
#if !NET9_0_OR_GREATER
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().AsReadOnlySpan())
				{
#else
				ReadOnlySpan<Assembly>.Enumerator enumerator =
					AppDomain.CurrentDomain.GetAssemblies().AsReadOnlySpan().GetEnumerator();
				while (enumerator.MoveNext())
				{
					ref readonly Assembly assembly = ref enumerator.Current;
#endif
					if (assembly == Assembly.GetExecutingAssembly()) continue;
					Boolean useFullName = AotInfo.IsNativeAot || assembly.IsDynamic;
					Console.WriteLine(useFullName ? assembly.FullName : assembly.GetAssemblyName());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("**Unable to retrieve domain info**");
				if (!AotInfo.IsReflectionDisabled)
					Console.WriteLine(ex);
			}
		}
		private static String GetName(this Architecture architecture)
			=> architecture switch
			{
				Architecture.X86 => nameof(Architecture.X86),
				Architecture.X64 => nameof(Architecture.X64),
				Architecture.Arm => nameof(Architecture.Arm),
				Architecture.Arm64 => nameof(Architecture.Arm64),
#if NET5_0_OR_GREATER
				Architecture.Wasm => nameof(Architecture.Wasm),
#endif
#if NET6_0_OR_GREATER
				Architecture.S390x => nameof(Architecture.S390x),
#endif
#if NET7_0_OR_GREATER
				Architecture.LoongArch64 => nameof(Architecture.LoongArch64),
				Architecture.Armv6 => nameof(Architecture.Armv6),
				Architecture.Ppc64le => nameof(Architecture.Ppc64le),
#endif
#if NET9_0_OR_GREATER
				Architecture.RiscV64 => nameof(Architecture.RiscV64),
#endif
				_ => $"{architecture}",
			};
		private static String GetAssemblyName(this Assembly assembly) => $"{assembly.FullName} {assembly.Location}";
#if !CSHARP_90
		private static ReadOnlySpan<Byte> NullBytes()
		{
			Byte[] utf8 = { (Byte)'N', (Byte)'u', (Byte)'l', (Byte)'l', (Byte)'\0', };
			return utf8.AsSpan()[..^1];
		}
#endif
	}
}