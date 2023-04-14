namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a fixed in memory method.
/// </summary>
/// <typeparam name="TDelegate">Type of the fixed method.</typeparam>
public interface IFixedMethod<TDelegate> where TDelegate : Delegate
{
	/// <summary>
	/// Delegate to fixed method.
	/// </summary>
	public TDelegate Method { get; }
	/// <summary>
	/// Pointer to fixed method.
	/// </summary>
	public IntPtr Pointer { get; }
}
