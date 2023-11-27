namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedReference<T> : IConvertibleDisposable<IFixedReference<T>.IDisposable>
{
	/// <inheritdoc/>
	public IFixedReference<T>.IDisposable ToDisposable(IDisposable? disposable) => this.CreateDisposable(disposable);

	/// <summary>
	/// Creates a <see cref="Disposable"/> instance from current instance.
	/// </summary>
	/// <param name="disposable">A <see cref="IDisposable"/> instance.</param>
	/// <returns>A <see cref="Disposable"/> instance.</returns>
	private Disposable CreateDisposable(IDisposable? disposable) => new(this, disposable);

	/// <summary>
	/// Disposable implementation.
	/// </summary>
	private sealed record Disposable : DisposableFixedPointer<FixedReference<T>>, IFixedReference<T>.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(FixedReference<T> fixedPointer, IDisposable? disposable) : base(fixedPointer, disposable) { }

		ref T IReferenceable<T>.Reference => ref (this.Value as IReferenceable<T>).Reference;
		ref readonly T IReadOnlyReferenceable<T>.Reference => ref (this.Value as IReadOnlyFixedReference<T>).Reference;
		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		Span<Byte> IFixedMemory.Bytes => (this.Value as IFixedMemory).Bytes;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;

		IReadOnlyFixedContext<Byte>.IDisposable IReadOnlyFixedMemory.IDisposable.AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IReadOnlyFixedContext<Byte>.IDisposable>)!
				.ToDisposable(this.Disposable);
		IFixedContext<Byte>.IDisposable IFixedMemory.IDisposable.AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IFixedContext<Byte>.IDisposable>)!.ToDisposable(
				this.Disposable);

		/// <inheritdoc/>
		public IFixedReference<TDestination>.IDisposable Transformation<TDestination>(
			out IFixedMemory.IDisposable residual) where TDestination : unmanaged
		{
			IFixedReference<TDestination>.IDisposable result = this.Value
			                                                       .GetTransformation<TDestination>(
				                                                       out FixedOffset offset)
			                                                       .CreateDisposable(this.Disposable);
			residual = offset.ToDisposable(this.Disposable);
			return result;
		}
		/// <inheritdoc/>
		public IReadOnlyFixedReference<TDestination>.IDisposable Transformation<TDestination>(
			out IReadOnlyFixedMemory.IDisposable residual) where TDestination : unmanaged
		{
			IFixedReference<TDestination>.IDisposable result =
				this.Transformation<TDestination>(out IFixedMemory.IDisposable disposableResidual);
			residual = disposableResidual;
			return result;
		}
	}
}