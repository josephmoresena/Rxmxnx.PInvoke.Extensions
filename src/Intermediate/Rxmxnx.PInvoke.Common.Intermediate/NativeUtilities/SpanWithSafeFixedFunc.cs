﻿namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
public static unsafe partial class NativeUtilities
{
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="func">A <see cref="FixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, TResult>(Span<T0> span0, Span<T1> span1, FixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length));
			try
			{
				return func(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, TArg, TResult>(Span<T0> span0, Span<T1> span1, TArg arg,
		FixedListFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length));
			try
			{
				return func(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="func">A <see cref="FixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, TResult>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		FixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length));
			try
			{
				return func(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, TArg, TResult>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		TArg arg, FixedListFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length));
			try
			{
				return func(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="func">A <see cref="FixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, TResult>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, FixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length));
			try
			{
				return func(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, TArg, TResult>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, TArg arg, FixedListFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length));
			try
			{
				return func(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="func">A <see cref="FixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, TResult>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, Span<T4> span4, FixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length));
			try
			{
				return func(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, TArg, TResult>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, TArg arg, FixedListFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length));
			try
			{
				return func(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="func">A <see cref="FixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, TResult>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
		Span<T3> span3, Span<T4> span4, Span<T5> span5, FixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length),
			                          new FixedContext<T5>(ptr5, span5.Length));
			try
			{
				return func(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, TArg, TResult>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, TArg arg, FixedListFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length),
			                          new FixedContext<T5>(ptr5, span5.Length));
			try
			{
				return func(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="func">A <see cref="FixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, TResult>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, FixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length),
			                          new FixedContext<T5>(ptr5, span5.Length),
			                          new FixedContext<T6>(ptr6, span6.Length));
			try
			{
				return func(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, TArg, TResult>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, TArg arg,
		FixedListFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length),
			                          new FixedContext<T5>(ptr5, span5.Length),
			                          new FixedContext<T6>(ptr6, span6.Length));
			try
			{
				return func(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	/// <param name="func">A <see cref="FixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7,
		FixedListFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length),
			                          new FixedContext<T5>(ptr5, span5.Length),
			                          new FixedContext<T6>(ptr6, span6.Length),
			                          new FixedContext<T7>(ptr7, span7.Length));
			try
			{
				return func(lst);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2nd span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st span.</param>
	/// <param name="span1">2nd span.</param>
	/// <param name="span2">3rd span.</param>
	/// <param name="span3">4th span.</param>
	/// <param name="span4">5th span.</param>
	/// <param name="span5">6th span.</param>
	/// <param name="span6">7th span.</param>
	/// <param name="span7">8th span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7, TArg, TResult>(Span<T0> span0, Span<T1> span1,
		Span<T2> span2, Span<T3> span3, Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7, TArg arg,
		FixedListFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		{
			FixedMemoryList lst = new(new FixedContext<T0>(ptr0, span0.Length),
			                          new FixedContext<T1>(ptr1, span1.Length),
			                          new FixedContext<T2>(ptr2, span2.Length),
			                          new FixedContext<T3>(ptr3, span3.Length),
			                          new FixedContext<T4>(ptr4, span4.Length),
			                          new FixedContext<T5>(ptr5, span5.Length),
			                          new FixedContext<T6>(ptr6, span6.Length),
			                          new FixedContext<T7>(ptr7, span7.Length));
			try
			{
				return func(lst, arg);
			}
			finally
			{
				lst.Unload();
			}
		}
	}
}
#pragma warning restore CS8500