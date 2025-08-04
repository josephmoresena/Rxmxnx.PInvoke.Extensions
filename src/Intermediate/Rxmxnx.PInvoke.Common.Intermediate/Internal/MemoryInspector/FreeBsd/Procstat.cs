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
#if !PACKAGE
			[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
			static Procstat()
			{
				Procstat.procstat = Procstat.Open();
				AppDomain.CurrentDomain.ProcessExit += (_, _) => Procstat.Close(Procstat.procstat);
			}

			/// <summary>
			/// Appends the process maps to <paramref name="bsd"/>
			/// </summary>
			/// <param name="bsd">A <see cref="FreeBsd"/> instance</param>
			public static void AppendMaps(FreeBsd bsd)
			{
				void* maps = Procstat.GetMaps(Procstat.procstat, Util.KernelInfo, out UInt32 count);
				try
				{
					VmMap.AppendMaps(maps, count, bsd);
				}
				finally
				{
					Procstat.FreeMaps(Procstat.procstat, maps);
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