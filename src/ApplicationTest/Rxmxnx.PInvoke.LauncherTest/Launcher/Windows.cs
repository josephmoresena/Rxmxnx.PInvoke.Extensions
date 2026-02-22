namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	[SupportedOSPlatform("WINDOWS")]
	private sealed partial class Windows : Launcher, ILauncher<Windows>
	{
		public static OSPlatform Platform => OSPlatform.Windows;
		private readonly Dictionary<Architecture, CppCompiler> _cppCompilers = new();
		private readonly List<MonoLauncher>? _monoLaunchers;
		public override Architecture[] Architectures { get; }
		public override String RuntimeIdentifierPrefix => "win";
		public override ReadOnlySpan<MonoLauncher> MonoLaunchers => CollectionsMarshal.AsSpan(this._monoLaunchers);

		[SuppressMessage("ReSharper", "HeapView.DelegateAllocation")]
		private Windows(DirectoryInfo outputDirectory, Boolean useMono, out Task initialize) : base(
			outputDirectory, useMono)
		{
			this.Architectures = Enum.GetValues<Architecture>()
			                         .Where(a => a == this.CurrentArch || a is Architecture.X86 ||
				                                (a is Architecture.X64 && this.CurrentArch is not Architecture.X86))
			                         .ToArray();
			initialize = Windows.PrepareCompilers(this._cppCompilers, this.Architectures);
			Windows.AppendMonoLauncher(
				Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Mono"),
				Architecture.X64, ref this._monoLaunchers);
			if (this.CurrentArch is not Architecture.X86)
				Windows.AppendMonoLauncher(
					Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Mono"),
					Architecture.X86, ref this._monoLaunchers);
		}

		public override ICppCompiler? GetCompiler(Architecture arch) => this._cppCompilers.GetValueOrDefault(arch);

		public static Windows Create(DirectoryInfo outputDirectory, Boolean useMono, out Task initTask)
			=> new(outputDirectory, useMono, out initTask);
	}
}