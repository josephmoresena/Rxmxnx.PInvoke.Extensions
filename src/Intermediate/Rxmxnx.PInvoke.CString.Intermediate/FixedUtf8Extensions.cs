namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic fixing operations with UTF-8 texts.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class FixedUtf8Extensions
{
	/// <summary>
	/// Prevents the garbage collector from relocating the current UTF-8 string by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="cstr">The <see cref="CString"/> instance to pin during the action.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CString? cstr, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<Byte>
#else
		where TAction : IReadOnlyFixedContextAction<Byte>, allows ref struct
#endif
	{
		if (cstr is not null && action is not null)
			fixed (void* ptr = cstr)
				action.Accept(new(ptr, cstr.Length));
		else if (action is not null)
			action.Accept(default);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current UTF-8 string by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="cstr">The <see cref="CString"/> instance to pin during the function.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TResult, TFunction>(this CString? cstr, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<Byte, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<Byte, TResult>, allows ref struct
#endif
	{
		if (cstr is not null && func is not null)
			fixed (void* ptr = cstr)
				result = func.Apply(new(ptr, cstr.Length));
		else if (func is not null)
			result = func.Apply(default);
		else
			Unsafe.SkipInit(out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current UTF-8 string by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="cstr">The <see cref="CString"/> instance to pin during the action.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CString? cstr, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<Byte>
#else
		where TAction : struct, IReadOnlyFixedContextAction<Byte>, allows ref struct
#endif
	{
		if (cstr is not null)
			fixed (void* ptr = cstr)
				action.Accept(new(ptr, cstr.Length));
		else
			action.Accept(default);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes their
	/// memory addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CStringSequence? seq, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (seq is null || seq.Count == 0)
		{
			action.Accept(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Information =
					FixedUtf8Extensions.InitializeInfo(seq, stackalloc FixedPointerInfo[seq.Count]),
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes their
	/// memory addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CStringSequence? seq, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		if (seq is null || seq.Count == 0)
		{
			action.Accept(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Information =
					FixedUtf8Extensions.InitializeInfo(seq, stackalloc FixedPointerInfo[seq.Count]),
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes
	/// their memory addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult>(this CStringSequence? seq, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		if (seq is null || seq.Count == 0)
		{
			result = func.Apply(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Information =
					FixedUtf8Extensions.InitializeInfo(
						seq, stackalloc FixedPointerInfo[seq.Count]),
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes
	/// their memory addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult>(this CStringSequence? seq, ref TFunction func,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		if (seq is null || seq.Count == 0)
		{
			result = func.Apply(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Information =
					FixedUtf8Extensions.InitializeInfo(
						seq, stackalloc FixedPointerInfo[seq.Count]),
			});
		}
	}

	/// <summary>
	/// Initializes the <paramref name="span"/> span with <paramref name="source"/> elements information.
	/// </summary>
	/// <param name="source">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="span">Destination span.</param>
	/// <returns>Initialized <see cref="FixedPointerInfo"/> read-only span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<FixedPointerInfo> InitializeInfo(CStringSequence source, Span<FixedPointerInfo> span)
	{
		CStringSequence.Utf8View view = new(source, true);
		Int32 i = 0;
		foreach (ReadOnlySpan<Byte> value in view)
		{
			span[i++] = new()
			{
				Pointer = Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
				Count = value.Length,
				SizeOf = sizeof(Byte),
				IsUnmanaged = true,
				ConstructorOrFunctionPointer = default,
#if !NET5_0_OR_GREATER
				TypeOrFunctionPointer = CStringSequence.TypePointer,
#else
				TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<Byte>),
#endif
			};
		}
		return span;
	}
}