#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

#if !OBSOLETE_FIXED_INTERFACES
internal partial class FixedContext<T> : IConvertibleDisposable<IFixedContext<T>.IDisposable>
#else
#pragma warning disable CS0612
internal partial class FixedContext<T> : IConvertibleDisposable<IObsoleteFixedContext<T>.IDisposable>
#pragma warning restore CS0612
#endif
{
	/// <inheritdoc/>
#if !OBSOLETE_FIXED_INTERFACES
	public IFixedContext<T>.IDisposable ToDisposable(IDisposable? disposable)
#else
#if !GITHUB_ACTIONS
	[Obsolete]
#endif
	public IObsoleteFixedContext<T>.IDisposable ToDisposable(IDisposable? disposable)
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
#if !OBSOLETE_FIXED_INTERFACES
	private sealed class Disposable : Disposable<FixedContext<T>>, IFixedContext<T>.IDisposable
#else
#pragma warning disable CS0612
	private sealed class Disposable : Disposable<FixedContext<T>>, IObsoleteFixedContext<T>.IDisposable
#pragma warning restore CS0612
#endif
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

#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
		[Obsolete]
#endif
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
		[Obsolete]
#endif
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext() => this.AsObjectContext();
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
		[Obsolete]
#endif
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
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
		[Obsolete]
#endif
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
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
		[Obsolete]
#endif
		public IFixedContext<Byte> AsBinaryContext()
		{
			FixedContext<T>? ctx = this.GetValue<FixedContext<T>>();
			return ctx?.AsBinaryContext() is not IConvertibleDisposable<IFixedContext<Byte>.IDisposable> convertible ?
				FixedContext<Byte>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}
		/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
		[Obsolete]
#endif
		public IFixedContext<Object> AsObjectContext()
		{
			FixedContext<T>? ctx = this.GetValue<FixedContext<T>>();
			return ctx?.AsObjectContext() is not IConvertibleDisposable<IFixedContext<Object>.IDisposable> convertible ?
				FixedContext<Object>.EmptyDisposable :
				convertible.ToDisposable(this.GetDisposableParent());
		}

		/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
		[Obsolete]
#endif
		public IFixedContext<TDestination> Transformation<TDestination>(out IFixedMemory residual)
		{
			FixedOffset offset;
			if (this.GetValue<FixedContext<T>>() is not { } ctx)
			{
				FixedMemory emptyMem = !RuntimeHelpers.IsReferenceOrContainsReferences<T>() ?
					FixedContext<Byte>.Empty :
					FixedContext<Object>.Empty;
				offset = new(emptyMem, 0);
				residual = offset.ToDisposable(this.GetDisposableParent());
				return FixedContext<TDestination>.EmptyDisposable;
			}
			FixedContext<TDestination> result = ctx.GetTransformation<TDestination>(out offset);
			residual = offset.ToDisposable(this.GetDisposableParent());
			return result.CreateDisposable(this.GetDisposableParent());
		}
	}
}
#endif