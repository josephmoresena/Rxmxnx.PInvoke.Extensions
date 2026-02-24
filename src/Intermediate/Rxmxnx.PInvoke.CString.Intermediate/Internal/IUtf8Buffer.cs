namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Interface representing a UTF-8 read-only memory buffer.
/// </summary>
[Preserve(AllMembers = true)]
internal interface IUtf8Buffer
{
	/// <summary>
	/// A read-only UTF-8 buffer.
	/// </summary>
	ReadOnlySpan<Byte> Buffer { get; }

	/// <summary>
	/// Allocates a handle of the specified type for the current instance.
	/// </summary>
	/// <param name="type">
	/// One of the <see cref="GCHandleType"/> values, indicating the type of <see cref="GCHandle"/> to create.
	/// </param>
	/// <returns>
	/// A new <see cref="GCHandle"/> of the specified type. This <see cref="GCHandle"/> must be released with
	/// <see cref="GCHandle.Free()"/> when it is no longer needed.
	/// </returns>
	GCHandle Alloc(GCHandleType type);

	/// <summary>
	/// Retrieves the binary offset for the given index in the current buffer.
	/// </summary>
	/// <param name="index">The zero-based index of the element to get.</param>
	/// <returns>The binary offset for the specified index.</returns>
	Int32 GetBinaryOffset(Int32 index);
}