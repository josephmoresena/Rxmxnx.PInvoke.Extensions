namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a read-only reference to an object of type <typeparamref name="T"/>,
/// allowing the object to be used without modification.
/// </summary>
/// <typeparam name="T">The type of the object that the reference points to.</typeparam>
public interface IReadOnlyReferenceable<T> : IEquatable<IReadOnlyReferenceable<T>>
{
    /// <summary>
    /// Gets the read-only reference to the instance of an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>This reference cannot be used to modify the object.</remarks>
    ref readonly T Reference { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Boolean IEquatable<IReadOnlyReferenceable<T>>.Equals(IReadOnlyReferenceable<T>? other)
        => other is not null && Unsafe.AreSame(ref Unsafe.AsRef(this.Reference), ref Unsafe.AsRef(other.Reference));
}
