#if NETSTANDARD2_1 || NETCOREAPP
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed memory block for a specific type.
/// </summary>
/// <typeparam name="T">The type of the items in the fixed memory block.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe partial class FixedContext<T> : FixedMemory,
#if !OBSOLETE_FIXED_INTERFACES
	IFixedContext<T>
#else
#pragma warning disable CS0612
	IObsoleteFixedContext<T>
#pragma warning restore CS0612
#endif
{
#pragma warning disable CS8500
	/// <summary>
	/// An empty instance of <see cref="FixedContext{T}"/>.
	/// </summary>
	public static readonly FixedContext<T> Empty = new();
	/// <summary>
	/// An empty instance of <see cref="IFixedContext{T}.IDisposable"/>.
	/// </summary>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	public static readonly IFixedContext<T>.IDisposable EmptyDisposable = Disposable.Default;

	/// <summary>
	/// Gets the number of items of type <typeparamref name="T"/> in the memory block.
	/// </summary>
	public Int32 Count { get; }

	/// <inheritdoc/>
	public override Int32 BinaryOffset => default;
	/// <inheritdoc/>
	public override Boolean IsUnmanaged => !RuntimeHelpers.IsReferenceOrContainsReferences<T>();
	/// <inheritdoc/>
	public override Type Type => typeof(T);
	/// <inheritdoc/>
	public override Boolean IsFunction => false;

	/// <summary>
	/// Constructs a new <see cref="FixedContext{T}"/> instance using a pointer to a fixed memory block,
	/// and a count of items.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	public FixedContext(void* ptr, Int32 count) : base(ptr, count * sizeof(T)) => this.Count = count;
	/// <summary>
	/// Constructs a new <see cref="FixedContext{T}"/> instance using an offset and a fixed memory instance.
	/// </summary>
	/// <param name="offset">The offset in the memory block.</param>
	/// <param name="ctx">The fixed memory instance.</param>
	public FixedContext(Int32 offset, FixedMemory ctx) : base(ctx, offset)
		=> this.Count = this.BinaryLength / sizeof(T);
	/// <summary>
	/// Constructs a new <see cref="FixedContext{T}"/> instance using a pointer to a fixed memory block,
	/// a count of items, and a validity wrapper.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if NET9_0_OR_GREATER
	public FixedContext(void* ptr, Int32 count, FixedValueHandle handle) : base(ptr, count * sizeof(T), handle)
#else
	private FixedContext(void* ptr, Int32 count, FixedValueHandle handle) : base(ptr, count * sizeof(T), handle)
#endif
		=> this.Count = count;
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a pointer to a
	/// <see langword="null"/> memory.
	/// </summary>
	private FixedContext() : base(IntPtr.Zero.ToPointer(), 0) => this.Count = 0;
	/// <summary>
	/// Constructs a new <see cref="FixedContext{T}"/> instance using a fixed memory instance and a count of items.
	/// </summary>
	/// <param name="ctx">The fixed memory instance.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	private FixedContext(FixedMemory ctx, Int32 count) : base(ctx) => this.Count = count;

	Span<T> IFixedMemory<T>.Values => this.CreateSpan<T>(this.Count);
	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.CreateReadOnlySpan<T>(this.Count);
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
	{
#if !NETCOREAPP || NETCOREAPP3_1_OR_GREATER
		Unsafe.SkipInit(out residual);
#else
		residual = default!;
#endif
		IFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IFixedMemory, FixedOffset>(ref residual));
		return result;
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IReadOnlyFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual), true);
		return result;
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual), true);
		return result;
	}
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.GetTransformation<Byte>(out _, true);

	/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	public override IFixedContext<Byte> AsBinaryContext() => this.GetTransformation<Byte>(out _);
	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();

	/// <summary>
	/// Transforms the current memory context into a different type, and provides a fixed offset that represents the
	/// remaining portion of memory not included in the newly formed context.
	/// </summary>
	/// <typeparam name="TDestination">The type into which the current memory context should be transformed.</typeparam>
	/// <param name="fixedOffset">
	/// Output. Provides a fixed offset that represents the remaining portion of memory that is not included in the new
	/// context.
	/// This is calculated based on the size of the new type compared to the size of the original memory block.
	/// </param>
	/// <param name="isReadOnly">Indicates whether the transformation operation should be performed as a read-only operation.</param>
	/// <returns>
	/// A new instance of FixedContext for the destination type, which represents a fixed memory context of the new type.
	/// </returns>
	/// <remarks>
	/// If the size of the new type exceeds the total length of the current context, the resulting context will be empty, and
	/// <paramref name="fixedOffset"/> will contain all the memory from the original context.
	/// Conversely, if a type of lesser size is chosen, the resulting context will have a greater length, and
	/// <paramref name="fixedOffset"/> will represent the remaining memory not included in the new context.
	/// </remarks>
	public FixedContext<TDestination> GetTransformation<TDestination>(out FixedOffset fixedOffset,
		Boolean isReadOnly = false)
	{
		this.ValidateOperation(isReadOnly);
		this.ValidateTransformation(typeof(TDestination),
		                            !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this.GetCount(sizeOf);
		Int32 offset = count * sizeOf;
		fixedOffset = new(this, offset);
		return new(this, count);
	}
#pragma warning restore CS8500

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IFixedContext{T}.IDisposable"/> instance from
	/// current reference pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Optional object to dispose in order to free unmanaged resources.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing a fixed reference.</returns>
	/// <remarks>
	/// This method serves as a reference for the assembly patcher in .NET 9.0+. It is important to keep the
	/// attributes of its parameters compatible.
	/// </remarks>
#if OBSOLETE_FIXED_INTERFACES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterfaceExtensions, ObsoleteConstants.ErrorFixedInterface)]
#endif
	public static IFixedContext<T>.IDisposable CreateDisposable(ValPtr<T> valPtr, Int32 count,
		IDisposable? disposable = default)
	{
		FixedContext<T> ctx = new(valPtr, count);
		return ctx.ToDisposable(disposable);
	}
	/// <summary>
	/// Creates a new <see cref="ReadOnlyFixedMemory"/> instance.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
	/// <returns>A new <see cref="ReadOnlyFixedMemory"/> instance.</returns>
#if !PACKAGE && NET5_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlyFixedMemory CreateInstance(IntPtr ptr, Int32 count, FixedValueHandle handle)
		=> new FixedContext<T>(ptr.ToPointer(), count, handle);
#if NET5_0_OR_GREATER
	/// <summary>
	/// Creates a new <see cref="ReadOnlyFixedMemory"/> instance.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
	/// <returns>A new <see cref="ReadOnlyFixedMemory"/> instance.</returns>
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ReadOnlyFixedMemory CreateInstance(void* ptr, Int32 count, FixedValueHandle handle)
		=> new FixedContext<T>(ptr, count, handle);
#endif
}
#endif