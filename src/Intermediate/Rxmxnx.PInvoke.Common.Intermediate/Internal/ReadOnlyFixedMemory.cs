﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for managing fixed read-only memory blocks.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal abstract unsafe partial class ReadOnlyFixedMemory : FixedPointer, IReadOnlyFixedMemory
{
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedMemory"/> instance using a pointer to a memory block, its size, and
	/// a read-only flag.
	/// </summary>
	/// <param name="ptr">Pointer to fixed memory block.</param>
	/// <param name="binaryLength">Memory block size in bytes.</param>
	/// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
	protected ReadOnlyFixedMemory(void* ptr, Int32 binaryLength, Boolean isReadOnly) : base(
		ptr, binaryLength, isReadOnly) { }
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedMemory"/> instance using a pointer to a memory block, its size,
	/// a read-only flag, and a valid status.
	/// </summary>
	/// <param name="ptr">Pointer to fixed memory block.</param>
	/// <param name="binaryLength">Memory block size in bytes.</param>
	/// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
	/// <param name="isValid">Indicates whether current instance remains valid.</param>
	protected ReadOnlyFixedMemory(void* ptr, Int32 binaryLength, Boolean isReadOnly, IMutableWrapper<Boolean> isValid) :
		base(ptr, binaryLength, isReadOnly, isValid) { }
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedMemory"/> instance using another instance as a template.
	/// </summary>
	/// <param name="mem">The <see cref="ReadOnlyFixedMemory"/> instance to copy data from.</param>
	protected ReadOnlyFixedMemory(ReadOnlyFixedMemory mem) : base(mem) { }
	/// <summary>
	/// Constructs a new <see cref="ReadOnlyFixedMemory"/> instance using another instance as a template and specifying a
	/// memory offset.
	/// </summary>
	/// <param name="mem">The <see cref="ReadOnlyFixedMemory"/> instance to copy data from.</param>
	/// <param name="offset">The offset to be added to the pointer to the memory block.</param>
	protected ReadOnlyFixedMemory(ReadOnlyFixedMemory mem, Int32 offset) : base(mem, offset) { }

	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.CreateReadOnlyBinarySpan();
	ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => this.CreateReadOnlyObjectSpan();
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
	/// <inheritdoc cref="IReadOnlyFixedMemory.AsObjectContext()"/>
	public virtual IReadOnlyFixedContext<Object> AsObjectContext()
	{
		this.ValidateReferenceOperation();
		return new ReadOnlyFixedContext<Object>(this.BinaryOffset, this);
	}

	/// <inheritdoc cref="IReadOnlyFixedMemory.AsBinaryContext()"/>
	public virtual IReadOnlyFixedContext<Byte> AsBinaryContext()
	{
		this.ValidateUnmanagedOperation();
		return new ReadOnlyFixedContext<Byte>(this.BinaryOffset, this);
	}
}