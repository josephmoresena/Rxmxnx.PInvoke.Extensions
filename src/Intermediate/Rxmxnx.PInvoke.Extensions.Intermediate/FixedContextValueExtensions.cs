#if !NET6_0_OR_GREATER
using ArgumentNullExceptionCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif
#if NETSTANDARD
using PreserveAttribute = Rxmxnx.PInvoke.Internal.FrameworkCompat.PreserveAttribute;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations for <see cref="FixedContextValue{T}"/> instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
#if !PACKAGE
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
	/// <exception cref="ArgumentException">A read-only memory with non-unmanaged items cannot be pinned.</exception>
	/// <remarks>
	/// This method pins the memory to prevent the garbage collector from moving it, which is essential for safe
	/// operations on unmanaged memory.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the pinned memory
	/// and avoid memory leaks.
	/// </remarks>
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
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
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
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedContextAction<T>
#else
		where TAction : IFixedContextAction<T>, allows ref struct
#endif
	{
		if (action is null) return;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			action.Accept(new(ptr, span.Length));
#else
			new FixedAction<T, TAction>(ref action, new(ptr, span.Length)).Accept();
#endif
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this T[]? arr, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedContextAction<T>
#else
		where TAction : IFixedContextAction<T>, allows ref struct
#endif
	{
		if (arr is not null && action is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
#if !NETSTANDARD
				action.Accept(new(ptr, arr.Length));
#else
				new FixedAction<T, TAction>(ref action, new(ptr, arr.Length)).Accept();
#endif
		else if (action is not null)
#if !NETSTANDARD
			action.Accept(default);
#else
			new FixedAction<T, TAction>(ref action).Accept();
#endif
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current span by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="span">The current span of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this Span<T> span, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedContextAction<T>
#else
		where TAction : struct, IFixedContextAction<T>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			action.Accept(new(ptr, span.Length));
#else
		fixed (void* _ = &action)
			new FixedAction<T, TAction>(ref action, new(ptr, span.Length)).Accept();
#endif
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current array by pinning its memory address until the
	/// specified action has completed.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	/// <param name="arr">The current array of type <typeparamref name="T"/>.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TAction>(this T[]? arr, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedContextAction<T>
#else
		where TAction : struct, IFixedContextAction<T>, allows ref struct
#endif
	{
		if (arr is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
#if !NETSTANDARD
				action.Accept(new(ptr, arr.Length));
#else
			fixed (void* _ = &action)
				new FixedAction<T, TAction>(ref action, new(ptr, arr.Length)).Accept();
#endif
		else
#if !NETSTANDARD
			action.Accept(default);
#else
			fixed (void* _ = &action)
				new FixedAction<T, TAction>(ref action).Accept();
#endif
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
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			result = func.Apply(new(ptr, span.Length));
#else
			result = new FixedFunction<T, TResult, TFunction>(ref func, new(ptr, span.Length)).Apply();
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
		where TFunction : IFixedContextFunction<T, TResult>
#else
		where TFunction : IFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (arr is not null && func is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
#if !NETSTANDARD
				result = func.Apply(new(ptr, arr.Length));
#else
				result = new FixedFunction<T, TResult, TFunction>(ref func, new(ptr, arr.Length)).Apply();
#endif
		else if (func is not null)
#if !NETSTANDARD
			result = func.Apply(default);
#else
			result = new FixedFunction<T, TResult, TFunction>(ref func).Apply();
#endif
		else
			Unsafe.SkipInit(out result);
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TFunction, TResult>(this Span<T> span, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
#if !NETSTANDARD
			result = func.Apply(new(ptr, span.Length));
#else
		fixed (void* _ = &func)
			result = new FixedFunction<T, TResult, TFunction>(ref func, new(ptr, span.Length)).Apply();
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
		where TFunction : struct, IFixedContextFunction<T, TResult>
#else
		where TFunction : struct, IFixedContextFunction<T, TResult>, allows ref struct
#endif
	{
		if (arr is not null)
			fixed (void* ptr = &NativeUtilities.GetArrayDataReference(arr))
#if !NETSTANDARD
				result = func.Apply(new(ptr, arr.Length));
#else
			fixed (void* _ = &func)
				result = new FixedFunction<T, TResult, TFunction>(ref func, new(ptr, arr.Length)).Apply();
#endif
		else
#if !NETSTANDARD
			result = func.Apply(default);
#else
			fixed (void* _ = &func)
				result = new FixedFunction<T, TResult, TFunction>(ref func).Apply();
#endif
	}

#if NETSTANDARD
	/// <summary>
	/// Wrapper ref-struct for action value.
	/// </summary>
	/// <typeparam name="T">The type that is contained in the contiguous region of memory.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IFixedContextAction{T}"/>.</typeparam>
	[Preserve(AllMembers = true, Conditional = true)]
	private readonly ref struct FixedAction<T, TAction> where TAction : IFixedContextAction<T>
	{
		/// <summary>
		/// Action pointer.
		/// </summary>
		private readonly TAction* _aPointer;
		/// <summary>
		/// Fixed context value.
		/// </summary>
		private readonly FixedContextValue<T> _ctx;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
		/// <param name="ctx">A <see cref="FixedContextValue{T}"/> instance.</param>
		public FixedAction(ref TAction action, FixedContextValue<T> ctx = default)
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
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	[Preserve(AllMembers = true, Conditional = true)]
	private readonly ref struct FixedFunction<T, TResult, TFunction> where TFunction : IFixedContextFunction<T, TResult>
	{
		/// <summary>
		/// Action pointer.
		/// </summary>
		private readonly TFunction* _fPointer;
		/// <summary>
		/// Fixed context value.
		/// </summary>
		private readonly FixedContextValue<T> _ctx;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
		/// <param name="ctx">A <see cref="FixedContextValue{T}"/> instance.</param>
		public FixedFunction(ref TFunction func, FixedContextValue<T> ctx = default)
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