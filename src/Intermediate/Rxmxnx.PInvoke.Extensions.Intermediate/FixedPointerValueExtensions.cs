namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations for <see cref="FixedPointerValue"/> instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class FixedPointerValueExtensions
{
	/// <summary>
	/// Creates a <see cref="FixedPointerValue"/> instance by pinning the current <see cref="ReadOnlyMemory{T}"/> instance,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="ReadOnlyMemory{T}"/>.</typeparam>
	/// <param name="mem">A <see cref="ReadOnlyMemory{T}"/> instance.</param>
	/// <param name="fixedMemory">
	/// Output. The <see cref="FixedPointerValue"/> instance representing the pinned memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release memory pinning.</returns>
	/// <exception cref="ArgumentException">
	/// The executing runtime may throw if it cannot pin this memory. Whether a given <typeparamref name="T"/> can be
	/// pinned is a host policy; this library does not reject managed types.
	/// </exception>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable GetFixedMemory<T>(this ReadOnlyMemory<T> mem, out FixedPointerValue fixedMemory)
	{
		IDisposable result = mem.GetFixedContext(out ReadOnlyFixedContextValue<T> fixedContext);
		fixedMemory = fixedContext;
		return result;
	}
	/// <summary>
	/// Creates a <see cref="FixedPointerValue"/> instance by pinning the current <see cref="Memory{T}"/> instance,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="Memory{T}"/>.</typeparam>
	/// <param name="mem">A <see cref="Memory{T}"/> instance.</param>
	/// <param name="fixedMemory">
	/// Output. The <see cref="FixedPointerValue"/> instance representing the pinned memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release memory pinning.</returns>
	/// <exception cref="ArgumentException">
	/// The executing runtime may throw if it cannot pin this memory. Whether a given <typeparamref name="T"/> can be
	/// pinned is a host policy; this library does not reject managed types.
	/// </exception>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable GetFixedMemory<T>(this Memory<T> mem, out FixedPointerValue fixedMemory)
	{
		IDisposable result = mem.GetFixedContext(out FixedContextValue<T> fixedContext);
		fixedMemory = fixedContext;
		return result;
	}
#pragma warning disable CS8500
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedAction
#else
		where TAction : IFixedAction, allows ref struct
#endif
	{
		if (action is null) return;
		FixedPointerValueExtensions.WithSafeFixed(ref action, span);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this ReadOnlySpan<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedAction
#else
		where TAction : IFixedAction, allows ref struct
#endif
	{
		if (action is null) return;
		FixedPointerValueExtensions.WithSafeFixed(ref action, span);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedAction
#else
		where TAction : struct, IFixedAction, allows ref struct
#endif
		=> FixedPointerValueExtensions.WithSafeFixed(ref action, span);
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this ReadOnlySpan<T> span, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedAction
#else
		where TAction : struct, IFixedAction, allows ref struct
#endif
		=> FixedPointerValueExtensions.WithSafeFixed(ref action, span);
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this Span<T> span, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedFunction<TResult>
#else
		where TFunction : IFixedFunction<TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		FixedPointerValueExtensions.WithSafeFixed(ref func, span, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this ReadOnlySpan<T> span, TFunction? func,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedFunction<TResult>
#else
		where TFunction : IFixedFunction<TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		FixedPointerValueExtensions.WithSafeFixed(ref func, span, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this Span<T> span, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedFunction<TResult>
#else
		where TFunction : struct, IFixedFunction<TResult>, allows ref struct
#endif
		=> FixedPointerValueExtensions.WithSafeFixed(ref func, span, out result);
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this ReadOnlySpan<T> span, ref TFunction func,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedFunction<TResult>
#else
		where TFunction : struct, IFixedFunction<TResult>, allows ref struct
#endif
		=> FixedPointerValueExtensions.WithSafeFixed(ref func, span, out result);

	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TAction>(ref TAction action, Span<T> span)
#if !NET9_0_OR_GREATER
		where TAction : IFixedAction
#else
		where TAction : IFixedAction, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				action.Accept(new FixedContextValue<T>(ptr, span.Length));
			return;
		}
		NonGenericFixedAction nonGeneric = NonGenericFixedAction.Create(action);
		FixedPointerValueExtensions.WithSafeFixed(ref nonGeneric, span);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TAction>(ref TAction action, ReadOnlySpan<T> span)
#if !NET9_0_OR_GREATER
		where TAction : IFixedAction
#else
		where TAction : IFixedAction, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				action.Accept(new ReadOnlyFixedContextValue<T>(ptr, span.Length));
			return;
		}
		NonGenericFixedAction nonGeneric = NonGenericFixedAction.Create(action);
		FixedPointerValueExtensions.WithSafeFixed(ref nonGeneric, span);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TFunction, TResult>(ref TFunction func, Span<T> span, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedFunction<TResult>
#else
		where TFunction : IFixedFunction<TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
			return;
		}
		NonGenericFixedFunction<TResult> nonGeneric = NonGenericFixedFunction<TResult>.Create(func);
		FixedPointerValueExtensions.WithSafeFixed(ref nonGeneric, span, out result);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TFunction, TResult>(ref TFunction func, ReadOnlySpan<T> span,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedFunction<TResult>
#else
		where TFunction : IFixedFunction<TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				result = func.Apply(new ReadOnlyFixedContextValue<T>(ptr, span.Length));
			return;
		}
		NonGenericFixedFunction<TResult> nonGeneric = NonGenericFixedFunction<TResult>.Create(func);
		FixedPointerValueExtensions.WithSafeFixed(ref nonGeneric, span, out result);
	}

#pragma warning restore CS8500
}