using System.Runtime.InteropServices;
using Rxmxnx.PInvoke.Internal;

namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

/// <summary>
/// Base class for <see cref="FixedContext{T}"/> tests.
/// </summary>
public abstract class FixedContextTestsBase
{
    /// <summary>
    /// Message when <see cref="FixedContext{T}"/> instance is read-only.
    /// </summary>
    public static readonly String ReadOnlyError = "The current context is read-only.";
    /// <summary>
    /// Message when <see cref="FixedContext{T}"/> instance is invalid.
    /// </summary>
    public static readonly String InvalidError = "The current context is not valid.";

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
}

