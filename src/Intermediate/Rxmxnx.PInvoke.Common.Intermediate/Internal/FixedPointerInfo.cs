namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Fixed pointer information.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal readonly unsafe struct FixedPointerInfo
{
	/// <summary>
	/// Unmanaged memory block pointer.
	/// </summary>
	public void* Pointer { get; init; }
	/// <summary>
	/// Number of elements on the memory block.
	/// </summary>
	public Int32 Count { get; init; }
	/// <summary>
	/// Size of the element type on the memory block.
	/// </summary>
	public Int32 SizeOf { get; init; }
	/// <summary>
	/// Indicates whether the type of the memory block is unmanaged,
	/// </summary>
	public Boolean IsUnmanaged { get; init; }
	/// <summary>
	/// Function pointer to <see cref="ReadOnlyFixedMemory"/> instance for current memory block.
	/// </summary>
	public delegate*<void*, Int32, FixedValueHandle, ReadOnlyFixedMemory> ConstructorPointer { get; init; }
	/// <summary>
	/// Function pointer to retrieve current memory block type.
	/// </summary>
	public delegate*<Type> GetTypePointer { get; init; }

	/// <summary>
	/// Retrieves a <see cref="FixedPointerValue"/> instance to represent the current memory block.
	/// </summary>
	/// <param name="isReadOnly">Indicates whether the current memory block is read-only.</param>
	/// <param name="handle">Memory block handle.</param>
	/// <returns>A <see cref="FixedPointerValue"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FixedPointerValue GetValue(Boolean isReadOnly, FixedValueHandle handle)
		=> new((IntPtr)this.Pointer, this.Count * this.SizeOf)
		{
			IsReadOnly = isReadOnly,
			IsUnmanaged = this.IsUnmanaged,
			Type = this.GetTypePointer != default ? this.GetTypePointer() : default,
			Handle = handle,
		};
	/// <summary>
	/// Creates a new <see cref="ReadOnlyFixedMemory"/> instance to represent the current memory block.
	/// </summary>
	/// <param name="handle">Memory block handle.</param>
	/// <returns>A new <see cref="ReadOnlyFixedMemory"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlyFixedMemory? CreateContext(FixedValueHandle handle)
		=> this.ConstructorPointer != default ? this.ConstructorPointer(this.Pointer, this.Count, handle) : default;
}