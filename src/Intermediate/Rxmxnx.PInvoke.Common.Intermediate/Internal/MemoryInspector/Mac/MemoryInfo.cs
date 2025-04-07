namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Mac
	{
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		[StructLayout(LayoutKind.Sequential)]
		private readonly unsafe struct MemoryInfo
		{
			public static readonly UInt32 Flavor = (UInt32)(Environment.Is64BitProcess ? 9 : 10);
			public static readonly UInt32 Count = (UInt32)sizeof(MemoryInfo) / sizeof(Int32);

			public readonly Protection Protection;
			public readonly Protection MaxProtection;
			public readonly Inheritance Inheritance;
			public readonly UInt32 Shared;
			public readonly IntPtr Reserved;
			public readonly IntPtr Offset;
			public readonly Behavior Behavior;
			public readonly UInt16 UserWiredCount;
		}
	}
}