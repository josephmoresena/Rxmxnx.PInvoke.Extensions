namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public unsafe partial class CStringSequence
{
	/// <summary>
	/// Executes a specified action using the current instance treated as a <see cref="FixedCStringSequence"/>.
	/// </summary>
	/// <param name="action">The action to execute on the <see cref="FixedCStringSequence"/>.</param>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the action execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WithSafeTransform(CStringSequenceAction action)
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
	/// Executes a specified action on the current instance treated as a <see cref="FixedCStringSequence"/>,
	/// using an additional parameter passed to the action.
	/// </summary>
	/// <typeparam name="TState">The type of the additional parameter.</typeparam>
	/// <param name="state">The additional parameter to pass to the action.</param>
	/// <param name="action">The action to execute on the <see cref="FixedCStringSequence"/>.</param>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the action execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WithSafeTransform<TState>(TState state, CStringSequenceAction<TState> action)
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
	/// Executes a specified function using the current instance treated as a <see cref="FixedCStringSequence"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="func">The function to execute on the <see cref="FixedCStringSequence"/>.</param>
	/// <returns>The result of the function execution.</returns>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the function execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult WithSafeTransform<TResult>(CStringSequenceFunc<TResult> func)
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
	/// Executes a specified function using the current instance treated as a <see cref="FixedCStringSequence"/>,
	/// and an additional parameter passed to the function.
	/// </summary>
	/// <typeparam name="TState">The type of the additional parameter.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="state">The additional parameter to pass to the function.</param>
	/// <param name="func">The function to execute on the <see cref="FixedCStringSequence"/>.</param>
	/// <returns>The result of the function execution.</returns>
	/// <remarks>
	/// The method temporarily fixes the current sequence in memory, enabling safe pointer operations
	/// during the execution of the delegate.
	/// Memory safety is ensured by unloading the memory after the function execution.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult WithSafeTransform<TState, TResult>(TState state, CStringSequenceFunc<TState, TResult> func)
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