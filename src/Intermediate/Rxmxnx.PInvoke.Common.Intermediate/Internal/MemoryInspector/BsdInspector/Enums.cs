#if !UAP10_0
// ReSharper disable UnusedMember.Local

namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private abstract partial class BsdInspector
	{
		[Flags]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2342)]
#endif
		protected enum Protection : UInt32
		{
			None = 0x0,
			Read = 0x1,
			Write = 0x2,
			Execute = 0x4,
		}

		[Flags]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2342)]
#endif
		protected enum MapFlag : UInt32
		{
			None = 0x0,
			CopyOnWrite = 0x1,
			NeedsToCopy = 0x2,
			NoCoreDumps = 0x4,
			Pageable = 0x8,
			GrowsUp = 0x10,
			GrowsDown = 0x20,
			Guard = 0x40,
			Stack = 0x80,
		}
	}
}
#endif