namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a fixed in memory method.
/// </summary>
/// <typeparam name="TDelegate">Type of the fixed method.</typeparam>
public interface IFixedMethod<out TDelegate> : IFixedPointer where TDelegate : Delegate
{
    /// <summary>
    /// Delegate to fixed method.
    /// </summary>
    TDelegate Method { get; }
}
