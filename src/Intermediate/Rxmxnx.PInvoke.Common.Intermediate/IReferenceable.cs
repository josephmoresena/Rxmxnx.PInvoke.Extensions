namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a reference to an object of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the referenced object.</typeparam>
public interface IReferenceable<T> : IReadOnlyReferenceable<T>, IEquatable<IReferenceable<T>>
{
    /// <summary>
    /// Reference to the instance of an object of type <typeparamref name="T"/>.
    /// </summary>
    new ref T Reference { get; }

    ref readonly T IReadOnlyReferenceable<T>.Reference => ref this.Reference;

    [ExcludeFromCodeCoverage]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    Boolean IEquatable<IReferenceable<T>>.Equals(IReferenceable<T>? other)
        => other is not null && Unsafe.AreSame(ref this.Reference, ref other.Reference);
}
