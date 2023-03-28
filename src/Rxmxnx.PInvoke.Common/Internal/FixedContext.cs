namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Context from memory block fixing.
/// </summary>
/// <typeparam name="T">Type of items on the fixed memory block.</typeparam>
internal unsafe sealed class FixedContext<T> : FixedMemory, IFixedContext<T>, IEquatable<FixedContext<T>>
    where T : unmanaged
{
    /// <summary>
    /// Number of <typeparamref name="T"/> items in memory block.
    /// </summary>
    private readonly Int32 _count;

    /// <inheritdoc/>
    public override Int32 BinaryOffset => default;
    /// <inheritdoc/>
    public override Type? Type => typeof(T);
    /// <summary>
    /// Number of <typeparamref name="T"/> items in memory block.
    /// </summary>
    public Int32 Count => this._count;

    Span<T> IFixedMemory<T>.Values => base.CreateSpan<T>(this._count);
    ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => base.CreateReadOnlySpan<T>(this._count);

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

    IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
    {
        IFixedContext<TDestination> result = this.GetTransformation<TDestination>(out FixedOffset fixedOffset);
        residual = fixedOffset;
        return result;
    }
    IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
    {
        IReadOnlyFixedContext<TDestination> result = this.GetTransformation<TDestination>(out FixedOffset fixedOffset, true);
        residual = fixedOffset;
        return result;
    }
    IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
    {
        IFixedContext<TDestination> result = this.GetTransformation<TDestination>(out FixedOffset fixedOffset, true);
        residual = fixedOffset;
        return result;
    }

    /// <summary>
    /// Creates a <see cref="FixedContext{TDestination}"/> instance.
    /// </summary>
    /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
    /// <param name="fixedOffset">Output. <see cref="FixedOffset"/> instance.</param>
    /// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
    /// <returns>A <see cref="FixedContext{TDestination}"/> instance.</returns>
    public FixedContext<TDestination> GetTransformation<TDestination>(out FixedOffset fixedOffset, Boolean isReadOnly = false) where TDestination : unmanaged
    {
        base.ValidateOperation(isReadOnly);
        Int32 count = this.GetCount<TDestination>();
        Int32 offset = count * sizeof(TDestination);
        fixedOffset = new(this, offset);
        return new(this, count);
    }

    /// <inheritdoc/>
    public Boolean Equals(FixedContext<T>? other) => this.Equals(other as FixedMemory);
    /// <inheritdoc/>
    public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedContext<T>);
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => base.Equals(obj as FixedContext<T>);
    /// <inheritdoc/>
    public override Int32 GetHashCode() => base.GetHashCode();
}
