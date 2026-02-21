namespace Rxmxnx.PInvoke.ApplicationTest;

public partial class Launcher
{
	private sealed class MonoBundleSource(
		String bundlePath,
		String sourceFile,
		String objectFile,
		String assembliesPath) : IDisposable
	{
		private readonly DirectoryInfo _assembliesDirectory = new(Path.Combine(bundlePath, assembliesPath));
		private Boolean _disposed;

		public FileInfo SourceFile { get; } = new(Path.Combine(bundlePath, sourceFile));
		public FileInfo ObjectFile { get; } = new(Path.Combine(bundlePath, objectFile));
		public FileInfo[] AotFiles { get; } =
			new DirectoryInfo(Path.Combine(bundlePath)).GetFiles("*.aot_out", SearchOption.AllDirectories);
		public Boolean Exists
			=> !this._disposed && this.SourceFile.Exists && this.ObjectFile.Exists && this.AotFiles.Length > 0;

		public void Dispose()
		{
			if (this._disposed) return;
			this._disposed = true;

			foreach (FileInfo aotFile in this.AotFiles)
				aotFile.Delete();
			if (this._assembliesDirectory.Exists)
				this._assembliesDirectory.Delete();
			if (this.ObjectFile.Exists)
				this.ObjectFile.Delete();
			if (this.SourceFile.Exists)
				this.SourceFile.Delete();
		}
	}
}