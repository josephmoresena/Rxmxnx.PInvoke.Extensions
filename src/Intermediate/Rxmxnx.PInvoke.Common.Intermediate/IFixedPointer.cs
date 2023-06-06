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
}

