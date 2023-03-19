namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Context from memory block fixing.
/// </summary>
internal unsafe abstract class FixedContext : IEquatable<FixedContext>
{
    /// <summary>
    /// Pointer to fixed memory block.
    /// </summary>
    private readonly void* _ptr;
    /// <summary>
    /// Memory block size in bytes.
    /// </summary>
    private readonly Int32 _binaryLength;
    /// <summary>
    /// Indicates whether the memory block is read-only. 
    /// </summary>
    private readonly Boolean _isReadOnly;
    /// <summary>
    /// Indicates whether current instance remains valid.
    /// </summary>
    private readonly IMutableWrapper<Boolean> _isValid;

    /// <summary>
    /// Memory type.
    /// </summary>
    protected abstract Type Type { get; }
    /// <summary>
    /// Memory block size in bytes.
    /// </summary>
    public Int32 BinaryLength => this._binaryLength;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
    protected FixedContext(void* ptr, Int32 binaryLength, Boolean isReadOnly)
    {
        this._ptr = ptr;
        this._binaryLength = binaryLength;
        this._isValid = new Reference<Boolean>(true);
        this._isReadOnly = isReadOnly;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ctx">Fixed context of memory block.</param>
    protected FixedContext(FixedContext ctx)
    {
        this._ptr = ctx._ptr;
        this._binaryLength = ctx._binaryLength;
        this._isValid = ctx._isValid;
        this._isReadOnly = ctx._isReadOnly;
    }

    /// <summary>
    /// Creates a <see cref="Span{TValue}"/> instance over the memory block whose 
    /// length is <paramref name="length"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the objects in the span.</typeparam>
    /// <param name="length">Span length.</param>
    /// <returns>A <see cref="Span{TValue}"/> instance over the memory block.</returns>
    public Span<TValue> CreateSpan<TValue>(Int32 length)
    {
        this.ValidateOperation();
        return new(this._ptr, length);
    }
    /// <summary>
    /// Creates a <see cref="ReadOnlySpan{TValue}"/> instance over the memory block whose 
    /// length is <paramref name="length"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of the objects in the read-only span.</typeparam>
    /// <param name="length">Span length.</param>
    /// <returns>A <see cref="ReadOnlySpan{TValue}"/> instance over the memory block.</returns>
    public ReadOnlySpan<TValue> CreateReadOnlySpan<TValue>(Int32 length)
    {
        this.ValidateOperation(true);
        return new(this._ptr, length);
    }
    /// <summary>
    /// Validates any operation over the fixed memory block.
    /// </summary>
    /// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
    public void ValidateOperation(Boolean isReadOnly = false)
    {
        if (!this._isValid.Value)
            throw new InvalidOperationException("The current context is not valid.");
        if (!isReadOnly && this._isReadOnly)
            throw new InvalidOperationException("The current context is read-only.");
    }
    /// <summary>
    /// Invalidates current context.
    /// </summary>
    public void Unload() => this._isValid.SetInstance(false);

    /// <inheritdoc/>
    public virtual Boolean Equals(FixedContext? other)
        => other is not null && this._ptr == other._ptr &&
        this._binaryLength == other._binaryLength && this._isReadOnly == other._isReadOnly;
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => this.Equals(obj as FixedContext);
    /// <inheritdoc/>
    public override Int32 GetHashCode() => HashCode.Combine(new IntPtr(this._ptr), this._binaryLength, this._isReadOnly, this.Type);
}

/// <summary>
/// Context from memory block fixing.
/// </summary>
/// <typeparam name="T">Type of items on the fixed memory block.</typeparam>
internal unsafe sealed class FixedContext<T> : FixedContext, IFixedContext<T>, IReadOnlyFixedContext<T>, IEquatable<FixedContext<T>>
    where T : unmanaged
{
    /// <summary>
    /// Number of <typeparamref name="T"/> items in memory block.
    /// </summary>
    private readonly Int32 _count;

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
    private FixedContext(FixedContext ctx, Int32 count) : base(ctx)
    {
        this._count = count;
    }

    /// <inheritdoc/>
    public Boolean Equals(FixedContext<T>? other) => this.Equals(other as FixedContext);
    /// <inheritdoc/>
    public override Boolean Equals(FixedContext? other) => base.Equals(other as FixedContext<T>);

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
