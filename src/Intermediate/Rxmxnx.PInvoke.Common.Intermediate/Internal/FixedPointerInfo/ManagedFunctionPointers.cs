#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal readonly unsafe partial struct FixedPointerInfo
{
	public partial FixedPointerValue GetValue(Boolean isReadOnly, FixedValueHandle? handle)
	{
		delegate*<Type> getType = (delegate*<Type>)this.ConstructorOrFunctionPointer;
		return new((IntPtr)this.Pointer, this.Count * this.SizeOf)
		{
			IsReadOnly = isReadOnly,
			IsUnmanaged = this.IsUnmanaged,
			Type = getType != default ? getType() : default,
			Handle = handle,
		};
	}
	public partial ReadOnlyFixedMemory? CreateContext(FixedValueHandle? handle)
	{
		delegate*<void*, Int32, FixedValueHandle, ReadOnlyFixedMemory> constructor =
			(delegate*<void*, Int32, FixedValueHandle, ReadOnlyFixedMemory>)this.ConstructorOrFunctionPointer;
		return handle is not null && constructor != default ? constructor(this.Pointer, this.Count, handle) : default;
	}

	/// <summary>
	/// Creates an unmanaged pointer from <paramref name="value"/>.
	/// </summary>
	/// <param name="value">Managed function pointer.</param>
	/// <returns>A unmanaged pointer.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void* ToUnmanaged(delegate*<void*, Int32, FixedValueHandle, ReadOnlyFixedMemory> value)
		=> value;
	/// <summary>
	/// Creates an unmanaged pointer from <paramref name="value"/>.
	/// </summary>
	/// <param name="value">Managed function pointer.</param>
	/// <returns>A unmanaged pointer.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void* ToUnmanaged(delegate*<Type> value) => value;
}
#endif