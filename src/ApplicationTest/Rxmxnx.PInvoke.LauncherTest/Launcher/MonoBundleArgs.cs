namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoBundleArgs
	{
		public Architecture Architecture { get; init; }
		public String MonoExecutablePath { get; init; }
		public String AssemblyPathName { get; init; }
		public String StripAssemblyPath { get; init; }
		public Boolean CustomBuild { get; init; }
		public String OutputPath { get; init; }
		public Boolean UseLlvm { get; init; }
		public String? OutputObjectPath { get; init; }

		public static void MakeEnv(MonoBundleArgs bundleArgs, StringDictionary env)
			=> env.Add("VSCMD_ARG_TGT_ARCH", $"{bundleArgs.Architecture}");

		public static void Make(MonoBundleArgs bundleArgs, Collection<String> args)
		{
			args.Add(bundleArgs.AssemblyPathName);
			args.Add("-L");
			args.Add(new FileInfo(bundleArgs.AssemblyPathName).DirectoryName ?? ".");
			if (bundleArgs.CustomBuild)
			{
				args.Add("-z");
				args.Add("--custom");
				args.Add("--keeptemp");
			}
			args.Add("--deps");
			args.Add("--static");
			args.Add("--aot-mode");
			args.Add(!bundleArgs.UseLlvm ? "full" : "llvmonly");
			args.Add("--aot-runtime");
			args.Add(bundleArgs.MonoExecutablePath);
			args.Add("--cil-strip");
			args.Add(bundleArgs.StripAssemblyPath);
			if (!String.IsNullOrWhiteSpace(bundleArgs.OutputObjectPath))
			{
				args.Add("-oo");
				args.Add(bundleArgs.OutputObjectPath);
			}
			args.Add("-o");
			args.Add(bundleArgs.OutputPath);
			args.Add("-v");
		}
	}
}