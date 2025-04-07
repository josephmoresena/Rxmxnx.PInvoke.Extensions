namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Windows
	{
		[Flags]
		public enum MemoryState : uint
		{
			Unknown = 0x0,

			// Protection flags
			NoAccess = 0x01,
			ReadOnly = 0x02,
			ReadWrite = 0x04,
			WriteCopy = 0x08,
			Execute = 0x10,
			ExecuteRead = 0x20,
			ExecuteReadWrite = 0x40,
			ExecuteWriteCopy = 0x80,
			Guard = 0x100,
			NoCache = 0x200,
			WriteCombine = 0x400,

			// Section type
			File = 0x800000,
			Image = 0x1000000,
			ProtectedImage = 0x2000000,
			Reserve = 0x4000000,
			Commit = 0x8000000,
			SecNoCache = 0x10000000,
			SecWriteCombine = 0x40000000,
			SecLarges = 0x80000000,
		}

		private enum RegionState : UInt32
		{
			Commit = 0x1000,
			Reserve = 0x2000,
			Free = 0x10000,
		}
	}
}