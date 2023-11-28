namespace Rxmxnx.PInvoke.Tests;

internal sealed record TestMemoryHandle : IDisposable
{
	private readonly List<MemoryHandle> _handles = new();
	private Boolean _disposed;
	public void Dispose()
	{
		if (this._disposed) return;
		this._disposed = true;
		this._handles.ForEach(h => h.Dispose());
	}

	public void Add(MemoryHandle handle) => this._handles.Add(handle);
}