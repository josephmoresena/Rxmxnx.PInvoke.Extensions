#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe partial class FixedContext<T>
{
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IFixedContext{T}.IDisposable"/> instance from
	/// current reference pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Optional object to dispose in order to free unmanaged resources.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing a fixed reference.</returns>
	/// <remarks>
	/// This method serves as a reference for the assembly patcher in .NET 9.0+. It is important to keep the
	/// attributes of its parameters compatible.
	/// </remarks>
#if OBSOLETE_FIXED_INTERFACES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterfaceExtensions, ObsoleteConstants.ErrorFixedInterface)]
#endif
	public static IFixedContext<T>.IDisposable CreateDisposable(ValPtr<T> valPtr, Int32 count,
		IDisposable? disposable = default)
	{
		FixedContext<T> ctx = new(valPtr, count);
		return ctx.ToDisposable(disposable);
	}
	/// <summary>
	/// Creates a new <see cref="ReadOnlyFixedMemory"/> instance.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
	/// <returns>A new <see cref="ReadOnlyFixedMemory"/> instance.</returns>
#if !PACKAGE && NET5_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlyFixedMemory CreateInstance(IntPtr ptr, Int32 count, FixedValueHandle handle)
		=> new FixedContext<T>(ptr.ToPointer(), count, handle);
#if NET5_0_OR_GREATER
	/// <summary>
	/// Creates a new <see cref="ReadOnlyFixedMemory"/> instance.
	/// </summary>
	/// <param name="ptr">The pointer to the fixed memory block.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
	/// <returns>A new <see cref="ReadOnlyFixedMemory"/> instance.</returns>
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ReadOnlyFixedMemory CreateInstance(void* ptr, Int32 count, FixedValueHandle handle)
		=> new FixedContext<T>(ptr, count, handle);
#endif
}
#endif