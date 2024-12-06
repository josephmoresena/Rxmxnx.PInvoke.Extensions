namespace Rxmxnx.PInvoke.Tests.Internal.FixedReferenceTests;

/// <summary>
/// Base class for <see cref="FixedReference{T}"/> tests.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class FixedReferenceTestsBase : FixedMemoryTestsBase
{
#pragma warning disable CS8500
	/// <summary>
	/// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
	/// as parameter.
	/// </summary>
	/// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
	/// <param name="refValue">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
	/// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
	private protected static unsafe void WithFixed<T>(ref T refValue, Action<FixedReference<T>, IntPtr> actionTest)
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
	private protected static unsafe void WithFixed<T>(ref T refValue,
		Action<ReadOnlyFixedReference<T>, IntPtr> actionTest)
	{
		fixed (void* ptr = &refValue)
			actionTest(new(ptr), new(ptr));
	}

	/// <summary>
	/// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
	/// as parameter.
	/// </summary>
	/// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
	/// <typeparam name="TObj">Type of object.</typeparam>
	/// <param name="readOnlyRef">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
	/// <param name="obj">Object state.</param>
	/// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
	private protected static unsafe void WithFixed<T, TObj>(in T readOnlyRef, TObj obj,
		Action<FixedReference<T>, TObj> actionTest)
	{
		fixed (void* ptr = &readOnlyRef)
			actionTest(new(ptr), obj);
	}

	/// <summary>
	/// Invokes the action with a created <see cref="FixedReference{T}"/> instantance passed
	/// as parameter.
	/// </summary>
	/// <typeparam name="T">Type of value in <see cref="FixedReference{T}"/>.</typeparam>
	/// <typeparam name="TObj">Type of object.</typeparam>
	/// <param name="readOnlyRef">Reference over the <see cref="FixedReference{T}"/> instance is created.</param>
	/// <param name="obj">Object state.</param>
	/// <param name="actionTest">Action test to <see cref="FixedReference{T}"/> instance to be used.</param>
	private protected static unsafe void WithFixed<T, TObj>(in T readOnlyRef, TObj obj,
		Action<ReadOnlyFixedReference<T>, TObj> actionTest)
	{
		fixed (void* ptr = &readOnlyRef)
			actionTest(new(ptr), obj);
	}
#pragma warning restore CS8500
}