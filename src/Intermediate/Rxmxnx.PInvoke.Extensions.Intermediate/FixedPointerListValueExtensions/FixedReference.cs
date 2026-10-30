namespace Rxmxnx.PInvoke;

public static partial class FixedPointerListValueExtensions
{
	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1>(this TAction? action, ref T0 value0, ref T1 value1)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList2.WithSafeFixed(ref action, ref value0, ref value1);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList2.WithSafeFixed(ref nonGeneric, ref value0, ref value1);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T0, T1>(this TAction? action, in T0 value0, in T1 value1)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList2.WithSafeReadOnlyFixed(ref action, in value0, in value1);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList2.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1);
	}

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1>(this TFunction? func, ref T0 value0, ref T1 value1,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
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
			FixedPointerList2.WithSafeFixed(ref func, ref value0, ref value1, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList2.WithSafeFixed(ref nonGeneric, ref value0, ref value1, out result);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1>(this TFunction? func, in T0 value0,
		in T1 value1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
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
			FixedPointerList2.WithSafeReadOnlyFixed(ref func, in value0, in value1, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList2.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, out result);
	}

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1>(this ref TAction action, ref T0 value0, ref T1 value1)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList2.WithSafeFixed(ref action, ref value0, ref value1);

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T0, T1>(this ref TAction action, in T0 value0,
		in T1 value1)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList2.WithSafeReadOnlyFixed(ref action, in value0, in value1);

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1>(this ref TFunction func, ref T0 value0,
		ref T1 value1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList2.WithSafeFixed(ref func, ref value0, ref value1, out result);

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T0, T1>(this ref TFunction func, in T0 value0,
		in T1 value1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList2.WithSafeReadOnlyFixed(ref func, in value0, in value1, out result);

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2>(this TAction? action, ref T0 value0, ref T1 value1,
		ref T2 value2)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList3.WithSafeFixed(ref action, ref value0, ref value1, ref value2);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList3.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T0, T1, T2>(this TAction? action, in T0 value0, in T1 value1,
		in T2 value2)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList3.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList3.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2);
	}

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2>(this TFunction? func, ref T0 value0, ref T1 value1,
		ref T2 value2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
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
			FixedPointerList3.WithSafeFixed(ref func, ref value0, ref value1, ref value2, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList3.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, out result);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2>(this TFunction? func, in T0 value0,
		in T1 value1, in T2 value2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
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
			FixedPointerList3.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList3.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, out result);
	}

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2>(this ref TAction action, ref T0 value0,
		ref T1 value1, ref T2 value2)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList3.WithSafeFixed(ref action, ref value0, ref value1, ref value2);

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T0, T1, T2>(this ref TAction action, in T0 value0,
		in T1 value1, in T2 value2)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList3.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2);

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2>(this ref TFunction func, ref T0 value0,
		ref T1 value1, ref T2 value2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList3.WithSafeFixed(ref func, ref value0, ref value1, ref value2, out result);

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2>(this ref TFunction func,
		in T0 value0, in T1 value1, in T2 value2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList3.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, out result);

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3>(this TAction? action, ref T0 value0, ref T1 value1,
		ref T2 value2, ref T3 value3)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList4.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList4.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T0, T1, T2, T3>(this TAction? action, in T0 value0, in T1 value1,
		in T2 value2, in T3 value3)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList4.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList4.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3);
	}

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this TFunction? func, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
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
			FixedPointerList4.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList4.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, out result);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3>(this TFunction? func, in T0 value0,
		in T1 value1, in T2 value2, in T3 value3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
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
			FixedPointerList4.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList4.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, out result);
	}

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3>(this ref TAction action, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList4.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3);

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T0, T1, T2, T3>(this ref TAction action, in T0 value0,
		in T1 value1, in T2 value2, in T3 value3)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList4.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3);

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this ref TFunction func,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList4.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, out result);

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3>(this ref TFunction func,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList4.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, out result);

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4>(this TAction? action, ref T0 value0, ref T1 value1,
		ref T2 value2, ref T3 value3, ref T4 value4)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList5.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList5.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4>(this TAction? action, in T0 value0,
		in T1 value1, in T2 value2, in T3 value3, in T4 value4)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList5.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList5.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4);
	}

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this TFunction? func, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
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
			FixedPointerList5.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
			                                out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList5.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                out result);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this TFunction? func, in T0 value0,
		in T1 value1, in T2 value2, in T3 value3, in T4 value4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
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
			FixedPointerList5.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
			                                        out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList5.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4,
		                                        out result);
	}

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4>(this ref TAction action, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList5.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4);

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4>(this ref TAction action,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList5.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4);

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this ref TFunction func,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList5.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                   out result);

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this ref TFunction func,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList5.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
		                                           out result);

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this TAction? action, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList6.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4,
			                                ref value5);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList6.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                ref value5);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4, T5>(this TAction? action, in T0 value0,
		in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList6.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4,
			                                        in value5);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList6.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4,
		                                        in value5);
	}

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this TFunction? func, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
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
			FixedPointerList6.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
			                                ref value5, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList6.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                ref value5, out result);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this TFunction? func,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
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
			FixedPointerList6.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
			                                        in value5, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList6.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4,
		                                        in value5, out result);
	}

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this ref TAction action, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList6.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                   ref value5);

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4, T5>(this ref TAction action,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList6.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4,
		                                           in value5);

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this ref TFunction func,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList6.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                   ref value5, out result);

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(
		this ref TFunction func, in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList6.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
		                                           in value5, out result);

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this TAction? action, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, ref T6 value6)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList7.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4,
			                                ref value5, ref value6);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList7.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                ref value5, ref value6);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this TAction? action, in T0 value0,
		in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList7.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4,
			                                        in value5, in value6);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList7.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4,
		                                        in value5, in value6);
	}

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this TFunction? func,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, ref T6 value6,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
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
			FixedPointerList7.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
			                                ref value5, ref value6, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList7.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                ref value5, ref value6, out result);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this TFunction? func,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
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
			FixedPointerList7.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
			                                        in value5, in value6, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList7.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4,
		                                        in value5, in value6, out result);
	}

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this ref TAction action,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, ref T6 value6)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList7.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                   ref value5, ref value6);

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this ref TAction action,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList7.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4,
		                                           in value5, in value6);

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this ref TFunction func,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, ref T6 value6,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList7.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                   ref value5, ref value6, out result);

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(
		this ref TFunction func, in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5,
		in T6 value6, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList7.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
		                                           in value5, in value6, out result);

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
	/// <param name="value7">The reference at position 7.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this TAction? action, ref T0 value0,
		ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, ref T6 value6, ref T7 value7)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList8.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4,
			                                ref value5, ref value6, ref value7);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList8.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                ref value5, ref value6, ref value7);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
	/// <param name="value7">The read-only reference at position 7.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this TAction? action,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (typeof(TAction).IsValueType)
		{
			FixedPointerList8.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4,
			                                        in value5, in value6, in value7);
			return;
		}
		NonGenericAction nonGeneric = NonGenericAction.Create(action);
		FixedPointerList8.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4,
		                                        in value5, in value6, in value7);
	}

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
	/// <param name="value7">The reference at position 7.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this TFunction? func,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, ref T6 value6,
		ref T7 value7, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
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
			FixedPointerList8.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
			                                ref value5, ref value6, ref value7, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList8.WithSafeFixed(ref nonGeneric, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                ref value5, ref value6, ref value7, out result);
	}

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
	/// <param name="value7">The read-only reference at position 7.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this TFunction? func,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
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
			FixedPointerList8.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
			                                        in value5, in value6, in value7, out result);
			return;
		}
		NonGenericFunction<TResult> nonGeneric = NonGenericFunction<TResult>.Create(func);
		FixedPointerList8.WithSafeReadOnlyFixed(ref nonGeneric, in value0, in value1, in value2, in value3, in value4,
		                                        in value5, in value6, in value7, out result);
	}

	/// <summary>
	/// Pins the given references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
	/// <param name="value7">The reference at position 7.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TAction action,
		ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4, ref T5 value5, ref T6 value6,
		ref T7 value7)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList8.WithSafeFixed(ref action, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                   ref value5, ref value6, ref value7);

	/// <summary>
	/// Pins the given read-only references until <paramref name="action"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="action">An <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
	/// <param name="value7">The read-only reference at position 7.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TAction action,
		in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5, in T6 value6, in T7 value7)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
		=> FixedPointerList8.WithSafeReadOnlyFixed(ref action, in value0, in value1, in value2, in value3, in value4,
		                                           in value5, in value6, in value7);

	/// <summary>
	/// Pins the given references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The reference at position 0.</param>
	/// <param name="value1">The reference at position 1.</param>
	/// <param name="value2">The reference at position 2.</param>
	/// <param name="value3">The reference at position 3.</param>
	/// <param name="value4">The reference at position 4.</param>
	/// <param name="value5">The reference at position 5.</param>
	/// <param name="value6">The reference at position 6.</param>
	/// <param name="value7">The reference at position 7.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(
		this ref TFunction func, ref T0 value0, ref T1 value1, ref T2 value2, ref T3 value3, ref T4 value4,
		ref T5 value5, ref T6 value6, ref T7 value7, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList8.WithSafeFixed(ref func, ref value0, ref value1, ref value2, ref value3, ref value4,
		                                   ref value5, ref value6, ref value7, out result);

	/// <summary>
	/// Pins the given read-only references until <paramref name="func"/> completes. No handle is allocated.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of referenced value 0.</typeparam>
	/// <typeparam name="T1">Type of referenced value 1.</typeparam>
	/// <typeparam name="T2">Type of referenced value 2.</typeparam>
	/// <typeparam name="T3">Type of referenced value 3.</typeparam>
	/// <typeparam name="T4">Type of referenced value 4.</typeparam>
	/// <typeparam name="T5">Type of referenced value 5.</typeparam>
	/// <typeparam name="T6">Type of referenced value 6.</typeparam>
	/// <typeparam name="T7">Type of referenced value 7.</typeparam>
	/// <param name="func">An <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="value0">The read-only reference at position 0.</param>
	/// <param name="value1">The read-only reference at position 1.</param>
	/// <param name="value2">The read-only reference at position 2.</param>
	/// <param name="value3">The read-only reference at position 3.</param>
	/// <param name="value4">The read-only reference at position 4.</param>
	/// <param name="value5">The read-only reference at position 5.</param>
	/// <param name="value6">The read-only reference at position 6.</param>
	/// <param name="value7">The read-only reference at position 7.</param>
	/// <param name="result">Output. Function result.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithReferenceSafeReadOnlyFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(
		this ref TFunction func, in T0 value0, in T1 value1, in T2 value2, in T3 value3, in T4 value4, in T5 value5,
		in T6 value6, in T7 value7, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where T0 : allows ref struct
		where T1 : allows ref struct
		where T2 : allows ref struct
		where T3 : allows ref struct
		where T4 : allows ref struct
		where T5 : allows ref struct
		where T6 : allows ref struct
		where T7 : allows ref struct
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
		=> FixedPointerList8.WithSafeReadOnlyFixed(ref func, in value0, in value1, in value2, in value3, in value4,
		                                           in value5, in value6, in value7, out result);
}