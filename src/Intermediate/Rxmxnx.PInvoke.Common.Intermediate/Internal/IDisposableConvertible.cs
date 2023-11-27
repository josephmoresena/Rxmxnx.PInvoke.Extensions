namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// </summary>
/// <typeparam name="TDisposable"></typeparam>
internal interface IDisposableConvertible<out TDisposable> where TDisposable : IDisposable
{
	TDisposable ToDisposable(IDisposable disposable);
}