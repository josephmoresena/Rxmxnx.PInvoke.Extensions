namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

/// <summary>
/// Base class for <see cref="FixedReference{T}"/> tests.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class FixedReferenceTestsBase : FixedMemoryTestsBase
{
    /// <summary>
    /// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
    /// <param name="refValue">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal unsafe void WithFixed<T>(ref T refValue, Action<FixedReference<T>, IntPtr> actionTest) where T : unmanaged
    {
        fixed (void* ptr = &refValue)
            actionTest(new(ptr), new(ptr));
    }
    /// <summary>
    /// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
    /// <param name="refValue">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal unsafe void WithFixed<T>(ref T refValue, Action<ReadOnlyFixedReference<T>, IntPtr> actionTest) where T : unmanaged
    {
        fixed (void* ptr = &refValue)
            actionTest(new(ptr), new(ptr));
    }

    /// <summary>
    /// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
    /// <param name="readOnlyRef">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal static unsafe void WithFixed<T, TObj>(in T readOnlyRef, TObj obj, Action<FixedReference<T>, TObj> actionTest) where T : unmanaged
    {
        fixed (void* ptr = &readOnlyRef)
            actionTest(new(ptr), obj);
    }

    /// <summary>
    /// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
    /// <param name="readOnlyRef">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal static unsafe void WithFixed<T, TObj>(in T readOnlyRef, TObj obj, Action<ReadOnlyFixedReference<T>, TObj> actionTest) where T : unmanaged
    {
        fixed (void* ptr = &readOnlyRef)
            actionTest(new(ptr), obj);
    }
}

