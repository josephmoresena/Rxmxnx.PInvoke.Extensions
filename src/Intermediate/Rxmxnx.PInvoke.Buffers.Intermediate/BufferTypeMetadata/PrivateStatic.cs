namespace Rxmxnx.PInvoke;

public partial class BufferTypeMetadata
{
	/// <summary>
	/// Internal <see cref="ReaderWriterLockSlim"/> instance.
	/// </summary>
	private static readonly ReaderWriterLockSlim rwLock = new();
	/// <summary>
	/// Internal composition error.
	/// </summary>
	private static readonly HashSet<Composition> errors = [];
}