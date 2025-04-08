﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for managing fixed memory blocks.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal abstract unsafe partial class FixedMemory : ReadOnlyFixedMemory, IFixedMemory, IEquatable<FixedMemory>
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
	/// <summary>
	/// Constructs a new <see cref="FixedMemory"/> instance using another instance as a template and specifying a
	/// memory offset.
	/// </summary>
	/// <param name="mem">The <see cref="FixedMemory"/> instance to copy data from.</param>
	/// <param name="offset">The offset to be added to the pointer to the memory block.</param>
	protected FixedMemory(FixedMemory mem, Int32 offset) : base(mem, offset) { }

	Span<Byte> IFixedMemory.Bytes => this.CreateBinarySpan();
	Span<Object> IFixedMemory.Objects => this.CreateObjectSpan();
	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.CreateReadOnlyBinarySpan();
	ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => this.CreateReadOnlyObjectSpan();
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();

	/// <inheritdoc/>
	public new virtual IFixedContext<Byte> AsBinaryContext()
	{
		this.ValidateUnmanagedOperation();
		return new FixedContext<Byte>(this.BinaryOffset, this);
	}
	/// <inheritdoc/>
	public new virtual IFixedContext<Object> AsObjectContext()
	{
		this.ValidateReferenceOperation();
		return new FixedContext<Object>(this.BinaryOffset, this);
	}
}