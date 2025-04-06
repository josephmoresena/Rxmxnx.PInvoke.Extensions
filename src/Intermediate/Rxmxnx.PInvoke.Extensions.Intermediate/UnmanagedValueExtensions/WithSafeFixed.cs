namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
public unsafe partial class UnmanagedValueExtensions
{
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A delegate of type <see cref="FixedContextAction{T}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T>(this T[]? arr, FixedContextAction<T> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					action(ctx);
				}
				finally
				{
					ctx.Unload();
				}
			}
		else
			action(FixedContext<T>.Empty);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{T}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T>(this T[]? arr, ReadOnlyFixedContextAction<T> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					action(ctx);
				}
				finally
				{
					ctx.Unload();
				}
			}
		else
			action(FixedContext<T>.Empty);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object representing the state, of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A delegate of type <see cref="FixedContextAction{T, TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TArg>(this T[]? arr, TArg arg, FixedContextAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					action(ctx, arg);
				}
				finally
				{
					ctx.Unload();
				}
			}
		else
			action(FixedContext<T>.Empty, arg);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object representing the state, of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{T, TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TArg>(this T[]? arr, TArg arg, ReadOnlyFixedContextAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					action(ctx, arg);
				}
				finally
				{
					ctx.Unload();
				}
			}
		else
			action(FixedContext<T>.Empty, arg);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="func">A delegate of type <see cref="FixedContextFunc{T, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TResult>(this T[]? arr, FixedContextFunc<T, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					return func(ctx);
				}
				finally
				{
					ctx.Unload();
				}
			}
		return func(FixedContext<T>.Empty);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{T, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TResult>(this T[]? arr, ReadOnlyFixedContextFunc<T, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					return func(ctx);
				}
				finally
				{
					ctx.Unload();
				}
			}
		return func(FixedContext<T>.Empty);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> representing the state.</param>
	/// <param name="func">A delegate of type <see cref="FixedContextFunc{T, TArg, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TArg, TResult>(this T[]? arr, TArg arg,
		FixedContextFunc<T, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					return func(ctx, arg);
				}
				finally
				{
					ctx.Unload();
				}
			}
		return func(FixedContext<T>.Empty, arg);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> representing the state.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{T, TArg, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TArg, TResult>(this T[]? arr, TArg arg,
		ReadOnlyFixedContextFunc<T, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		if (arr is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(arr.AsSpan()))
			{
				FixedContext<T> ctx = new(ptr, arr.Length);
				try
				{
					return func(ctx, arg);
				}
				finally
				{
					ctx.Unload();
				}
			}
		return func(FixedContext<T>.Empty, arg);
	}
}
#pragma warning restore CS8500