#if !NET5_0_OR_GREATER && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
#if PACKAGE
using B1 = Rxmxnx.PInvoke.Buffers.Atomic<System.Object>;
#else
using B1 = Rxmxnx.PInvoke.NativeUtilities.B1;
#endif

namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Static buffer for type instance.
	/// </summary>
	[FixedAddressValueType]
	private static B1 bufferType;
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Static buffer for delegate instance.
	/// </summary>
	[FixedAddressValueType]
	private static B1 bufferConstructor;
#endif

	/// <summary>
	/// Static constructor.
	/// </summary>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static CStringSequence()
	{
		CStringSequence.bufferType = new();
		Span<Type> types = NativeUtilities.CreateTypeSpan(ref CStringSequence.bufferType);
		types[0] = typeof(Byte);
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
		CStringSequence.bufferConstructor = new();
		Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> constructors =
			NativeUtilities.CreateConstructorSpan(ref CStringSequence.bufferConstructor);
		constructors[0] = ReadOnlyFixedContext<Byte>.CreateInstance;
#endif
	}
}
#endif