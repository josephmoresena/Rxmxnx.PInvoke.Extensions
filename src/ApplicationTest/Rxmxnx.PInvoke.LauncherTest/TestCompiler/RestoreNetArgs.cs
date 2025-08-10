namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class TestCompiler
{
	private readonly struct RestoreNetArgs
	{
		public String ProjectFile { get; init; }
		public String RuntimeIdentifier { get; init; }
		public NetVersion Version { get; init; }

		public static void Append(RestoreNetArgs restoreArgs, Collection<String> args)
		{
			args.Add("restore");
			args.Add(restoreArgs.ProjectFile);
			if (!String.IsNullOrEmpty(restoreArgs.RuntimeIdentifier))
			{
				args.Add("-r");
				args.Add(restoreArgs.RuntimeIdentifier);
			}
			args.Add("/p:UsePackage=true");
			args.Add($"/p:RequiredFramework={restoreArgs.Version.GetTargetFramework()}");
			args.Add($"/p:TargetFramework={restoreArgs.Version.GetTargetFramework()}");
		}
	}
}