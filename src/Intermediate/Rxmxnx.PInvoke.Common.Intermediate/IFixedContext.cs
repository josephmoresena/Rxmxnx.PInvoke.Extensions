namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a context from a block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IFixedContext<T> : IReadOnlyFixedContext<T>, IFixedMemory<T> where T : unmanaged
{
	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory block as
	/// a <typeparamref name="TDestination"/> memory block.
	/// </summary>
	/// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
	/// <param name="residual">Output. Residual memory from the transformation.</param>
	/// <returns>An instance of <see cref="IFixedContext{TDestination}"/>.</returns>
	IFixedContext<TDestination> Transformation<TDestination>(out IFixedMemory residual) where TDestination : unmanaged;

	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory block as
	/// a <typeparamref name="TDestination"/> memory block.
	/// </summary>
	/// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
	/// <param name="residual">Output. Residual read-only memory from the transformation.</param>
	/// <returns>An instance of <see cref="IReadOnlyFixedContext{TDestination}"/>.</returns>
	new IFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		where TDestination : unmanaged;

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IFixedContext{T}"/> object.
	/// </summary>
	public new interface IDisposable : IFixedContext<T>, IFixedMemory<T>.IDisposable, IReadOnlyFixedContext<T>.IDisposable
	{
		IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
			=> this.Transformation<TDestination>(out residual);
		IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
			=> this.Transformation<TDestination>(out residual);
		IReadOnlyFixedContext<TDestination>.IDisposable IReadOnlyFixedContext<T>.IDisposable.
			Transformation<TDestination>(out IReadOnlyFixedMemory.IDisposable residual)
		{
			IFixedContext<TDestination>.IDisposable result =
				this.Transformation<TDestination>(out IFixedMemory.IDisposable disposableResidual);
			residual = disposableResidual;
			return result;
		}
		/// <inheritdoc cref="IFixedContext{T}.Transformation{TDestination}(out IFixedMemory)"/>
		new IFixedContext<TDestination>.IDisposable Transformation<TDestination>(out IFixedMemory residual)
			where TDestination : unmanaged
		{
			IFixedContext<TDestination>.IDisposable result =
				this.Transformation<TDestination>(out IFixedMemory.IDisposable disposableResidual);
			residual = disposableResidual;
			return result;
		}
		/// <inheritdoc cref="IFixedContext{T}.Transformation{TDestination}(out IReadOnlyFixedMemory)"/>
		new IFixedContext<TDestination>.IDisposable Transformation<TDestination>(out IReadOnlyFixedMemory residual)
			where TDestination : unmanaged
		{
			IFixedContext<TDestination>.IDisposable result =
				this.Transformation<TDestination>(out IFixedMemory.IDisposable disposableResidual);
			residual = disposableResidual;
			return result;
		}
		/// <inheritdoc cref="IFixedContext{T}.Transformation{TDestination}(out IFixedMemory)"/>
		IFixedContext<TDestination>.IDisposable Transformation<TDestination>(out IFixedMemory.IDisposable residual)
			where TDestination : unmanaged;
	}
}