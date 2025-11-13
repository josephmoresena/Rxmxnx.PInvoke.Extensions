namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoAotArgs
	{
		public String AssemblyPathName { get; init; }
		public Boolean Hybrid { get; init; }

		public static void Link(MonoAotArgs aotArgs, Collection<String> args)
		{
			String outFile = Path.GetTempFileName();

			args.Add(!aotArgs.Hybrid ?
				         $"--aot=full,nodebug,outfile={outFile}" :
				         $"--aot=hybrid,nodebug,outfile={outFile}");
			args.Add("--verbose");
			args.Add("-O=all");
			args.Add(aotArgs.AssemblyPathName);
		}
	}
}