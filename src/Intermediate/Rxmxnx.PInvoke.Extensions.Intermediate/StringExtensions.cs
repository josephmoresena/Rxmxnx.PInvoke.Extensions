#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="String"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class StringExtensions
{
	/// <summary>
	/// Pins the current string to prevent the garbage collector from relocating its memory
	/// address during the execution of the action specified in <paramref name="action"/>.
	/// </summary>
	/// <param name="str">The <see cref="String"/> instance to pin during the action.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedContextAction{Char}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed(this String? str, ReadOnlyFixedContextAction<Char> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		if (str is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
			{
				ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
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
			action(ReadOnlyFixedContext<Char>.Empty);
	}

	/// <summary>
	/// Pins the current string to prevent the garbage collector from relocating its memory
	/// address during the execution of the action specified in <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the action.</param>
	/// <param name="arg">An object of type <typeparamref name="TArg"/> that represents the state.</param>
	/// <param name="action">A delegate of type <see cref="ReadOnlyFixedContextAction{Char, TArg}"/>.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TArg>(this String? str, TArg arg, ReadOnlyFixedContextAction<Char, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		if (str is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
			{
				ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
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
			action(ReadOnlyFixedContext<Char>.Empty, arg);
	}

	/// <summary>
	/// Pins the current string to prevent the garbage collector from relocating its memory
	/// address during the execution of the function specified in <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the function.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{Char, TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TResult>(this String? str, ReadOnlyFixedContextFunc<Char, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		if (str is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
			{
				ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
				try
				{
					return func(ctx);
				}
				finally
				{
					ctx.Unload();
				}
			}
		return func(ReadOnlyFixedContext<Char>.Empty);
	}

	/// <summary>
	/// Pins the current string to prevent the garbage collector from relocating its memory
	/// address during the execution of the function specified in <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="TArg">The type of the object representing the state.</typeparam>
	/// <typeparam name="TResult">The type of the result returned by <paramref name="func"/>.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the function.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A delegate of type <see cref="ReadOnlyFixedContextFunc{Char, TArg, TResult}"/>.</param>
	/// <returns>The result of executing <paramref name="func"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TArg, TResult>(this String? str, TArg arg,
		ReadOnlyFixedContextFunc<Char, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		if (str is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
			{
				ReadOnlyFixedContext<Char> ctx = new(ptr, str.Length);
				try
				{
					return func(ctx, arg);
				}
				finally
				{
					ctx.Unload();
				}
			}
		return func(ReadOnlyFixedContext<Char>.Empty, arg);
	}
}