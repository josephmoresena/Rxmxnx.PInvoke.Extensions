namespace Rxmxnx.PInvoke;

[SuppressMessage("csharpsquid", "S107")]
[SuppressMessage("csharpsquid", "S2436")]
public static partial class NativeUtilities
{
    /// <summary>
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="action">A <see cref="FixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1>(Span<T0> span0, Span<T1> span1,
        FixedListAction action)
        where T0 : unmanaged where T1 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, TArg>(Span<T0> span0, Span<T1> span1,
        TArg arg, FixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="action">A <see cref="FixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
        FixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2,
        TArg arg, FixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="action">A <see cref="FixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        FixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        TArg arg, FixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="action">A <see cref="FixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4,
        FixedListAction action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4,
        TArg arg, FixedListAction<TArg> action)
        where T0 : unmanaged where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged
    {
        fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
        fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
        fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
        fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
        fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
        {
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="span5">6th span.</param>
    /// <param name="action">A <see cref="FixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4, Span<T5> span5,
        FixedListAction action)
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
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length),
                new FixedContext<T5>(ptr5, span5.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="span5">6th span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4, Span<T5> span5,
        TArg arg, FixedListAction<TArg> action)
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
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length),
                new FixedContext<T5>(ptr5, span5.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="T6">Type of the items in 7th span.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="span5">6th span.</param>
    /// <param name="span6">7th span.</param>
    /// <param name="action">A <see cref="FixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4, Span<T5> span5, Span<T6> span6,
        FixedListAction action)
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
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length),
                new FixedContext<T5>(ptr5, span5.Length),
                new FixedContext<T6>(ptr6, span6.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="T6">Type of the items in 7th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="span5">6th span.</param>
    /// <param name="span6">7th span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4, Span<T5> span5, Span<T6> span6,
        TArg arg, FixedListAction<TArg> action)
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
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length),
                new FixedContext<T5>(ptr5, span5.Length),
                new FixedContext<T6>(ptr6, span6.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="T6">Type of the items in 7th span.</typeparam>
    /// <typeparam name="T7">Type of the items in 8th span.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="span5">6th span.</param>
    /// <param name="span6">7th span.</param>
    /// <param name="span7">8th span.</param>
    /// <param name="action">A <see cref="FixedListAction"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7,
        FixedListAction action)
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
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length),
                new FixedContext<T5>(ptr5, span5.Length),
                new FixedContext<T6>(ptr6, span6.Length),
                new FixedContext<T7>(ptr7, span7.Length)
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
    /// Prevents the garbage collector from reallocating given spans and fixes their memory
    /// addresses until <paramref name="action"/> completes.
    /// </summary>
    /// <typeparam name="T0">Type of the items in 1st span.</typeparam>
    /// <typeparam name="T1">Type of the items in 2st span.</typeparam>
    /// <typeparam name="T2">Type of the items in 3th span.</typeparam>
    /// <typeparam name="T3">Type of the items in 4th span.</typeparam>
    /// <typeparam name="T4">Type of the items in 5th span.</typeparam>
    /// <typeparam name="T5">Type of the items in 6th span.</typeparam>
    /// <typeparam name="T6">Type of the items in 7th span.</typeparam>
    /// <typeparam name="T7">Type of the items in 8th span.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    /// <param name="span0">1st span.</param>
    /// <param name="span1">2nd span.</param>
    /// <param name="span2">3th span.</param>
    /// <param name="span3">4th span.</param>
    /// <param name="span4">5th span.</param>
    /// <param name="span5">6th span.</param>
    /// <param name="span6">7th span.</param>
    /// <param name="span7">8th span.</param>
    /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
    /// <param name="action">A <see cref="FixedListAction{TArg}"/> delegate.</param>
    public static unsafe void WithSafeFixed<T0, T1, T2, T3, T4, T5, T6, T7, TArg>(Span<T0> span0, Span<T1> span1, Span<T2> span2, Span<T3> span3,
        Span<T4> span4, Span<T5> span5, Span<T6> span6, Span<T7> span7,
        TArg arg, FixedListAction<TArg> action)
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
            FixedMemoryList lst = new
            (
                new FixedContext<T0>(ptr0, span0.Length),
                new FixedContext<T1>(ptr1, span1.Length),
                new FixedContext<T2>(ptr2, span2.Length),
                new FixedContext<T3>(ptr3, span3.Length),
                new FixedContext<T4>(ptr4, span4.Length),
                new FixedContext<T5>(ptr5, span5.Length),
                new FixedContext<T6>(ptr6, span6.Length),
                new FixedContext<T7>(ptr7, span7.Length)
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