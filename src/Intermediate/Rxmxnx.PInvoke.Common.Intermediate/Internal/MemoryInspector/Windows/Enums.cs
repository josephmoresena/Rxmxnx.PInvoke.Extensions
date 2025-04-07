namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Windows
	{
		[Flags]
		private enum AllocationProtection : UInt32
		{
			NoAccess = 0x1,
			ReadOnly = 0x2,
			ReadWrite = 0x4,
			WriteCopy = 0x8,
			Execute = 0x10,
			ExecuteRead = 0x20,
			ExecuteReadWrite = 0x40,
			ExecuteWriteCopy = 0x80,
			Guard = 0x100,
			NoCache = 0x200,
			WriteCombine = 0x400,
		}

		private enum RegionState : UInt32
		{
			Commit = 0x1000,
			Reserve = 0x2000,
			Free = 0x10000,
		}

		private enum RegionType : UInt32
		{
			Private = 0x20000,
			Mapped = 0x40000,
			Image = 0x1000000,
		}
	}
}