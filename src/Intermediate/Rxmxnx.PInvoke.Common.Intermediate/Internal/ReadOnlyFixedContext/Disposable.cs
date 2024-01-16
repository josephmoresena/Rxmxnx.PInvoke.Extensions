namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedContext<T> : IConvertibleDisposable<IReadOnlyFixedContext<T>.IDisposable>
{
	/// <inheritdoc/>
	public IReadOnlyFixedContext<T>.IDisposable ToDisposable(IDisposable? disposable)
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
	private sealed class Disposable : Disposable<ReadOnlyFixedContext<T>>, IReadOnlyFixedContext<T>.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(ReadOnlyFixedContext<T> fixedPointer, IDisposable? disposable) : base(
			fixedPointer, disposable) { }

		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;
		ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => (this.Value as IReadOnlyFixedMemory<T>).Values;

		/// <inheritdoc/>
		public IReadOnlyFixedContext<Byte> AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IReadOnlyFixedContext<Byte>.IDisposable>)!
				.ToDisposable(this.GetDisposableParent());

		/// <inheritdoc/>
		public IReadOnlyFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual)
			where TDestination : unmanaged
		{
			IReadOnlyFixedContext<TDestination> result = this.Value
			                                                 .GetTransformation<TDestination>(
				                                                 out ReadOnlyFixedOffset offset)
			                                                 .CreateDisposable(this.GetDisposableParent());
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result;
		}
	}
}