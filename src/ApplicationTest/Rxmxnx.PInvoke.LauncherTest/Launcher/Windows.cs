using System.Security.Cryptography;

namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed partial class Windows : Launcher, ILauncher<Windows>
	{
		public static OSPlatform Platform => OSPlatform.Windows;

		private readonly List<MonoLauncher>? _monoLaunchers;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "win";
		public override ReadOnlySpan<MonoLauncher> MonoLaunchers => CollectionsMarshal.AsSpan(this._monoLaunchers);

		private Windows(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X86 ||
				                                (a is Architecture.X64 && this.CurrentArch is not Architecture.X86))
			                         .ToArray();
			initialize = Task.CompletedTask;
			Windows.AppendMonoLauncher(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono"),
				Architecture.X64, ref this._monoLaunchers);
			if (this.CurrentArch is not Architecture.X86)
				Windows.AppendMonoLauncher(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Mono"),
					Architecture.X86, ref this._monoLaunchers);
		}
		[LibraryImport("Rxmxnx.PInvoke.Mkbundle.Patcher", StringMarshalling = StringMarshalling.Utf16)]
		private static partial SByte PatchAssembly(String monoLibPath, Int32 monoLibPathLength, String outputPath,
			Int32 outputPathLength);

		private static void AppendMonoLauncher(String monoPath, Architecture arch,
			ref List<MonoLauncher>? monoLaunchers)
		{
			String monoBinPath = Path.Combine(monoPath, "bin");
			String monoLibPath = Path.Combine(monoPath, "lib", "mono", "4.5");
			String executablePath = Path.Combine(monoPath, "mono.exe");
			if (!File.Exists(executablePath))
				return;
			monoLaunchers ??= new(2);
			String monoPatchPath = Windows.GetMkbundlePath(monoBinPath);
			if (Windows.PatchAssembly(monoLibPath, monoLibPath.Length, monoPatchPath, monoPatchPath.Length) < 0)
				throw new InvalidOperationException("Unable to patch mono installation.");
			String mkbundleAssemblyPath = Path.Combine(monoPatchPath, "mkbundle.exe");
			String mkbundleBatchPath = Path.Combine(monoPatchPath, "mkbundle.bat");

			File.WriteAllText(mkbundleBatchPath, $@"@echo off
""{executablePath}"" %MONO_OPTIONS% ""{mkbundleAssemblyPath}"" %*
", Encoding.ASCII);
			monoLaunchers.Add(new()
			{
				Architecture = arch,
				MsbuildPath = Path.Combine(monoBinPath, "msbuild.bat"),
				LinkerPath = Path.Combine(monoBinPath, "monolinker.bat"),
				MakerPath = mkbundleBatchPath,
				GarbageCollectorPath = Path.Combine(monoBinPath, "mono-2.0-sgen.dll"),
				ExecutablePath = executablePath,
			});
		}

		private static String GetMkbundlePath(String monoPath)
		{
			String hexHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(monoPath)));
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), hexHash);
		}

		public static Windows Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);
	}
}