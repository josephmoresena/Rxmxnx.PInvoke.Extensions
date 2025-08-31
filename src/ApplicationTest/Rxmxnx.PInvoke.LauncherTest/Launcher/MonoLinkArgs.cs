namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private readonly struct MonoLinkArgs
	{
		public String AssemblyPathName { get; init; }
		public Boolean Hybrid { get; init; }

		public static void Link(MonoLinkArgs linkArgs, Collection<String> args)
		{
			args.Add(!linkArgs.Hybrid ? "--aot=full,nodebug" : "--aot=hybrid,nodebug");
			args.Add("--verbose");
			args.Add("-O=all");
			args.Add(linkArgs.AssemblyPathName);
		}
	}
}