namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed class FreeBsd : Launcher, ILauncher<FreeBsd>
	{
		private static readonly NetVersion[] netVersions =
		[
#if NET10_0_OR_GREATER
			NetVersion.Net100,
#elif NET9_0_OR_GREATER
			NetVersion.Net90,
#elif NET8_0_OR_GREATER
			NetVersion.Net80,
#elif NET7_0_OR_GREATER
			NetVersion.Net70,
#endif
		];
		private static readonly Architecture[] currentArch = [RuntimeInformation.OSArchitecture,];

		public static OSPlatform Platform => OSPlatform.FreeBSD;

		private readonly MonoLauncher[] _monoLaunchers;

		private String _runtimeIdentifier = "freebsd";
		public override String RuntimeIdentifierPrefix => this._runtimeIdentifier;
		public override Architecture[] Architectures => FreeBsd.currentArch;
		public override ReadOnlySpan<MonoLauncher> MonoLaunchers => this._monoLaunchers;
		public override NetVersion[] NetVersions => FreeBsd.netVersions;

		private FreeBsd(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this._monoLaunchers = new MonoLauncher[1];
			this._monoLaunchers[0] = new()
			{
				Architecture = this.CurrentArch,
				NativeRuntimeName = "libSystem.Native.so",
				MsbuildPath = "/usr/local/bin/xbuild",
				LinkerPath = "/usr/local/bin/monolinker",
				MakerPath = "/usr/local/bin/mkbundle",
				MonoCilStripAssemblyPath = "/usr/local/lib/mono/4.5/mono-cil-strip.exe",
				NativeRuntimePath = "/usr/local/lib/libmono-native.so",
				ExecutablePath = "/usr/local/bin/mono",
			};
			initialize = this.Initialize();
		}

		private async Task Initialize()
		{
			ExecuteState state = new()
			{
				ExecutablePath = "freebsd-version",
				AppendArgs = a => a.Add("-r"),
				Notifier = ConsoleNotifier.Notifier,
			};
			String freeBsdVersion = await Utilities.ExecuteWithOutput(state, ConsoleNotifier.CancellationToken);
			this._runtimeIdentifier += $".{freeBsdVersion[..2]}";
		}

		public static FreeBsd Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);
	}
}