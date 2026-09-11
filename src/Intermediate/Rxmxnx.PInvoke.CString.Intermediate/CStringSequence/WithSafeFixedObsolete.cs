namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CStringSequence
{
	/// <summary>
	/// Internal pointer to <c>typeof(System.Byte)</c> instance.
	/// </summary>
#if !NET5_0_OR_GREATER && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
	internal static void* TypePointer => Unsafe.AsPointer(ref CStringSequence.bufferType);
#else
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static void* TypePointer => default;
#endif
	/// <summary>
	/// Internal pointer to <c>ReadOnlyFixedContext&lt;Byte&gt;.CreateInstance</c> delegate instance.
	/// </summary>
#if !NET5_0_OR_GREATER && (NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER)
	internal static void* ConstructorPointer => Unsafe.AsPointer(ref CStringSequence.bufferConstructor);
#else
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static void* ConstructorPointer => default;
#endif

	/// <summary>
	/// Creates an <see cref="MemoryHandle"/> instance by pinning the current instance.
	/// </summary>
	/// <returns>A <see cref="MemoryHandle"/> for the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="MemoryHandle"/> value returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public MemoryHandle Pin()
	{
		ReadOnlyMemory<Char> mem = this._value.AsMemory();
		return mem.Pin();
	}
	/// <summary>
	/// Creates an <see cref="IFixedPointer.IDisposable"/> instance by pinning the current instance, allowing safe
	/// access to the fixed memory region.
	/// </summary>
	/// <returns>An <see cref="IFixedPointer.IDisposable"/> instance representing the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
	public IDisposable GetFixedPointer(out FixedPointerValue fixedPointer)
	{
		MemoryHandle handle = this.Pin();
		// ReSharper disable once HeapView.BoxingAllocation
		fixedPointer = new ReadOnlyFixedContextValue<Char>(handle, this._value.Length, true, out IDisposable result);
		return result;
	}
}