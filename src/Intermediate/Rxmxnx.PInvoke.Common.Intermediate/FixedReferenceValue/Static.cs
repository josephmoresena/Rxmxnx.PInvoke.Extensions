#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER && !UAP10_0_16299
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct FixedReferenceValue<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Creates a new <see cref="FixedReferenceValue{T}"/> value from <paramref name="instance"/>.
	/// </summary>
	/// <param name="instance">An <see cref="IFixedReference{T}"/> instance.</param>
	/// <returns>A new <see cref="FixedReferenceValue{T}"/> instance.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static FixedReferenceValue<T> CreateValue(IFixedReference<T> instance)
	{
		if (FixedPointerValue.TryCreateFixedValue(instance, out FixedPointerValue value))
			return new(value);
		if (instance is not IDisposable dis)
			return new(Unsafe.AsPointer(ref instance.Reference));
		FixedReferenceValue<T>.CreateDisposable((ValPtr<T>)Unsafe.AsPointer(ref instance.Reference), dis,
		                                        out FixedReferenceValue<T> result);
		return result;
	}

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="FixedReferenceValue{T}"/> instance from the current reference
	/// pointer.
	/// </summary>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	/// <param name="ptr">Current <see cref="ValPtr{T}"/> value.</param>
	/// <param name="disposable">Object to dispose to free <see langword="unmanaged"/> resources.</param>
	/// <param name="fixedReference">
	/// Output. The <see cref="FixedReferenceValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static IDisposable CreateDisposable<TDisposable>(ValPtr<T> ptr, TDisposable disposable,
		out FixedReferenceValue<T> fixedReference) where TDisposable : IDisposable
	{
		if (ptr.IsZero)
		{
			fixedReference = default;
			return FixedValueHandle.EmptyDisposable;
		}
		FixedValueHandle result = FixedValueHandle.CreateFromDisposable(disposable);
		fixedReference = new(ptr, result);
		return result;
	}
}