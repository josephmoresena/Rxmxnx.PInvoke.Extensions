namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoAotArgs
	{
		public String AssemblyPathName { get; init; }
		public Boolean Hybrid { get; init; }
		public String OutputFile { get; init; }

		public static void Link(MonoAotArgs aotArgs, Collection<String> args)
		{
			args.Add(!aotArgs.Hybrid ?
				         $"--aot=full,nodebug,asmonly,outfile={aotArgs.OutputFile}" :
				         $"--aot=hybrid,nodebug,asmonly,outfile={aotArgs.OutputFile}");
			args.Add("--verbose");
			args.Add("-O=all");
			args.Add(aotArgs.AssemblyPathName);
		}
	}
}