namespace Rxmxnx.PInvoke.Tests.Internal.FixedContextTests;

/// <summary>
/// Base class for <see cref="FixedContext{T}"/> tests.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class FixedContextTestsBase : FixedMemoryTestsBase
{
	/// <summary>
	/// Invokes the action with a created <see cref="FixedContext{T}"/> instantance passed
	/// as parameter.
	/// </summary>
	/// <typeparam name="T">Type of value in <see cref="FixedContext{T}"/>.</typeparam>
	/// <param name="values">Array over the <see cref="FixedContext{T}"/> instance is created.</param>
	/// <param name="actionTest">Action test to <see cref="FixedContext{T}"/> instance to be used.</param>
	internal unsafe void WithFixed<T>(T[] values, Action<FixedContext<T>, T[]> actionTest) where T : unmanaged
	{
		ReadOnlySpan<T> span = values.AsSpan();
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			actionTest(new(ptr, span.Length), values);
	}

	/// <summary>
	/// Invokes the action with a created <see cref="ReadOnlyFixedContext{T}"/> instantance passed
	/// as parameter.
	/// </summary>
	/// <typeparam name="T">Type of value in <see cref="ReadOnlyFixedContext{T}"/>.</typeparam>
	/// <param name="values">Array over the <see cref="ReadOnlyFixedContext{T}"/> instance is created.</param>
	/// <param name="actionTest">Action test to <see cref="ReadOnlyFixedContext{T}"/> instance to be used.</param>
	internal unsafe void WithFixed<T>(T[] values, Action<ReadOnlyFixedContext<T>, T[]> actionTest) where T : unmanaged
	{
		ReadOnlySpan<T> span = values.AsSpan();
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			actionTest(new(ptr, span.Length), values);
	}

	/// <summary>
	/// Invokes the action with a created <see cref="FixedContext{T}"/> instantance passed
	/// as parameter.
	/// </summary>
	/// <typeparam name="T">Type of value in <see cref="FixedContext{T}"/>.</typeparam>
	/// <param name="span">Span over the <see cref="FixedContext{T}"/> instance is created.</param>
	/// <param name="actionTest">Action test to <see cref="FixedContext{T}"/> instance to be used.</param>
	internal static unsafe void WithFixed<T, TObj>(ReadOnlySpan<T> span, TObj obj,
		Action<FixedContext<T>, TObj> actionTest) where T : unmanaged
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			actionTest(new(ptr, span.Length), obj);
	}

	/// <summary>
	/// Invokes the action with a created <see cref="ReadOnlyFixedContext{T}"/> instantance passed
	/// as parameter.
	/// </summary>
	/// <typeparam name="T">Type of value in <see cref="ReadOnlyFixedContext{T}"/>.</typeparam>
	/// <param name="span">Span over the <see cref="ReadOnlyFixedContext{T}"/> instance is created.</param>
	/// <param name="readOnly">Indicates whether the created <see cref="ReadOnlyFixedContext{T}"/> instance should be readonly.</param>
	/// <param name="actionTest">Action test to <see cref="ReadOnlyFixedContext{T}"/> instance to be used.</param>
	internal static unsafe void WithFixed<T, TObj>(ReadOnlySpan<T> span, TObj obj,
		Action<ReadOnlyFixedContext<T>, TObj> actionTest) where T : unmanaged
	{
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
			actionTest(new(ptr, span.Length), obj);
	}
}