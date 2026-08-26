namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
	private readonly struct CompileFrameworkArgs
	{
		public String ProjectFile { get; init; }
		public String PlatformTarget { get; init; }

		public static void Append(CompileFrameworkArgs compileArgs, Collection<String> args)
		{
			args.Add("build");
			args.Add(compileArgs.ProjectFile);
			args.Add("-c");
			args.Add("Release");
			args.Add("/p:UsePackage=true");
			args.Add("/p:FrameworkLegacyOnly=true");
			args.Add("/p:BuildInParallel=false");
			args.Add("-r");
			args.Add($"win-{compileArgs.PlatformTarget}");
		}
	}
}