namespace Rxmxnx.PInvoke;

[SuppressMessage("csharpsquid", "S107")]
[SuppressMessage("csharpsquid", "S2436")]
public static partial class NativeUtilities
{
    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
        ReadOnlyFixedListAction action)
        where T0 : unmanaged where T1 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true)
            );
            try
            {
                action(lst);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, TArg>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1,
        TArg arg, ReadOnlyFixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true)
            );
            try
            {
                action(lst, arg);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2,
        ReadOnlyFixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true)
            );
            try
            {
                action(lst);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, TArg>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2,
        TArg arg, ReadOnlyFixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true)
            );
            try
            {
                action(lst, arg);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlyFixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true)
            );
            try
            {
                action(lst);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, TArg>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        TArg arg, ReadOnlyFixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true)
            );
            try
            {
                action(lst, arg);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4,
        ReadOnlyFixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true)
            );
            try
            {
                action(lst);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, TArg>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4,
        TArg arg, ReadOnlyFixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true)
            );
            try
            {
                action(lst, arg);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="span5">6th read-only span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5,
        ReadOnlyFixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
        where T5 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true),
                new FixedContext<T5>(ptr5, span5.Length, true)
            );
            try
            {
                action(lst);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="span5">6th read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, TArg>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5,
        TArg arg, ReadOnlyFixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
        where T5 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true),
                new FixedContext<T5>(ptr5, span5.Length, true)
            );
            try
            {
                action(lst, arg);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="T6">Type of the items in 7th span.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="span5">6th read-only span.</param>
    /// <param name="span6">7th read-only span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6,
        ReadOnlyFixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
        where T5 : unmanaged where T6 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
        fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true),
                new FixedContext<T5>(ptr5, span5.Length, true),
                new FixedContext<T6>(ptr6, span6.Length, true)
            );
            try
            {
                action(lst);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="T6">Type of the items in 7th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="span5">6th read-only span.</param>
    /// <param name="span6">7th read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, TArg>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6,
        TArg arg, ReadOnlyFixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
        where T5 : unmanaged where T6 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
        fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
        {
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true),
                new FixedContext<T5>(ptr5, span5.Length, true),
                new FixedContext<T6>(ptr6, span6.Length, true)
            );
            try
            {
                action(lst, arg);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3rd span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="T6">Type of the items in 7th span.</typeparam>
    /// <typeparam name="T7">Type of the items in 8th span.</typeparam>
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="span5">6th read-only span.</param>
    /// <param name="span6">7th read-only span.</param>
    /// <param name="span7">8th read-only span.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7,
        ReadOnlyFixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
        where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
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
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true),
                new FixedContext<T5>(ptr5, span5.Length, true),
                new FixedContext<T6>(ptr6, span6.Length, true),
                new FixedContext<T7>(ptr7, span7.Length, true)
            );
            try
            {
                action(lst);
            }
            finally
            {
                lst.Unload();
            }
        }
    }

    /// <summary>
    /// Prevents the garbage collector from reallocating given read-only spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
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
    /// <param name="span0">1st read-only span.</param>
    /// <param name="span1">2nd read-only span.</param>
    /// <param name="span2">3rd read-only span.</param>
    /// <param name="span3">4th read-only span.</param>
    /// <param name="span4">5th read-only span.</param>
    /// <param name="span5">6th read-only span.</param>
    /// <param name="span6">7th read-only span.</param>
    /// <param name="span7">8th read-only span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="ReadOnlyFixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7, TArg>(ReadOnlySpan<T0> span0, ReadOnlySpan<T1> span1, ReadOnlySpan<T2> span2, ReadOnlySpan<T3> span3,
        ReadOnlySpan<T4> span4, ReadOnlySpan<T5> span5, ReadOnlySpan<T6> span6, ReadOnlySpan<T7> span7,
        TArg arg, ReadOnlyFixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
        where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged
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
            ReadOnlyFixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length, true),
                new FixedContext<T1>(ptr1, span1.Length, true),
                new FixedContext<T2>(ptr2, span2.Length, true),
                new FixedContext<T3>(ptr3, span3.Length, true),
                new FixedContext<T4>(ptr4, span4.Length, true),
                new FixedContext<T5>(ptr5, span5.Length, true),
                new FixedContext<T6>(ptr6, span6.Length, true),
                new FixedContext<T7>(ptr7, span7.Length, true)
            );
            try
            {
                action(lst, arg);
            }
            finally
            {
                lst.Unload();
            }
        }
    }
}