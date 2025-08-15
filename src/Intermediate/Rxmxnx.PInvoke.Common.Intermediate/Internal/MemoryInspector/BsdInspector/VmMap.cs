namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private abstract partial class BsdInspector
	{
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS4487)]
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		[StructLayout(LayoutKind.Sequential)]
		protected readonly unsafe struct VmMap
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

			/// <summary>
			/// Appends to <paramref name="bsdInspector"/> the maps from <paramref name="maps"/>.
			/// </summary>
			/// <param name="maps">A pointer to <see cref="VmMap"/> buffer.</param>
			/// <param name="count">Count of <see cref="VmMap"/> in the buffer.</param>
			/// <param name="bsdInspector">A <see cref="BsdInspector"/> instance.</param>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void AppendMaps(void* maps, UInt32 count, BsdInspector bsdInspector)
			{
				ref VmMap mapRef = ref Unsafe.AsRef<VmMap>(maps);
				while (count > 0)
				{
					Boolean isReadOnly = (mapRef.Protection & Protection.Read) == Protection.Read &&
						(mapRef.Protection & Protection.Write) == Protection.None;
					bsdInspector.AddBoundary(new(mapRef.StartAddress, false), isReadOnly);
					bsdInspector.AddBoundary(new(mapRef.EndAddress, true), isReadOnly);
#if NET7_0_OR_GREATER
					mapRef = ref Unsafe.AddByteOffset(ref mapRef, mapRef.StructSize);
#else
					mapRef = ref Unsafe.AddByteOffset(ref mapRef, (IntPtr)mapRef.StructSize);
#endif
					count--;
				}
			}
		}
	}
}