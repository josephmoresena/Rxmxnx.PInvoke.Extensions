namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a <see cref="ReaderWriterLockSlim"/> write scope.
/// </summary>
internal struct WriteScope : IDisposable
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
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS7133)]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private WriteScope(ReaderWriterLockSlim rwLock)
	{
		this._rwLock = rwLock;
		this._rwLock.EnterWriteLock();
		this._active = true;
	}

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose()
	{
		if (!this._active || this._rwLock is not { } rwLock) return;
		rwLock.ExitWriteLock();
		this._active = false;
	}

	/// <summary>
	/// Defines an implicit conversion of a given <see cref="ReaderWriterLockSlim"/> to a <see cref="WriteScope"/>.
	/// </summary>
	/// <param name="value">A <see cref="ReaderWriterLockSlim"/> to implicitly convert.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator WriteScope(ReaderWriterLockSlim value) => new(value);
}