namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Windows
	{
		[StructLayout(LayoutKind.Sequential)]
		private unsafe struct MemoryInfo
		{
			public static readonly UIntPtr Size = (UIntPtr)sizeof(MemoryInfo);

			public IntPtr BaseAddress;
			public IntPtr AllocationBase;
			public MemoryState AllocationProtect;
			public UInt16 PartitionId;
			public UIntPtr RegionSize;
			public RegionState State;
			public MemoryState Protect;
			public MemoryState Type;
		}
	}
}