namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed memory block for a specific type.
/// </summary>
/// <typeparam name="T">
/// The type of the items in the fixed memory block. Must be <see langword="unmanaged"/>.
/// </typeparam>
internal unsafe sealed class FixedContext<T> : FixedMemory, IFixedContext<T>, IEquatable<FixedContext<T>>
    where T : unmanaged
{
    /// <summary>
    /// The number of items of type <typeparamref name="T"/> in the memory block.
    /// </summary>
    private readonly Int32 _count;

    /// <inheritdoc/>
    public override Int32 BinaryOffset => default;
    /// <inheritdoc/>
    public override Type? Type => typeof(T);
    /// <inheritdoc/>
    public override Boolean IsFunction => false;

    /// <summary>
    /// Gets the number of items of type <typeparamref name="T"/> in the memory block.
    /// </summary>
    public Int32 Count => this._count;

    Span<T> IFixedMemory<T>.Values => base.CreateSpan<T>(this._count);
    ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => base.CreateReadOnlySpan<T>(this._count);
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
    IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.GetTransformation<Byte>(out _, true);

    /// <summary>
    /// Constructs a new <see cref="FixedContext{T}"/> instance using a pointer to a fixed memory block,
    /// and a count of items.
    /// </summary>
    /// <param name="ptr">The pointer to the fixed memory block.</param>
    /// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
    /// <param name="isReadOnly">A value that indicates whether the memory block is read-only.</param>
    public FixedContext(void* ptr, Int32 count) : base(ptr, count * sizeof(T))
    {
        this._count = count;
    }
    /// <summary>
    /// Constructs a new <see cref="FixedContext{T}"/> instance using a pointer to a fixed memory block,
    /// a count of items, and a validity wrapper.
    /// </summary>
    /// <param name="ptr">The pointer to the fixed memory block.</param>
    /// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
    /// <param name="isReadOnly">A value that indicates whether the memory block is read-only.</param>
    /// <param name="isValid">A mutable wrapper that indicates whether the current instance remains valid.</param>
    public FixedContext(void* ptr, Int32 count, IMutableWrapper<Boolean> isValid) : base(ptr, count * sizeof(T), isValid)
    {
        this._count = count;
    }
    /// <summary>
    /// Constructs a new <see cref="FixedContext{T}"/> instance using an offset and a fixed memory instance.
    /// </summary>
    /// <param name="offset">The offset in the memory block.</param>
    /// <param name="ctx">The fixed memory instance.</param>
    public FixedContext(Int32 offset, FixedMemory ctx) : base(ctx, offset)
    {
        this._count = this.BinaryLength / sizeof(T);
    }

    /// <summary>
    /// Constructs a new <see cref="FixedContext{T}"/> instance using a fixed memory instance and a count of items.
    /// </summary>
    /// <param name="ctx">The fixed memory instance.</param>
    /// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
    private FixedContext(FixedMemory ctx, Int32 count) : base(ctx)
    {
        this._count = count;
    }

    /// <summary>
    /// Transforms the current memory context into a different type, and provides a fixed offset that represents the
    /// remaining portion of memory not included in the newly formed context.
    /// </summary>
    /// <typeparam name="TDestination">The type into which the current memory context should be transformed.</typeparam>
    /// <param name="fixedOffset">
    /// Output. Provides a fixed offset that represents the remaining portion of memory that is not included in the new context.
    /// This is calculated based on the size of the new type compared to the size of the original memory block.
    /// </param>
    /// A new instance of FixedContext for the destination type, which represents a fixed memory context of the new type.
    /// </returns>
    /// <remarks>
    /// If the size of the new type exceeds the total length of the current context, the resulting context will be empty, and
    /// <paramref name="fixedOffset"/> will contain all the memory from the original context.
    /// Conversely, if a type of lesser size is chosen, the resulting context will have a greater length, and
    /// <paramref name="fixedOffset"/> will represent the remaining memory not included in the new context.
    /// </remarks>
    public FixedContext<TDestination> GetTransformation<TDestination>(out FixedOffset fixedOffset, Boolean isReadOnly = false) where TDestination : unmanaged
    {
        base.ValidateOperation(isReadOnly);
        Int32 count = base.GetCount<TDestination>();
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
    /// <inheritdoc/>
    public override IFixedContext<Byte> AsBinaryContext() => this.GetTransformation<Byte>(out _);
}
