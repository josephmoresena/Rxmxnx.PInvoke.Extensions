#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
public static unsafe partial class MemoryBlockExtensions
{
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A delegate of type <see cref="FixedContextAction{T}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T>(this Span<T> span, FixedContextAction<T> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				action(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{T}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T>(this Span<T> span, ReadOnlyFixedContextAction<T> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				action(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{T}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T>(this ReadOnlySpan<T> span, ReadOnlyFixedContextAction<T> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				action(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object representing the state, of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A delegate of type <see cref="FixedContextAction{T, TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TArg>(this Span<T> span, TArg arg, FixedContextAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				action(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object representing the state, of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{T, TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TArg>(this Span<T> span, TArg arg, ReadOnlyFixedContextAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				action(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object representing the state, of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{T, TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TArg>(this ReadOnlySpan<T> span, TArg arg,
		ReadOnlyFixedContextAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				action(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A delegate of type <see cref="FixedContextFunc{T, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TResult>(this Span<T> span, FixedContextFunc<T, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				return func(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{T, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TResult>(this Span<T> span, ReadOnlyFixedContextFunc<T, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				return func(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{T, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TResult>(this ReadOnlySpan<T> span,
		ReadOnlyFixedContextFunc<T, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				return func(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the function specified by <paramref name="func"/> has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> representing the state.</param>
	/// <param name="func">A delegate of type <see cref="FixedContextFunc{T, TArg, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TArg, TResult>(this Span<T> span, TArg arg,
		FixedContextFunc<T, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				return func(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the function specified by <paramref name="func"/> has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> representing the state.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{T, TArg, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TArg, TResult>(this Span<T> span, TArg arg,
		ReadOnlyFixedContextFunc<T, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				return func(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the function specified by <paramref name="func"/> has completed.
	/// </summary>
	/// <typeparam name="T">
	/// The type that is contained in the contiguous region of memory.
	/// </typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> representing the state.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{T, TArg, TResult}"/>.</param>
	/// <returns>The result of executing the function specified by <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TArg, TResult>(this ReadOnlySpan<T> span, TArg arg,
		ReadOnlyFixedContextFunc<T, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<T> ctx = new(ptr, span.Length);
			try
			{
				return func(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
}
#pragma warning restore CS8500