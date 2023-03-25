namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

/// <summary>
/// Base class for <see cref="FixedReference{T}"/> tests.
/// </summary>
public abstract class FixedReferenceTestsBase
{
    /// <summary>
    /// Message when <see cref="FixedReference{T}"/> instance is read-only.
    /// </summary>
    public static readonly String ReadOnlyError = "The current instance is read-only.";
    /// <summary>
    /// Message when <see cref="FixedReference{T}"/> instance is invalid.
    /// </summary>
    public static readonly String InvalidError = "The current instance is not valid.";
    /// <summary>
    /// Message when <see cref="FixedReference{T}"/> instance is not enough for hold a reference.
    /// </summary>
    public static readonly String InvalidSizeFormat = "The current instance is insufficent to contain a value of {0} type.";

    /// <summary>
    /// Fixture instance.
    /// </summary>
    protected static readonly IFixture fixture = new Fixture();

    /// <summary>
    /// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
    /// <param name="reference">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
    /// <param name="readOnly">Indicates whether the created <see cref="FixedReference{T}"/> instance should be readonly.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal void WithFixed<T>(ref T reference, Boolean readOnly, Action<FixedReference<T>, IntPtr> actionTest) where T : unmanaged
    {
        unsafe
        {
            fixed (void* ptr = &reference)
                actionTest(new(ptr, readOnly), new(ptr));
        }
    }

    /// <summary>
    /// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
    /// <param name="readOnlyRef">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
    /// <param name="readOnly">Indicates whether the created <see cref="FixedReference{T}"/> instance should be readonly.</param>
    /// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
    internal static void WithFixed<T, TObj>(in T readOnlyRef, Boolean readOnly, TObj obj, Action<FixedReference<T>, TObj> actionTest) where T : unmanaged
    {
        unsafe
        {
            fixed (void* ptr = &readOnlyRef)
                actionTest(new(ptr, readOnly), obj);
        }
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

