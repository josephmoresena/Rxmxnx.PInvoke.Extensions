namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class from memory block fixing.
/// </summary>
internal unsafe abstract class FixedMemory : FixedPointer, IFixedMemory, IEquatable<FixedMemory>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="binaryLength">Memory block size in bytes.</param>
    /// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
    protected FixedMemory(void* ptr, Int32 binaryLength, Boolean isReadOnly)
        : base(ptr, binaryLength, isReadOnly) { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory block.</param>
    /// <param name="binaryLength">Memory block size in bytes.</param>
    /// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
    /// <param name="isValid">Indicates whether current instance remains valid.</param>
    protected FixedMemory(void* ptr, Int32 binaryLength, Boolean isReadOnly, IMutableWrapper<Boolean> isValid)
        : base(ptr, binaryLength, isReadOnly, isValid) { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mem">Fixed context of memory block.</param>
    protected FixedMemory(FixedMemory mem) : base(mem) { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mem">Fixed context of memory block.</param>
    /// <param name="offset">Memory offset.</param>
    protected FixedMemory(FixedMemory mem, Int32 offset) : base(mem, offset) { }

    Span<Byte> IFixedMemory.Bytes => base.CreateBinarySpan();
    ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => base.CreateReadOnlyBinarySpan();

    /// <inheritdoc/>
    public virtual Boolean Equals(FixedMemory? other) => this.Equals(other as FixedPointer);
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public override Boolean Equals(Object? obj) => base.Equals(obj as FixedMemory);
    /// <inheritdoc/>
    public override Int32 GetHashCode() => base.GetHashCode();
    /// <inheritdoc/>
    public virtual IFixedContext<Byte> AsBinaryContext() => new FixedContext<Byte>(this.BinaryOffset, this);

    IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.AsBinaryContext();
}