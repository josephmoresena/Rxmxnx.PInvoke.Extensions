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
#pragma warning disable CS8500
	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> instance.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private FixedReferenceValue(ValPtr<T> valPtr, FixedValueHandle handle)
		=> this._value = new(valPtr.Pointer, sizeof(T))
		{
			IsReadOnly = false,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = handle,
			Type = typeof(T),
		};
#pragma warning restore CS8500
}