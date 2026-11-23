#if !NET5_0_OR_GREATER && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CStringSequence
{
	/// <summary>
	/// Internal pointer to <c>typeof(System.Byte)</c> instance.
	/// </summary>
	internal static void* TypePointer
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get => Unsafe.AsPointer(ref CStringSequence.bufferType);
	}
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Internal pointer to <c>ReadOnlyFixedContext&lt;Byte&gt;.CreateInstance</c> delegate instance.
	/// </summary>
	internal static void* ConstructorPointer => Unsafe.AsPointer(ref CStringSequence.bufferConstructor);
#endif
}
#endif