namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoBundleArgs
	{
		public String MonoExecutablePath { get; init; }
		public String AssemblyPathName { get; init; }
		public String StripAssemblyPath { get; init; }
		public String OutputBinaryPath { get; init; }
		public Boolean UseLlvm { get; init; }

		public static void Make(MonoBundleArgs bundleArgs, Collection<String> args)
		{
			args.Add(bundleArgs.AssemblyPathName);
			args.Add("-L");
			args.Add(new FileInfo(bundleArgs.AssemblyPathName).DirectoryName ?? ".");
			args.Add("--deps");
			args.Add("--static");
			args.Add("--aot-mode");
			args.Add(!bundleArgs.UseLlvm ? "full" : "llvmonly");
			args.Add("--runtime");
			args.Add(bundleArgs.MonoExecutablePath);
			args.Add("--cil-strip");
			args.Add(bundleArgs.StripAssemblyPath);
			args.Add("-o");
			args.Add(bundleArgs.OutputBinaryPath);
			args.Add("-v");
		}
	}
}