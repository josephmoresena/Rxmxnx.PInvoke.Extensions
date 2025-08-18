namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	private struct CompileNetArgs(RestoreNetArgs args, String outputPath)
	{
		private readonly RestoreNetArgs _args = args;
		private readonly String _outputPath = outputPath;

		public Publish Publish { get; set; }
		public Boolean BuildDependencies { get; set; }

		public static void Append(CompileNetArgs compileArgs, Collection<String> args)
		{
			args.Add("publish");
			args.Add(compileArgs._args.ProjectFile);
			args.Add("-c");
			args.Add("Release");
			if (!String.IsNullOrEmpty(compileArgs._args.RuntimeIdentifier))
			{
				args.Add("-r");
				args.Add(compileArgs._args.RuntimeIdentifier);
			}
			args.Add("/p:UsePackage=true");
			args.Add($"/p:RequiredFramework={compileArgs._args.Version.GetTargetFramework()}");
			args.Add("/p:RestorePackages=false");
			args.Add($"/p:BuildProjectReferences={compileArgs.BuildDependencies}");
			args.Add($"/p:TargetFramework={compileArgs._args.Version.GetTargetFramework()}");
			args.Add($"/p:PublishReadyToRun={compileArgs.Publish.HasFlag(Publish.ReadyToRun)}");
			args.Add($"/p:PublishAot={compileArgs.Publish.HasFlag(Publish.NativeAot)}");
			args.Add($"/p:IlcDisableReflection={compileArgs.Publish.HasFlag(Publish.NoReflection)}");
			args.Add($"/p:InvariantGlobalization={compileArgs.Publish.HasFlag(Publish.InvariantGlobalization)}");
			args.Add(
				$"/p:DisableBufferAutoComposition={compileArgs.Publish.HasFlag(Publish.DisableBufferAutoComposition)}");
			args.Add($"/p:CopyTargetTo={compileArgs._outputPath}");
		}
	}
}