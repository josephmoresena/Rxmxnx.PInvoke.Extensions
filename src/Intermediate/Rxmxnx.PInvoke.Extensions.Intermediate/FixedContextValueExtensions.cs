namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations for <see cref="FixedContextValue{T}"/> instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class FixedContextValueExtensions
{
	/// <summary>
	/// Creates a <see cref="FixedContextValue{T}"/>  instance by pinning the current <see cref="Memory{T}"/> instance,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="Memory{T}"/>.</typeparam>
	/// <param name="mem">A <see cref="Memory{T}"/> instance.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the pinned memory.
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
	public static IDisposable GetFixedContext<T>(this Memory<T> mem, out FixedContextValue<T> fixedContext)
	{
		MemoryHandle handle = mem.Pin();
		fixedContext = new(handle, mem.Length, out IDisposable result);
		return result;
	}
	/// <summary>
	/// Rents and pins an array of minimum <paramref name="count"/> elements from <paramref name="arrayPool"/>,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">
	/// The unmanaged type from which the contiguous region of memory will be fixed.
	/// </typeparam>
	/// <param name="arrayPool">An <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="count">Minimum size of rented array.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the pinned memory.
	/// </param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <returns>An <see cref="IDisposable"/> instance representing the pinned memory.</returns>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable RentFixed<T>(this ArrayPool<T> arrayPool, Int32 count,
		out FixedContextValue<T> fixedContext, Boolean clearArray = false) where T : unmanaged
	{
#if !NET6_0_OR_GREATER
		ArgumentNullExceptionCompat.ThrowIfNull(arrayPool);
#else
		ArgumentNullException.ThrowIfNull(arrayPool);
#endif
		return RentedMemoryOwner<T>.CreateContext(arrayPool, count, clearArray, out fixedContext, out _);
	}
	/// <summary>
	/// Rents and pins an array of minimum <paramref name="count"/> elements from <paramref name="arrayPool"/>,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">
	/// The unmanaged type from which the contiguous region of memory will be fixed.
	/// </typeparam>
	/// <param name="arrayPool">An <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="count">Minimum size of rented array.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the pinned memory.
	/// </param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <param name="arrayLength">Output. Rented array length.</param>
	/// <returns>An <see cref="IDisposable"/> instance representing the pinned memory.</returns>
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
	public static IDisposable RentFixed<T>(this ArrayPool<T> arrayPool, Int32 count,
		out FixedContextValue<T> fixedContext, Boolean clearArray, out Int32 arrayLength) where T : unmanaged
	{
#if !NET6_0_OR_GREATER
		ArgumentNullExceptionCompat.ThrowIfNull(arrayPool);
#else
		ArgumentNullException.ThrowIfNull(arrayPool);
#endif
		return RentedMemoryOwner<T>.CreateContext(arrayPool, count, clearArray, out fixedContext, out arrayLength);
	}
#pragma warning disable CS8500
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedContextAction<T>
#else
		where TAction : IFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		FixedContextValueExtensions.WithSafeFixed(ref action, span);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this T[]? arr, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedContextAction<T>
#else
		where TAction : IFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		FixedContextValueExtensions.WithSafeFixed(ref action, arr);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
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
		where TAction : struct, IFixedContextAction<T>
#else
		where TAction : struct, IFixedContextAction<T>, allows ref struct
#endif
		=> FixedContextValueExtensions.WithSafeFixed(ref action, span);
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
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
		where TAction : struct, IFixedContextAction<T>
#else
		where TAction : struct, IFixedContextAction<T>, allows ref struct
#endif
		=> FixedContextValueExtensions.WithSafeFixed(ref action, arr);
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TFunction, TResult>(this Span<T> span, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedContextFunction<T, TResult>
#else
		where TFunction : IFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		FixedContextValueExtensions.WithSafeFixed(ref func, span, out result);
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
		where TFunction : IFixedContextFunction<T, TResult>
#else
		where TFunction : IFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		FixedContextValueExtensions.WithSafeFixed(ref func, arr, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
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
	public static void WithSafeFixed<T, TFunction, TResult>(this Span<T> span, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IFixedContextFunction<T, TResult>, allows ref struct
#endif
		=> FixedContextValueExtensions.WithSafeFixed(ref func, span, out result);
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
		where TFunction : struct, IFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IFixedContextFunction<T, TResult>, allows ref struct
#endif
		=> FixedContextValueExtensions.WithSafeFixed(ref func, arr, out result);

	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TAction>(ref TAction action, Span<T> span)
#if !NET9_0_OR_GREATER
		where TAction : IFixedContextAction<T>
#else
		where TAction : IFixedContextAction<T>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				action.Accept(new(ptr, span.Length));
			return;
		}
		NonGenericFixedContextAction<T> nonGeneric = NonGenericFixedContextAction<T>.Create(action);
		FixedContextValueExtensions.WithSafeFixed(ref nonGeneric, span);
	}
	/// <summary>
	/// Pins <paramref name="arr"/> and executes <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WithSafeFixed<T, TAction>(ref TAction action, T[]? arr)
#if !NET9_0_OR_GREATER
		where TAction : IFixedContextAction<T>
#else
		where TAction : IFixedContextAction<T>, allows ref struct
#endif
	{
		if (typeof(TAction).IsValueType)
		{
			if (arr is not null)
				fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
					action.Accept(new(ptr, arr.Length));
			else
				action.Accept(default);
			return;
		}
		NonGenericFixedContextAction<T> nonGeneric = NonGenericFixedContextAction<T>.Create(action);
		FixedContextValueExtensions.WithSafeFixed(ref nonGeneric, arr);
	}
	/// <summary>
	/// Pins <paramref name="span"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
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
		where TFunction : IFixedContextFunction<T, TResult>
#else
		where TFunction : IFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
				result = func.Apply(new(ptr, span.Length));
			return;
		}
		NonGenericFixedContextFunction<T, TResult> nonGeneric = NonGenericFixedContextFunction<T, TResult>.Create(func);
		FixedContextValueExtensions.WithSafeFixed(ref nonGeneric, span, out result);
	}
	/// <summary>
	/// Pins <paramref name="arr"/> and executes <paramref name="func"/>.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
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
		where TFunction : IFixedContextFunction<T, TResult>
#else
		where TFunction : IFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (typeof(TFunction).IsValueType)
		{
			if (arr is not null)
				fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
					result = func.Apply(new(ptr, arr.Length));
			else
				result = func.Apply(default);
			return;
		}
		NonGenericFixedContextFunction<T, TResult> nonGeneric = NonGenericFixedContextFunction<T, TResult>.Create(func);
		FixedContextValueExtensions.WithSafeFixed(ref nonGeneric, arr, out result);
	}

#pragma warning restore CS8500
}