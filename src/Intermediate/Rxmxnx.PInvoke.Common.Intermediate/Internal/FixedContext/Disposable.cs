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
		/// <summary>
		/// An empty instance of <see cref="FixedContext{T}.Disposable"/>.
		/// </summary>
		public static readonly Disposable Default = new(FixedContext<T>.Empty, default);

		/// <inheritdoc/>
		public Disposable(FixedContext<T> fixedPointer, IDisposable? disposable) : base(fixedPointer, disposable) { }

		Span<Byte> IFixedMemory.Bytes => this.GetValue<IFixedMemory>() is { } val ? val.Bytes : default;
		Span<T> IFixedMemory<T>.Values => this.GetValue<IFixedMemory<T>>() is { } val ? val.Values : default;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes
			=> this.GetValue<IReadOnlyFixedMemory>() is { } val ? val.Bytes : default;
		ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values
			=> this.GetValue<IReadOnlyFixedMemory<T>>() is { } val ? val.Values : default;
		Span<Object> IFixedMemory.Objects => this.GetValue<IFixedMemory>() is { } val ? val.Objects : default;
		ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects
			=> this.GetValue<IReadOnlyFixedMemory>() is { } val ? val.Objects : default;

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
		IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
			out IReadOnlyFixedMemory residual)
		{
			Unsafe.SkipInit(out residual);
			IReadOnlyFixedContext<TDestination> result =
				this.Transformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		{
			Unsafe.SkipInit(out residual);
			IFixedContext<TDestination> result =
				this.Transformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual));
			return result;
		}

		/// <inheritdoc/>
		public IFixedContext<Byte> AsBinaryContext()
		{
			FixedContext<T>? ctx = this.GetValue<FixedContext<T>>();
			return ctx?.AsBinaryContext() is not IConvertibleDisposable<IFixedContext<Byte>.IDisposable> convertible ?
				FixedContext<Byte>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}
		/// <inheritdoc/>
		public IFixedContext<Object> AsObjectContext()
		{
			FixedContext<T>? ctx = this.GetValue<FixedContext<T>>();
			return ctx?.AsBinaryContext() is not IConvertibleDisposable<IFixedContext<Object>.IDisposable> convertible ?
				FixedContext<Object>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}

		/// <inheritdoc/>
		public IFixedContext<TDestination> Transformation<TDestination>(out IFixedMemory residual)
		{
			FixedOffset offset;
			if (this.GetValue<FixedContext<T>>() is not { } ctx)
			{
				offset = new(FixedContext<Byte>.Empty, 0);
				residual = offset.ToDisposable(this.GetDisposableParent());
				return FixedContext<TDestination>.EmptyDisposable;
			}
			FixedContext<TDestination> result = ctx.GetTransformation<TDestination>(out offset);
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result.CreateDisposable(this.GetDisposableParent());
		}
	}
}