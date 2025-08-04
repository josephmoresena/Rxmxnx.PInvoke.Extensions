namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class FreeBsd
	{
		/// <summary>
		/// Interop API for <c>libprocstat.so</c> library.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private static unsafe class Procstat
		{
			/// <summary>
			/// Pointer to current process state information.
			/// </summary>
			private static readonly IntPtr procstat;

			/// <summary>
			/// Static constructor.
			/// </summary>
			static Procstat()
			{
				Procstat.procstat = Procstat.Open();
				AppDomain.CurrentDomain.ProcessExit += (_, _) => Procstat.Close(Procstat.procstat);
			}

			/// <summary>
			/// Appends the process maps to <paramref name="freeBsd"/>
			/// </summary>
			/// <param name="freeBsd">A <see cref="FreeBsd"/> instance</param>
			public static void AppendMaps(FreeBsd freeBsd)
			{
				void* maps = Procstat.GetMaps(Procstat.procstat, Util.KernelInfo, out UInt32 count);
				ref VmMap mapRef = ref Unsafe.AsRef<VmMap>(maps);
				try
				{
					while (count > 0)
					{
						Boolean isReadOnly = mapRef.Protection is Protection.Read and not Protection.Write;
						freeBsd.AddBoundary(new(mapRef.StartAddress, false), isReadOnly);
						freeBsd.AddBoundary(new(mapRef.EndAddress, true), isReadOnly);
#if NET7_0_OR_GREATER
						mapRef = ref Unsafe.AddByteOffset(ref mapRef, mapRef.StructSize);
#else
						mapRef = ref Unsafe.AddByteOffset(ref mapRef, (IntPtr)mapRef.StructSize);
#endif
						count--;
					}
				}
				finally
				{
					Procstat.FreeMaps(Procstat.procstat, maps);
				}
			}

			/// <summary>
			/// Represents a FreeBSD memory map.
			/// </summary>
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

#pragma warning disable SYSLIB1054
			[DllImport("libprocstat", EntryPoint = "procstat_open_sysctl", SetLastError = false)]
			private static extern IntPtr Open();
			[DllImport("libprocstat", EntryPoint = "procstat_close", SetLastError = false)]
			private static extern void Close(IntPtr ps);
			[DllImport("libprocstat", EntryPoint = "procstat_getvmmap")]
			private static extern void* GetMaps(IntPtr ps, IntPtr kp, out UInt32 cnt);
			[DllImport("libprocstat", EntryPoint = "procstat_freevmmap")]
			private static extern void FreeMaps(IntPtr ps, void* maps);
#pragma warning restore SYSLIB1054
		}
	}
}