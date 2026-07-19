namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations with <see cref="FixedPointerValueList"/> instances.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
[Browsable(false)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class FixedPointerListValueExtensions
{
#pragma warning disable CS8500
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1>(this TAction action, Span<T0> span0, Span<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1>(this TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1>(this ref TAction action, Span<T0> span0, Span<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1>(this TFunction func, Span<T0> span0, Span<T1> span1,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1>(this TFunction func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1>(this ref TFunction func, Span<T0> span0,
		Span<T1> span1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1>(this ref TFunction func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2>(this TAction action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2>(this TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2>(this ref TAction action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2>(this TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2>(this TFunction func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2>(this ref TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2>(this ref TFunction func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3>(this TAction action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3>(this TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3>(this ref TAction action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this TFunction func, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this ref TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4>(this TAction action, Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4>(this TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this ref TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5>(this ref TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this TFunction func, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this ref TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this TAction action, ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6>(this ref TAction action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this ref TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="span7">8th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this TAction action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TAction action, Span<T0> span0,
		Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			action.Accept(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="span7">8th read-only span.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TAction action,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		Span<T7> span7, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="span7">8th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TFunction func,
		Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6,
		Span<T7> span7, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			result = func.Apply(new()
			{
				IsReadOnly = false,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="span7">8th read-only span.</param>
	/// <param name="result">Output. Function result.</param>
	public static void WithSafeFixed<TFunction, TResult, T0, T1, T2, T3, T4, T5, T6, T7>(this ref TFunction func,
		ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
		ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IFixedPointerListFunction<TResult>
#else
		where TFunction : struct, IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Instances = [],
				Information =
				[
					span0.CreateFixedPointerInfo(ptr0),
					span1.CreateFixedPointerInfo(ptr1),
					span2.CreateFixedPointerInfo(ptr2),
					span3.CreateFixedPointerInfo(ptr3),
					span4.CreateFixedPointerInfo(ptr4),
					span5.CreateFixedPointerInfo(ptr5),
					span6.CreateFixedPointerInfo(ptr6),
					span7.CreateFixedPointerInfo(ptr7),
				],
			});
		}
	}

	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(this ReadOnlySpan<T> span, void* ptr)
		=> new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorPointer = default,
			GetTypePointer = &NativeUtilities.GetType<T>,
		};
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(this Span<T> span, void* ptr)
		=> new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorPointer = default,
			GetTypePointer = &NativeUtilities.GetType<T>,
		};
#pragma warning restore CS8500
}