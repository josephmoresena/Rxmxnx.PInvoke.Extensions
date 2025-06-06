#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CStringSequence
{
	/// <summary>
	/// Creates an <see cref="IFixedPointer.IDisposable"/> instance by pinning the current
	/// instance, allowing safe access to the fixed memory region.
	/// </summary>
	/// <returns>An <see cref="IFixedPointer.IDisposable"/> instance representing the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
	public IFixedPointer.IDisposable GetFixedPointer()
	{
		ReadOnlyMemory<Char> mem = this._value.AsMemory();
		MemoryHandle handle = mem.Pin();
		return new FixedContext<Char>(handle.Pointer, mem.Length).ToDisposable(handle);
	}
	/// <summary>
	/// Executes a specified action using the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <param name="action">The action to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the action execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WithSafeFixed(ReadOnlyFixedListAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			FixedCStringSequence fseq = this.GetFixedSequence(ptr);
			try
			{
				action(fseq);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
	/// <summary>
	/// Executes a specified action on the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>,
	/// using an additional parameter passed to the action.
	/// </summary>
	/// <typeparam name="TState">The type of the additional parameter.</typeparam>
	/// <param name="state">The additional parameter to pass to the action.</param>
	/// <param name="action">The action to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the action execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WithSafeFixed<TState>(TState state, ReadOnlyFixedListAction<TState> action)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			FixedCStringSequence fseq = this.GetFixedSequence(ptr);
			try
			{
				action(fseq, state);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
	/// <summary>
	/// Executes a specified function using the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="func">The function to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <returns>The result of the function execution.</returns>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the function execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult WithSafeFixed<TResult>(ReadOnlyFixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			FixedCStringSequence fseq = this.GetFixedSequence(ptr);
			try
			{
				return func(fseq);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
	/// <summary>
	/// Executes a specified function using the current instance treated as a <see cref="ReadOnlyFixedMemoryList"/>,
	/// and an additional parameter passed to the function.
	/// </summary>
	/// <typeparam name="TState">The type of the additional parameter.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="state">The additional parameter to pass to the function.</param>
	/// <param name="func">The function to execute on the <see cref="ReadOnlyFixedMemoryList"/>.</param>
	/// <returns>The result of the function execution.</returns>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the function execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult WithSafeFixed<TState, TResult>(TState state, ReadOnlyFixedListFunc<TState, TResult> func)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (Char* ptr = &MemoryMarshal.GetReference<Char>(this._value))
		{
			FixedCStringSequence fseq = this.GetFixedSequence(ptr);
			try
			{
				return func(fseq, state);
			}
			finally
			{
				fseq.Unload();
			}
		}
	}
}