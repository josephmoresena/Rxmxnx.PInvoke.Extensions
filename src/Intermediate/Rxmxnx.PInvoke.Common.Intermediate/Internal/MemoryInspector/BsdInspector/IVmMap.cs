#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private abstract partial class BsdInspector
	{
		/// <summary>
		/// BSD VmMap
		/// </summary>
		private protected interface IVmMap
		{
			IntPtr StructSize { get; }
			UInt64 StartAddress { get; }
			UInt64 EndAddress { get; }
			Boolean IsReadOnly { get; }
			Boolean IsImageBacked { get; }
		}
	}
}
#endif