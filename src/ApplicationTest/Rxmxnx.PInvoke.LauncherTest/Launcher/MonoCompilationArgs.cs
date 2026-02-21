namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoCompilationArgs
	{
		public Architecture Architecture { get; init; }
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

		public static void Compile(MonoCompilationArgs compilationArgs, Collection<String> args)
		{
			ICppCompiler compiler = compilationArgs.Compiler;

			args.AddArg(compiler.DynamicRuntime);
			args.AddArg(compiler.EnableAllWarnings);
			args.AddArg(compiler.RemovePointerWarnings);
			foreach (String additionalArg in compilationArgs.MonoFlags)
				args.AddArg(additionalArg);
			args.AddIncludeArg(compiler.IncludeFlag, compilationArgs.MonoIncludePath);
			args.AddIncludeArg(compiler.IncludeFlag, compilationArgs.GetZLibInclude());
			args.AddArg(compilationArgs.SourceFile);
			args.AddArgs(compiler.BeginLink(compilationArgs.WindowApp));
			args.AddWholeLibArg(compiler.BeginWholeLink, compiler.EndWholeLink, compilationArgs.StaticRuntimePath,
			                    out Boolean linked);
			args.AddArg(compilationArgs.ObjectFile);
			foreach (String aotFile in compilationArgs.AotFiles)
				args.AddArg(aotFile);
#if ZLINK_STATIC
			args.AddArg(compilationArgs.GetStaticZLibPath());
#endif
			if (!linked)
				args.AddArg(compilationArgs.StaticRuntimePath);
			foreach (String libPath in compiler.LibraryPaths)
				args.AddLibPathArg(compiler.StaticLibPathFlag, libPath);
			args.AddArgs(compiler.DefaultLink);
			args.AddOutputArg(compiler.OutputFlag, compilationArgs.BinaryOutputPath);
		}

		private String? GetZLibInclude() => this.ZLibPath is null ? default : Path.Combine(this.ZLibPath, "include");
#if ZLINK_STATIC
		private String? GetStaticZLibPath()
			=> this.ZLibPath is null ? default : Path.Combine(this.ZLibPath, $"{this.Architecture}", "zlibstat.lib");
#endif
	}
}