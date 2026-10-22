namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This interface exposes an object that can be converted to an <see cref="IDisposable"/> instance.
/// </summary>
/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/></typeparam>
internal interface IConvertibleDisposable<out TDisposable> where TDisposable : IDisposable
{
	/// <summary>
	/// Converts the current instance to a <typeparamref name="TDisposable"/> instance.
	/// </summary>
	/// <param name="disposable">
	/// An <see cref="IDisposable"/> instance to dispose when <see cref="IDisposable.Dispose()"/> is called.
	/// </param>
	/// <returns>A <typeparamref name="TDisposable"/> instance.</returns>
	TDisposable ToDisposable(IDisposable? disposable);
}