namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private abstract partial class BsdInspector
	{
		[Flags]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2342)]
#endif
		protected enum Protection
		{
			None = 0x0,
			Read = 0x1,
			Write = 0x2,
			Execute = 0x4,
		}

		protected enum MapType
		{
			None = 0x0,
			Default = 0x1,
			VNode = 0x2,
			Device = 0x3,
			Physical = 0x4,
			Dead = 0x5,
			SuperPage = 0x6,
			Unknown = 0x7,
		}

		[Flags]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2342)]
#endif
		protected enum MapFlag
		{
			None = 0x0,
			CopyOnWrite = 0x1,
			NeedsToCopy = 0x2,
			NoCoreDumps = 0x4,
			SuperPage = 0x8,
			GrowsUp = 0x10,
			GrowsDown = 0x20,
			Guard = 0x40,
			Stack = 0x80,
		}
	}
}