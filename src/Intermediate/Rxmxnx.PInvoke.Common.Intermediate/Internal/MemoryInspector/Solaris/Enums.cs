namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Solaris
	{
		[Flags]
		private enum MapFlags
		{
			None = 0x0,
			Exec = 0x01,
			Write = 0x02,
			Read = 0x04,
			Shared = 0x08,
			Anonymous = 0x40,
			Ism = 0x80,
			NoReserve = 0x100,
			Shm = 0x200,
		}
	}
}