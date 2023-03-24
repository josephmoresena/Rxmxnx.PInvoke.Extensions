namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed memory offset class.
/// </summary>
internal sealed class FixedOffset : FixedMemory, IEquatable<FixedOffset>
{
    /// <summary>
    /// Memory offset.
    /// </summary>
    private readonly Int32 _offset;

    /// <inheritdoc/>
    public override Int32 BinaryOffset => this._offset;
    /// <inheritdoc/>
    public override Type? Type => default;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="mem"><see cref="FixedMemory"/> instance.</param>
    /// <param name="offset">Memory offset.</param>
    public FixedOffset(FixedMemory mem, Int32 offset) : base(mem)
    {
        this._offset = offset;
    }

    /// <inheritdoc/>
    public Boolean Equals(FixedOffset? other) => this.Equals(other as FixedMemory);
    /// <inheritdoc/>
    public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedOffset);
    /// <inheritdoc/>
    public override Boolean Equals(Object? obj) => base.Equals(obj as FixedOffset);
    /// <inheritdoc/>
    public override Int32 GetHashCode() => base.GetHashCode();
}

