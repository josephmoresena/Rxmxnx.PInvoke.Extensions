namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed read-only memory block for a specific type.
/// </summary>
/// <typeparam name="T">The type of the items in the fixed memory block.</typeparam>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal sealed unsafe partial class ReadOnlyFixedContext<T> : ReadOnlyFixedMemory, IReadOnlyFixedContext<T>
{
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <returns>A <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance.</returns>
	/// <remarks>
	/// This method serves as a reference for the assembly patcher in .NET 9.0+. It is important to keep the
	/// attributes of its parameters compatible.
	/// </remarks>
	public static IReadOnlyFixedContext<T>.IDisposable CreateDisposable(ReadOnlyValPtr<T> valPtr, Int32 count,
		IDisposable? disposable = default)
	{
		ReadOnlyFixedContext<T> ctx = new(valPtr, count);
		return ctx.ToDisposable(disposable);
	}
#pragma warning disable CS8500
	/// <summary>
	/// An empty instance of <see cref="ReadOnlyFixedContext{T}"/>.
	/// </summary>
	public static readonly ReadOnlyFixedContext<T> Empty = new();
	/// <summary>
	/// An empty instance of <see cref="IReadOnlyFixedContext{T}.IDisposable"/>.
	/// </summary>
	public static readonly IReadOnlyFixedContext<T>.IDisposable EmptyDisposable = Disposable.Default;

	/// <inheritdoc/>
	public override Boolean IsUnmanaged => !RuntimeHelpers.IsReferenceOrContainsReferences<T>();

	/// <summary>
	/// Gets the number of items of type <typeparamref name="T"/> in the memory block.
	/// </summary>
	public Int32 Count { get; }

	/// <inheritdoc/>
	public override Int32 BinaryOffset => default;
	/// <inheritdoc/>
	public override Type Type => typeof(T);
	/// <inheritdoc/>
	public override Boolean IsFunction => false;

	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a pointer to a fixed memory block,
	/// and a count of items.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	public ReadOnlyFixedContext(void* ptr, Int32 count) : base(ptr, count * sizeof(T), true) => this.Count = count;
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a pointer to a fixed memory block,
	/// a count of items, and a validity wrapper.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="isValid">A mutable wrapper that indicates whether the current instance remains valid.</param>
	public ReadOnlyFixedContext(void* ptr, Int32 count, IMutableWrapper<Boolean> isValid) : base(
		ptr, count * sizeof(T), true, isValid)
		=> this.Count = count;
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using an offset and a fixed memory instance.
	/// </summary>
	/// <param name="offset">The offset in the memory block.</param>
	/// <param name="ctx">The fixed memory instance.</param>
	public ReadOnlyFixedContext(Int32 offset, ReadOnlyFixedMemory ctx) : base(ctx, offset)
		=> this.Count = this.BinaryLength / sizeof(T);

	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a pointer to a
	/// <see langword="null"/> memory.
	/// </summary>
	private ReadOnlyFixedContext() : base(IntPtr.Zero.ToPointer(), 0, true) => this.Count = 0;

	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a fixed memory instance and a count of items.
	/// </summary>
	/// <param name="ctx">The fixed memory instance.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	private ReadOnlyFixedContext(ReadOnlyFixedMemory ctx, Int32 count) : base(ctx) => this.Count = count;

	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.CreateReadOnlySpan<T>(this.Count);
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IReadOnlyFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(
				out Unsafe.As<IReadOnlyFixedMemory, ReadOnlyFixedOffset>(ref residual));
		return result;
	}
	/// <inheritdoc cref="IReadOnlyFixedMemory.AsBinaryContext()"/>
	public override IReadOnlyFixedContext<Byte> AsBinaryContext() => this.GetTransformation<Byte>(out _);

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
	/// <returns>
	/// A new instance of FixedContext for the destination type, which represents a fixed memory context of the new type.
	/// </returns>
	/// <remarks>
	/// If the size of the new type exceeds the total length of the current context, the resulting context will be empty, and
	/// <paramref name="fixedOffset"/> will contain all the memory from the original context.
	/// Conversely, if a type of lesser size is chosen, the resulting context will have a greater length, and
	/// <paramref name="fixedOffset"/> will represent the remaining memory not included in the new context.
	/// </remarks>
	public ReadOnlyFixedContext<TDestination> GetTransformation<TDestination>(out ReadOnlyFixedOffset fixedOffset)
	{
		this.ValidateOperation(true);
		this.ValidateTransformation(typeof(TDestination),
		                            !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this.GetCount(sizeOf);
		Int32 offset = count * sizeOf;
		fixedOffset = new(this, offset);
		return new(this, count);
	}
#pragma warning restore CS8500
}