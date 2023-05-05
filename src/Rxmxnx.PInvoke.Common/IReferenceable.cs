namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a reference to <typeparamref name="T"/> object.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
public interface IReferenceable<T> : IReadOnlyReferenceable<T>, IEquatable<IReferenceable<T>>
{
    /// <summary>
    /// Reference to instance <typeparamref name="T"/> object.
    /// </summary>
    new ref T Reference { get; }

    ref readonly T IReadOnlyReferenceable<T>.Reference => ref this.Reference;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Boolean IEquatable<IReferenceable<T>>.Equals(IReferenceable<T>? other)
        => other is not null && Unsafe.AreSame(ref this.Reference, ref other.Reference);
}
