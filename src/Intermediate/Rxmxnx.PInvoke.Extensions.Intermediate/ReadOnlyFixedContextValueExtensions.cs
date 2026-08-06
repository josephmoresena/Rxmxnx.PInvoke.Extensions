#if NETSTANDARD
using PreserveAttribute = Rxmxnx.PInvoke.Internal.FrameworkCompat.PreserveAttribute;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations for <see cref="ReadOnlyFixedContextValue{T}"/> instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
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
#if !NETSTANDARD
				action.Accept(new(ptr, str.Length));
#else
				new ReadOnlyFixedAction<Char, TAction>(ref action, new(ptr, str.Length)).Accept();
#endif
		else if (action is not null)
#if !NETSTANDARD
			action.Accept(default);
#else
			new ReadOnlyFixedAction<Char, TAction>(ref action).Accept();
#endif
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
#if !NETSTANDARD
				action.Accept(new(ptr, str.Length));
#else
#pragma warning disable CS8500
			fixed (void* _ = &action)
#pragma warning restore CS8500
				new ReadOnlyFixedAction<Char, TAction>(ref action, new(ptr, str.Length)).Accept();
#endif
		else
#if !NETSTANDARD
			action.Accept(default);
#else
#pragma warning disable CS8500
			fixed (void* _ = &action)
#pragma warning restore CS8500
				new ReadOnlyFixedAction<Char, TAction>(ref action).Accept();
#endif
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
#if !NETSTANDARD
				result = func.Apply(new(ptr, str.Length));
#else
				result = new ReadOnlyFixedFunction<Char, TResult, TFunction>(ref func, new(ptr, str.Length)).Apply();
#endif
		else if (func is not null)
#if !NETSTANDARD
			result = func.Apply(default);
#else
			result = new ReadOnlyFixedFunction<Char, TResult, TFunction>(ref func).Apply();
