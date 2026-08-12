namespace Rxmxnx.PInvoke;

/// <summary>
/// Internal fixed pointer handle.
/// </summary>
/// <remarks>
/// The <see cref="FixedPointer"/> compatibility requires this instance implements <see cref="IWrapper{Boolean}"/>.
/// </remarks>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal unsafe class FixedValueHandle : IDisposable, IWrapper<Boolean>
{
	/// <summary>
	/// Empty instance.
	/// </summary>
	public static readonly IDisposable EmptyDisposable = new Empty();

	/// <summary>
	/// Internal <see cref="MemoryHandle"/> instance.
	/// </summary>
	private readonly MemoryHandle? _handle;
	/// <summary>
	/// Indicates whether the current instance is disposed.
	/// </summary>
	private Boolean _isDisposed;

	/// <summary>
	/// Internal pointer.
	/// </summary>
	protected void* Pointer => this._handle.HasValue ? this._handle.Value.Pointer : default;

	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	public FixedValueHandle() => this._isDisposed = false;
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="handle">A <see cref="MemoryHandle"/> instance.</param>
	public FixedValueHandle(MemoryHandle handle) : this() => this._handle = handle;

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <inheritdoc cref="IWrapper{T}.Value"/>
	public Boolean Value => !this._isDisposed;

	/// <summary>
	/// Destructor.
	/// </summary>
	~FixedValueHandle() => this.Dispose(false);

	/// <inheritdoc cref="IDisposable.Dispose()"/>
	/// <param name="disposing">
	/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only
	/// unmanaged resources.
	/// </param>
	protected virtual void Dispose(Boolean disposing)
	{
		if (this._isDisposed) return;
		this._isDisposed = true;
		this._handle?.Dispose();
	}

	/// <summary>
	/// Creates a <see cref="FixedValueHandle"/> using a <typeparamref name="TDisposable"/> instance.
	/// </summary>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	/// <param name="disposable">A <typeparamref name="TDisposable"/> instance.</param>
	/// <returns>A <see cref="FixedValueHandle"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FixedValueHandle CreateFromDisposable<TDisposable>(TDisposable disposable)
		where TDisposable : IDisposable
	{
		if (disposable is FixedValueHandle result) return result; // Avoid re-instantiation.  
		return new Generic<TDisposable>(disposable);
	}

	/// <summary>
	/// Internal fixed pointer handle.
	/// </summary>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	private sealed class Generic<TDisposable>(TDisposable disposable) : FixedValueHandle where TDisposable : IDisposable
	{
		/// <inheritdoc/>
		protected override void Dispose(Boolean disposing)
		{
			base.Dispose(disposing);
			if (disposing)
				disposable.Dispose();
		}
	}

	/// <summary>
	/// Empty disposable.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private sealed class Empty : IFixedPointer.IDisposable, IWrapper<Boolean>
	{
		/// <inheritdoc/>
		public IntPtr Pointer => default;

		/// <inheritdoc/>
		public void Dispose() { }
		/// <inheritdoc/>
		public Boolean Value => true;
	}
}