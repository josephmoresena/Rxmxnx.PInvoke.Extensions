#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal readonly unsafe partial struct FixedPointerInfo
{
	/// <summary>
	/// Function pointer to <see cref="ReadOnlyFixedMemory"/> constructor for current memory block.
	/// </summary>
	public delegate*<void*, Int32, FixedValueHandle, ReadOnlyFixedMemory> ConstructorPointer
	{
		get => (delegate*<void*, Int32, FixedValueHandle, ReadOnlyFixedMemory>)this.ConstructorOrFunctionPointer;
		init => this.ConstructorOrFunctionPointer = value;
	}
	/// <summary>
	/// Function pointer to retrieve current memory block type.
	/// </summary>
	public delegate*<Type> GetTypePointer
	{
		get => (delegate*<Type>)this.TypeOrFunctionPointer;
		init => this.TypeOrFunctionPointer = value;
	}

	public partial FixedPointerValue GetValue(Boolean isReadOnly, FixedValueHandle? handle)
		=> new((IntPtr)this.Pointer, this.Count * this.SizeOf)
		{
			IsReadOnly = isReadOnly,
			IsUnmanaged = this.IsUnmanaged,
			Type = this.TypeOrFunctionPointer != default ? this.GetTypePointer() : default,
			Handle = handle,
		};
	public partial ReadOnlyFixedMemory? CreateContext(FixedValueHandle? handle)
		=> handle is not null && this.ConstructorOrFunctionPointer != default ?
			this.ConstructorPointer(this.Pointer, this.Count, handle) :
			default;
}
#endif