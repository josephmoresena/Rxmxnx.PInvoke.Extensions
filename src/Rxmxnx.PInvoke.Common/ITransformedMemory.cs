﻿namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a fixed memory block.
/// </summary>
public interface ITransformedMemory : IReadOnlyTransformedMemory
{
    /// <summary>
    /// A read-only binary span over the residual fixed memory block. 
    /// </summary>
    new Span<Byte> ResidualBytes { get; }
}

/// <summary>
/// This interface represents the reinterpretation of a fixed memory block.
/// </summary>
/// <typeparam name="TMemory">Type of fixed memory.</typeparam>
/// <typeparam name="TReadOnlyMemory">Type of read-only fixed memory.</typeparam>
public interface ITransformedMemory<out TMemory, out TReadOnlyMemory> : ITransformedMemory, IReadOnlyTransformedMemory<TReadOnlyMemory>
    where TMemory : TReadOnlyMemory, IFixedMemory
    where TReadOnlyMemory : IReadOnlyFixedMemory
{
    /// <summary>
    /// Fixed transformed memory instance.
    /// </summary>
    new TMemory Transformation { get; }
}

/// <summary>
/// This interface represents the reinterpretation of a fixed memory block.
/// </summary>
/// <typeparam name="TMemory">Type of fixed memory.</typeparam>
/// <typeparam name="TReadOnlyMemory">Type of read-only fixed memory.</typeparam>
/// <typeparam name="TSource">Original type of fixed memory.</typeparam>
/// <typeparam name="TDestination">Destination type of fixed memory.</typeparam>
public interface ITransformedMemory<out TMemory, out TReadOnlyMemory, TSource, TDestination> : ITransformedMemory<TMemory, TReadOnlyMemory>, IReadOnlyTransformedMemory<TReadOnlyMemory, TSource, TDestination>
    where TMemory : TReadOnlyMemory, IFixedMemory
    where TReadOnlyMemory : IReadOnlyFixedMemory
    where TSource : unmanaged
    where TDestination : unmanaged
{
}