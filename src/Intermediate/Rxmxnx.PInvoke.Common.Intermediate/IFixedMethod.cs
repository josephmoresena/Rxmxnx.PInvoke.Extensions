namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a method that is fixed in memory.
/// </summary>
/// <typeparam name="TDelegate">Type of the delegate that corresponds to the fixed method.</typeparam>
public interface IFixedMethod<out TDelegate> : IFixedPointer where TDelegate : Delegate
{
	/// <summary>
	/// Gets the delegate that points to the fixed method in memory.
	/// </summary>
	TDelegate Method { get; }
}