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
			Mac.AppendMonoLauncher("/Library/Frameworks/Mono.framework/Versions/Current", Architecture.X64,
			                       ref this._monoLaunchers);
			if (this.CurrentArch is Architecture.Arm64)
				Mac.AppendMonoLauncher("/opt/homebrew/Cellar/mono/6.14.1/", Architecture.Arm64,
				                       ref this._monoLaunchers);
		}

		public override ICppCompiler GetCompiler(Architecture arch) => new XCppCompiler(arch);

		private static void AppendMonoLauncher(String monoPath, Architecture arch,
			ref List<MonoLauncher>? monoLaunchers)
		{
			String monoBinPath = Path.Combine(monoPath, "bin");
			String executablePath = Path.Combine(monoBinPath, "mono");
			if (!File.Exists(executablePath)) return;
			String monoLibPath = Path.Combine(monoPath, "lib");
			String monoRuntimePath = Path.Combine(monoLibPath, "mono", "4.5");
			String libSystemNativePath = Path.Combine(monoLibPath, "libmono-native-compat.dylib");
			if (!File.Exists(libSystemNativePath))
				libSystemNativePath = Path.Combine(monoLibPath, "libmono-native.dylib");
			monoLaunchers ??= new(2);
			monoLaunchers.Add(new()
			{
				Architecture = arch,
				NativeRuntimeName = "libSystem.Native.dylib",
				MsbuildPath = Path.Combine(monoBinPath, "msbuild"),
				LinkerPath = Path.Combine(monoBinPath, "monolinker"),
				MakerPath = Path.Combine(monoBinPath, "mkbundle"),
				MonoCilStripAssemblyPath = Path.Combine(monoRuntimePath, "mono-cil-strip.exe"),
				NativeRuntimePath = libSystemNativePath,
				ExecutablePath = executablePath,
				IncludeRuntimePath = Path.Combine(monoPath, "include"),
				StaticRuntimePath = Path.Combine(monoLibPath, "libmonosgen-2.0.a"),
				PkgConfigPath = Path.Combine(monoLibPath, "pkgconfig"),
			});
		}

		public static Mac Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);

		private sealed class XCppCompiler(Architecture arch) : UnixCppCompiler
		{
			public override String BeginWholeLink => "-Wl,-force_load,";
			protected override String LocalRuntimePath => "@loader_path";

			public override IEnumerable<String> BeginLink(Boolean _)
			{
				yield return "-arch";
				yield return arch switch
				{
					Architecture.X86 => "x86",
					Architecture.X64 => "x86_64",
					Architecture.Arm64 => "arm64",
					_ => arch.ToString().ToLowerInvariant(),
				};
			}
			protected override IEnumerable<String> AdditionalLink()
			{
				yield return "-lobjc";
				yield return "-liconv";
				yield return "-framework";
				yield return "Foundation";
			}
		}
	}
}