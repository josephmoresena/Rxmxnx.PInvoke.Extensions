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
    /// <param name="readOnly">Indicates whether the created <see cref="FixedReference{T}"/> instance should be readonly.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal unsafe void WithFixed<T>(ref T refValue, Boolean readOnly, Action<FixedReference<T>, IntPtr> actionTest) where T : unmanaged
    {
        fixed (void* ptr = &refValue)
            actionTest(new(ptr, readOnly), new(ptr));
    }

    /// <summary>
    /// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
    /// <param name="readOnlyRef">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
    /// <param name="readOnly">Indicates whether the created <see cref="FixedReference{T}"/> instance should be readonly.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal unsafe static void WithFixed<T, TObj>(in T readOnlyRef, Boolean readOnly, TObj obj, Action<FixedReference<T>, TObj> actionTest) where T : unmanaged
    {
        fixed (void* ptr = &readOnlyRef)
            actionTest(new(ptr, readOnly), obj);
    }

    /// <summary>
    /// Indicates whether <see cref="FixedContext{T}"/> instance is read-only.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="FixedContext{T}"/> value.</typeparam>
    /// <param name="ctx"><see cref="FixedContext{T}"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="ctx"/> is read-only; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    internal static Boolean IsReadOnly<T>(FixedReference<T> ctx) where T : unmanaged
    {
        Boolean isReadOnly = false;
        try
        {
            _ = ctx.CreateReference<T>();
        }
        catch (Exception)
        {
            isReadOnly = true;
        }
        return isReadOnly;
    }

}

