namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedOffset : IConvertibleDisposable<IFixedMemory.IDisposable>
{
	/// <inheritdoc/>
	public IFixedMemory.IDisposable ToDisposable(IDisposable disposable) => this.CreateDisposable(disposable);

	/// <summary>
	/// Creates a <see cref="Disposable"/> instance from current instance.
	/// </summary>
	/// <param name="disposable">A <see cref="IDisposable"/> instance.</param>
	/// <returns>A <see cref="Disposable"/> instance.</returns>
	private Disposable CreateDisposable(IDisposable disposable) => new(this, disposable);

	/// <summary>
	/// Disposable implementation.
	/// </summary>
	private sealed record Disposable : DisposableFixedPointer<FixedOffset>, IFixedMemory.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(FixedOffset fixedPointer, IDisposable? disposable) : base(fixedPointer, disposable) { }

		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		Span<Byte> IFixedMemory.Bytes => (this.Value as IFixedMemory).Bytes;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;

		IReadOnlyFixedContext<Byte>.IDisposable IReadOnlyFixedMemory.IDisposable.AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IReadOnlyFixedContext<Byte>.IDisposable>)!
				.ToDisposable(this.Disposable);
		IFixedContext<Byte>.IDisposable IFixedMemory.IDisposable.AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IFixedContext<Byte>.IDisposable>)!.ToDisposable(
				this.Disposable);
	}
}