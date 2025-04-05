namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private partial class Windows
	{
		[StructLayout(LayoutKind.Sequential)]
		private unsafe struct MemoryInfo
		{
			public static readonly UIntPtr Size = (UIntPtr)sizeof(MemoryInfo);

			public IntPtr BaseAddress;
			public IntPtr AllocationBase;
			public AllocationProtection AllocationProtect;
			public UInt16 PartitionId;
			public UIntPtr RegionSize;
			public RegionState State;
			public AllocationProtection Protect;
			public RegionType Type;
		}
	}
}