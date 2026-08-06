#if NETSTANDARD
using PreserveAttribute = Rxmxnx.PInvoke.Internal.FrameworkCompat.PreserveAttribute;
#endif

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
	/// <exception cref="ArgumentException">A read-only memory with non-unmanaged items cannot be pinned.</exception>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable GetFixedMemory<T>(this ReadOnlyMemory<T> mem, out FixedPointerValue fixedMemory)
	{
		IDisposable result = mem.GetFixedContext(out ReadOnlyFixedContextValue<T> fixedContext);
		fixedMemory = fixedContext;
		return result;
	}
	/// <summary>
	/// Creates an <see cref="FixedPointerValue"/> instance by pinning the current <see cref="Memory{T}"/> instance,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">The type of items in the <see cref="Memory{T}"/>.</typeparam>
	/// <param name="mem">A <see cref="Memory{T}"/> instance.</param>
	/// <param name="fixedMemory">
	/// Output. The <see cref="FixedPointerValue"/> instance representing the pinned memory.
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedAction
#else
		where TAction : IFixedAction, allows ref struct
#endif
	{
		if (action is null) return;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			action.Accept(new FixedContextValue<T>(ptr, span.Length));
#else
			new FixedAction<TAction>(ref action, new FixedContextValue<T>(ptr, span.Length)).Accept();
#endif
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this ReadOnlySpan<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedAction
#else
		where TAction : IFixedAction, allows ref struct
#endif
	{
		if (action is null) return;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			action.Accept(new ReadOnlyFixedContextValue<T>(ptr, span.Length));
#else
			new FixedAction<TAction>(ref action, new ReadOnlyFixedContextValue<T>(ptr, span.Length)).Accept();
#endif
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedAction
#else
		where TAction : struct, IFixedAction, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			action.Accept(new FixedContextValue<T>(ptr, span.Length));
#else
		fixed (void* _ = &action)
			new FixedAction<TAction>(ref action, new FixedContextValue<T>(ptr, span.Length)).Accept();
#endif
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current read-only span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	/// <param name="span">The current read-only span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this ReadOnlySpan<T> span, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedAction
#else
		where TAction : struct, IFixedAction, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			action.Accept(new ReadOnlyFixedContextValue<T>(ptr, span.Length));
#else
		fixed (void* _ = &action)
			new FixedAction<TAction>(ref action, new ReadOnlyFixedContextValue<T>(ptr, span.Length)).Accept();
#endif
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
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
#else
		{
			result = new FixedFunction<TResult, TFunction>(ref func, new FixedContextValue<T>(ptr, span.Length))
				.Apply();
		}
#endif
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
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			result = func.Apply(new ReadOnlyFixedContextValue<T>(ptr, span.Length));
#else
		{
			result = new FixedFunction<TResult, TFunction>(ref func, new ReadOnlyFixedContextValue<T>(ptr, span.Length))
				.Apply();
		}
#endif
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this Span<T> span, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedFunction<TResult>
#else
		where TFunction : struct, IFixedFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			result = func.Apply(new FixedContextValue<T>(ptr, span.Length));
#else
		fixed (void* _ = &func)
		{
			result = new FixedFunction<TResult, TFunction>(ref func, new FixedContextValue<T>(ptr, span.Length))
				.Apply();
		}
#endif
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TResult, TFunction>(this ReadOnlySpan<T> span, ref TFunction func,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedFunction<TResult>
#else
		where TFunction : struct, IFixedFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			result = func.Apply(new ReadOnlyFixedContextValue<T>(ptr, span.Length));
#else
		fixed (void* _ = &func)
		{
			result = new FixedFunction<TResult, TFunction>(ref func, new ReadOnlyFixedContextValue<T>(ptr, span.Length))
				.Apply();
		}
#endif
	}

#if NETSTANDARD
	/// <summary>
	/// Wrapper ref-struct for action value.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedAction"/>.</typeparam>
	[Preserve(AllMembers = true, Conditional = true)]
	private readonly ref struct FixedAction<TAction> where TAction : IFixedAction
	{
		/// <summary>
		/// Action pointer.
		/// </summary>
		private readonly TAction* _aPointer;
		/// <summary>
		/// Fixed context value.
		/// </summary>
		private readonly FixedPointerValue _ptr;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
		/// <param name="ptr">A <see cref="FixedPointerValue"/> instance.</param>
		public FixedAction(ref TAction action, FixedPointerValue ptr = default)
		{
			this._aPointer = (TAction*)Unsafe.AsPointer(ref action);
			this._ptr = ptr;
		}
		/// <summary>
		/// Performs an operation using the fixed context.
		/// </summary>
		public void Accept() => this._aPointer[0].Accept(this._ptr);
	}

	/// <summary>
	/// Wrapper ref-struct for function value.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the function.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedFunction{TResult}"/>.</typeparam>
	[Preserve(AllMembers = true, Conditional = true)]
	private readonly ref struct FixedFunction<TResult, TFunction> where TFunction : IFixedFunction<TResult>
	{
		/// <summary>
		/// Action pointer.
		/// </summary>
		private readonly TFunction* _fPointer;
		/// <summary>
		/// Fixed context value.
		/// </summary>
		private readonly FixedPointerValue _ptr;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
		/// <param name="ptr">A <see cref="FixedPointerValue"/> instance.</param>
		public FixedFunction(ref TFunction func, FixedPointerValue ptr = default)
		{
			this._fPointer = (TFunction*)Unsafe.AsPointer(ref func);
			this._ptr = ptr;
		}
		/// <summary>
		/// Performs an operation using the fixed context and returns a result.
		/// </summary>
		/// <returns>The result produced by the operation.</returns>
		public TResult Apply() => this._fPointer[0].Apply(this._ptr);
	}
#endif
#pragma warning restore CS8500
}