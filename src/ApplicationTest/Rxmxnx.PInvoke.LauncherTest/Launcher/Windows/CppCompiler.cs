namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed partial class Windows
	{
		private sealed class CppCompiler(String vcBuildPath, String msvcPath, String kitLibPath, Architecture arch)
			: ICppCompiler
		{
			public IEnumerable<String> LibraryPaths
			{
				get
				{
					yield return Path.Combine(kitLibPath, "um", $"{arch}");
					yield return Path.Combine(kitLibPath, "ucrt", $"{arch}");
					yield return Path.Combine(msvcPath, "lib", $"{arch}");
				}
			}
			public String CompilerExecutable => Path.Combine(msvcPath, "bin", $"Host{arch}", $"{arch}", "cl.exe");
			public String DynamicRuntime => "/MD";
			public String IncludeFlag => "/I";
			public String OutputFlag => "/OUT:";
			public String StaticLibPathFlag => "/LIBPATH:";
			public IEnumerable<String> DefaultLink
			{
				get
				{
					yield return "kernel32.lib";
					yield return "user32.lib";
					yield return "advapi32.lib";
					yield return "shell32.lib";
					yield return "ole32.lib";
					yield return "oleaut32.lib";
					yield return "version.lib";
					yield return "ws2_32.lib";
					yield return "mswsock.lib";
					yield return "psapi.lib";
					yield return "winmm.lib";
					yield return "bcrypt.lib";
				}
			}
			public String EnableAllWarnings => String.Empty;
			public String BeginWholeLink => String.Empty;
			public String EndWholeLink => String.Empty;
			public String ExportDynamicSymbols => String.Empty;
			public String RuntimePath => String.Empty;
			public IEnumerable<String> BeginLink(Boolean windowApp)
			{
				yield return "/link";
				yield return $"/MACHINE:{arch}";
				yield return $"/SUBSYSTEM:{(windowApp ? "WINDOWS" : "CONSOLE")}";
			}
			public async Task<(String name, String value)[]> GetEnv()
			{
				String vcvarsallPath = Path.GetFullPath(Path.Combine(vcBuildPath, "vcvarsall.bat"));
				ProcessStartInfo info = new("cmd.exe")
				{
					Arguments = $"/c \"\"{vcvarsallPath}\" {arch} && set\"",
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
				};
				using Process prog = Process.Start(info)!;
				String envResult = await Utilities.ReadOutput(prog, ConsoleNotifier.CancellationToken);
				MatchCollection matches = Windows.VsEnvRegex().Matches(envResult);

				if (matches.Count == 0) return [];

				(String name, String value)[] result = new (String name, String value)[matches.Count];
#if NET10_0_OR_GREATER
				using Span<(String name, String value)>.Enumerator enumerable = result.AsSpan().GetEnumerator();
#else
				Span<(String name, String value)>.Enumerator enumerable = result.AsSpan().GetEnumerator();
#endif
				foreach (Match match in matches)
				{
					enumerable.MoveNext();
					enumerable.Current = (match.Groups[1].Value, match.Groups[2].Value);
				}
				return result;
			}

			async Task<String[]> ICppCompiler.GetPkgConfigArgs(String packageName, String? pkgPath)
			{
				await Task.Yield();
				return [];
			}
		}
	}
}