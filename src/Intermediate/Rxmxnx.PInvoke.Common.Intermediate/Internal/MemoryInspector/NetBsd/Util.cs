namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class NetBsd
	{
		/// <summary>
		/// Interop API for <c>libutil.so</c> library.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private static unsafe class Util
		{
			/// <summary>
			/// Appends the process maps to <paramref name="bsd"/>
			/// </summary>
			/// <param name="bsd">A <see cref="FreeBsd"/> instance</param>
			public static void AppendMaps(NetBsd bsd)
			{
				void* maps = Util.GetMaps(C.ProcessId, out UIntPtr count);
				try
				{
					VmMap.AppendMaps(maps, (UInt32)count, bsd);
				}
				finally
				{
					C.Free(maps);
				}
			}

#pragma warning disable SYSLIB1054
			[DllImport("libutil", EntryPoint = "kinfo_getvmmap", SetLastError = true)]
			private static extern void* GetMaps(Int32 pid, out UIntPtr count);
#pragma warning restore SYSLIB1054
		}
	}
}