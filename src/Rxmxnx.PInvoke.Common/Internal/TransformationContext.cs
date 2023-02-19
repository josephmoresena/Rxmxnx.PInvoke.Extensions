namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Reinterpretation of a <typeparamref name="TSource"/> fixed memory block as a 
/// <typeparamref name="TDestination"/> memory block.
/// </summary>
/// <typeparam name="TSource">Type of items on the fixed memory block.</typeparam>
/// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
internal unsafe sealed record TransformationContext<TSource, TDestination> :
    ITransformationContext<TSource, TDestination>, IReadOnlyTransformationContext<TSource, TDestination>
    where TSource : unmanaged
    where TDestination : unmanaged
{
    /// <summary>
    /// Fixed context.
    /// </summary>
    private readonly FixedContext<TSource> _ctx;
    /// <summary>
    /// Transformation length.
    /// </summary>
    private readonly Int32 _length;
    /// <summary>
    /// Transformation offset.
    /// </summary>
    private readonly Int32 _offset;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ctx"><see cref="FixedContext{TSource}"/> instance.</param>
    public TransformationContext(FixedContext<TSource> ctx)
    {
        this._ctx = ctx;
        this._length = ctx.BinaryLength / sizeof(TDestination);
        this._offset = this._length * sizeof(TDestination);
    }

    IFixedContext<TSource> ITransformationContext<TSource, TDestination>.Context => this._ctx;
    IReadOnlyFixedContext<TSource> IReadOnlyTransformationContext<TSource, TDestination>.Context => this._ctx;
    Span<TDestination> ITransformationContext<TSource, TDestination>.Values => this._ctx.CreateSpan<TDestination>(this._length);
    ReadOnlySpan<TDestination> IReadOnlyTransformationContext<TSource, TDestination>.Values => this._ctx.CreateReadOnlySpan<TDestination>(this._length);
    Span<Byte> ITransformationContext<TSource, TDestination>.ResidualBytes => (this._ctx as IFixedContext<TSource>)!.BinaryValues[this._offset..];
    ReadOnlySpan<Byte> IReadOnlyTransformationContext<TSource, TDestination>.ResidualBytes
        => (this._ctx as IReadOnlyFixedContext<TSource>)!.BinaryValues[this._offset..];
    IReadOnlyTransformationContext<TSource, TDestination> ITransformationContext<TSource, TDestination>.AsReadOnly()
    {
        this._ctx.ValidateOperation(true);
        return this;
    }
}
