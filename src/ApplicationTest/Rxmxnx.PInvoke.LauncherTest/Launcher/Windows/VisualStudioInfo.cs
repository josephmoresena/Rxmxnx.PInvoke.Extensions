namespace Rxmxnx.PInvoke.ApplicationTest;

// ReSharper disable once ClassCannotBeInstantiated
public partial class Launcher
{
	private sealed partial class Windows
	{
		private struct VisualStudioInfo
		{
			public String VcBuildPath { get; init; }
			public String MsVcPath { get; init; }
			public String MsBuildPath { get; init; }
		}
	}
}