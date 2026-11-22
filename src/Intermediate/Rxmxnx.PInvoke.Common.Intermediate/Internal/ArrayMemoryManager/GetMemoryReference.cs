namespace Rxmxnx.PInvoke.Internal;

internal sealed partial class ArrayMemoryManager<T>
{
#if !NET6_0_OR_GREATER
	/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected override ref T GetMemoryReference(out Pinnable<T>? pinnable)
	{
		pinnable = Unsafe.As<Array, Pinnable<T>>(ref Unsafe.AsRef(this._array));
		return ref Unsafe.AddByteOffset(ref pinnable.Data, ArrayMemoryManager<T>.GetArrayOffset(this._array)!.Value);
	}
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
	/// <inheritdoc/>
	protected override ref T GetMemoryReference() => ref this.GetMemoryReference(out _);
#endif
#else
	/// <inheritdoc/>
	protected override ref T GetMemoryReference() => ref ArrayMemoryManager<T>.GetArrayDataReference(this._array);
#endif
}