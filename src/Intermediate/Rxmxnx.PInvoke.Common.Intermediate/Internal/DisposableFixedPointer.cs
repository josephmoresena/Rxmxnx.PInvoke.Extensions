namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for managing fixed memory pointer blocks in a disposable context.
/// </summary>
/// <typeparam name="TFixed">A <see cref="FixedPointer"/> type.</typeparam>
internal abstract record DisposableFixedPointer<TFixed> : IWrapper<TFixed>, IDisposable where TFixed : FixedPointer
{
	/// <inheritdoc cref="DisposableFixedPointer{TFixed}.Disposable"/>
	private readonly IDisposable? _disposable;
	/// <inheritdoc cref="DisposableFixedPointer{TFixed}.Value"/>
	private readonly TFixed _fixed;

	/// <inheritdoc/>
	public TFixed Value => this._fixed;
	/// <summary>
	/// Internal <see cref="IDisposable"/> instance.
	/// </summary>
	public IDisposable? Disposable => this._disposable;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="fixedPointer">Current <typeparamref name="TFixed"/> instance.</param>
	/// <param name="disposable"><see cref="IDisposable"/> instance.</param>
	protected DisposableFixedPointer(TFixed fixedPointer, IDisposable? disposable)
	{
		this._fixed = fixedPointer;
		this._disposable = disposable;
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		if (!this._fixed.IsValid) return;
		this._fixed.Unload();
		this._disposable?.Dispose();
	}
}