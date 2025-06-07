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
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		ref readonly T IReadOnlyReferenceable<T>.Reference => ref (this.Value as IReadOnlyFixedReference<T>).Reference;
		IntPtr IFixedPointer.Pointer => (this.Value as IFixedPointer).Pointer;
		Boolean IReadOnlyFixedMemory.IsNullOrEmpty => (this.Value as IReadOnlyFixedMemory).IsNullOrEmpty;
		Span<Byte> IFixedMemory.Bytes => (this.Value as IFixedMemory).Bytes;
		Span<Object> IFixedMemory.Objects => (this.Value as IFixedMemory).Objects;
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => (this.Value as IReadOnlyFixedMemory).Objects;
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;

#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext() => this.AsObjectContext();
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>(
			out IReadOnlyFixedMemory residual)
		{
#if !NETCOREAPP && PACKAGE || NETCOREAPP3_1_OR_GREATER
			Unsafe.SkipInit(out residual);
#else
			residual = default!;
#endif
			IReadOnlyFixedReference<TDestination> result =
				this.Transformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		{
#if !NETCOREAPP && PACKAGE || NETCOREAPP3_1_OR_GREATER
			Unsafe.SkipInit(out residual);
#else
			residual = default!;
#endif
			IFixedReference<TDestination> result =
				this.Transformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}

		/// <inheritdoc/>
		public IFixedContext<Byte> AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IFixedContext<Byte>.IDisposable>)!.ToDisposable(
				this.GetDisposableParent());
		/// <inheritdoc/>
		public IFixedContext<Object> AsObjectContext()
			=> (this.Value.AsObjectContext() as IConvertibleDisposable<IFixedContext<Object>.IDisposable>)!
				.ToDisposable(this.GetDisposableParent());
		/// <inheritdoc/>
		public IFixedReference<TDestination> Transformation<TDestination>(out IFixedMemory residual)
		{
			IFixedReference<TDestination>.IDisposable result = this.Value
			                                                       .GetTransformation<TDestination>(
				                                                       out FixedOffset offset)
			                                                       .CreateDisposable(this.GetDisposableParent());
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result;
		}
	}
}