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
	private sealed class Disposable : Disposable<FixedReference<T>>, IFixedReference<T>.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(FixedReference<T> fixedPointer, IDisposable? disposable) : base(fixedPointer, disposable) { }

		ref T IReferenceable<T>.Reference => ref (this.Value as IReferenceable<T>).Reference;
		[ExcludeFromCodeCoverage]
		ref readonly T IReadOnlyReferenceable<T>.Reference => ref (this.Value as IReadOnlyFixedReference<T>).Reference;
		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		Span<Byte> IFixedMemory.Bytes => (this.Value as IFixedMemory).Bytes;
		[ExcludeFromCodeCoverage]
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;

		[ExcludeFromCodeCoverage]
		IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
		[ExcludeFromCodeCoverage]
		IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		{
			Unsafe.SkipInit(out residual);
			IReadOnlyFixedReference<TDestination> result =
				this.Transformation<TDestination>(
					out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}
		[ExcludeFromCodeCoverage]
		IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		{
			Unsafe.SkipInit(out residual);
			IFixedReference<TDestination> result =
				this.Transformation<TDestination>(
					out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}
		
		/// <inheritdoc/>
		public IFixedContext<Byte> AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IFixedContext<Byte>.IDisposable>)!.ToDisposable(
				this.GetDisposableParent());
		/// <inheritdoc/>
		public IFixedReference<TDestination> Transformation<TDestination>(
			out IFixedMemory residual) where TDestination : unmanaged
		{
			IFixedReference<TDestination>.IDisposable result = this.Value
			                                                       .GetTransformation<TDestination>(
				                                                       out FixedOffset offset).CreateDisposable(this.GetDisposableParent());
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result;
		}
	}
}