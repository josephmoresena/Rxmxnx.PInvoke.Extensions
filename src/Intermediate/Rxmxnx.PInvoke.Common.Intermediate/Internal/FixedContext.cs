﻿namespace Rxmxnx.PInvoke.Internal;

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
    /// <inheritdoc/>
    public override Boolean IsFunction => false;
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
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="count">Number of <typeparamref name="T"/> items in memory block.</param>
    /// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
    /// <param name="isValid">Indicates whether current instance remains valid.</param>
    public FixedContext(void* ptr, Int32 count, Boolean isReadOnly, IMutableWrapper<Boolean> isValid) : base(ptr, count * sizeof(T), isReadOnly, isValid)
    {
        this._count = count;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ctx">Fixed context of memory block.</param>
    private FixedContext(FixedMemory ctx) : base(ctx)
    {
        this._count = this.BinaryLength / sizeof(T);
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

    /// <summary>
    /// Creates a <see cref="FixedContext{Byte}"/> instance from <paramref name="fmem"/>.
    /// </summary>
    /// <param name="fmem">A <see cref="IReadOnlyFixedMemory"/> instance.</param>
    /// <returns>A new a <see cref="FixedContext{Byte}"/> instance.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe FixedContext<Byte> CreateBinaryContext(IReadOnlyFixedMemory fmem)
    {
        if (fmem is FixedMemory mem)
            return new(mem);
        else
            fixed (void* ptr = &MemoryMarshal.GetReference(fmem.Bytes))
                return new(ptr, fmem.Bytes.Length, fmem is not IFixedMemory);
    }
}