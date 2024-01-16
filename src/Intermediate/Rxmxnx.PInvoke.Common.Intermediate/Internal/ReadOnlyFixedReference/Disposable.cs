namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedReference<T> : IConvertibleDisposable<IReadOnlyFixedReference<T>.IDisposable>
{
	/// <inheritdoc/>
	public IReadOnlyFixedReference<T>.IDisposable ToDisposable(IDisposable? disposable)
		=> this.CreateDisposable(disposable);

	/// <summary>
	/// Creates a <see cref="Disposable"/> instance from current instance.
	/// </summary>
	/// <param name="disposable">A <see cref="IDisposable"/> instance.</param>
	/// <returns>A <see cref="Disposable"/> instance.</returns>
	private Disposable CreateDisposable(IDisposable? disposable) => new(this, disposable);

	/// <summary>
	/// Disposable implementation.
	/// </summary>
	private sealed class Disposable : Disposable<ReadOnlyFixedReference<T>>, IReadOnlyFixedReference<T>.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(ReadOnlyFixedReference<T> fixedPointer, IDisposable? disposable) : base(
			fixedPointer, disposable) { }

		ref readonly T IReadOnlyReferenceable<T>.Reference => ref (this.Value as IReadOnlyFixedReference<T>).Reference;
		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;

		/// <inheritdoc/>
		public IReadOnlyFixedContext<Byte> AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IReadOnlyFixedContext<Byte>.IDisposable>)!
				.ToDisposable(this.GetDisposableParent());
		/// <inheritdoc/>
		public IReadOnlyFixedReference<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual)
			where TDestination : unmanaged
		{
			IReadOnlyFixedReference<TDestination>.IDisposable result = this.Value
			                                                               .GetTransformation<TDestination>(
				                                                               out ReadOnlyFixedOffset offset)
			                                                               .ToDisposable(this.GetDisposableParent());
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result;
		}
	}
}