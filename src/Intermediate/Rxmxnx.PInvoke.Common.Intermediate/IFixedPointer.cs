namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a fixed memory pointer.
/// </summary>
public interface IFixedPointer
{
    /// <summary>
    /// Pointer to fixed memory.
    /// </summary>
    IntPtr Pointer { get; }
}

