using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if NET5_0_OR_GREATER
	[UnconditionalSuppressMessage("SingleFile", "IL3000")]
#endif
	public static class RuntimeHelper
	{
		private const String runtimeName =
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
                    "Mono"
#endif
#if REFLECTION_FREE
                    + " Reflection-free"
#endif
#if NATIVE_AOT
                    + " NativeAOT"
#endif
			;

		public static readonly Random Shared = new();
		public static readonly CString Null = new(static () =>
		{
			Byte[] utf8 = { (Byte)'N', (Byte)'u', (Byte)'l', (Byte)'l', (Byte)'\0', };
			return utf8.AsSpan()[..^1];
		});

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
			catch (Exception)
			{
				Console.WriteLine("**Unable to retrieve runtime info**");
			}
			Console.WriteLine("========== Rxmxnx.PInvoke Runtime information ==========");
			Console.WriteLine($"Native AOT: {AotInfo.IsNativeAot}");
			Console.WriteLine($"Reflection Enabled: {!AotInfo.IsReflectionDisabled}");
			Console.WriteLine($"Mono Runtime: {AotInfo.IsMono}");
			Console.WriteLine($"Pointer Size: {NativeUtilities.PointerSize}");
			Console.WriteLine($"Globalization-Invariant Mode: {NativeUtilities.GlobalizationInvariantModeEnabled}");
			Console.WriteLine($"UI Iso639-1: {NativeUtilities.UserInterfaceIso639P1}");
			Console.WriteLine($"Buffer AutoComposition Enabled: {BufferManager.BufferAutoCompositionEnabled}");
			Console.WriteLine($"String constant: {RuntimeHelper.runtimeName.AsSpan().IsLiteral()}");
			Console.WriteLine($"CString.Empty literal: {RuntimeHelper.GetEmptyUtf8().IsLiteral()}");
		}
		private static void PrintDomainInfo()
		{
			try
			{
				ReadOnlySpan<Assembly> span = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in span)
				{
					if (assembly == Assembly.GetExecutingAssembly()) continue;
					Console.WriteLine(AotInfo.IsNativeAot ? assembly.FullName : assembly.GetAssemblyName());
				}
			}
			catch (Exception)
			{
				Console.WriteLine("**Unable to retrieve domain info**");
			}
		}
		private static ReadOnlySpan<Byte> GetEmptyUtf8()
		{
			ref readonly Byte ref0 = ref CString.Empty.GetPinnableReference();
			return NativeUtilities.AsBytes(in ref0);
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
				_ => architecture.ToString(),
			};
		private static String GetAssemblyName(this Assembly assembly) => $"{assembly.FullName} {assembly.Location}";
	}
}