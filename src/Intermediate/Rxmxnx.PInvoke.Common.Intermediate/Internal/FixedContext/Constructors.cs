namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe partial class FixedContext<T>
{
	/// <summary>
	/// Constructs a new <see cref="FixedContext{T}"/> instance using a fixed memory instance and a count of items.
	/// </summary>
	/// <param name="ctx">The fixed memory instance.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	private FixedContext(FixedMemory ctx, Int32 count) : base(ctx) => this.Count = count;
#pragma warning disable CS8500
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
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
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
#endif
#pragma warning restore CS8500
}