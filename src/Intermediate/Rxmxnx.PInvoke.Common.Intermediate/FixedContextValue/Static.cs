#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct FixedContextValue<T>
{
	/// <summary>
	/// Defines an implicit conversion of a <see cref="FixedContextValue{T}"/> to a <see cref="FixedPointerValue"/>
	/// instance.
	/// </summary>
	/// <param name="value">A pointer to implicitly convert.</param>
	public static implicit operator FixedPointerValue(FixedContextValue<T> value) => value._value;
	/// <summary>
	/// Defines an implicit conversion of a <see cref="FixedContextValue{T}"/> to a <see cref="ReadOnlyFixedContextValue{T}"/>
	/// instance.
	/// </summary>
	/// <param name="value">A pointer to implicitly convert.</param>
	public static implicit operator ReadOnlyFixedContextValue<T>(FixedContextValue<T> value) => new(value._value);
	/// <summary>
	/// Defines an explicit conversion of a given <see cref="FixedPointerValue"/> to a <see cref="FixedContextValue{T}"/>
	/// instance.
	/// </summary>
	/// <param name="value">An <see cref="FixedPointerValue"/> to explicitly convert.</param>
	public static explicit operator FixedContextValue<T>(FixedPointerValue value)
	{
		value.ValidateOperation();
		value.ValidateTransformation(typeof(T), !RuntimeHelpers.IsReferenceOrContainsReferences<T>());
		return new(value);
	}

	/// <summary>
	/// Creates a new <see cref="FixedPointerValue"/> value from <paramref name="instance"/>.
	/// </summary>
	/// <param name="instance">A <see cref="IFixedPointer"/> instance.</param>
	/// <returns>
	/// A new <see cref="FixedContextValue{T}"/> instance.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static FixedContextValue<T> CreateValue(IFixedMemory<T> instance)
	{
		if (FixedPointerValue.TryCreateFixedValue(instance, out FixedPointerValue value))
			return new(value);
		if (instance is not IDisposable dis)
			return new(instance.ValuePointer, instance.Values.Length);
		FixedContextValue<T>.CreateDisposable(instance.ValuePointer, instance.Values.Length, dis,
		                                      out FixedContextValue<T> result);
		return result;
	}

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="FixedContextValue{T}"/> instance from current reference pointer.
	/// </summary>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	/// <param name="ptr">Current <see cref="ValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// This object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static IDisposable CreateDisposable<TDisposable>(ValPtr<T> ptr, Int32 count, TDisposable disposable,
		out FixedContextValue<T> fixedContext) where TDisposable : IDisposable
	{
		if (ptr.IsZero)
		{
			fixedContext = default;
			return FixedValueHandle.EmptyDisposable;
		}
		FixedValueHandle result = FixedValueHandle.CreateFromDisposable(disposable);
		fixedContext = new(ptr, count, result);
		return result;
	}
}