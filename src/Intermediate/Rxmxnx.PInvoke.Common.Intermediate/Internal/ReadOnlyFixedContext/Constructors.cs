namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal unsafe partial class ReadOnlyFixedContext<T>
#pragma warning disable CS8500
{
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
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
	public ReadOnlyFixedContext(void* ptr, Int32 count, FixedValueHandle handle) : base(
		ptr, count * sizeof(T), true, handle)
		=> this.Count = count;
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using an offset and a fixed memory instance.
	/// </summary>
	/// <param name="offset">The offset in the memory block.</param>
	/// <param name="ctx">The fixed memory instance.</param>
	public ReadOnlyFixedContext(Int32 offset, ReadOnlyFixedMemory ctx) : base(ctx, offset)
		=> this.Count = this.BinaryLength / sizeof(T);
#pragma warning restore CS8500

	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a fixed memory instance and a count of items.
	/// </summary>
	/// <param name="ctx">The fixed memory instance.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	private ReadOnlyFixedContext(ReadOnlyFixedMemory ctx, Int32 count) : base(ctx) => this.Count = count;
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedContext{T}"/> instance using a pointer to a
	/// <see langword="null"/> memory.
	/// </summary>
	private ReadOnlyFixedContext() : base(IntPtr.Zero.ToPointer(), 0, true) => this.Count = 0;
#endif
}