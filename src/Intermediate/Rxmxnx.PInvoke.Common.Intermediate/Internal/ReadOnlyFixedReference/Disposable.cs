namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedReference<T> : IDisposableConvertible<IReadOnlyFixedReference<T>.IDisposable>
{
	/// <inheritdoc/>
	public IReadOnlyFixedReference<T>.IDisposable ToDisposable(IDisposable disposable)
		=> this.CreateDisposable(disposable);

	/// <summary>
	/// Creates a <see cref="Disposable"/> instance from current instance.
	/// </summary>
	/// <param name="disposable">A <see cref="IDisposable"/> instance.</param>
	/// <returns>A <see cref="Disposable"/> instance.</returns>
	private Disposable CreateDisposable(IDisposable disposable) => new(this, disposable);

	/// <summary>
	/// Disposable implementation.
	/// </summary>
	private sealed record Disposable : DisposableFixedPointer<ReadOnlyFixedReference<T>>,
		IReadOnlyFixedReference<T>.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(ReadOnlyFixedReference<T> fixedPointer, IDisposable disposable) : base(
			fixedPointer, disposable) { }

		ref readonly T IReadOnlyReferenceable<T>.Reference => ref (this.Value as IReadOnlyFixedReference<T>).Reference;
		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;

		IReadOnlyFixedContext<Byte>.IDisposable IReadOnlyFixedMemory.IDisposable.AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IDisposableConvertible<IReadOnlyFixedContext<Byte>.IDisposable>)!
				.ToDisposable(this.Disposable);

		/// <inheritdoc/>
		public IReadOnlyFixedReference<TDestination>.IDisposable Transformation<TDestination>(
			out IReadOnlyFixedMemory.IDisposable residual) where TDestination : unmanaged
		{
			IReadOnlyFixedReference<TDestination>.IDisposable result = this.Value
			                                                               .GetTransformation<TDestination>(
				                                                               out ReadOnlyFixedOffset offset)
			                                                               .CreateDisposable(this.Disposable);
			residual = offset.ToDisposable(this.Disposable);
			return result;
		}
	}
}