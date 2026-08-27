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
internal class FixedValueHandle : IDisposable, IWrapper<Boolean>
{
	/// <summary>
	/// Empty instance.
	/// </summary>
	public static readonly IDisposable EmptyDisposable = new Empty();

	/// <summary>
	/// Indicates whether the current instance is disposed.
	/// </summary>
	private Boolean? _isDisposed;

	/// <inheritdoc cref="IWrapper{T}.Value"/>
	public Boolean Value => !this._isDisposed.GetValueOrDefault();

	/// <summary>
	/// Constructor.
	/// </summary>
	public FixedValueHandle() : this(false) { }

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="isDisposed">Indicates whether the current instance is disposed.</param>
	private FixedValueHandle(Boolean? isDisposed) => this._isDisposed = isDisposed;

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Destructor.
	/// </summary>
	~FixedValueHandle() => this.Dispose(false);

	/// <inheritdoc cref="IDisposable.Dispose()"/>
	/// <param name="disposing">
	/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only
	/// unmanaged resources.
	/// </param>
	/// <returns>
	/// <see langword="true"/> when the disposing should be performed; otherwise, <see langword="false"/>.
	/// </returns>
	protected virtual Boolean Dispose(Boolean disposing)
	{
		if (!this._isDisposed.HasValue || this._isDisposed.Value) return false;
		this._isDisposed = true;
		return true;
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
		=> disposable switch
		{
			FixedValueHandle result => result,
			MemoryHandle handle => new Memory(handle),
			_ => new Generic<TDisposable>(disposable),
		};

	/// <summary>
	/// Internal fixed pointer handle.
	/// </summary>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private sealed class Generic<TDisposable>(TDisposable disposable) : FixedValueHandle where TDisposable : IDisposable
	{
		/// <inheritdoc/>
		protected override Boolean Dispose(Boolean disposing)
		{
			if (!base.Dispose(disposing) || !disposing) return false;
			disposable.Dispose();
			return true;
		}
	}

	/// <summary>
	/// Empty disposable.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private sealed class Empty() : FixedValueHandle(null), IFixedPointer.IDisposable
	{
		/// <inheritdoc/>
		public IntPtr Pointer => default;
	}

	/// <summary>
	/// An owned memory fixed value handle.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	public unsafe class Memory : FixedValueHandle
	{
		/// <summary>
		/// Internal <see cref="MemoryHandle"/> instance.
		/// </summary>
		private MemoryHandle _handle;

		/// <summary>
		/// Internal pointer.
		/// </summary>
		protected void* Pointer => this._handle.Pointer;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="handle">A <see cref="MemoryHandle"/> instance.</param>
		public Memory(MemoryHandle handle) => this._handle = handle;

		/// <inheritdoc/>
		protected override Boolean Dispose(Boolean disposing)
		{
			if (!base.Dispose(disposing) || !disposing) return false;
			this._handle.Dispose();
			return true;
		}
	}
}