namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Context from memory block fixing.
/// </summary>
/// <typeparam name="T">Type of items on the fixed memory block.</typeparam>
internal unsafe sealed class FixedContext<T> : FixedMemory, IFixedContext<T>, IReadOnlyFixedContext<T>, IEquatable<FixedContext<T>>
    where T : unmanaged
{
    /// <summary>
    /// Number of <typeparamref name="T"/> items in memory block.
    /// </summary>
    private readonly Int32 _count;

    /// <summary>
    /// Number of <typeparamref name="T"/> items in memory block.
    /// </summary>
    public Int32 Count => this._count;

    /// <inheritdoc/>
    protected override Type Type => typeof(T);

    Span<T> IFixedContext<T>.Values => base.CreateSpan<T>(this._count);
    Span<Byte> IFixedContext<T>.BinaryValues => base.CreateSpan<Byte>(base.BinaryLength);
    ReadOnlySpan<T> IReadOnlyFixedContext<T>.Values => base.CreateReadOnlySpan<T>(this._count);
    ReadOnlySpan<Byte> IReadOnlyFixedContext<T>.BinaryValues => base.CreateReadOnlySpan<Byte>(base.BinaryLength);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="count">Number of <typeparamref name="T"/> items in memory block.</param>
    /// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
    public FixedContext(void* ptr, Int32 count, Boolean isReadOnly = false) : base(ptr, count * sizeof(T), isReadOnly)
    {
        this._count = count;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ctx">Fixed context of memory block.</param>
    /// <param name="count">Number of <typeparamref name="T"/> items in memory block.</param>
    private FixedContext(FixedMemory ctx, Int32 count) : base(ctx)
    {
        this._count = count;
    }

    /// <inheritdoc/>
    public Boolean Equals(FixedContext<T>? other) => this.Equals(other as FixedMemory);
    /// <inheritdoc/>
    public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedContext<T>);
    /// <inheritdoc/>
    public override bool Equals(Object? obj) => base.Equals(obj as FixedContext<T>);
    /// <inheritdoc/>
    public override Int32 GetHashCode() => base.GetHashCode();

    ITransformationContext<T, TDestination> IFixedContext<T>.Transformation<TDestination>() => this.GetTransformation<TDestination>();
    IReadOnlyTransformationContext<T, TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>() => this.GetTransformation<TDestination>(true);

    /// <summary>
    /// Creates a <see cref="TransformationContext{T, TDestination}"/> instance.
    /// </summary>
    /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
    /// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
    /// <returns>A <see cref="TransformationContext{T, TDestination}"/> instance.</returns>
    public TransformationContext<T, TDestination> GetTransformation<TDestination>(Boolean isReadOnly = false) where TDestination : unmanaged
    {
        this.ValidateOperation(isReadOnly);
        Int32 count = this.BinaryLength / sizeof(TDestination);
        Int32 offset = count * sizeof(TDestination);
        return new(this, new(this, count), offset);
    }
}
