namespace Rxmxnx.PInvoke;

[SuppressMessage("csharpsquid", "S107")]
[SuppressMessage("csharpsquid", "S2436")]
[SuppressMessage("csharpsquid", "S6640")]
public static unsafe partial class NativeUtilities
{
	/// <summary>
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
		ReadOnlyFixedListFunc<TResult> func) where T0 : unmanaged where T1 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, TArg, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, TArg arg,
		ReadOnlyFixedListFunc<TArg, TResult> func) where T0 : unmanaged where T1 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
		ReadOnlySpan<T2> span2, ReadOnlyFixedListFunc<TResult> func)
		where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, TArg, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
		ReadOnlySpan<T2> span2, TArg arg, ReadOnlyFixedListFunc<TArg, TResult> func) where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
		ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlyFixedListFunc<TResult> func) where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, TArg, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
		ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, TArg arg, ReadOnlyFixedListFunc<TArg, TResult> func)
		where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
		ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4, ReadOnlyFixedListFunc<TResult> func)
		where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, TArg, TResult>(ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4, TArg arg,
		ReadOnlyFixedListFunc<TArg, TResult> func) where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
		where T4 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, TResult>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
		ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5,
		ReadOnlyFixedListFunc<TResult> func) where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
		where T4 : unmanaged
		where T5 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length),
			                                  new ReadOnlyFixedContext<T5>(ptr5, span5.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, TArg, TResult>(ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5, TArg arg, ReadOnlyFixedListFunc<TArg, TResult> func) where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
		where T4 : unmanaged
		where T5 : unmanaged
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		{
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length),
			                                  new ReadOnlyFixedContext<T5>(ptr5, span5.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, TResult>(ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlyFixedListFunc<TResult> func) where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
		where T4 : unmanaged
		where T5 : unmanaged
		where T6 : unmanaged
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
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length),
			                                  new ReadOnlyFixedContext<T5>(ptr5, span5.Length),
			                                  new ReadOnlyFixedContext<T6>(ptr6, span6.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, TArg, TResult>(ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, TArg arg, ReadOnlyFixedListFunc<TArg, TResult> func)
		where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
		where T4 : unmanaged
		where T5 : unmanaged
		where T6 : unmanaged
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
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length),
			                                  new ReadOnlyFixedContext<T5>(ptr5, span5.Length),
			                                  new ReadOnlyFixedContext<T6>(ptr6, span6.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="span7">8th read-only span.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7, TResult>(ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7, ReadOnlyFixedListFunc<TResult> func)
		where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
		where T4 : unmanaged
		where T5 : unmanaged
		where T6 : unmanaged
		where T7 : unmanaged
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
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length),
			                                  new ReadOnlyFixedContext<T5>(ptr5, span5.Length),
			                                  new ReadOnlyFixedContext<T6>(ptr6, span6.Length),
			                                  new ReadOnlyFixedContext<T7>(ptr7, span7.Length));
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
	/// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
	/// addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="T0">Type of the items in 1st span.</typeparam>
	/// <typeparam name="T1">Type of the items in 2st span.</typeparam>
	/// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
	/// <typeparam name="T3">Type of the items in 4th span.</typeparam>
	/// <typeparam name="T4">Type of the items in 5th span.</typeparam>
	/// <typeparam name="T5">Type of the items in 6th span.</typeparam>
	/// <typeparam name="T6">Type of the items in 7th span.</typeparam>
	/// <typeparam name="T7">Type of the items in 8th span.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="span0">1st read-only span.</param>
	/// <param name="span1">2nd read-only span.</param>
	/// <param name="span2">3rd read-only span.</param>
	/// <param name="span3">4th read-only span.</param>
	/// <param name="span4">5th read-only span.</param>
	/// <param name="span5">6th read-only span.</param>
	/// <param name="span6">7th read-only span.</param>
	/// <param name="span7">8th read-only span.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedListFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	public static TResult WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7, TArg, TResult>(ReadOnlySpan<T0> span0,
		ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3, ReadOnlySpan<T4> span4,
		ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7, TArg arg,
		ReadOnlyFixedListFunc<TArg, TResult> func) where T0 : unmanaged
		where T1 : unmanaged
		where T2 : unmanaged
		where T3 : unmanaged
		where T4 : unmanaged
		where T5 : unmanaged
		where T6 : unmanaged
		where T7 : unmanaged
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
			ReadOnlyFixedMemoryList lst = new(new ReadOnlyFixedContext<T0>(ptr0, span0.Length),
			                                  new ReadOnlyFixedContext<T1>(ptr1, span1.Length),
			                                  new ReadOnlyFixedContext<T2>(ptr2, span2.Length),
			                                  new ReadOnlyFixedContext<T3>(ptr3, span3.Length),
			                                  new ReadOnlyFixedContext<T4>(ptr4, span4.Length),
			                                  new ReadOnlyFixedContext<T5>(ptr5, span5.Length),
			                                  new ReadOnlyFixedContext<T6>(ptr6, span6.Length),
			                                  new ReadOnlyFixedContext<T7>(ptr7, span7.Length));
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