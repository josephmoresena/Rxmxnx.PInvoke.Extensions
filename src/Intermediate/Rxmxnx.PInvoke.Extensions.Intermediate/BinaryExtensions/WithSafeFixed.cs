// ReSharper disable ConvertToExtensionBlock

#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe partial class BinaryExtensions
{
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <param name="span">The current binary span.</param>
	/// <param name="action">A <see cref="FixedAction"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed(this Span<Byte> span, FixedAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <param name="span">The current binary span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedAction"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed(this Span<Byte> span, ReadOnlyFixedAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <param name="span">The current read-only binary span.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedAction"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed(this ReadOnlySpan<Byte> span, ReadOnlyFixedAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span">The current binary span.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> that represents the state.</param>
	/// <param name="action">A delegate of type <see cref="FixedAction{TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TArg>(this Span<Byte> span, TArg arg, FixedAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span">The current binary span.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> that represents the state.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedAction{TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TArg>(this Span<Byte> span, TArg arg, ReadOnlyFixedAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="span">The current read-only binary span.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> that represents the state.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedAction{TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TArg>(this ReadOnlySpan<Byte> span, TArg arg, ReadOnlyFixedAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="span">The current binary span.</param>
	/// <param name="func">A delegate of type <see cref="FixedFunc{TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TResult>(this Span<Byte> span, FixedFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="span">The current binary span.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedFunc{TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TResult>(this Span<Byte> span, ReadOnlyFixedFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="span">The current read-only binary span.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedFunc{TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TResult>(this ReadOnlySpan<Byte> span, ReadOnlyFixedFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// <typeparam name="TArg">The type of the object representing the state.</typeparam>
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="span">The current binary span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A delegate of type <see cref="FixedFunc{TArg, TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TArg, TResult>(this Span<Byte> span, TArg arg, FixedFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="TArg">The type of the object representing the state.</typeparam>
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="span">The current binary span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedFunc{TArg, TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TArg, TResult>(this Span<Byte> span, TArg arg,
		ReadOnlyFixedFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			FixedContext<Byte> ctx = new(ptr, span.Length);
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
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="TArg">The type of the object representing the state.</typeparam>
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="span">The current read-only binary span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedFunc{TArg, TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TArg, TResult>(this ReadOnlySpan<Byte> span, TArg arg,
		ReadOnlyFixedFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, span.Length);
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