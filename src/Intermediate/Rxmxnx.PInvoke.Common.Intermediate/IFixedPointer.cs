namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a pointer to a fixed block of memory.
/// </summary>
public interface IFixedPointer
{
	/// <summary>
	/// Gets the pointer to the fixed block of memory.
	/// </summary>
	IntPtr Pointer { get; }

	/// <summary>
	/// Interface representing a <see cref="IDisposable"/> <see cref="IFixedPointer"/> object.
	/// </summary>
	public interface IDisposable : IFixedPointer, System.IDisposable { }
}