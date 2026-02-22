namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	public abstract class UnixCppCompiler : ICppCompiler
	{
		protected abstract String LocalRuntimePath { get; }
		public abstract String BeginWholeLink { get; }
		public virtual String EndWholeLink => String.Empty;
		public virtual String ExportDynamicSymbols => String.Empty;

		public IEnumerable<String> LibraryPaths => [];
		public String CompilerExecutable => "cc";
		public String EnableAllWarnings => "-Wall";
		public String IncludeFlag => "-I";
		public String OutputFlag => "-o";
		public String StaticLibPathFlag => String.Empty;
		public String DynamicRuntime => "";
		public IEnumerable<String> DefaultLink
		{
			get
			{
				yield return "-lz";
				yield return "-lstdc++";
				yield return "-ldl";
				yield return "-lpthread";
				yield return "-lm";
				foreach (String link in this.AdditionalLink())
					yield return link;
			}
		}
		public String RuntimePath => $"-Wl,-rpath,{this.LocalRuntimePath}";

		public virtual IEnumerable<String> BeginLink(Boolean _) => [];
		public async Task<String[]> GetPkgConfigArgs(String packageName, String? pkgPath)
		{
			if (OperatingSystem.IsWindows()) return [];
			ExecuteState<PkgConfigState> state = new()
			{
				ExecutablePath = "pkg-config",
				ArgState = new() { Package = packageName, Path = pkgPath, },
				AppendEnvs = (s, e) =>
				{
					if (String.IsNullOrWhiteSpace(s.Path)) return;
					e["PKG_CONFIG_PATH"] = s.Path;
				},
				AppendArgs = (s, a) =>
				{
					a.Add("--cflags");
					if (!String.IsNullOrWhiteSpace(s.Package))
						a.Add(s.Package);
				},
				Notifier = ConsoleNotifier.Notifier,
			};
			String value = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
			return String.IsNullOrWhiteSpace(value) ? [] : UnixCppCompiler.ParseMonoFlags(value);
		}

		async Task<(String name, String value)[]> ICppCompiler.GetEnv()
		{
			await Task.Yield();
			return [];
		}

		protected abstract IEnumerable<String> AdditionalLink();

		private static String[] ParseMonoFlags(String value)
		{
			Collection<String> result = [];
			Int32 start = 0;
			while (value.AsSpan()[start..].IndexOf([' ', '-',]) is var end and > 0)
			{
				result.AddArg(value.AsSpan().Slice(start, end).ToString());
				start += end;
			}
			result.AddArg(value.AsSpan()[start..].ToString());
			return result.ToArray();
		}

		private readonly struct PkgConfigState
		{
			public String Package { get; init; }
			public String? Path { get; init; }
		}
	}
}