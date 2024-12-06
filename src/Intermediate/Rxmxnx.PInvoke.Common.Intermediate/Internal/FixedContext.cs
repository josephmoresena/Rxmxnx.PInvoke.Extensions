namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed memory block for a specific type.
/// </summary>
/// <typeparam name="T">The type of the items in the fixed memory block.</typeparam>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal sealed unsafe partial class FixedContext<T> : FixedMemory, IFixedContext<T>
{
#pragma warning disable CS8500
	/// <summary>
	/// An empty instance of <see cref="FixedContext{T}"/>.
	/// </summary>
	public static readonly FixedContext<T> Empty = new();

	/// <summary>
	/// The number of items of type <typeparamref name="T"/> in the memory block.
	/// </summary>
	private readonly Int32 _count;

	/// <summary>
	/// Gets the number of items of type <typeparamref name="T"/> in the memory block.
	/// </summary>
	public Int32 Count => this._count;

	/// <inheritdoc/>
	public override Int32 BinaryOffset => default;
	/// <inheritdoc/>
	public override Boolean IsUnmanaged => ReadOnlyValPtr<T>.IsUnmanaged;
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
	public FixedContext(void* ptr, Int32 count) : base(ptr, count * sizeof(T)) => this._count = count;
	/// <summary>
	/// Constructs a new <see cref="FixedContext{T}"/> instance using an offset and a fixed memory instance.
	/// </summary>
	/// <param name="offset">The offset in the memory block.</param>
	/// <param name="ctx">The fixed memory instance.</param>
	public FixedContext(Int32 offset, FixedMemory ctx) : base(ctx, offset)
		=> this._count = this.BinaryLength / sizeof(T);
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a pointer to a
	/// <see langword="null"/> memory.
	/// </summary>
	private FixedContext() : base(IntPtr.Zero.ToPointer(), 0) => this._count = 0;
	/// <summary>
	/// Constructs a new <see cref="FixedContext{T}"/> instance using a fixed memory instance and a count of items.
	/// </summary>
	/// <param name="ctx">The fixed memory instance.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	private FixedContext(FixedMemory ctx, Int32 count) : base(ctx) => this._count = count;

	Span<T> IFixedMemory<T>.Values => this.CreateSpan<T>(this._count);
	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.CreateReadOnlySpan<T>(this._count);
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IFixedMemory, FixedOffset>(ref residual));
		return result;
	}
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IReadOnlyFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual), true);
		return result;
	}
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		IFixedContext<TDestination> result =
			this.GetTransformation<TDestination>(out Unsafe.As<IReadOnlyFixedMemory, FixedOffset>(ref residual), true);
		return result;
	}
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.GetTransformation<Byte>(out _, true);

	/// <inheritdoc/>
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
		this.ValidateTransformation(typeof(TDestination), ReadOnlyValPtr<TDestination>.IsUnmanaged);
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this.GetCount(sizeOf);
		Int32 offset = count * sizeOf;
		fixedOffset = new(this, offset);
		return new(this, count);
	}
#pragma warning restore CS8500
}