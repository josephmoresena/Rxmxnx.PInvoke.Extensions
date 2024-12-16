namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedOffset : IConvertibleDisposable<IReadOnlyFixedMemory.IDisposable>
{
	/// <inheritdoc/>
	public IReadOnlyFixedMemory.IDisposable ToDisposable(IDisposable? disposable) => this.CreateDisposable(disposable);

	/// <summary>
	/// Creates a <see cref="Disposable"/> instance from current instance.
	/// </summary>
	/// <param name="disposable">A <see cref="IDisposable"/> instance.</param>
	/// <returns>A <see cref="Disposable"/> instance.</returns>
	private Disposable CreateDisposable(IDisposable? disposable) => new(this, disposable);

	/// <summary>
	/// Disposable implementation.
	/// </summary>
	private sealed class Disposable : Disposable<ReadOnlyFixedOffset>, IReadOnlyFixedMemory.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(ReadOnlyFixedOffset fixedPointer, IDisposable? disposable) :
			base(fixedPointer, disposable) { }

		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		Boolean IReadOnlyFixedMemory.IsNullOrEmpty => (this.Value as IReadOnlyFixedMemory).IsNullOrEmpty;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;
		ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => (this.Value as IReadOnlyFixedMemory).Objects;

		/// <inheritdoc/>
		public IReadOnlyFixedContext<Byte> AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IReadOnlyFixedContext<Byte>.IDisposable>)!
				.ToDisposable(this.GetDisposableParent());
		/// <inheritdoc/>
		public IReadOnlyFixedContext<Object> AsObjectContext()
			=> (this.Value.AsObjectContext() as IConvertibleDisposable<IReadOnlyFixedContext<Object>.IDisposable>)!
				.ToDisposable(this.GetDisposableParent());
	}
}