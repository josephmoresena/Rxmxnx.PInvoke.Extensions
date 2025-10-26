namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoLinkArgs
	{
		public String ExecutableName { get; init; }
		public String OutputPath { get; init; }

		public static void Link(MonoLinkArgs monoLinkArgs, Collection<String> args)
		{
			args.Add("--deterministic");
			args.Add("--keep-facades");
			args.Add("--verbose");
			args.Add("-c");
			args.Add("link");
			args.Add("-a");
			args.Add(monoLinkArgs.ExecutableName);
			args.Add("-out");
			args.Add(monoLinkArgs.OutputPath);
		}
	}
}