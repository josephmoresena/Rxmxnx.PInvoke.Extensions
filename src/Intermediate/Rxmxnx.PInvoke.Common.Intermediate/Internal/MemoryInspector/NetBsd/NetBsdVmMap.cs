// ReSharper disable UnusedMember.Local

#if !UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class NetBsd
	{
		private enum MapType : UInt32
		{
			None = 0,
			Default = 1,
			VNode = 2,
			Swap = 3,
			Device = 4,
			Physical = 5,
			Dead = 6,
			ScatterGather = 7,
			ManagedDevice = 8,
			Guard = 9,
			Unknown = 255,
		}

		[StructLayout(LayoutKind.Sequential)]
		[Preserve(AllMembers = true, Conditional = true)]
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private readonly unsafe struct VmMap : IVmMap
		{
			public readonly UInt64 StartAddress;
			public readonly UInt64 EndAddress;
			public readonly UInt64 Offset;
			public readonly MapType Type;
			public readonly MapFlag Flags;
			public readonly UInt32 PageCount;
			public readonly UInt32 WiredPageCount;
			public readonly UInt32 Advice;
			public readonly UInt32 Attributes;
			public readonly Protection Protection;
			public readonly Protection MaxProtection;
			public readonly UInt32 ReferenceCount;
			public readonly UInt32 Inheritance;
			public readonly UInt64 FileId;
			public readonly UInt64 FileSize;
			public readonly UInt64 FileSystemId;
			public readonly UInt64 DeviceId;
			public readonly UInt32 VNodeType;
			public readonly UInt32 FileMode;
			public readonly MapSpare Spare;

#if !NET7_0_OR_GREATER
			IntPtr IVmMap.StructSize => (IntPtr)sizeof(VmMap);
#else
			IntPtr IVmMap.StructSize => sizeof(VmMap);
#endif
			UInt64 IVmMap.StartAddress => this.StartAddress;
			UInt64 IVmMap.EndAddress => this.EndAddress;

			Boolean IVmMap.IsReadOnly
				=> (this.Protection & Protection.Read) != Protection.None &&
					(this.Protection & Protection.Write) == Protection.None;

			Boolean IVmMap.IsImageBacked
				=> this.Type == MapType.VNode && (this.Protection & Protection.Execute) != Protection.None;

			[StructLayout(LayoutKind.Explicit, Size = 1024)]
#if !PACKAGE
			[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
#endif
			public struct MapSpare
			{
				[FieldOffset(0)]
				private readonly Byte _byte0;
			}
		}
	}
}
#endif