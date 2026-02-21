namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	public abstract class UnixCppCompiler(Architecture arch) : ICppCompiler
	{
		protected abstract String LocalRuntimePath { get; }
		protected abstract String WarningName { get; }
		public abstract String BeginWholeLink { get; }
		public virtual String EndWholeLink => String.Empty;
		public virtual String ExportDynamicSymbols => String.Empty;

		public IEnumerable<String> LibraryPaths => [];
		public String CompilerExecutable => "cpp";
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
		public String RemovePointerWarnings => $"-Wno-{this.WarningName}";

		public IEnumerable<String> BeginLink(Boolean _)
		{
			yield return "-arch";
			yield return arch switch
			{
				Architecture.X86 => "x86",
				Architecture.X64 => "x86_64",
				Architecture.Arm64 => "arm64",
				_ => arch.ToString().ToLowerInvariant(),
			};
		}

		protected abstract IEnumerable<String> AdditionalLink();
	}
}