namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedOffset : IConvertibleDisposable<IFixedMemory.IDisposable>
{
	/// <inheritdoc/>
	public IFixedMemory.IDisposable ToDisposable(IDisposable? disposable) => this.CreateDisposable(disposable);

	/// <summary>
	/// Creates a <see cref="Disposable"/> instance from current instance.
	/// </summary>
	/// <param name="disposable">A <see cref="IDisposable"/> instance.</param>
	/// <returns>A <see cref="Disposable"/> instance.</returns>
	private Disposable CreateDisposable(IDisposable? disposable) => new(this, disposable);

	/// <summary>
	/// Disposable implementation.
	/// </summary>
	private sealed class Disposable : Disposable<FixedOffset>, IFixedMemory.IDisposable
	{
		/// <inheritdoc/>
		public Disposable(FixedOffset fixedPointer, IDisposable? disposable) : base(fixedPointer, disposable) { }

		Span<Byte> IFixedMemory.Bytes => (this.Value as IFixedMemory).Bytes;
		ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => (this.Value as IReadOnlyFixedMemory).Bytes;
		Span<Object> IFixedMemory.Objects => (this.Value as IFixedMemory).Objects;
		ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => (this.Value as IReadOnlyFixedMemory).Objects;

#if OBSOLETE_FIXED_INTERFACES
		[Obsolete]
#endif
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
#if OBSOLETE_FIXED_INTERFACES
		[Obsolete]
#endif
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext() => this.AsObjectContext();

		/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
		[Obsolete]
#endif
		public IFixedContext<Byte> AsBinaryContext()
			=> (this.Value.AsBinaryContext() as IConvertibleDisposable<IFixedContext<Byte>.IDisposable>)!.ToDisposable(
				this.GetDisposableParent());
		/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
		[Obsolete]
#endif
		public IFixedContext<Object> AsObjectContext()
			=> (this.Value.AsObjectContext() as IConvertibleDisposable<IFixedContext<Object>.IDisposable>)!
				.ToDisposable(this.GetDisposableParent());
	}
}