#endif
		else
			Unsafe.SkipInit(out result);
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
	public static void WithSafeFixed<TResult, TFunction>(this String? str, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedContextFunction<Char, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<Char, TResult>, allows ref struct
#endif
	{
		if (str is not null)
			fixed (void* ptr = &MemoryMarshal.GetReference(str.AsSpan()))
#if !NETSTANDARD
				result = func.Apply(new(ptr, str.Length));
#else
#pragma warning disable CS8500
			fixed (void* _ = &func)
#pragma warning restore CS8500
				result = new ReadOnlyFixedFunction<Char, TResult, TFunction>(ref func, new(ptr, str.Length)).Apply();
#endif
		else
#if !NETSTANDARD
			result = func.Apply(default);
#else
#pragma warning disable CS8500
			fixed (void* _ = &func)
#pragma warning restore CS8500
				result = new ReadOnlyFixedFunction<Char, TResult, TFunction>(ref func).Apply();
#endif
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
#if !NETSTANDARD
			action.Accept(new(ptr, span.Length));
#else
			new ReadOnlyFixedAction<T, TAction>(ref action, new(ptr, span.Length)).Accept();
#endif
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
#if !NETSTANDARD
			action.Accept(new FixedContextValue<T>(ptr, span.Length));
#else
			new ReadOnlyFixedAction<T, TAction>(ref action, new FixedContextValue<T>(ptr, span.Length)).Accept();
#endif
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
#if !NETSTANDARD
				action.Accept(new FixedContextValue<T>(ptr, arr.Length));
#else
				new ReadOnlyFixedAction<T, TAction>(ref action, new FixedContextValue<T>(ptr, arr.Length)).Accept();
#endif
		else if (action is not null)
#if !NETSTANDARD
			action.Accept(default);
#else
			new ReadOnlyFixedAction<T, TAction>(ref action).Accept();
#endif
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
#if !NETSTANDARD
			action.Accept(new(ptr, span.Length));
#else
		fixed (void* _ = &action)
			new ReadOnlyFixedAction<T, TAction>(ref action, new(ptr, span.Length)).Accept();
#endif
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
#if !NETSTANDARD
			action.Accept(new FixedContextValue<T>(ptr, span.Length));
#else
		fixed (void* _ = &action)
			new ReadOnlyFixedAction<T, TAction>(ref action, new FixedContextValue<T>(ptr, span.Length)).Accept();
#endif
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
#if !NETSTANDARD
				action.Accept(new FixedContextValue<T>(ptr, arr.Length));
#else
			fixed (void* _ = &action)
				new ReadOnlyFixedAction<T, TAction>(ref action, new FixedContextValue<T>(ptr, arr.Length)).Accept();
#endif
		else
#if !NETSTANDARD
			action.Accept(default);
#else
			fixed (void* _ = &action)
				new ReadOnlyFixedAction<T, TAction>(ref action).Accept();
#endif
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
#if !NETSTANDARD
			result = func.Apply(new(ptr, span.Length));
#else
			result = new ReadOnlyFixedFunction<T, TResult, TFunction>(ref func, new(ptr, span.Length)).Apply();
#endif
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
#if !NETSTANDARD
			result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
#else
		{
			result = new ReadOnlyFixedFunction<T, TResult, TFunction>(
				ref func, new FixedContextValue<T>(ptr, span.Length)).Apply();
		}
#endif
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
#if !NETSTANDARD
				result = func.Apply(new FixedContextValue<T>(ptr, arr.Length));
#else
			{
				result = new ReadOnlyFixedFunction<T, TResult, TFunction>(
					ref func, new FixedContextValue<T>(ptr, arr.Length)).Apply();
			}
#endif
		else if (func is not null)
#if !NETSTANDARD
			result = func.Apply(default);
#else
			result = new ReadOnlyFixedFunction<T, TResult, TFunction>(ref func).Apply();
#endif
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
#if !NETSTANDARD
			result = func.Apply(new(ptr, span.Length));
#else
		fixed (void* _ = &func)
			result = new ReadOnlyFixedFunction<T, TResult, TFunction>(ref func, new(ptr, span.Length)).Apply();
#endif
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
#if !NETSTANDARD
			result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
#else
		fixed (void* _ = &func)
		{
			result = new ReadOnlyFixedFunction<T, TResult, TFunction>(
				ref func, new FixedContextValue<T>(ptr, span.Length)).Apply();
		}
#endif
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
#if !NETSTANDARD
				result = func.Apply(new FixedContextValue<T>(ptr, arr.Length));
#else
			fixed (void* _ = &func)
			{
				result = new ReadOnlyFixedFunction<T, TResult, TFunction>(
					ref func, new FixedContextValue<T>(ptr, arr.Length)).Apply();
			}
#endif
		else
#if !NETSTANDARD
			result = func.Apply(default);
#else
			fixed (void* _ = &func)
				result = new ReadOnlyFixedFunction<T, TResult, TFunction>(ref func).Apply();
#endif
	}

#if NETSTANDARD
	/// <summary>
	/// Wrapper ref-struct for action value.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	[Preserve(AllMembers = true, Conditional = true)]
	private readonly ref struct ReadOnlyFixedAction<T, TAction> where TAction : IReadOnlyFixedContextAction<T>
	{
		/// <summary>
		/// Action pointer.
		/// </summary>
		private readonly TAction* _aPointer;
		/// <summary>
		/// Fixed context value.
		/// </summary>
		private readonly ReadOnlyFixedContextValue<T> _ctx;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
		/// <param name="ctx">A <see cref="ReadOnlyFixedContextValue{T}"/> instance.</param>
		public ReadOnlyFixedAction(ref TAction action, ReadOnlyFixedContextValue<T> ctx = default)
		{
			this._aPointer = (TAction*)Unsafe.AsPointer(ref action);
			this._ctx = ctx;
		}
		/// <summary>
		/// Performs an operation using the fixed context.
		/// </summary>
		public void Accept() => this._aPointer[0].Accept(this._ctx);
	}

	/// <summary>
	/// Wrapper ref-struct for function value.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IReadOnlyFixedContextFunction{T,TResult}"/>.</typeparam>
	[Preserve(AllMembers = true, Conditional = true)]
	private readonly ref struct ReadOnlyFixedFunction<T, TResult, TFunction>
		where TFunction : IReadOnlyFixedContextFunction<T, TResult>
	{
		/// <summary>
		/// Action pointer.
		/// </summary>
		private readonly TFunction* _fPointer;
		/// <summary>
		/// Fixed context value.
		/// </summary>
		private readonly ReadOnlyFixedContextValue<T> _ctx;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
		/// <param name="ctx">A <see cref="ReadOnlyFixedContextValue{T}"/> instance.</param>
		public ReadOnlyFixedFunction(ref TFunction func, ReadOnlyFixedContextValue<T> ctx = default)
		{
			this._fPointer = (TFunction*)Unsafe.AsPointer(ref func);
			this._ctx = ctx;
		}
		/// <summary>
		/// Performs an operation using the fixed context and returns a result.
		/// </summary>
		/// <returns>The result produced by the operation.</returns>
		public TResult Apply() => this._fPointer[0].Apply(this._ctx);
	}
#endif
#pragma warning restore CS8500
}