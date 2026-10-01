namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="FixedPointerValueList"/> instances.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static partial class FixedPointerListValueExtensions
{
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1>(this TAction? action, Span<T0> span0, Span<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList2.WithSafeFixed(ref action, span0, span1);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList2.WithSafeFixed(ref nonGeneric, span0, span1);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1>(this TAction? action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList2.WithSafeFixed(ref action, span0, span1);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList2.WithSafeFixed(ref nonGeneric, span0, span1);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1>(this TFunction? func, Span<T0> span0, Span<T1> span1,
		out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList2.WithSafeFixed(ref func, span0, span1, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList2.WithSafeFixed(ref nonGeneric, span0, span1, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1>(this TFunction? func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList2.WithSafeFixed(ref func, span0, span1, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList2.WithSafeFixed(ref nonGeneric, span0, span1, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2>(this TAction? action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList3.WithSafeFixed(ref action, span0, span1, span2);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList3.WithSafeFixed(ref nonGeneric, span0, span1, span2);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2>(this TAction? action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList3.WithSafeFixed(ref action, span0, span1, span2);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList3.WithSafeFixed(ref nonGeneric, span0, span1, span2);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2>(this TFunction? func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList3.WithSafeFixed(ref func, span0, span1, span2, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList3.WithSafeFixed(ref nonGeneric, span0, span1, span2, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2>(this TFunction? func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList3.WithSafeFixed(ref func, span0, span1, span2, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList3.WithSafeFixed(ref nonGeneric, span0, span1, span2, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3>(this TAction? action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList4.WithSafeFixed(ref action, span0, span1, span2, span3);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList4.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3>(this TAction? action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList4.WithSafeFixed(ref action, span0, span1, span2, span3);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList4.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this TFunction? func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList4.WithSafeFixed(ref func, span0, span1, span2, span3, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList4.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this TFunction? func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList4.WithSafeFixed(ref func, span0, span1, span2, span3, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList4.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4>(this TAction? action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList5.WithSafeFixed(ref action, span0, span1, span2, span3, span4);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList5.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4>(this TAction? action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList5.WithSafeFixed(ref action, span0, span1, span2, span3, span4);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList5.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this TFunction? func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList5.WithSafeFixed(ref func, span0, span1, span2, span3, span4, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList5.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this TFunction? func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList5.WithSafeFixed(ref func, span0, span1, span2, span3, span4, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList5.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this TAction? action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList6.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList6.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this TAction? action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList6.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList6.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this TFunction? func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList6.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList6.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this TFunction? func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList6.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList6.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this TAction? action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList7.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList7.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this TAction? action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList7.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList7.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this TFunction? func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList7.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList7.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this TFunction? func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList7.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList7.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6, out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
	/// <param name="span7">The eighth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this TAction? action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList8.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6, span7);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList8.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6, span7);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
	/// <param name="span7">The eighth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this TAction? action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList8.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6, span7);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList8.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6, span7);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
	/// <param name="span7">The eighth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this TFunction? func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		Span<T7> span7, out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList8.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, span7,
			                                out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList8.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6, span7,
		                                out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
	/// <param name="span7">The eighth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this TFunction? func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7,
		out TResult result)
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
		if (typeof(TFunction).IsValueType)
		{
			FixedPointerList8.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, span7,
			                                out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList8.WithSafeFixed(ref nonGeneric, span0, span1, span2, span3, span4, span5, span6, span7,
		                                out result);
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1>(this ref TAction action, Span<T0> span0, Span<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList2.WithSafeFixed(ref action, span0, span1);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList2.WithSafeFixed(ref action, span0, span1);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1>(this ref TFunction func, Span<T0> span0,
		Span<T1> span1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList2.WithSafeFixed(ref func, span0, span1, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList2.WithSafeFixed(ref func, span0, span1, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList3.WithSafeFixed(ref action, span0, span1, span2);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList3.WithSafeFixed(ref action, span0, span1, span2);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2>(this ref TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList3.WithSafeFixed(ref func, span0, span1, span2, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList3.WithSafeFixed(ref func, span0, span1, span2, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList4.WithSafeFixed(ref action, span0, span1, span2, span3);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList4.WithSafeFixed(ref action, span0, span1, span2, span3);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this ref TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList4.WithSafeFixed(ref func, span0, span1, span2, span3, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList4.WithSafeFixed(ref func, span0, span1, span2, span3, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList5.WithSafeFixed(ref action, span0, span1, span2, span3, span4);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4>(this ref TAction action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList5.WithSafeFixed(ref action, span0, span1, span2, span3, span4);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this ref TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList5.WithSafeFixed(ref func, span0, span1, span2, span3, span4, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList5.WithSafeFixed(ref func, span0, span1, span2, span3, span4, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList6.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this ref TAction action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList6.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this ref TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList6.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList6.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this ref TAction action,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList7.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this ref TAction action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList7.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this ref TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList7.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList7.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
	/// <param name="span7">The eighth span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TAction action,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		Span<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList8.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6, span7);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
	/// <param name="span7">The eighth read-only span.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TAction action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList8.WithSafeFixed(ref action, span0, span1, span2, span3, span4, span5, span6, span7);
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first span.</param>
	/// <param name="span1">The second span.</param>
	/// <param name="span2">The third span.</param>
	/// <param name="span3">The fourth span.</param>
	/// <param name="span4">The fifth span.</param>
	/// <param name="span5">The sixth span.</param>
	/// <param name="span6">The seventh span.</param>
	/// <param name="span7">The eighth span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(
		this ref TFunction func, Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4,
		Span<T5> span5, Span<T6> span6, Span<T7> span7, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList8.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, span7,
		                                   out result);
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in the first span.</typeparam>
	/// <typeparam name="T1">Type of the items in the second span.</typeparam>
	/// <typeparam name="T2">Type of the items in the third span.</typeparam>
	/// <typeparam name="T3">Type of the items in the fourth span.</typeparam>
	/// <typeparam name="T4">Type of the items in the fifth span.</typeparam>
	/// <typeparam name="T5">Type of the items in the sixth span.</typeparam>
	/// <typeparam name="T6">Type of the items in the seventh span.</typeparam>
	/// <typeparam name="T7">Type of the items in the eighth span.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">The first read-only span.</param>
	/// <param name="span1">The second read-only span.</param>
	/// <param name="span2">The third read-only span.</param>
	/// <param name="span3">The fourth read-only span.</param>
	/// <param name="span4">The fifth read-only span.</param>
	/// <param name="span5">The sixth read-only span.</param>
	/// <param name="span6">The seventh read-only span.</param>
	/// <param name="span7">The eighth read-only span.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(
		this ref TFunction func, ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2,
		ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6,
		ReadOnlySpan<T7> span7, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList8.WithSafeFixed(ref func, span0, span1, span2, span3, span4, span5, span6, span7,
		                                   out result);
}