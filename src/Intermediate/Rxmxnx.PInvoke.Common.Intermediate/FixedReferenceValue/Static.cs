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
		if (instance.Pointer == default)
			return default;
		return new((ValPtr<T>)instance.Pointer, FixedValueHandle.CreateFromDisposable(dis));
	}
}