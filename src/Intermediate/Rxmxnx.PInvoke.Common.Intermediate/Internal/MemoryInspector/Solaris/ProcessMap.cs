namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Solaris
	{
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
#endif
		[StructLayout(LayoutKind.Sequential)]
		private readonly struct ProcessMap
		{
			public readonly UIntPtr Address;
			public readonly UIntPtr Size;
			public readonly MapName Name;
			public readonly UIntPtr Offset;
			public readonly MapFlags Flags;
			public readonly Int32 PageSize;
			public readonly Int32 SharedId;

			[StructLayout(LayoutKind.Explicit, Size = 64)]
			public readonly struct MapName
			{
				[FieldOffset(0)]
				private readonly Byte _byte0;
			}
		}
	}
}