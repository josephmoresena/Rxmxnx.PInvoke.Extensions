namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedContext<T> :
#if !OBSOLETE_FIXED_INTERFACES
	IConvertibleDisposable<IReadOnlyFixedContext<T>.IDisposable>
#else
#pragma warning disable CS0612
	IConvertibleDisposable<IObsoleteReadOnlyFixedContext<T>.IDisposable>
#pragma warning restore CS0612
#endif
{
	/// <inheritdoc/>
#if !OBSOLETE_FIXED_INTERFACES
	public IReadOnlyFixedContext<T>.IDisposable ToDisposable(IDisposable? disposable)
#else
	[Obsolete]
	public IObsoleteReadOnlyFixedContext<T>.IDisposable ToDisposable(IDisposable? disposable)
#endif
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
	private sealed class Disposable : Disposable<ReadOnlyFixedContext<T>>,
#if !OBSOLETE_FIXED_INTERFACES
		IReadOnlyFixedContext<T>.IDisposable
#else
#pragma warning disable CS0612
		IObsoleteReadOnlyFixedContext<T>.IDisposable
#pragma warning restore CS0612
#endif
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
#if OBSOLETE_FIXED_INTERFACES
		[Obsolete]
#endif
		public IReadOnlyFixedContext<Byte> AsBinaryContext()
		{
			ReadOnlyFixedContext<T>? ctx = this.GetValue<ReadOnlyFixedContext<T>>();
			return ctx?.AsBinaryContext() is not IConvertibleDisposable<IReadOnlyFixedContext<Byte>.IDisposable>
				convertible ?
				ReadOnlyFixedContext<Byte>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}
		/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
		[Obsolete]
#endif
		public IReadOnlyFixedContext<Object> AsObjectContext()
		{
			ReadOnlyFixedContext<T>? ctx = this.GetValue<ReadOnlyFixedContext<T>>();
			return ctx?.AsObjectContext() is not IConvertibleDisposable<IReadOnlyFixedContext<Object>.IDisposable>
				convertible ?
				ReadOnlyFixedContext<Object>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}

		/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
		[Obsolete]
#endif
		public IReadOnlyFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		{
			ReadOnlyFixedOffset offset;
			if (this.GetValue<ReadOnlyFixedContext<T>>() is not { } ctx)
			{
				ReadOnlyFixedMemory emptyMem = !RuntimeHelpers.IsReferenceOrContainsReferences<T>() ?
					ReadOnlyFixedContext<Byte>.Empty :
					ReadOnlyFixedContext<Object>.Empty;
				offset = new(emptyMem, 0);
				residual = offset.ToDisposable(this.GetDisposableParent());
				return ReadOnlyFixedContext<TDestination>.EmptyDisposable;
			}
			ReadOnlyFixedContext<TDestination> result = ctx.GetTransformation<TDestination>(out offset);
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result.CreateDisposable(this.GetDisposableParent());
		}
	}
}