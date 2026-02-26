using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
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

#if !CSHARP9_0
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

		public static void PrintRuntimeInfo() => RuntimeHelper.PrintRuntimeInfo(Console.Out);
		public static void PrintRuntimeInfo(StringBuilder strBuilder)
		{
			using StringWriter writer = new(strBuilder);
			RuntimeHelper.PrintRuntimeInfo(writer);
		}

		private static void PrintRuntimeInfo(TextWriter writer)
		{
			writer.WriteLine("========== Application for " + RuntimeHelper.runtimeName + " ==========");
			RuntimeHelper.PrintDomainInfo(writer);
			writer.WriteLine("========== Runtime information ==========");
			writer.WriteLine($"Number of Cores: {Environment.ProcessorCount}");
			writer.WriteLine($"Is Little-Endian: {BitConverter.IsLittleEndian}");
			writer.WriteLine($"OS: {RuntimeInformation.OSDescription}");
			writer.WriteLine($"OS Arch: {RuntimeInformation.OSArchitecture.GetName()}");
			writer.WriteLine($"OS Version: {Environment.OSVersion}");
			writer.WriteLine($"Computer: {Environment.MachineName}");
			writer.WriteLine($"User: {Environment.UserName}");
			writer.WriteLine($"UI Culture: {CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}");
			writer.WriteLine($"System Path: {Environment.SystemDirectory}");
			writer.WriteLine($"Current Path: {Environment.CurrentDirectory}");
			writer.WriteLine($"Process Arch: {RuntimeInformation.ProcessArchitecture.GetName()}");
			try
			{
				writer.WriteLine($"Framework Version: {Environment.Version}");
				writer.WriteLine($"Runtime Path: {RuntimeEnvironment.GetRuntimeDirectory()}");
				writer.WriteLine($"Runtime Version: {RuntimeEnvironment.GetSystemVersion()}");
			}
			catch (Exception ex)
			{
				writer.WriteLine("**Unable to retrieve runtime info**");
				if (!AotInfo.IsReflectionDisabled)
					writer.WriteLine(ex);
			}
			writer.WriteLine($"Dynamic Code Compiled: {RuntimeFeature.IsDynamicCodeCompiled}");
			writer.WriteLine($"Dynamic Code Supported: {RuntimeFeature.IsDynamicCodeSupported}");
#if NET6_0_OR_GREATER
			writer.WriteLine($"IL compiled bytes: {JitInfo.GetCompiledILBytes()}");
			writer.WriteLine($"IL method count: {JitInfo.GetCompiledMethodCount()}");
			writer.WriteLine($"IL compilation time: {JitInfo.GetCompilationTime()}");
#endif
			writer.WriteLine("========== Rxmxnx.PInvoke Runtime information ==========");
#if !RELEASE_PACKAGE
			writer.WriteLine($"Package: {SystemInfo.CompilationFramework}");
#endif
			writer.WriteLine($"Native AOT: {AotInfo.IsNativeAot}");
			writer.WriteLine($"Reflection Enabled: {!AotInfo.IsReflectionDisabled}");
			writer.WriteLine($"IL Code Generation Supported: {AotInfo.IsCodeGenerationSupported}");
			writer.WriteLine($"Trimmed Runtime: {AotInfo.IsPlatformTrimmed}");
			writer.WriteLine($"Mono Runtime: {SystemInfo.IsMonoRuntime}");
			writer.WriteLine($"Web Runtime: {SystemInfo.IsWebRuntime}");
			writer.WriteLine($"Windows Platform: {SystemInfo.IsWindows}");
			writer.WriteLine($"Linux Platform: {SystemInfo.IsLinux}");
			writer.WriteLine($"macOS Platform: {SystemInfo.IsMac}");
			writer.WriteLine($"FreeBSD Platform: {SystemInfo.IsFreeBsd}");
			writer.WriteLine($"NetBSD Platform: {SystemInfo.IsNetBsd}");
			writer.WriteLine($"Solaris Platform: {SystemInfo.IsSolaris}");
			writer.WriteLine($"Pointer Size: {NativeUtilities.PointerSize}");
			writer.WriteLine($"Globalization-Invariant Mode: {NativeUtilities.GlobalizationInvariantModeEnabled}");
			writer.WriteLine($"UI Iso639-1: {NativeUtilities.UserInterfaceIso639P1}");
			writer.WriteLine($"Buffer AutoComposition Enabled: {BufferManager.BufferAutoCompositionEnabled}");
			if (!SystemInfo.IsWebRuntime)
			{
				writer.WriteLine($"String constant: {RuntimeHelper.runtimeName.AsSpan().IsLiteral()}");
				writer.WriteLine($"CString.Empty literal: {CString.IsImagePersistent(CString.Empty)}");
			}
			writer.WriteLine($"Hardcoded Array literal: {!RuntimeHelper.Null.AsSpan().MayBeNonLiteral()}");
		}
		private static void PrintDomainInfo(TextWriter writer)
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
					writer.WriteLine(useFullName ? assembly.FullName : assembly.GetAssemblyName());
				}
			}
			catch (Exception ex)
			{
				writer.WriteLine("**Unable to retrieve domain info**");
				if (!AotInfo.IsReflectionDisabled)
					writer.WriteLine(ex);
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
#if !CSHARP9_0
		private static ReadOnlySpan<Byte> NullBytes()
		{
			Byte[] utf8 = { (Byte)'N', (Byte)'u', (Byte)'l', (Byte)'l', (Byte)'\0', };
			return utf8.AsSpan()[..^1];
		}
#endif
	}
}