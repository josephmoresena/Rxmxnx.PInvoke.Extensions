namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Reinterpretation of a <typeparamref name="TSource"/> fixed memory block as a 
/// <typeparamref name="TDestination"/> memory block.
/// </summary>
/// <typeparam name="TSource">Type of items on the fixed memory block.</typeparam>
/// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
internal unsafe sealed record TransformationContext<TSource, TDestination> : ITransformationContext<TSource, TDestination>, IReadOnlyTransformationContext<TSource, TDestination>
    where TSource : unmanaged
    where TDestination : unmanaged
{
    /// <summary>
    /// Fixed context.
    /// </summary>
    private readonly FixedContext<TSource> _ctx0;
    /// <summary>
    /// Transformed fixed context.
    /// </summary>
    private readonly FixedContext<TDestination> _ctx1;
    /// <summary>
    /// Transformation offset.
    /// </summary>
    private readonly Int32 _offset;

    /// <summary>
    /// Transformation offset.
    /// </summary>
    public Int32 Offset => this._offset;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ctx0"><see cref="FixedContext{TSource}"/> instance.</param>
    /// <param name="ctx1"><see cref="FixedContext{TDestination}"/> instance.</param>
    /// <param name="offset">Transformation offset.</param>
    public TransformationContext(FixedContext<TSource> ctx0, FixedContext<TDestination> ctx1, Int32 offset)
    {
        this._ctx0 = ctx0;
        this._ctx1 = ctx1;
        this._offset = offset;
    }

    IFixedContext<TSource> ITransformationContext<TSource, TDestination>.Context => this._ctx0;
    IReadOnlyFixedContext<TSource> IReadOnlyTransformationContext<TSource, TDestination>.Context => this._ctx0;
    IFixedContext<TDestination> ITransformedMemory<IFixedContext<TDestination>, IReadOnlyFixedContext<TDestination>>.Transformation => this._ctx1;
    IReadOnlyFixedContext<TDestination> IReadOnlyTransformedMemory<IReadOnlyFixedContext<TDestination>>.Transformation => this._ctx1;
    Span<TDestination> ITransformationContext<TSource, TDestination>.Values => (this._ctx1 as IFixedContext<TDestination>)!.Values;
    ReadOnlySpan<TDestination> IReadOnlyTransformationContext<TSource, TDestination>.Values => (this._ctx1 as IReadOnlyFixedContext<TDestination>)!.Values;
    Span<Byte> ITransformedMemory.ResidualBytes => this._ctx0.CreateBinarySpan(this._offset);
    ReadOnlySpan<Byte> IReadOnlyTransformedMemory.ResidualBytes => this._ctx0.CreateReadOnlyBinarySpan(this._offset);

    /// <summary>
    /// Implicit operator. <see cref="TransformationContext{TSource, TDestination}"/> -> <see cref="FixedContext{TSource}"/>.
    /// </summary>
    /// <param name="transformation"><see cref="TransformationContext{TSource, TDestination}"/> instance.</param>
    public static implicit operator FixedContext<TSource>(TransformationContext<TSource, TDestination> transformation) => transformation._ctx0;
    /// <summary>
    /// Implicit operator. <see cref="TransformationContext{TSource, TDestination}"/> -> <see cref="FixedContext{TDestination}"/>.
    /// </summary>
    /// <param name="transformation"><see cref="TransformationContext{TSource, TDestination}"/> instance.</param>
    public static implicit operator FixedContext<TDestination>(TransformationContext<TSource, TDestination> transformation) => transformation._ctx1;
}
