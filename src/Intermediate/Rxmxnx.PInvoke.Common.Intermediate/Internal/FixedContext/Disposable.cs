namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedContext<T> : IConvertibleDisposable<IFixedContext<T>.IDisposable>
{
	/// <inheritdoc/>
	public IFixedContext<T>.IDisposable ToDisposable(IDisposable? disposable) => this.CreateDisposable(disposable);

	/// <summary>
	/// Creates a <see cref="Disposable"/> instance from current instance.
	/// </summary>
	/// <param name="disposable">A <see cref="IDisposable"/> instance.</param>
	/// <returns>A <see cref="Disposable"/> instance.</returns>
	private Disposable CreateDisposable(IDisposable? disposable) => new(this, disposable);

	/// <summary>
	/// Disposable implementation.
	/// </summary>
	private sealed class Disposable : Disposable<FixedContext<T>>, IFixedContext<T>.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(FixedContext<T> fixedPointer, IDisposable? disposable) : base(fixedPointer, disposable) { }

		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		Span<Byte> IFixedMemory.Bytes => (this.Value as IFixedMemory).Bytes;
		Span<T> IFixedMemory<T>.Values => (this.Value as IFixedMemory<T>).Values;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;
		ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => (this.Value as IReadOnlyFixedMemory<T>).Values;

		[ExcludeFromCodeCoverage]
		IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
		[ExcludeFromCodeCoverage]
		IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
			out IReadOnlyFixedMemory residual)
		{
			Unsafe.SkipInit(out residual);
			IReadOnlyFixedContext<TDestination> result =
				this.Transformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}
		[ExcludeFromCodeCoverage]
		IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		{
			Unsafe.SkipInit(out residual);
			IFixedContext<TDestination> result =
				this.Transformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}

		/// <inheritdoc/>
		public IFixedContext<Byte> AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IFixedContext<Byte>.IDisposable>)!.ToDisposable(
				this.GetDisposableParent());

		/// <inheritdoc/>
		public IFixedContext<TDestination> Transformation<TDestination>(out IFixedMemory residual)
			where TDestination : unmanaged
		{
			IFixedContext<TDestination>.IDisposable result = this.Value
			                                                     .GetTransformation<TDestination>(
				                                                     out FixedOffset offset)
			                                                     .CreateDisposable(this.GetDisposableParent());
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result;
		}
	}
}