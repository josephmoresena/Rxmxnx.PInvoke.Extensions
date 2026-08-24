namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	private readonly struct CompileMonoArgs
	{
		public String ProjectFile { get; init; }
		public String OutputPath { get; init; }
		public String? FrameworkPath { get; init; }

		public static void Append(CompileMonoArgs compileArgs, Collection<String> args)
		{
			args.Add("-restore");
			args.Add(compileArgs.ProjectFile);
			args.Add("/p:Configuration=Release");
			args.Add("/p:UsePackage=true");
			if (!String.IsNullOrWhiteSpace(compileArgs.FrameworkPath))
				args.Add($"/p:MonoFrameworkPath={compileArgs.FrameworkPath}");
			args.Add($"/p:OutDir={compileArgs.OutputPath}");
		}
	}
}