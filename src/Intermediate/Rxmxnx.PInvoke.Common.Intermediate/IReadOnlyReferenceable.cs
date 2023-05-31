namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a read-only reference to <typeparamref name="T"/> object.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
public interface IReadOnlyReferenceable<T> : IEquatable<IReadOnlyReferenceable<T>>
{
    /// <summary>
    /// Reference to instance <typeparamref name="T"/> object.
    /// </summary>
    ref readonly T Reference { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Boolean IEquatable<IReadOnlyReferenceable<T>>.Equals(IReadOnlyReferenceable<T>? other)
        => other is not null && Unsafe.AreSame(ref Unsafe.AsRef(this.Reference), ref Unsafe.AsRef(other.Reference));
}
