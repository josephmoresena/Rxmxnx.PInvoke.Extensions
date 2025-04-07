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
			public WinNtConstants AllocationProtect;
			public UInt16 PartitionId;
			public UIntPtr RegionSize;
			public WinNtConstants State;
			public WinNtConstants Protect;
			public WinNtConstants Type;
		}
	}
}