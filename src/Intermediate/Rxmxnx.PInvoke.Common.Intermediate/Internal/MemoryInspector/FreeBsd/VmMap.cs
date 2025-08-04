namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class FreeBsd
	{
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
#endif
		[StructLayout(LayoutKind.Sequential)]
		private readonly struct VmMap
		{
			public readonly Int32 StructSize;
			public readonly MapType Type;
			public readonly UInt64 StartAddress;
			public readonly UInt64 EndAddress;
			public readonly UInt64 Offset;
			public readonly UInt64 FileId;
			public readonly UInt32 FileSystemId;
			public readonly MapFlag Flags;
			public readonly Int32 ResidentPages;
			public readonly Int32 PrivateResidentPages;
			public readonly Protection Protection;
			public readonly Int32 ReferenceCount;
			public readonly Int32 ShadowCount;
			public readonly MapType VNodeType;
			public readonly UInt64 FileSize;
			public readonly UInt32 DeviceId;
			public readonly UInt16 FileMode;
			public readonly UInt16 Status;
			public readonly IntSpare Spare;

			[StructLayout(LayoutKind.Sequential)]
			public struct IntSpare
			{
				public Int32 Value0;
				public Int32 Value1;
				public Int32 Value2;
				public Int32 Value3;
				public Int32 Value4;
				public Int32 Value5;
				public Int32 Value6;
				public Int32 Value7;
				public Int32 Value8;
				public Int32 Value9;
				public Int32 Value10;
				public Int32 Value11;
			}
		}
	}
}