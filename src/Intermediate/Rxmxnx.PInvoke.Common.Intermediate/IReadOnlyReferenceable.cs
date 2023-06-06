namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a read-only reference to an object of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the referenced object.</typeparam>
public interface IReadOnlyReferenceable<T> : IEquatable<IReadOnlyReferenceable<T>>
{
    /// <summary>
    /// Read-only reference to the instance of an object of type <typeparamref name="T"/>.
    /// </summary>
    ref readonly T Reference { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Boolean IEquatable<IReadOnlyReferenceable<T>>.Equals(IReadOnlyReferenceable<T>? other)
        => other is not null && Unsafe.AreSame(ref Unsafe.AsRef(this.Reference), ref Unsafe.AsRef(other.Reference));
}
