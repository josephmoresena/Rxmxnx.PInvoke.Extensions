namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a mutable reference to a fixed memory location.
/// </summary>
/// <typeparam name="T">Type of the value referenced in memory.</typeparam>
public interface IFixedReference<T> : IReferenceable<T>, IReadOnlyFixedReference<T>, IFixedMemory where T : unmanaged
{
	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory reference as a
	/// <typeparamref name="TDestination"/> memory reference.
	/// </summary>
	/// <typeparam name="TDestination">Type of the reinterpreted memory reference.</typeparam>
	/// <param name="residual">Output. Residual memory left after the transformation.</param>
	/// <returns>
	/// A <see cref="IFixedReference{TDestination}"/> instance.
	/// </returns>
	IFixedReference<TDestination> Transformation<TDestination>(out IFixedMemory residual)
		where TDestination : unmanaged;

	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory reference as a
	/// <typeparamref name="TDestination"/> memory reference.
	/// </summary>
	/// <typeparam name="TDestination">Type of the reinterpreted memory reference.</typeparam>
	/// <param name="residual">Output. Residual read-only memory left after the transformation.</param>
	/// <returns>
	/// A <see cref="IReadOnlyFixedReference{TDestination}"/> instance.
	/// </returns>
	new IFixedReference<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		where TDestination : unmanaged;

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IFixedReference{T}"/> object.
	/// </summary>
	public new interface IDisposable : IFixedReference<T>, IReadOnlyFixedReference<T>.IDisposable, IFixedMemory.IDisposable
	{
		[ExcludeFromCodeCoverage]
		IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IFixedMemory residual)
			=> this.Transformation<TDestination>(out residual);
		[ExcludeFromCodeCoverage]
		IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
			=> this.Transformation<TDestination>(out residual);
		/// <inheritdoc cref="IReadOnlyFixedReference{T}.Transformation{TDestination}(out IReadOnlyFixedMemory)"/>
		new IFixedReference<TDestination>.IDisposable Transformation<TDestination>(out IFixedMemory residual)
			where TDestination : unmanaged
		{
			Unsafe.SkipInit(out residual);
			IFixedReference<TDestination>.IDisposable result =
				this.Transformation<TDestination>(
					out Unsafe.As<IFixedMemory, IFixedMemory.IDisposable>(ref residual));
			return result;
		}
		/// <inheritdoc cref="IReadOnlyFixedReference{T}.Transformation{TDestination}(out IReadOnlyFixedMemory)"/>
		[ExcludeFromCodeCoverage]
		new IFixedReference<TDestination>.IDisposable Transformation<TDestination>(out IReadOnlyFixedMemory residual)
			where TDestination : unmanaged
		{
			Unsafe.SkipInit(out residual);
			IFixedReference<TDestination>.IDisposable result =
				this.Transformation<TDestination>(
					out Unsafe.As<IReadOnlyFixedMemory, IFixedMemory.IDisposable>(ref residual));
			return result;
		}
		/// <inheritdoc cref="IReadOnlyFixedReference{T}.Transformation{TDestination}(out IReadOnlyFixedMemory)"/>
		IFixedReference<TDestination>.IDisposable Transformation<TDestination>(out IFixedMemory.IDisposable residual)
			where TDestination : unmanaged;
	}
}