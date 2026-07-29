namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for managing fixed memory blocks.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal abstract unsafe partial class FixedMemory : ReadOnlyFixedMemory, IEquatable<FixedMemory>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	, IFixedMemory
#endif
{
	/// <summary>
	/// Constructs a new <see cref="FixedMemory"/> instance using a pointer to a memory block, and its size.
	/// </summary>
	/// <param name="ptr">Pointer to fixed memory block.</param>
	/// <param name="binaryLength">Memory block size in bytes.</param>
	protected FixedMemory(void* ptr, Int32 binaryLength) : base(ptr, binaryLength, false) { }
	/// <summary>
	/// Constructs a new <see cref="FixedMemory"/> instance using another instance as a template.
	/// </summary>
	/// <param name="mem">The <see cref="FixedMemory"/> instance to copy data from.</param>
	protected FixedMemory(FixedMemory mem) : base(mem) { }
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Constructs a new <see cref="FixedMemory"/> instance using another instance as a template and specifying a
	/// memory offset.
	/// </summary>
	/// <param name="mem">The <see cref="FixedMemory"/> instance to copy data from.</param>
	/// <param name="offset">The offset to be added to the pointer to the memory block.</param>
	protected FixedMemory(FixedMemory mem, Int32 offset) : base(mem, offset) { }
	/// <summary>
	/// Constructs a new <see cref="FixedMemory"/> instance using a pointer to a memory block, its size, and a valid status.
	/// </summary>
	/// <param name="ptr">Pointer to fixed memory block.</param>
	/// <param name="binaryLength">Memory block size in bytes.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	protected FixedMemory(void* ptr, Int32 binaryLength, FixedValueHandle handle) : base(
		ptr, binaryLength, false, handle) { }

	Span<Byte> IFixedMemory.Bytes => this.CreateBinarySpan();
	Span<Object> IFixedMemory.Objects => this.CreateObjectSpan();
	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.CreateReadOnlyBinarySpan();
	ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => this.CreateReadOnlyObjectSpan();
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();

	/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	public new virtual IFixedContext<Byte> AsBinaryContext()
	{
		this.ValidateUnmanagedOperation();
		return new FixedContext<Byte>(this.BinaryOffset, this);
	}
	/// <inheritdoc/>
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	public new virtual IFixedContext<Object> AsObjectContext()
	{
		this.ValidateReferenceOperation();
		return new FixedContext<Object>(this.BinaryOffset, this);
	}
#endif
}