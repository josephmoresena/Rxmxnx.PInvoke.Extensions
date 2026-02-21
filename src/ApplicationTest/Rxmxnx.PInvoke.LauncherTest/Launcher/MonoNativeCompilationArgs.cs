namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoNativeCompilationArgs
	{
		public ICppCompiler Compiler { get; init; }
		public Boolean WindowApp { get; init; }
		public IEnumerable<String> AotFiles { get; init; }
		public String ObjectFile { get; init; }
		public String SourceFile { get; init; }
		public String? MonoIncludePath { get; init; }
		public String StaticRuntimePath { get; init; }
		public IEnumerable<String> MonoFlags { get; init; }
		public String? ZLibPath { get; init; }
		public String BinaryOutputPath { get; init; }
		public (String name, String value)[] Environment { get; init; }
#if ZLINK_STATIC
		public Architecture Architecture { get; init; }
#endif
		public static void CompileEnv(MonoNativeCompilationArgs nativeCompilationArgs, StringDictionary env)
		{
			foreach ((String name, String value) in nativeCompilationArgs.Environment)
			{
				switch (name)
				{
					case "INCLUDE":
					case "LIB":
					case "LIBPATH":
					case "EXTERNAL_INCLUDE":
					case "PATH":
						break;
					default:
						if (!name.StartsWith("VS") && !name.StartsWith("VC") && !name.StartsWith("Windows") &&
						    !name.Contains("SDK", StringComparison.InvariantCultureIgnoreCase))
							continue;
						break;
				}
				env[name] = value;
			}
		}
		public static void Compile(MonoNativeCompilationArgs nativeCompilationArgs, Collection<String> args)
		{
			ICppCompiler compiler = nativeCompilationArgs.Compiler;

			args.AddArg(compiler.DynamicRuntime);
			args.AddArg(compiler.EnableAllWarnings);
			args.AddArg(compiler.RemovePointerWarnings);
			foreach (String additionalArg in nativeCompilationArgs.MonoFlags)
				args.AddArg(additionalArg);
			args.AddIncludeArg(compiler.IncludeFlag, nativeCompilationArgs.MonoIncludePath);
			args.AddIncludeArg(compiler.IncludeFlag, nativeCompilationArgs.GetZLibInclude());
			args.AddArg(nativeCompilationArgs.SourceFile);
			args.AddArgs(compiler.BeginLink(nativeCompilationArgs.WindowApp));
			args.AddWholeLibArg(compiler.BeginWholeLink, compiler.EndWholeLink, nativeCompilationArgs.StaticRuntimePath,
			                    out Boolean linked);
			args.AddArg(nativeCompilationArgs.ObjectFile);
			foreach (String aotFile in nativeCompilationArgs.AotFiles)
				args.AddArg(aotFile);
#if ZLINK_STATIC
			args.AddArg(compilationArgs.GetStaticZLibPath());
#endif
			if (!linked)
				args.AddArg(nativeCompilationArgs.StaticRuntimePath);
			foreach (String libPath in compiler.LibraryPaths)
				args.AddLibPathArg(compiler.StaticLibPathFlag, libPath);
			args.AddArgs(compiler.DefaultLink);
			args.AddOutputArg(compiler.OutputFlag, nativeCompilationArgs.BinaryOutputPath);
		}

		private String? GetZLibInclude() => this.ZLibPath is null ? default : Path.Combine(this.ZLibPath, "include");
#if ZLINK_STATIC
		private String? GetStaticZLibPath()
			=> this.ZLibPath is null ? default : Path.Combine(this.ZLibPath, $"{this.Architecture}", "zlibstat.lib");
#endif
	}
}