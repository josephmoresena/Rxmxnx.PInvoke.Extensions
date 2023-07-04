namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed read-only memory block with a specific offset.
/// </summary>
/// <remarks>
/// This class is used to work with fixed read-only memory blocks by providing an additional offset for precise memory
/// management.
/// </remarks>
internal sealed class ReadOnlyFixedOffset : ReadOnlyFixedMemory, IEquatable<ReadOnlyFixedOffset>
{
	/// <summary>
	/// The offset from the start of the fixed memory block.
	/// </summary>
	private readonly Int32 _offset;

	/// <summary>
	/// Constructs a new <see cref="FixedOffset"/> instance using a <see cref="ReadOnlyFixedMemory"/> instance and
	/// an offset.
	/// </summary>
	/// <param name="mem">The <see cref="ReadOnlyFixedMemory"/> instance to use.</param>
	/// <param name="offset">The offset from the start of the fixed memory block.</param>
	public ReadOnlyFixedOffset(ReadOnlyFixedMemory mem, Int32 offset) : base(mem) => this._offset = offset;

	/// <summary>
	/// Gets the offset from the start of the fixed memory block.
	/// </summary>
	public override Int32 BinaryOffset => this._offset;
	/// <inheritdoc/>
	public override Type? Type => default;
	/// <inheritdoc/>
	public override Boolean IsFunction => false;

	/// <inheritdoc/>
	public Boolean Equals(ReadOnlyFixedOffset? other) => this.Equals(other as ReadOnlyFixedMemory);
	/// <inheritdoc/>
	public override Boolean Equals(ReadOnlyFixedMemory? other) => base.Equals(other as ReadOnlyFixedOffset);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as ReadOnlyFixedOffset);
	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}