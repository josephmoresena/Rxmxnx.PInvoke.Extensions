namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="IntPtr"/> and <see cref="UIntPtr"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
public static class ValuePointerExtensions
{
#if !PACKAGE && NET9_0_OR_GREATER
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="ptr">Current <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <returns>A <see cref="IReadOnlyFixedContext{T}.IDisposable"/> instance.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// If provided, this object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if OBSOLETE_FIXED_INTERFACES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterfaceExtensions, ObsoleteConstants.ErrorFixedInterface)]
#endif
	public static IReadOnlyFixedContext<T>.IDisposable GetUnsafeFixedContext<T>(this ReadOnlyValPtr<T> ptr, Int32 count,
		IDisposable? disposable = default)
		=> ReadOnlyFixedContext<T>.CreateDisposable(ptr, count, disposable);
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IFixedContext{T}.IDisposable"/> instance from
	/// current reference pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="ptr">Current <see cref="ValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Optional object to dispose in order to free unmanaged resources.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing a fixed reference.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// If provided, this object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if OBSOLETE_FIXED_INTERFACES
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete(ObsoleteConstants.ObsoleteFixedInterfaceExtensions, ObsoleteConstants.ErrorFixedInterface)]
#endif
	public static IFixedContext<T>.IDisposable GetUnsafeFixedContext<T>(this ValPtr<T> ptr, Int32 count,
		IDisposable? disposable = default)
		=> FixedContext<T>.CreateDisposable(ptr, count, disposable);
#endif
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="ReadOnlyFixedContextValue{T}"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="ptr">Current <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="ReadOnlyFixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable GetUnsafeFixedContext<T>(ReadOnlyValPtr<T> ptr, Int32 count,
		out ReadOnlyFixedContextValue<T> fixedContext)
	{
		fixedContext = new(ptr, count, out IDisposable disposable);
		return disposable;
	}
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="FixedContextValue{T}"/> instance from current reference pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <param name="ptr">Current <see cref="ValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable GetUnsafeFixedContext<T>(this ValPtr<T> ptr, Int32 count,
		out FixedContextValue<T> fixedContext)
	{
		fixedContext = new(ptr, count, out IDisposable disposable);
		return disposable;
	}
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="ReadOnlyFixedContextValue{T}"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	/// <param name="ptr">Current <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="ReadOnlyFixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// This object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable GetUnsafeFixedContext<T, TDisposable>(ReadOnlyValPtr<T> ptr, Int32 count,
		TDisposable disposable, out ReadOnlyFixedContextValue<T> fixedContext) where TDisposable : IDisposable
		=> ReadOnlyFixedContextValue<T>.CreateDisposable(ptr, count, disposable, out fixedContext);
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="FixedContextValue{T}"/> instance from current reference pointer.
	/// </summary>
	/// <typeparam name="T">Type of pointer.</typeparam>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	/// <param name="ptr">Current <see cref="ValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// This object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable GetUnsafeFixedContext<T, TDisposable>(this ValPtr<T> ptr, Int32 count,
		TDisposable disposable, out FixedContextValue<T> fixedContext) where TDisposable : IDisposable
		=> FixedContextValue<T>.CreateDisposable(ptr, count, disposable, out fixedContext);
}