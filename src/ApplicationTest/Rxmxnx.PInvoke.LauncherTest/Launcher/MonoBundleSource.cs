namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed class MonoBundleSource(
		String bundlePath,
		String sourceFile,
		String objectFile,
		String assembliesPath) : IDisposable
	{
		private readonly FileInfo _asmFile = new(Path.Combine(bundlePath, "temp.s"));
		private readonly DirectoryInfo _assembliesDirectory = new(Path.Combine(bundlePath, assembliesPath));
		private Boolean _disposed;

		public FileInfo SourceFile { get; } = new(Path.Combine(bundlePath, sourceFile));
		public FileInfo ObjectFile { get; } = new(Path.Combine(bundlePath, objectFile));
		public FileInfo[] AotFiles { get; } = MonoBundleSource.GetAotFiles(bundlePath);
		public Boolean Exists
			=> !this._disposed && this.SourceFile.Exists && this.ObjectFile.Exists && this.AotFiles.Length > 0;

		public void Dispose()
		{
			if (this._disposed) return;
			this._disposed = true;

			MonoBundleSource.DeleteAotFiles(this.AotFiles);
			if (this._assembliesDirectory.Exists)
				this._assembliesDirectory.Delete(true);
			if (this.ObjectFile.Exists)
				this.ObjectFile.Delete();
			if (this._asmFile.Exists)
				this._asmFile.Delete();
			if (this.SourceFile.Exists)
				this.SourceFile.Delete();
		}

		public static void DeleteAotFiles(String bundlePath)
			=> MonoBundleSource.DeleteAotFiles(MonoBundleSource.GetAotFiles(bundlePath));

		private static FileInfo[] GetAotFiles(String bundlePath)
			=> new DirectoryInfo(Path.Combine(bundlePath)).GetFiles("*.aot_out", SearchOption.AllDirectories);
		private static void DeleteAotFiles(ReadOnlySpan<FileInfo> aotFiles)
		{
			foreach (FileInfo aotFile in aotFiles)
				aotFile.Delete();
		}
	}
}