namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct LinkMonoArgs
	{
		public String ExecutableName { get; init; }
		public String OutputPath { get; init; }

		public static void Link(LinkMonoArgs linkArgs, Collection<String> args)
		{
			args.Add("--deterministic");
			args.Add("--keep-facades");
			args.Add("--verbose");
			args.Add("-c");
			args.Add("link");
			args.Add("-a");
			args.Add(linkArgs.ExecutableName);
			args.Add("-out");
			args.Add(linkArgs.OutputPath);
		}
	}
}