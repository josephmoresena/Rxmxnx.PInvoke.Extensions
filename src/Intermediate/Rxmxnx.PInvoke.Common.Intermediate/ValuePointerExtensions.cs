#if NET9_0_OR_GREATER
namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="IntPtr"/> and <see cref="UIntPtr"/> instances.
/// </summary>
#if !PACKAGE
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public static unsafe class ValuePointerExtensions
#else
public static unsafe partial class PointerExtensions
#endif
{
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
	public static IReadOnlyFixedContext<T>.IDisposable GetUnsafeFixedContext<T>(this ReadOnlyValPtr<T> ptr, Int32 count,
		IDisposable? disposable = default)
		=> new ReadOnlyFixedContext<T>(ptr, count).ToDisposable(disposable);
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
	public static IFixedContext<T>.IDisposable GetUnsafeFixedContext<T>(this ValPtr<T> ptr, Int32 count,
		IDisposable? disposable = default)
		=> new FixedContext<T>(ptr, count).ToDisposable(disposable);
}
#endif