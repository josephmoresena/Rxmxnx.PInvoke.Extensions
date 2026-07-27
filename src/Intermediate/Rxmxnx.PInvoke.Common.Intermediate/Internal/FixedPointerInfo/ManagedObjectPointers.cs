#if !NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal readonly unsafe partial struct FixedPointerInfo
{
#pragma warning disable CS8500
	public partial FixedPointerValue GetValue(Boolean isReadOnly, FixedValueHandle? handle)
	{
		Type* typePointer = (Type*)this.TypeOrFunctionPointer;
		return new((IntPtr)this.Pointer, this.Count * this.SizeOf)
		{
			IsReadOnly = isReadOnly,
			IsUnmanaged = this.IsUnmanaged,
			Type = typePointer != default ? typePointer[0] : default,
			Handle = handle,
		};
	}
#if NETSTANDARD2_1 || NETCOREAPP
	public partial ReadOnlyFixedMemory? CreateContext(FixedValueHandle? handle)
	{
		Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>* constructorPointer =
			(Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>*)this.ConstructorOrFunctionPointer;
		return handle is not null && constructorPointer != default ?
			constructorPointer[0]((IntPtr)this.Pointer, this.Count, handle) :
			default;
	}
#endif
#pragma warning restore CS8500
}
#endif