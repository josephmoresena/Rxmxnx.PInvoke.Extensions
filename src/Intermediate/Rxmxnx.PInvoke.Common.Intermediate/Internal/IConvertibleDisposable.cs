namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This interface exposes an object that can be converted to a <see cref="IDisposable"/> instance.
/// </summary>
/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/></typeparam>
internal interface IConvertibleDisposable<out TDisposable> where TDisposable : IDisposable
{
	/// <summary>
	/// Converts current instance to a <typeparamref name="TDisposable"/> instance.
	/// </summary>
	/// <param name="disposable">
	/// A <see cref="IDisposable"/> to dispose in <see cref="IDisposable.Dispose()"/> call.
	/// </param>
	/// <returns>A <typeparamref name="TDisposable"/> instance.</returns>
	TDisposable ToDisposable(IDisposable? disposable);
}