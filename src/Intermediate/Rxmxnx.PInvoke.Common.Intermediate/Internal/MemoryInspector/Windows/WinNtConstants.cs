namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Windows
	{
		[Flags]
		public enum WinNtConstants : uint
		{
			Unknown = 0x0,

			// Access rights
			Delete = 0x00010000,
			ReadControl = 0x00020000,
			WriteDac = 0x00040000,
			WriteOwner = 0x00080000,
			Synchronize = 0x00100000,

			// Aattributes
			Readonly = 0x00000001,
			Hidden = 0x00000002,
			System = 0x00000004,
			Directory = 0x00000010,
			Archive = 0x00000020,
			Device = 0x00000040,
			Normal = 0x00000080,
			Temporary = 0x00000100,
			SparseFile = 0x00000200,
			ReparsePoint = 0x00000400,
			Compressed = 0x00000800,
			Offline = 0x00001000,
			Encrypted = 0x00004000,

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

			// Section attributes
			File = 0x800000,
			Image = 0x1000000,
			ProtectedImage = 0x2000000,
			Reserve = 0x4000000,
			Commit = 0x8000000,
			SecNoCache = 0x10000000,
			SecWriteCombine = 0x40000000,
			SecLarges = 0x80000000,
		}
	}
}