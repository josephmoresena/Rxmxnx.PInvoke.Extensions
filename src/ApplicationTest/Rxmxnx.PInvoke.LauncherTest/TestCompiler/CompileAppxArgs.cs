namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	private readonly struct CompileAppxArgs
	{
		public String ProjectPath { get; init; }
		public String OutputPath { get; init; }

		public static void Append(CompileAppxArgs appxArgs, Collection<String> args)
		{
			args.Add("-restore");
			args.Add(appxArgs.ProjectPath);
			args.Add("/p:Configuration=Release");
			args.Add("/p:UsePackage=true");
			args.Add("/p:AppxBundle=Always");
			args.Add("/p:AppxBundlePlatforms=\"x86|x64|arm64\"");
			args.Add("/p:GenerateAppxPackageOnBuild=true");
			args.Add($"/p:AppxPackageDir=\"{appxArgs.OutputPath}\"");
		}
	}
}