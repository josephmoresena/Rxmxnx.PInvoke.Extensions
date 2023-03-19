namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

/// <summary>
/// Base class for <see cref="FixedContext{T}"/> tests.
/// </summary>
public abstract class FixedContextTestsBase
{
    /// <summary>
    /// Message when <see cref="FixedContext{T}"/> instance is read-only.
    /// </summary>
    public static readonly String ReadOnlyError = "The current instance is read-only.";
    /// <summary>
    /// Message when <see cref="FixedContext{T}"/> instance is invalid.
    /// </summary>
    public static readonly String InvalidError = "The current instance is not valid.";

    /// <summary>
    /// Fixture instance.
    /// </summary>
    protected static readonly IFixture fixture = new Fixture();

    /// <summary>
    /// Invokes the action with a created <see cref="FixedContext{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedContext{T}"/>.</typeparam>
    /// <param name="values">Array over the <see cref="FixedContext{T}"/> instance is created.</param>
    /// <param name="readOnly">Indicates whether the created <see cref="FixedContext{T}"/> instance should be readonly.</param>
    /// <param name="actionTest">Action test to <see cref="FixedContext{T}"/> instance to be used.</param>
    internal void WithFixed<T>(T[] values, Boolean readOnly, Action<FixedContext<T>, T[]> actionTest) where T : unmanaged
    {
        ReadOnlySpan<T> span = values.AsSpan();
        unsafe
        {
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
                actionTest(new(ptr, span.Length, readOnly), values);
        }
    }

    /// <summary>
    /// Invokes the action with a created <see cref="FixedContext{T}"/> instantance passed
    /// as parameter.
    /// </summary>
    /// <typeparam name="T">Type of value in <see cref="FixedContext{T}"/>.</typeparam>
    /// <param name="span">Span over the <see cref="FixedContext{T}"/> instance is created.</param>
    /// <param name="readOnly">Indicates whether the created <see cref="FixedContext{T}"/> instance should be readonly.</param>
    /// <param name="actionTest">Action test to <see cref="FixedContext{T}"/> instance to be used.</param>
    internal static void WithFixed<T, TObj>(ReadOnlySpan<T> span, Boolean readOnly, TObj obj, Action<FixedContext<T>, TObj> actionTest) where T : unmanaged
    {
        unsafe
        {
            fixed (void* ptr = &MemoryMarshal.GetReference(span))
                actionTest(new(ptr, span.Length, readOnly), obj);
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
    internal static Boolean IsReadOnly<T>(FixedContext<T> ctx) where T : unmanaged
    {
        Boolean isReadOnly = false;
        try
        {
            _ = ctx.CreateSpan<T>(0);
        }
        catch (Exception)
        {
            isReadOnly = true;
        }
        return isReadOnly;
    }
}

