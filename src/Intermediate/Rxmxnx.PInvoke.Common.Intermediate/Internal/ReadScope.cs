namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a <see cref="ReaderWriterLockSlim"/> read scope.
/// </summary>
internal struct ReadScope : IDisposable
{
	/// <summary>
	/// Internal <see cref="ReaderWriterLockSlim"/> instance.
	/// </summary>
	private readonly ReaderWriterLockSlim? _rwLock;
	/// <summary>
	/// Indicates whether the current scope is active.
	/// </summary>
	private Boolean _active;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="rwLock">A <see cref="ReaderWriterLockSlim"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private ReadScope(ReaderWriterLockSlim rwLock)
	{
		this._rwLock = rwLock;
		this._rwLock.EnterReadLock();
		this._active = true;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose()
	{
		if (!this._active || this._rwLock is not { } rwLock) return;
		rwLock.ExitReadLock();
		this._active = false;
	}

	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ReaderWriterLockSlim"/> to a <see cref="ReadScope"/>.
	/// </summary>
	/// <param name="value">A <see cref="ReaderWriterLockSlim"/> to implicitly convert.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator ReadScope(ReaderWriterLockSlim value) => new(value);
}