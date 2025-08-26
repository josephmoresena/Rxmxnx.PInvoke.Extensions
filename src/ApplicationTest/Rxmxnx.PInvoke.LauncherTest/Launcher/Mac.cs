namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed class Mac : Launcher, ILauncher<Mac>
	{
		public static OSPlatform Platform => OSPlatform.OSX;
		private readonly List<MonoLauncher>? _monoLaunchers;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "osx";
		public override ReadOnlySpan<MonoLauncher> MonoLaunchers => CollectionsMarshal.AsSpan(this._monoLaunchers);

		private Mac(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X64).ToArray();
			initialize = Task.CompletedTask;
			Mac.AppendMonoLauncher("/Library/Frameworks/Mono.framework/Versions/Current/bin", Architecture.X64,
			                       ref this._monoLaunchers);
			if (this.CurrentArch is Architecture.Arm64)
				Mac.AppendMonoLauncher("/opt/homebrew/bin", Architecture.Arm64, ref this._monoLaunchers);
		}

		private static void AppendMonoLauncher(String binPath, Architecture arch, ref List<MonoLauncher>? monoLaunchers)
		{
			String executablePath = Path.Combine(binPath, "mono");
			if (!File.Exists(executablePath))
				return;
			monoLaunchers ??= new(2);
			monoLaunchers.Add(new()
			{
				Architecture = arch,
				MsbuildPath = Path.Combine(binPath, "msbuild"),
				LinkerPath = Path.Combine(binPath, "monolinker"),
				MakerPath = Path.Combine(binPath, "mkbundle"),
				ExecutablePath = executablePath,
			});
		}

		public static Mac Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);
	}
}