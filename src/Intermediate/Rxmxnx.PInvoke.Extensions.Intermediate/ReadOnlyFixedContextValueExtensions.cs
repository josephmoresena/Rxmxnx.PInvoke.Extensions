namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations for <see cref="ReadOnlyFixedContext{T}"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
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
	/// <exception cref="ArgumentException">A read-only memory with non-unmanaged items cannot be pinned.</exception>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this String? str, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<Char>
#else
		where TAction : IReadOnlyFixedContextAction<Char>, allows ref struct
#endif
	{
		if (str is not null && action is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
				action.Accept(new(ptr, str.Length));
		else if (action is not null)
			action.Accept(default);
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TResult, TFunction>(this String? str, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<Char, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<Char, TResult>, allows ref struct
#endif
	{
		if (str is not null && func is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
				result = func.Apply(new(ptr, str.Length));
		else if (func is not null)
			result = func.Apply(default);
		else
			Unsafe.SkipInit(out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current string by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="str">The <see cref="String"/> instance to pin during the action.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this String? str, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<Char>
#else
		where TAction : struct, IReadOnlyFixedContextAction<Char>, allows ref struct
#endif
	{
		if (str is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
				action.Accept(new(ptr, str.Length));
		else
			action.Accept(default);
	}
#pragma warning disable CS8500
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this ReadOnlySpan<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			action.Accept(new(ptr, span.Length));
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			action.Accept(new FixedContextValue<T>(ptr, span.Length));
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this T[]? arr, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<T>
#else
		where TAction : IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (arr is not null && action is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
				action.Accept(new FixedContextValue<T>(ptr, arr.Length));
		else if (action is not null)
			action.Accept(default);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this ReadOnlySpan<T> span, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<T>
#else
		where TAction : struct, IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			action.Accept(new(ptr, span.Length));
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<T>
#else
		where TAction : struct, IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			action.Accept(new FixedContextValue<T>(ptr, span.Length));
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this T[]? arr, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<T>
#else
		where TAction : struct, IReadOnlyFixedContextAction<T>, allows ref struct
#endif
	{
		if (arr is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
				action.Accept(new FixedContextValue<T>(ptr, arr.Length));
		else
			action.Accept(default);
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
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			result = func.Apply(new(ptr, span.Length));
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
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this T[]? arr, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (arr is not null && func is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
				result = func.Apply(new(ptr, arr.Length));
		else if (func is not null)
			result = func.Apply(default);
		else
			Unsafe.SkipInit(out result);
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this ReadOnlySpan<T> span, ref TFunction func,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			result = func.Apply(new(ptr, span.Length));
	}
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this Span<T> span, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this T[]? arr, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (arr is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
				result = func.Apply(new(ptr, arr.Length));
		else
			result = func.Apply(default);
	}
#pragma warning restore CS8500
}