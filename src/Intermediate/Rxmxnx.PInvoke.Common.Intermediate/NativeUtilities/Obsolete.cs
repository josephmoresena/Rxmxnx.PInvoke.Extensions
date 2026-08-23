#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class NativeUtilities
{
	/// <summary>
	/// Creates an <see cref="IReadOnlyFixedContext{TEnum}.IDisposable"/> instance by pinning an array of the values of
	/// the constants in a specified enumeration type.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>An <see cref="IFixedContext{TEnum}.IDisposable"/> instance representing the pinned array.</returns>
	/// <remarks>
	/// This method pins the array to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned array
	/// and avoid memory leaks.
	/// </remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterfaceMethods, ObsoleteConstants.ErrorFixedInterface)]
#endif
	public static IReadOnlyFixedContext<TEnum>.IDisposable GetValuesFixedContext<TEnum>() where TEnum : unmanaged, Enum
	{
		ReadOnlyMemory<TEnum> mem = EnumValueHelper<TEnum>.Values;
		MemoryHandle handle = mem.Pin();
		return handle.Pointer == default ?
			ReadOnlyFixedContext<TEnum>.EmptyDisposable :
			// ReSharper disable once HeapView.BoxingAllocation
			new ReadOnlyFixedContext<TEnum>(handle.Pointer, mem.Length).ToDisposable(handle);
	}
	/// <summary>
	/// Allocates a native memory block for <paramref name="count"/> values of type <typeparamref name="T"/> and exposes
	/// it through an <see cref="IFixedContext{T}.IDisposable"/> instance.
	/// </summary>
	/// <typeparam name="T">The unmanaged value type stored in the allocated memory block.</typeparam>
	/// <param name="count">The number of values of type <typeparamref name="T"/> to allocate.</param>
	/// <returns>
	/// An <see cref="IFixedContext{T}.IDisposable"/> instance over the allocated native memory block.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is negative.</exception>
	/// <exception cref="OverflowException">
	/// Thrown when the requested allocation size exceeds <see cref="Int32.MaxValue"/>.
	/// </exception>
	/// <remarks>
	/// The returned context owns the native memory allocation and releases it when disposed. The allocated memory is not
	/// initialized. Consumers should use a <see langword="using"/> statement or otherwise dispose the returned context.
	/// </remarks>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterfaceMethods, ObsoleteConstants.ErrorFixedInterface)]
#endif
	public static IFixedContext<T>.IDisposable HeapAlloc<T>(Int32 count) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidLength(count);
		return NativeMemoryOwner.CreateContext<T>(count);
	}
}
#endif