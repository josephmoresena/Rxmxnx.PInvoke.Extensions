namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private partial class Mac
	{
		[Flags]
		private enum Protection : UInt32
		{
			None = 0x0,
			Read = 0x1,
			Write = 0x2,
			Execute = 0x4,
		}

		[Flags]
		private enum Inheritance : UInt32
		{
			Share = 0x0,
			Copy = 0x1,
			None = 0x2,
		}

		private enum Behavior : UInt32
		{
			Default = 0x0,
			Random = 0x1,
			Sequential = 0x2,
			ReverseSequential = 0x3,
			WillNeed = 0x4,
			DontNeed = 0x5,
			Free = 0x6,
			ZeroWiredPages = 0x7,
		}
	}
}