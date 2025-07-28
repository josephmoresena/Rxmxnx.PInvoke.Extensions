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
		/// <summary>
		/// An empty instance of <see cref="ReadOnlyFixedContext{T}.Disposable"/>.
		/// </summary>
		public static readonly Disposable Default = new(ReadOnlyFixedContext<T>.Empty, default);

		/// <inheritdoc/>
		public Disposable(ReadOnlyFixedContext<T> fixedPointer, IDisposable? disposable) : base(
			fixedPointer, disposable) { }

		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes
			=> this.GetValue<IReadOnlyFixedMemory>() is { } val ? val.Bytes : default;
		ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects
			=> this.GetValue<IReadOnlyFixedMemory>() is { } val ? val.Objects : default;
		ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values
			=> this.GetValue<IReadOnlyFixedMemory<T>>() is { } val ? val.Values : default;

		/// <inheritdoc/>
		public IReadOnlyFixedContext<Byte> AsBinaryContext()
		{
			ReadOnlyFixedContext<T>? ctx = this.GetValue<ReadOnlyFixedContext<T>>();
			return ctx?.AsBinaryContext() is not IConvertibleDisposable<IReadOnlyFixedContext<Byte>.IDisposable>
				convertible ?
				ReadOnlyFixedContext<Byte>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}
		/// <inheritdoc/>
		public IReadOnlyFixedContext<Object> AsObjectContext()
		{
			ReadOnlyFixedContext<T>? ctx = this.GetValue<ReadOnlyFixedContext<T>>();
			return ctx?.AsObjectContext() is not IConvertibleDisposable<IReadOnlyFixedContext<Object>.IDisposable>
				convertible ?
				ReadOnlyFixedContext<Object>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}

		/// <inheritdoc/>
		public IReadOnlyFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		{
			ReadOnlyFixedOffset offset;
			if (this.GetValue<ReadOnlyFixedContext<T>>() is not { } ctx)
			{
				offset = new(ReadOnlyFixedContext<Byte>.Empty, 0);
				residual = offset.ToDisposable(this.GetDisposableParent());
				return ReadOnlyFixedContext<TDestination>.EmptyDisposable;
			}
			IReadOnlyFixedContext<TDestination> result = ctx.GetTransformation<TDestination>(out offset)
			                                                .CreateDisposable(this.GetDisposableParent());
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result;
		}
	}
}