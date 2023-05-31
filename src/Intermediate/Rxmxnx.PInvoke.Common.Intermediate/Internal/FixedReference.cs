namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed memory reference class.
/// </summary>
/// <typeparam name="T">Type of the fixed memory reference.</typeparam>
internal unsafe sealed class FixedReference<T> : FixedMemory, IFixedReference<T>, IEquatable<FixedReference<T>>
    where T : unmanaged
{
    /// <inheritdoc/>
    public override Type? Type => typeof(T);
    /// <inheritdoc/>
    public override Int32 BinaryOffset => default;
    /// <inheritdoc/>
    public override Boolean IsFunction => false;

    ref T IReferenceable<T>.Reference => ref base.CreateReference<T>();
    ref readonly T IReadOnlyReferenceable<T>.Reference => ref base.CreateReadOnlyReference<T>();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory reference.</param>
    /// <param name="isReadOnly">Indicates whether the memory reference is read-only.</param>
    public FixedReference(void* ptr, Boolean isReadOnly = false) : base(ptr, sizeof(T), isReadOnly)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mem"><see cref="FixedMemory"/> instance.</param>
    private FixedReference(FixedMemory mem) : base(mem)
    {
    }

    IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IFixedMemory residual)
    {
        IFixedReference<TDestination> result = this.GetTransformation<TDestination>(out FixedOffset fixedOffset);
        residual = fixedOffset;
        return result;
    }
    IFixedReference<TDestination> IFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
    {
        IFixedReference<TDestination> result = this.GetTransformation<TDestination>(out FixedOffset fixedOffset);
        residual = fixedOffset;
        return result;
    }
    IReadOnlyFixedReference<TDestination> IReadOnlyFixedReference<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
    {
        IReadOnlyFixedReference<TDestination> result = this.GetTransformation<TDestination>(out FixedOffset fixedOffset);
        residual = fixedOffset;
        return result;
    }

    /// <summary>
    /// Creates a <see cref="FixedReference{TDestination}"/> instance.
    /// </summary>
    /// <typeparam name="TDestination">Type of items on the reinterpreded memory reference.</typeparam>
    /// <param name="fixedOffset">Output. <see cref="FixedOffset"/> instance.</param>
    /// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
    /// <returns>A <see cref="FixedReference{TDestination}"/> instance.</returns>
    public FixedReference<TDestination> GetTransformation<TDestination>(out FixedOffset fixedOffset, Boolean isReadOnly = false) where TDestination : unmanaged
    {
        base.ValidateOperation(isReadOnly);
        base.ValidateReferenceSize<TDestination>();
        fixedOffset = new FixedOffset(this, sizeof(TDestination));
        return new(this);
    }

    /// <inheritdoc/>
    public Boolean Equals(FixedReference<T>? other) => this.Equals(other as FixedMemory);
    /// <inheritdoc/>
    public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedReference<T>);
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => base.Equals(obj as FixedReference<T>);
    /// <inheritdoc/>
    public override Int32 GetHashCode() => base.GetHashCode();
}
