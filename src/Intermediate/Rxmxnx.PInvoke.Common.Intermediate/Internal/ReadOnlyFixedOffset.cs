namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a fixed read-only memory block with a specific offset.
/// </summary>
/// <remarks>
/// This class is used to work with fixed read-only memory blocks by providing an additional offset for precise memory
/// management.
/// </remarks>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
internal sealed partial class ReadOnlyFixedOffset : ReadOnlyFixedMemory
#else
internal sealed class ReadOnlyFixedOffset : ReadOnlyFixedMemory
#endif
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
	/// Constructs a new <see cref="FixedOffset"/> instance using a <see cref="ReadOnlyFixedMemory"/> instance and
	/// an offset.
	/// </summary>
	/// <param name="mem">The <see cref="ReadOnlyFixedMemory"/> instance to use.</param>
	/// <param name="offset">The offset from the start of the fixed memory block.</param>
	public ReadOnlyFixedOffset(ReadOnlyFixedMemory mem, Int32 offset) : base(mem) => this._offset = offset;
}