namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed memory block with a specific offset.
/// </summary>
/// <remarks>
/// This class is used to work with fixed memory blocks by providing an additional offset for precise memory management.
/// </remarks>
internal sealed partial class FixedOffset : FixedMemory
{
	/// <summary>
	/// The offset from the start of the fixed memory block.
	/// </summary>
	private readonly Int32 _offset;

	/// <summary>
	/// Gets the offset from the start of the fixed memory block.
	/// </summary>
	public override Int32 BinaryOffset => this._offset;
	/// <inheritdoc/>
	public override Boolean IsUnmanaged => true;
	/// <inheritdoc/>
	public override Type? Type => default;
	/// <inheritdoc/>
	public override Boolean IsFunction => false;

	/// <summary>
	/// Constructs a new <see cref="FixedOffset"/> instance using a <see cref="FixedMemory"/> instance and
	/// an offset.
	/// </summary>
	/// <param name="mem">The <see cref="FixedMemory"/> instance to use.</param>
	/// <param name="offset">The offset from the start of the fixed memory block.</param>
	public FixedOffset(FixedMemory mem, Int32 offset) : base(mem) => this._offset = offset;
}