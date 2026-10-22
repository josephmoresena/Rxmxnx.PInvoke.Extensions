namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations for <see cref="ReadOnlyFixedContextValue{T}"/> instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class ReadOnlyFixedContextValueExtensions
{
	/// <summary>
	/// Creates a <see cref="ReadOnlyFixedContextValue{T}"/>  instance by pinning the current
	/// <see cref="ReadOnlyMemory{T}"/> instance, ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="ReadOnlyMemory{T}"/>.</typeparam>
	/// <param name="mem">A <see cref="ReadOnlyMemory{T}"/> instance.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="ReadOnlyFixedContextValue{T}"/> instance representing the pinned memory.
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
	public static IDisposable GetFixedContext<T>(this ReadOnlyMemory<T> mem,
		out ReadOnlyFixedContextValue<T> fixedContext)
	{
		MemoryHandle handle = mem.Pin();
		Boolean isReadOnly = !MemoryMarshal.TryGetMemoryManager<T, MemoryManager<T>>(mem, out _) &&
			!MemoryMarshal.TryGetArray(mem, out _);
		fixedContext = new(handle, mem.Length, isReadOnly, out IDisposable result);
		return result;
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current string by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the action.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this String? str, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<Char>
#else
		where TAction : IReadOnlyFixedContextAction<Char>, allows ref struct
#endif
	{
		if (action is null) return;
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, str);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current string by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the action.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this String? str, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<Char>
#else
		where TAction : struct, IReadOnlyFixedContextAction<Char>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, str);
	/// <summary>
	/// Prevents the garbage collector from relocating the current string by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the function.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TResult, TFunction>(this String? str, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<Char, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<Char, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, str, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current string by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the function.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TResult, TFunction>(this String? str, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedContextFunction<Char, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<Char, TResult>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, str, out result);
#pragma warning disable CS8500
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this ReadOnlySpan<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, span);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, span);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this T[]? arr, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, arr);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
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
		where TAction : struct, IReadOnlyFixedContextAction<T>
#else
		where TAction : struct, IReadOnlyFixedContextAction<T>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, span);
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
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
		where TAction : struct, IReadOnlyFixedContextAction<T>
#else
		where TAction : struct, IReadOnlyFixedContextAction<T>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, span);
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this T[]? arr, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<T>
#else
		where TAction : struct, IReadOnlyFixedContextAction<T>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref action, arr);
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
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
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, span, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this Span<T> span, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, span, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this T[]? arr, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, arr, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
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
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, span, out result);
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
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
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, span, out result);
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this T[]? arr, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
		=> ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref func, arr, out result);

	/// <summary>
	/// Pins <paramref name="str"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="str">The <see cref="String"/> instance to pin during the action.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<TAction>(ref TAction action, String? str)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<Char>
#else
		where TAction : IReadOnlyFixedContextAction<Char>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			if (str is not null)
				fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
					action.Accept(new(ptr, str.Length));
			else
				action.Accept(default);
			return;
		}
		NonGenericReadOnlyFixedContextAction<Char> nonGeneric =
			NonGenericReadOnlyFixedContextAction<Char>.Create(action);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, str);
	}
	/// <summary>
	/// Pins <paramref name="str"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="str">The <see cref="String"/> instance to pin during the function.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<TFunction, TResult>(ref TFunction func, String? str, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<Char, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<Char, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			if (str is not null)
				fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
					result = func.Apply(new(ptr, str.Length));
			else
				result = func.Apply(default);
			return;
		}
		NonGenericReadOnlyFixedContextFunction<Char, TResult> nonGeneric =
			NonGenericReadOnlyFixedContextFunction<Char, TResult>.Create(func);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, str, out result);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TAction>(ref TAction action, ReadOnlySpan<T> span)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				action.Accept(new(ptr, span.Length));
			return;
		}
		NonGenericReadOnlyFixedContextAction<T> nonGeneric = NonGenericReadOnlyFixedContextAction<T>.Create(action);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, span);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TAction>(ref TAction action, Span<T> span)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				action.Accept(new FixedContextValue<T>(ptr, span.Length));
			return;
		}
		NonGenericReadOnlyFixedContextAction<T> nonGeneric = NonGenericReadOnlyFixedContextAction<T>.Create(action);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, span);
	}
	/// <summary>
	/// Pins <paramref name="arr"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TAction>(ref TAction action, T[]? arr)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			if (arr is not null)
				fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
					action.Accept(new FixedContextValue<T>(ptr, arr.Length));
			else
				action.Accept(default);
			return;
		}
		NonGenericReadOnlyFixedContextAction<T> nonGeneric = NonGenericReadOnlyFixedContextAction<T>.Create(action);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, arr);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
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
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				result = func.Apply(new(ptr, span.Length));
			return;
		}
		NonGenericReadOnlyFixedContextFunction<T, TResult> nonGeneric =
			NonGenericReadOnlyFixedContextFunction<T, TResult>.Create(func);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, span, out result);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
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
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
			return;
		}
		NonGenericReadOnlyFixedContextFunction<T, TResult> nonGeneric =
			NonGenericReadOnlyFixedContextFunction<T, TResult>.Create(func);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, span, out result);
	}
	/// <summary>
	/// Pins <paramref name="arr"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TFunction, TResult>(ref TFunction func, T[]? arr, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			if (arr is not null)
				fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
					result = func.Apply(new FixedContextValue<T>(ptr, arr.Length));
			else
				result = func.Apply(default);
			return;
		}
		NonGenericReadOnlyFixedContextFunction<T, TResult> nonGeneric =
			NonGenericReadOnlyFixedContextFunction<T, TResult>.Create(func);
		ReadOnlyFixedContextValueExtensions.WithSafeFixed(ref nonGeneric, arr, out result);
	}

#pragma warning restore CS8500
}