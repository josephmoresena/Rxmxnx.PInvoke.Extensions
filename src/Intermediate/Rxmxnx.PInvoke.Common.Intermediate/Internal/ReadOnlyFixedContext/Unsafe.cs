#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal unsafe partial class ReadOnlyFixedContext<T>
{
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <returns>A <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance.</returns>
	/// <remarks>
	/// This method serves as a reference for the assembly patcher in .NET 9.0+. It is important to keep the
	/// attributes of its parameters compatible.
	/// </remarks>
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterfaceExtensions, ObsoleteConstants.ErrorFixedInterface)]
#endif
	public static IReadOnlyFixedContext<T>.IDisposable CreateDisposable(ReadOnlyValPtr<T> valPtr, Int32 count,
		IDisposable? disposable = default)
	{
		ReadOnlyFixedContext<T> ctx = new(valPtr, count);
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
		=> new ReadOnlyFixedContext<T>(ptr.ToPointer(), count, handle);
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
		=> new ReadOnlyFixedContext<T>(ptr, count, handle);
#endif
}
#endif