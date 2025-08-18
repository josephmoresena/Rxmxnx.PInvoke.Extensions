namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class FixedDelegate
{
	/// <summary>
	/// An empty instance of <see cref="FixedDelegate{T}"/>.
	/// </summary>
	protected static readonly FixedDelegate Empty = new EmptyInstance();

	/// <summary>
	/// Null delegate class.
	/// </summary>
	private sealed class EmptyInstance : FixedDelegate
	{
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public override Type Type => typeof(MulticastDelegate);
	}
}

internal sealed partial class FixedDelegate<TDelegate> : IConvertibleDisposable<IFixedMethod<TDelegate>.IDisposable>
{
	/// <inheritdoc/>
	public IFixedMethod<TDelegate>.IDisposable ToDisposable(IDisposable? disposable)
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
	private sealed class Disposable : Disposable<FixedDelegate>, IFixedMethod<TDelegate>.IDisposable
	{
		/// <inheritdoc cref="Disposable.Default"/>
		private static Disposable? defaultInstance;

		/// <summary>
		/// An empty instance of <see cref="FixedDelegate{T}.Disposable"/>.
		/// </summary>
		public static IFixedMethod<TDelegate>.IDisposable Default
			=> Disposable.defaultInstance ??= new(FixedDelegate.Empty, default);

		/// <inheritdoc/>
		public Disposable(FixedDelegate fixedPointer, IDisposable? disposable) : base(fixedPointer, disposable) { }

		TDelegate IFixedMethod<TDelegate>.Method => this.GetValue<IFixedMethod<TDelegate>>()?.Method!;
	}
}