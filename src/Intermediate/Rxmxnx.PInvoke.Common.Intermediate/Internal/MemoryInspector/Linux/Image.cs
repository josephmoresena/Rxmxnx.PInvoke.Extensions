#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Linux
	{
		/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecurityCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Boolean IsImageMethod(RuntimeMethodHandle methodHandle)
		{
			IntPtr address = methodHandle.GetFunctionPointer();
			if (DynamicLinker.TryLocateImage(address, out Int32 located))
				return located != 0;
			return StandardC.LocateImage(address) != 0;
		}
	}
}
#endif