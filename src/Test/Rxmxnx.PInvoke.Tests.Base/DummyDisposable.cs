namespace Rxmxnx.PInvoke.Tests;

[ExcludeFromCodeCoverage]
public sealed record DummyDisposable : IDisposable
{
	public Boolean IsDisposed { get; private set; }

	public void Dispose() => this.IsDisposed = true;
}