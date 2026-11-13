// ReSharper disable UnusedMember.Local

#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class FreeBsd
	{
		private enum MapType
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

		[StructLayout(LayoutKind.Sequential)]
		[Preserve(AllMembers = true, Conditional = true)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
#endif
		private readonly struct FreeBsdVmMap : IVmMap
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
			public readonly MapSpare Spare;

#if !NET7_0_OR_GREATER
			IntPtr IVmMap.StructSize => (IntPtr)this.StructSize;
#else
			IntPtr IVmMap.StructSize => this.StructSize;
#endif
			UInt64 IVmMap.StartAddress => this.StartAddress;
			UInt64 IVmMap.EndAddress => this.EndAddress;

			Boolean IVmMap.IsReadOnly
				=> (this.Protection & Protection.Read) != Protection.None &&
					(this.Protection & Protection.Write) == Protection.None;

			Boolean IVmMap.IsImageBacked
				=> this.Type == MapType.VNode && (this.Protection & Protection.Execute) != Protection.None;

			[StructLayout(LayoutKind.Explicit, Size = 48)]
#if !PACKAGE
			[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
#endif
			public readonly struct MapSpare
			{
				[FieldOffset(0)]
				private readonly Byte _byte0;
			}
		}
	}
}
#endif